using MultiLanguageExamManagementSystem.Models.Dtos.UserAccess;

namespace MultiLanguageExamManagementSystem.Services.IServices
{
    public interface IUserService
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO);
    }
}