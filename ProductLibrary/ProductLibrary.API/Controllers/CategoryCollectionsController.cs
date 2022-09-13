using AutoMapper;
using ProductLibrary.API.Helpers;
using ProductLibrary.API.Models;
using ProductLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductLibrary.API.Controllers
{
    [ApiController]
    [Route("api/categorycollections")]
    public class CategoryCollectionsController : ControllerBase
    {
        private readonly IProductLibraryRepository _productLibraryRepository;
        private readonly IMapper _mapper;

        public CategoryCollectionsController(IProductLibraryRepository productLibraryRepository,
            IMapper mapper)
        {
            _productLibraryRepository = productLibraryRepository ??
                throw new ArgumentNullException(nameof(productLibraryRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("({ids})", Name ="GetCategoryCollection")]
        public IActionResult GetCategoryCollection(
        [FromRoute]
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var categoryEntities = _productLibraryRepository.GetCategories(ids);

            if (ids.Count() != categoryEntities.Count())
            {
                return NotFound();
            }

            var categoriesToReturn = _mapper.Map<IEnumerable<CategoryDto>>(categoryEntities);

            return Ok(categoriesToReturn);
        }


        [HttpPost]
        public ActionResult<IEnumerable<CategoryDto>> CreateCategoryCollection(
            IEnumerable<CategoryForCreationDto> categoryCollection)
        {
            var categoryEntities = _mapper.Map<IEnumerable<Entities.Category>>(categoryCollection);
            foreach (var category in categoryEntities)
            {
                _productLibraryRepository.AddCategory(category);
            }

            _productLibraryRepository.Save();

            var categoryCollectionToReturn = _mapper.Map<IEnumerable<CategoryDto>>(categoryEntities);
            var idsAsString = string.Join(",", categoryCollectionToReturn.Select(a => a.Id));
            return CreatedAtRoute("GetCategoryCollection",
             new { ids = idsAsString },
             categoryCollectionToReturn);
        }
    }
}
 
