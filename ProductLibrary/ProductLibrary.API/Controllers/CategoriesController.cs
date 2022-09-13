using AutoMapper;
using ProductLibrary.API.ActionConstraints;
using ProductLibrary.API.Helpers;
using ProductLibrary.API.Models;
using ProductLibrary.API.ResourceParameters;
using ProductLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ProductLibrary.API.Filter;

namespace ProductLibrary.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly IProductLibraryRepository _productLibraryRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _propertyCheckerService;

        public CategoriesController(IProductLibraryRepository productLibraryRepository,
            IMapper mapper, IPropertyMappingService propertyMappingService,
            IPropertyCheckerService propertyCheckerService)
        {
            _productLibraryRepository = productLibraryRepository ??
                throw new ArgumentNullException(nameof(productLibraryRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService = propertyMappingService ??
              throw new ArgumentNullException(nameof(propertyMappingService));
            _propertyCheckerService = propertyCheckerService ??
              throw new ArgumentNullException(nameof(propertyCheckerService));
        }

        [HttpGet(Name = "GetCategories")]
        [ResponseCache(CacheProfileName = "240SecondsCacheProfile")]
        public IActionResult GetCategories(
            [FromQuery] CategoriesResourceParameters categoriesResourceParameters)
        {            
            if (!_propertyMappingService.ValidMappingExistsFor<CategoryDto, Entities.Category>
                (categoriesResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_propertyCheckerService.TypeHasProperties<CategoryDto>
              (categoriesResourceParameters.Fields))
            {
                return BadRequest();
            }

            var categoriesFromRepo = _productLibraryRepository.GetCategories(categoriesResourceParameters);
                    
            var paginationMetadata = new
            {
                totalCount = categoriesFromRepo.TotalCount,
                pageSize = categoriesFromRepo.PageSize,
                currentPage = categoriesFromRepo.CurrentPage,
                totalPages = categoriesFromRepo.TotalPages 
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var links = CreateLinksForCategories(categoriesResourceParameters,
                categoriesFromRepo.HasNext,
                categoriesFromRepo.HasPrevious);

            var shapedCategories = _mapper.Map<IEnumerable<CategoryDto>>(categoriesFromRepo)
                               .ShapeData(categoriesResourceParameters.Fields);

            var shapedCategoriesWithLinks = shapedCategories.Select(category =>
            {
                var categoryAsDictionary = category as IDictionary<string, object>;
                var categoryLinks = CreateLinksForCategory((Guid)categoryAsDictionary["Id"], null);
                categoryAsDictionary.Add("links", categoryLinks);
                return categoryAsDictionary;
            });

            var linkedCollectionResource = new
            {
                value = shapedCategoriesWithLinks,
                links
            };

            return Ok(linkedCollectionResource);             
        }
        [Produces("application/json", 
            "application/x.hateoas+json",
            "application/x.category.full+json", 
            "application/x.category.full.hateoas+json",
            "application/x.category.friendly+json", 
            "application/x.category.friendly.hateoas+json")]
        [ETagFilter]
        [HttpGet("{categoryId}", Name ="GetCategory")]
        public IActionResult GetCategory(Guid categoryId, string fields,
              [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType,
                out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            if (!_propertyCheckerService.TypeHasProperties<CategoryDto>
               (fields))
            {
                return BadRequest();
            }

            var categoryFromRepo = _productLibraryRepository.GetCategory(categoryId);

            if (categoryFromRepo == null)
            {
                return NotFound();
            }

            var includeLinks = parsedMediaType.SubTypeWithoutSuffix
               .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

            IEnumerable<LinkDto> links = new List<LinkDto>();

            if (includeLinks)
            {
                links = CreateLinksForCategory(categoryId, fields);
            }

            var primaryMediaType = includeLinks ?
                parsedMediaType.SubTypeWithoutSuffix
                .Substring(0, parsedMediaType.SubTypeWithoutSuffix.Length - 8)
                : parsedMediaType.SubTypeWithoutSuffix;

            // full category
            if (primaryMediaType == "x.category.full")
            {
                var fullResourceToReturn = _mapper.Map<CategoryFullDto>(categoryFromRepo)
                    .ShapeData(fields) as IDictionary<string, object>;

                if (includeLinks)
                {
                    fullResourceToReturn.Add("links", links);
                }

                return Ok(fullResourceToReturn);
            }

            // friendly category
            var friendlyResourceToReturn = _mapper.Map<CategoryDto>(categoryFromRepo)
                .ShapeData(fields) as IDictionary<string, object>;

            if (includeLinks)
            {
                friendlyResourceToReturn.Add("links", links);
            }

            return Ok(friendlyResourceToReturn);
        }
		
		[HttpPost(Name = "CreateCategoryWithDateDeleted")]
        [RequestHeaderMatchesMediaType("Content-Type",
            "application/x.categoryforcreationwithdatedeleted+json")]
        [Consumes("application/x.categoryforcreationwithdatedeleted+json")]
        public IActionResult CreateCategoryWithDateDeleted(CategoryForCreationWithDateDeletedDto category)
        {
            var categoryEntity = _mapper.Map<Entities.Category>(category);
            _productLibraryRepository.AddCategory(categoryEntity);
            _productLibraryRepository.Save();

            var categoryToReturn = _mapper.Map<CategoryDto>(categoryEntity);

            var links = CreateLinksForCategory(categoryToReturn.Id, null);

            var linkedResourceToReturn = categoryToReturn.ShapeData(null)
                as IDictionary<string, object>;
            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetCategory",
                new { categoryId = linkedResourceToReturn["Id"] },
                linkedResourceToReturn);
        }

        [HttpPost(Name = "CreateCategory")]
        [RequestHeaderMatchesMediaType("Content-Type", "application/json", "application/x.categoryforcreation+json")]
        [Consumes("application/json",
            "application/x.categoryforcreation+json")]
        public ActionResult<CategoryDto> CreateCategory(CategoryForCreationDto category)
        {
            var categoryEntity = _mapper.Map<Entities.Category>(category);
            _productLibraryRepository.AddCategory(categoryEntity);
            _productLibraryRepository.Save();

            var categoryToReturn = _mapper.Map<CategoryDto>(categoryEntity);

            var links = CreateLinksForCategory(categoryToReturn.Id, null);
             
            var linkedResourceToReturn = categoryToReturn.ShapeData(null)
                as IDictionary<string, object>;
            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetCategory",
                new { categoryId = linkedResourceToReturn["Id"] },
                linkedResourceToReturn);
        }
 

        [HttpOptions]
        public IActionResult GetCategoriesOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        [HttpDelete("{categoryId}", Name = "DeleteCategory")]
        public ActionResult DeleteCategory(Guid categoryId)
        {
            var categoryFromRepo = _productLibraryRepository.GetCategory(categoryId);

            if (categoryFromRepo == null)
            {
                return NotFound();
            }

            _productLibraryRepository.DeleteCategory(categoryFromRepo);

            _productLibraryRepository.Save();

            return NoContent();
        }

        private string CreateCategoriesResourceUri(
           CategoriesResourceParameters categoriesResourceParameters,
           ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetCategories",
                      new
                      {
                          fields = categoriesResourceParameters.Fields,
                          orderBy = categoriesResourceParameters.OrderBy,
                          pageNumber = categoriesResourceParameters.PageNumber - 1,
                          pageSize = categoriesResourceParameters.PageSize,
                          mainCategory = categoriesResourceParameters.MainCategory,
                          searchQuery = categoriesResourceParameters.SearchQuery
                      });
                case ResourceUriType.NextPage:
                    return Url.Link("GetCategories",
                      new
                      {
                          fields = categoriesResourceParameters.Fields,
                          orderBy = categoriesResourceParameters.OrderBy,
                          pageNumber = categoriesResourceParameters.PageNumber + 1,
                          pageSize = categoriesResourceParameters.PageSize,
                          mainCategory = categoriesResourceParameters.MainCategory,
                          searchQuery = categoriesResourceParameters.SearchQuery
                      });
                case ResourceUriType.Current:
                default:
                    return Url.Link("GetCategories",
                    new
                    {
                        fields = categoriesResourceParameters.Fields,
                        orderBy = categoriesResourceParameters.OrderBy,
                        pageNumber = categoriesResourceParameters.PageNumber,
                        pageSize = categoriesResourceParameters.PageSize,
                        mainCategory = categoriesResourceParameters.MainCategory,
                        searchQuery = categoriesResourceParameters.SearchQuery
                    });
            }

        }

        private IEnumerable<LinkDto> CreateLinksForCategory(Guid categoryId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(Url.Link("GetCategory", new { categoryId }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(Url.Link("GetCategory", new { categoryId, fields }),
                  "self",
                  "GET"));
            }

            links.Add(
               new LinkDto(Url.Link("DeleteCategory", new { categoryId }),
               "delete_category",
               "DELETE"));

            links.Add(
                new LinkDto(Url.Link("CreateProductForCategory", new { categoryId }),
                "create_product_for_category",
                "POST"));

            links.Add(
               new LinkDto(Url.Link("GetProductsForCategory", new { categoryId }),
               "products",
               "GET"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForCategories(
            CategoriesResourceParameters categoriesResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateCategoriesResourceUri(
                   categoriesResourceParameters, ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateCategoriesResourceUri(
                      categoriesResourceParameters, ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateCategoriesResourceUri(
                        categoriesResourceParameters, ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }

    }
}
