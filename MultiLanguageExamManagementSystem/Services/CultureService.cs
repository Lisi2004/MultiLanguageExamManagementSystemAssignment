using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;
using MultiLanguageExamManagementSystem.Services.IServices;
using AutoMapper;
using Microsoft.Extensions.Localization;

namespace MultiLanguageExamManagementSystem.Services
{
    public class CultureService : ICultureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CultureService> _logger;

        public CultureService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CultureService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #region String Localization

        public LocalizedString this[string key]
        {
            get
            {
                var parts = key.Split('.');
                var namespacePart = parts[0];
                var keyPart = parts[1];
                var acceptLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

                var localizedString = GetLocalizationResource(namespacePart, keyPart, acceptLanguage).Result;

                return new LocalizedString(key, localizedString?.Value ?? key, localizedString == null);
            }
        }

        #endregion

        #region Languages

        public async Task CreateLanguage(LanguageCreateDto language)
        {
            var languageToCreate = _mapper.Map<Language>(language);

            _unitOfWork.Repository<Language>().Create(languageToCreate);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<LanguageDto>> GetAllLanguages()
        {
            var languages = await _unitOfWork.Repository<Language>().GetAll().ToListAsync();

            return _mapper.Map<List<LanguageDto>>(languages);
        }

        public async Task<LanguageDto> GetLanguage(int id)
        {
            try
            {
                Expression<Func<Language, bool>> condition = language => language.Id == id;
                var language = await _unitOfWork.Repository<Language>().GetByCondition(condition).FirstOrDefaultAsync();

                if (language == null)
                {
                    throw new ArgumentException("Language not found with the provided id.", nameof(id));
                }

                return _mapper.Map<LanguageDto>(language);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching language by id.");
                throw;
            }
        }

        public async Task UpdateLanguage(LanguageDto language)
        {
            var languageToUpdate = await _unitOfWork.Repository<Language>()
                .GetById(x => x.LanguageCode == language.LanguageCode).FirstOrDefaultAsync();

            if (languageToUpdate != null)
            {
                languageToUpdate.Name = language.Name;
                languageToUpdate.LanguageCode = language.LanguageCode;
            }

            _unitOfWork.Repository<Language>().Update(languageToUpdate);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteLanguage(int id)
        {
            var languageToDelete = await _unitOfWork.Repository<Language>().GetById(x => x.Id == id).FirstOrDefaultAsync();
            _unitOfWork.Repository<Language>().Delete(languageToDelete);
            await _unitOfWork.CompleteAsync();
        }

        #endregion

        #region Localization Resources

        public async Task CreateLocalizationResource(LocalizationResourceCreateDto localizationResource)
        {
            var resourceExists = await _unitOfWork.Repository<LocalizationResource>()
                .GetByCondition(x => x.Namespace == localizationResource.Namespace &&
                                     x.Key == localizationResource.Key &&
                                     x.LanguageId == localizationResource.LanguageId)
                .AnyAsync();

            if (resourceExists)
            {
                throw new InvalidOperationException("Localization resource already exists");
            }

            var languageExists = await _unitOfWork.Repository<Language>()
                    .GetByCondition(x => x.Id == localizationResource.LanguageId)
                    .AnyAsync();

            if (!languageExists)
            {
                throw new ArgumentException("Language does not exist", nameof(localizationResource.LanguageId));
            }

            var localizationResourceToCreate = _mapper.Map<LocalizationResource>(localizationResource);
            _unitOfWork.Repository<LocalizationResource>().Create(localizationResourceToCreate);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<LocalizationResourceDto>> GetAllLocalizationResources()
        {
            var localizationResources = await _unitOfWork.Repository<LocalizationResource>()
                .GetAll()
                .ToListAsync();

            return _mapper.Map<List<LocalizationResourceDto>>(localizationResources);
        }

        public async Task<LocalizationResource> GetLocalizationResource(string namespacePart, string keyPart, string acceptLanguage = "")
        {
            Expression<Func<LocalizationResource, bool>> condition;

            if (!string.IsNullOrEmpty(acceptLanguage))
            {
                var language = await _unitOfWork.Repository<Language>().GetByCondition(x => x.LanguageCode == acceptLanguage).FirstOrDefaultAsync();
                if (language == null)
                {
                    throw new Exception("Language does not exist");
                }

                condition = resource => resource.Namespace == namespacePart &&
                                resource.Key == keyPart &&
                                resource.LanguageId == language.Id;
            }
            else
            {
                condition = resource => resource.Namespace == namespacePart &&
                                resource.Key == keyPart;
            }

            var resource = await _unitOfWork.Repository<LocalizationResource>().GetByCondition(condition).FirstOrDefaultAsync();

            return resource;
        }

        public async Task UpdateLocalizationResource(LocalizationResourceCreateDto localizationResource)
        {
            var localizationResourceToUpdate = await GetLocalizationResource(localizationResource.Namespace, localizationResource.Key);

            if (localizationResourceToUpdate != null)
            {
                localizationResourceToUpdate.Key = localizationResource.Key;
                localizationResourceToUpdate.Value = localizationResource.Value;
                localizationResourceToUpdate.LanguageId = localizationResource.LanguageId;
                localizationResourceToUpdate.Namespace = localizationResource.Namespace;
                localizationResourceToUpdate.BeautifiedNamespace = localizationResource.BeautifiedNamespace;
            }

            _unitOfWork.Repository<LocalizationResource>().Update(localizationResourceToUpdate);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteLocalizationResource(int id)
        {
            var localizationResource = await _unitOfWork.Repository<LocalizationResource>().GetById(x => x.Id == id).FirstOrDefaultAsync();
            _unitOfWork.Repository<LocalizationResource>().Delete(localizationResource);
            await _unitOfWork.CompleteAsync();
        }

        Task<LocalizationResourceDto> ICultureService.GetLocalizationResource(string namespacePart, string keyPart, string acceptLanguage)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
