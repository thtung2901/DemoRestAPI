using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductLibrary.API.Profiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            CreateMap<Entities.Product, Models.ProductDto>();
            CreateMap<Models.ProductForCreationDto, Entities.Product>();
            CreateMap<Models.ProductForUpdateDto, Entities.Product>();
            CreateMap<Entities.Product, Models.ProductForUpdateDto>();
        }
    }
}
