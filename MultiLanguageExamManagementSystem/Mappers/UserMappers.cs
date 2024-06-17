using AutoMapper;
using MultiLanguageExamManagementSystem.Models.Dtos.UserAccess;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Mappers
{
    public class UserMappers : Profile
    {
        public UserMappers()
        {
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
