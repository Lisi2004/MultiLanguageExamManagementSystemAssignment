using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Entities;
using MultiLanguageExamManagementSystem.Services.IServices;
using System.Linq.Expressions;
using AutoMapper;
using MultiLanguageExamManagementSystem.Models.Dtos;

namespace MultiLanguageExamManagementSystem.Services
{
    public class CultureService : ICultureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CultureService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Your code here

        #region String Localization

        // String localization methods implementation here
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

        // language methods implementation here
        public async Task CreateLanguage(LanguageCreateDto language)
        {
            var languageToCreate = _mapper.Map<Language>(language);

            _unitOfWork.Repository<Language>().Create(languageToCreate);
            _unitOfWork.Complete();
        }

        public async Task<List<LanguageDto>> GetAllLanguages()
        {
            var languages = await _unitOfWork.Repository<Language>().GetAll().ToListAsync();

            var languagesToReturn = _mapper.Map<List<LanguageDto>>(languages);

            return languagesToReturn;
        }

        public async Task<Language> GetLanguage(int id)
        {
            var language = _unitOfWork.Repository<Language>().GetById(x => x.Id == id);

            return await language.FirstAsync();
        }

        public async Task UpdateLanguage(LanguageDto language)
        {
            var languageToUpdate = _unitOfWork.Repository<Language>()
                .GetById(x => x.LanguageCode == language.LanguageCode).FirstOrDefault();

            if (languageToUpdate != null)
            {
                languageToUpdate.Name = language.Name;
                languageToUpdate.LanguageCode = language.LanguageCode;
            }

            _unitOfWork.Repository<Language>().Update(languageToUpdate);

            _unitOfWork.Complete();
        }

        public async Task DeleteLanguage(int id)
        {
            var languageToDelete = _unitOfWork.Repository<Language>().GetById(x => x.Id == id).FirstOrDefault();
            _unitOfWork.Repository<Language>().Delete(languageToDelete);

            _unitOfWork.Complete();
        }
        #endregion

        #region Localization Resources

        // localization resource methods implementation here

        public async Task CreateLocalizationResource(LocalizationResourceCreateDto localizationResource)
        {
            var resource = await _unitOfWork.Repository<LocalizationResource>()
                .GetByCondition(x => x.Namespace == localizationResource.Namespace &&
                                     x.Key == localizationResource.Key &&
                                     x.LanguageId == localizationResource.LanguageId)
                .AnyAsync();

            if (resource)
            {
                throw new InvalidOperationException("Localization resource already exists");
            }

            var language = await _unitOfWork.Repository<Language>()
                    .GetByCondition(x => x.Id == localizationResource.LanguageId)
                    .AnyAsync();

            if (!language)
            {
                throw new ArgumentException("Language does not exist", nameof(localizationResource.LanguageId));
            }

            var localizationResourceToCreate = _mapper.Map<LocalizationResource>(localizationResource);
            _unitOfWork.Repository<LocalizationResource>().Create(localizationResourceToCreate);

            _unitOfWork.Complete();
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
            if (!string.IsNullOrEmpty(acceptLanguage))
            {
                var language = _unitOfWork.Repository<Language>().GetByCondition(x => x.LanguageCode == acceptLanguage).FirstOrDefault();
                if (language == null)
                {
                    throw new Exception("Language does not exist");
                }

                Expression<Func<LocalizationResource, bool>> condition =
                    resource => resource.Namespace == namespacePart &&
                                resource.Key == keyPart &&
                                resource.LanguageId == language.Id;

                var resource = _unitOfWork.Repository<LocalizationResource>().GetByCondition(condition);

                return resource.FirstOrDefault();
            }
            else
            {
                Expression<Func<LocalizationResource, bool>> condition =
                    resource => resource.Namespace == namespacePart &&
                                resource.Key == keyPart;

                var resource = _unitOfWork.Repository<LocalizationResource>().GetByCondition(condition);

                return resource.FirstOrDefault();
            }

        }

        public async Task UpdateLocalizationResource(LocalizationResourceCreateDto localizationResource)
        {
            var localizationResourceToUpdate =
                await GetLocalizationResource(localizationResource.Namespace, localizationResource.Key);

            if (localizationResourceToUpdate != null)
            {
                localizationResourceToUpdate.Key = localizationResource.Key;
                localizationResourceToUpdate.Value = localizationResource.Value;
                localizationResourceToUpdate.LanguageId = localizationResource.LanguageId;
                localizationResourceToUpdate.Namespace = localizationResource.Namespace;
                localizationResourceToUpdate.BeautifiedNamespace = localizationResource.BeautifiedNamespace;
            }

            _unitOfWork.Repository<LocalizationResource>().Update(localizationResourceToUpdate);

            _unitOfWork.Complete();
        }

        public async Task DeleteLocalizationResource(int id)
        {
            var localizationResource = _unitOfWork.Repository<LocalizationResource>().GetById(x => x.Id == id).FirstOrDefault();

            _unitOfWork.Repository<LocalizationResource>().Delete(localizationResource);

            _unitOfWork.Complete();
        }

        Task<LanguageDto> ICultureService.GetLanguage(int id)
        {
            throw new NotImplementedException();
        }

        Task<LocalizationResourceDto> ICultureService.GetLocalizationResource(string namespacePart, string keyPart, string acceptLanguage)
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
