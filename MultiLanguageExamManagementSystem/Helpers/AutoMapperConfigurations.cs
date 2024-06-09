using AutoMapper;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace LifeEcommerce.Helpers
{
    public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations()
        {
            CreateMap<LocalizationResource, LocalizationResourceDto>().ReverseMap();
            CreateMap<LocalizationResource, LocalizationResourceCreateDto>().ReverseMap();
            CreateMap<Language, LanguageDto>().ReverseMap();
            CreateMap<Language, LanguageCreateDto>().ReverseMap();
        }
    }
}
