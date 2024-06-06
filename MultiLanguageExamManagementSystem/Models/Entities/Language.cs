namespace MultiLanguageExamManagementSystem.Models.Entities
{
    public class Language
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public List<LocalizationResource> LocalizationResources { get; set; }

        // Language will have Id, Name, LanguageCode, CountryId (IsDefault and IsSelected are optional properties)

    }
}
