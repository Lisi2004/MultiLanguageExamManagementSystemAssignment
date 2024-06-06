using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Services.IServices
{
    /// <summary>
    /// Defines the interface for managing cultures, languages, and localization resources.
    /// </summary>
    public interface ICultureService
    {
        /// <summary>
        /// Gets a localized string based on the given key.
        /// </summary>
        /// <param name="key">The key of the string to localize.</param>
        LocalizedString this[string key] { get; }

        // Language methods
        Task CreateLanguage(LanguageCreateDto language);
        Task<List<LanguageDto>> GetAllLanguages();
        Task<LanguageDto> GetLanguage(int id);
        Task UpdateLanguage(LanguageDto language);
        Task DeleteLanguage(int id);

        // Localization resource methods
        Task CreateLocalizationResource(LocalizationResourceCreateDto localizationResource);
        Task<List<LocalizationResourceDto>> GetAllLocalizationResources();
        Task<LocalizationResourceDto> GetLocalizationResource(string namespacePart, string keyPart, string acceptLanguage = "");
        Task UpdateLocalizationResource(LocalizationResourceCreateDto localizationResource);
        Task DeleteLocalizationResource(int id);
    }
}
