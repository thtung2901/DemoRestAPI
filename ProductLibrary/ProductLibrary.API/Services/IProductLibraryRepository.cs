using ProductLibrary.API.Entities;
using ProductLibrary.API.Helpers;
using ProductLibrary.API.ResourceParameters;
using System;
using System.Collections.Generic;

namespace ProductLibrary.API.Services
{
    public interface IProductLibraryRepository
    {    
        IEnumerable<Product> GetProducts(Guid categoryId);
        Product GetProduct(Guid categoryId, Guid productId);
        void AddProduct(Guid categoryId, Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        IEnumerable<Category> GetCategories();
        PagedList<Category> GetCategories(CategoriesResourceParameters categoriesResourceParameters);
        Category GetCategory(Guid categoryId);
        IEnumerable<Category> GetCategories(IEnumerable<Guid> categoryIds);
        void AddCategory(Category category);
        void DeleteCategory(Category category);
        void UpdateCategory(Category category);
        bool CategoryExists(Guid categoryId);
        bool Save();
    }
}
