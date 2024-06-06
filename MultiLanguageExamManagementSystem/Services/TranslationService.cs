using System.Globalization;
using AutoMapper;
using Google.Cloud.Translation.V2;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;
using MultiLanguageExamManagementSystem.Services.IServices;
using Language = MultiLanguageExamManagementSystem.Models.Entities.Language;

namespace MultiLanguageExamManagementSystem.Services
{
    public class TranslationService : ITranslationService
    {

        // Your code here

        // You can make it static, add interface or whatever u want

        //https://www.deepl.com/pro-api?cta=header-pro-api - check the documentation
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TranslationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<LocalizationResourceDto>> GetAllLocalizationResourcesEnglish()
        {
            var localizationResources = await _unitOfWork.Repository<LocalizationResource>()
                .GetAll()
                .Where(x => x.LanguageId == 1)
                .ToListAsync();

            return _mapper.Map<List<LocalizationResourceDto>>(localizationResources);
        }

        public async Task<List<LocalizationResourceCreateDto>> TranslateResources(string targetLanguage)
        {
            var resourcesDto = await GetAllLocalizationResourcesEnglish();
            var resources = _mapper.Map<List<LocalizationResource>>(resourcesDto);

            var language = await _unitOfWork.Repository<Language>()
                .GetByCondition(x => x.LanguageCode == targetLanguage)
                .FirstOrDefaultAsync();

            var translatedResources = new List<LocalizationResourceCreateDto>();

            //Replace it with a valid key
            /*var authKey = "f63c02c5-f056-...";
            var translator = new Translator(authKey);*/
            var client = TranslationClient.Create();


            foreach (var localizationResource in resources)
            {
                try
                {
                    var translatedValue = client.TranslateText(
                        localizationResource.Value,
                        targetLanguage,
                        LanguageCodes.English);

                    var newLocalizationResource = new LocalizationResourceCreateDto()
                    {
                        Key = localizationResource.Key,
                        BeautifiedNamespace = localizationResource.BeautifiedNamespace,
                        Value = translatedValue.TranslatedText,
                        LanguageId = language.Id,
                        Namespace = localizationResource.Namespace
                    };

                    translatedResources.Add(newLocalizationResource);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error translating resource");
                }
            }

            var translatedResourcesToCreate = _mapper.Map<List<LocalizationResource>>(translatedResources);
            _unitOfWork.Repository<LocalizationResource>().CreateRange(translatedResourcesToCreate);
            _unitOfWork.Complete();

            return translatedResources;
        }
    }
}
