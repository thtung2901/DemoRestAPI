using AutoMapper;
using ProductLibrary.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductLibrary.API.Profiles
{
    public class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {
            CreateMap<Entities.Category, Models.CategoryDto>()
                .ForMember(
                    dest => dest.Name, 
                    opt => opt.MapFrom(src => $"{src.Name} {src.FullName}"))
                .ForMember(
                    dest => dest.Age, 
                    opt => opt.MapFrom(src => src.DateCreated.GetCurrentAge(src.DateDeleted)));

            CreateMap<Models.CategoryForCreationDto, Entities.Category>();

            CreateMap<Models.CategoryForCreationWithDateDeletedDto, Entities.Category>();

            CreateMap<Entities.Category, Models.CategoryFullDto>();
        }
    }
}
