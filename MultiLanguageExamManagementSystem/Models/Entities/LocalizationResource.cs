using System.ComponentModel.DataAnnotations;

namespace MultiLanguageExamManagementSystem.Models.Entities
{
    public class LocalizationResource
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Key { get; set; }
        public string Value { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        
        [MaxLength(200)]
        public string Namespace { get; set; }
        public string BeautifiedNamespace { get; set; }
    }
}
