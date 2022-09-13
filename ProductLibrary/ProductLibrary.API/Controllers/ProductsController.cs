using AutoMapper;
using ProductLibrary.API.Models;
using ProductLibrary.API.Services;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductLibrary.API.Controllers
{
    [ApiController]
    [Route("api/categories/{categoryId}/products")]
    // [ResponseCache(CacheProfileName = "240SecondsCacheProfile")]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public)]
    [HttpCacheValidation(MustRevalidate = true)]
    public class ProductsController : ControllerBase
    {
        private readonly IProductLibraryRepository _productLibraryRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductLibraryRepository productLibraryRepository,
            IMapper mapper)
        {
            _productLibraryRepository = productLibraryRepository ??
                throw new ArgumentNullException(nameof(productLibraryRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetProductsForCategory")]
        public ActionResult<IEnumerable<ProductDto>> GetProductsForCategory(Guid categoryId)
        {
            if (!_productLibraryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var productsForCategoryFromRepo = _productLibraryRepository.GetProducts(categoryId);
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(productsForCategoryFromRepo));
        }

        [HttpGet(Name = "GetProductForCategory")]
        [Route("/api/products/{categoryId}/{productId}")]
        // [ResponseCache(Duration = 120)]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 1000)]
        [HttpCacheValidation(MustRevalidate = false)]
        public ActionResult<ProductDto> GetProductForCategory(Guid categoryId, Guid productId)
        {
            if (!_productLibraryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var productForCategoryFromRepo = _productLibraryRepository.GetProduct(categoryId, productId);

            if (productForCategoryFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProductDto>(productForCategoryFromRepo));
        }

        [HttpPost(Name = "CreateProductForCategory")]
        public ActionResult<ProductDto> CreateProductForCategory(
            Guid categoryId, ProductForCreationDto product)
        {
            if (!_productLibraryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var productEntity = _mapper.Map<Entities.Product>(product);
            _productLibraryRepository.AddProduct(categoryId, productEntity);
            _productLibraryRepository.Save();

            var productToReturn = _mapper.Map<ProductDto>(productEntity);
            return CreatedAtRoute("GetProductForCategory",
                new { categoryId = categoryId, productId = productToReturn.Id }, 
                productToReturn);
        }

        [HttpPut("{productId}")]
        public IActionResult UpdateProductForCategory(Guid categoryId, 
            Guid productId, 
            ProductForUpdateDto product)
        {
            if (!_productLibraryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var productForCategoryFromRepo = _productLibraryRepository.GetProduct(categoryId, productId);

            if (productForCategoryFromRepo == null)
            {
                var productToAdd = _mapper.Map<Entities.Product>(product);
                productToAdd.Id = productId;

                _productLibraryRepository.AddProduct(categoryId, productToAdd);

                _productLibraryRepository.Save();

                var productToReturn = _mapper.Map<ProductDto>(productToAdd);

                return CreatedAtRoute("GetProductForCategory",
                    new { categoryId, productId = productToReturn.Id },
                    productToReturn);
            }

            // map the entity to a ProductForUpdateDto
            // apply the updated field values to that dto
            // map the ProductForUpdateDto back to an entity
            _mapper.Map(product, productForCategoryFromRepo);

            _productLibraryRepository.UpdateProduct(productForCategoryFromRepo);

            _productLibraryRepository.Save();
            return NoContent();
        }

        [HttpPatch("{productId}")]
        public ActionResult PartiallyUpdateProductForCategory(Guid categoryId, 
            Guid productId,
            JsonPatchDocument<ProductForUpdateDto> patchDocument)
        {
            if (!_productLibraryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var productForCategoryFromRepo = _productLibraryRepository.GetProduct(categoryId, productId);

            if (productForCategoryFromRepo == null)
            {
                var productDto = new ProductForUpdateDto();
                patchDocument.ApplyTo(productDto, ModelState);

                if (!TryValidateModel(productDto))
                {
                    return ValidationProblem(ModelState);
                }

                var productToAdd = _mapper.Map<Entities.Product>(productDto);
                productToAdd.Id = productId;

                _productLibraryRepository.AddProduct(categoryId, productToAdd);
                _productLibraryRepository.Save();

                var productToReturn = _mapper.Map<ProductDto>(productToAdd);

                return CreatedAtRoute("GetProductForCategory",
                    new { categoryId, productId = productToReturn.Id }, 
                    productToReturn);
            }

            var productToPatch = _mapper.Map<ProductForUpdateDto>(productForCategoryFromRepo);
            // add validation
            patchDocument.ApplyTo(productToPatch, ModelState);

            if (!TryValidateModel(productToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(productToPatch, productForCategoryFromRepo);

            _productLibraryRepository.UpdateProduct(productForCategoryFromRepo);

            _productLibraryRepository.Save();

            return NoContent();
        }

        [HttpDelete("{productId}")]
        public ActionResult DeleteProductForCategory(Guid categoryId, Guid productId)
        {
            if (!_productLibraryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var productForCategoryFromRepo = _productLibraryRepository.GetProduct(categoryId, productId);

            if (productForCategoryFromRepo == null)
            {
                return NotFound();
            }

            _productLibraryRepository.DeleteProduct(productForCategoryFromRepo);
            _productLibraryRepository.Save();

            return NoContent();
        }

        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}