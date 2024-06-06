using MultiLanguageExamManagementSystem.Models.Dtos;

namespace MultiLanguageExamManagementSystem.Services.IServices;

public interface ITranslationService
{
    Task<List<LocalizationResourceDto>> GetAllLocalizationResourcesEnglish();
    Task<List<LocalizationResourceCreateDto>> TranslateResources(string targetLanguage);
}