using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Services.IServices;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MultiLanguageExamManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocalizationsController : ControllerBase
    {
        private readonly ILogger<LocalizationsController> _logger;
        private readonly ICultureService _cultureService;
        private readonly ITranslationService _translationService;

        public LocalizationsController(ILogger<LocalizationsController> logger, ICultureService cultureService, ITranslationService translationService)
        {
            _logger = logger;
            _cultureService = cultureService;
            _translationService = translationService;
        }

        [HttpGet("GetLocalizationResource", Name = "GetLocalizationResource")]
        public async Task<IActionResult> GetLocalizationResource()
        {
            try
            {
                var current = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                var message = _cultureService["ne.1"];

                return Ok(new { message.Value, current });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching localization resource.");
                return StatusCode(500, "Internal server error occurred.");
            }
        }

        #region LocalizationResources CRUD
        [HttpGet("GetAllLocalizationResources")]
        public async Task<IActionResult> GetAllLocalizationResources()
        {
            try
            {
                var resources = await _cultureService.GetAllLocalizationResources();
                return Ok(resources);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all localization resources.");
                return StatusCode(500, "Internal server error occurred.");
            }
        }

        [HttpPost("CreateLocalizationResource")]
        public async Task<IActionResult> CreateLocalizationResource(LocalizationResourceCreateDto localizationResource)
        {
            try
            {
                await _cultureService.CreateLocalizationResource(localizationResource);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating localization resource.");
                return StatusCode(500, "Internal server error occurred.");
            }
        }

        [HttpPut("UpdateLocalizationResource")]
        public async Task<IActionResult> UpdateLocalizationResource(LocalizationResourceCreateDto resourceDto)
        {
            try
            {
                await _cultureService.UpdateLocalizationResource(resourceDto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating localization resource.");
                return StatusCode(500, "Internal server error occurred.");
            }
        }

        [HttpDelete("DeleteLocalizationResource/{id}")]
        public async Task<IActionResult> DeleteLocalizationResource(int id)
        {
            try
            {
                await _cultureService.DeleteLocalizationResource(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting localization resource.");
                return StatusCode(500, "Internal server error occurred.");
            }
        }
        #endregion

        #region Languages CRUD
        [HttpGet("GetAllLanguages", Name = "GetAllLanguages")]
        public async Task<IActionResult> GetAllLanguages()
        {
            try
            {
                var languages = await _cultureService.GetAllLanguages();
                return Ok(languages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all languages.");
                return StatusCode(500, "Internal server error occurred.");
            }
        }

        [HttpGet("GetLanguage/{id}", Name = "GetLanguage")]
        public async Task<IActionResult> GetLanguage(int id)
        {
            try
            {
                var language = await _cultureService.GetLanguage(id);
                return Ok(language);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching language.");
                return StatusCode(500, "Internal server error occurred.");
            }
        }

        [HttpPost("CreateLanguage")]
        public async Task<IActionResult> CreateLanguage(LanguageCreateDto language)
        {
            try
            {
                await _cultureService.CreateLanguage(language);
                var translatedResources = await _translationService.TranslateResources(language.LanguageCode);
                return Ok(translatedResources);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating language.");
                return StatusCode(500, "Internal server error occurred.");
            }
        }

        [HttpPut("UpdateLanguage")]
        public async Task<IActionResult> UpdateLanguage(LanguageDto language)
        {
            try
            {
                await _cultureService.UpdateLanguage(language);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating language.");
                return StatusCode(500, "Internal server error occurred.");
            }
        }

        [HttpDelete("DeleteLanguage/{id}")]
        public async Task<IActionResult> DeleteLanguage(int id)
        {
            try
            {
                await _cultureService.DeleteLanguage(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting language.");
                return StatusCode(500, "Internal server error occurred.");
            }
        }
        #endregion
    }
}
