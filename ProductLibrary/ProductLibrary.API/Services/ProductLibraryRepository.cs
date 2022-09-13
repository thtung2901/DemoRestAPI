using ProductLibrary.API.DbContexts;
using ProductLibrary.API.Entities;
using ProductLibrary.API.Helpers;
using ProductLibrary.API.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductLibrary.API.Services
{
    public class ProductLibraryRepository : IProductLibraryRepository, IDisposable
    {
        private readonly ProductLibraryContext _context;
        private readonly IPropertyMappingService _propertyMappingService;

        public ProductLibraryRepository(ProductLibraryContext context,
            IPropertyMappingService propertyMappingService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _propertyMappingService = propertyMappingService ?? 
                throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public void AddProduct(Guid categoryId, Product product)
        {
            if (categoryId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(categoryId));
            }

            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            // always set the CategoryId to the passed-in categoryId
            product.CategoryId = categoryId;
            _context.Products.Add(product); 
        }         

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }
  
        public Product GetProduct(Guid categoryId, Guid productId)
        {
            if (categoryId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(categoryId));
            }

            if (productId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(productId));
            }

            return _context.Products.FirstOrDefault(c => c.CategoryId == categoryId && c.Id == productId);
        }

        public IEnumerable<Product> GetProducts(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(categoryId));
            }

            return _context.Products
                        .Where(c => c.CategoryId == categoryId)
                        .OrderBy(c => c.Title).ToList();
        }

        public void UpdateProduct(Product product)
        {
            // no code in this implementation
        }

        public void AddCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            // the repository fills the id (instead of using identity columns)
            category.Id = Guid.NewGuid();

            foreach (var product in category.Products)
            {
                product.Id = Guid.NewGuid();
            }

            _context.Categories.Add(category);
        }

        public bool CategoryExists(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(categoryId));
            }

            return _context.Categories.Any(a => a.Id == categoryId);
        }

        public void DeleteCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            _context.Categories.Remove(category);
        }
        
        public Category GetCategory(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(categoryId));
            }

            return _context.Categories.FirstOrDefault(a => a.Id == categoryId);
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.ToList<Category>();
        }

        public PagedList<Category> GetCategories(CategoriesResourceParameters categoriesResourceParameters)
        {
            if (categoriesResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(categoriesResourceParameters));
            }
            
            var collection = _context.Categories as IQueryable<Category>;

            if (!string.IsNullOrWhiteSpace(categoriesResourceParameters.MainCategory))
            {
                var mainCategory = categoriesResourceParameters.MainCategory.Trim();
                collection = collection.Where(a => a.MainCategory == mainCategory);
            }

            if (!string.IsNullOrWhiteSpace(categoriesResourceParameters.SearchQuery))
            {

                var searchQuery = categoriesResourceParameters.SearchQuery.Trim();
                collection = collection.Where(a => a.MainCategory.Contains(searchQuery)
                    || a.Name.Contains(searchQuery)
                    || a.FullName.Contains(searchQuery));
            }

            if (!string.IsNullOrWhiteSpace(categoriesResourceParameters.OrderBy))
            {
                // get property mapping dictionary
                var categoryPropertyMappingDictionary = 
                    _propertyMappingService.GetPropertyMapping<Models.CategoryDto, Category>();

                collection = collection.ApplySort(categoriesResourceParameters.OrderBy,
                    categoryPropertyMappingDictionary);
            }

            return PagedList<Category>.Create(collection,
                categoriesResourceParameters.PageNumber,
                categoriesResourceParameters.PageSize); 
        }

        public IEnumerable<Category> GetCategories(IEnumerable<Guid> categoryIds)
        {
            if (categoryIds == null)
            {
                throw new ArgumentNullException(nameof(categoryIds));
            }

            return _context.Categories.Where(a => categoryIds.Contains(a.Id))
                .OrderBy(a => a.Name)
                .OrderBy(a => a.FullName)
                .ToList();
        }

        public void UpdateCategory(Category category)
        {
            // no code in this implementation
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
               // dispose resources when needed
            }
        }
    }
}
