using System.ComponentModel.DataAnnotations;

namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class LocalizationResourceCreateDto
{
    public string Key { get; set; }
    public string Value { get; set; }
    public int LanguageId { get; set; }
    [MaxLength(200)]
    public string Namespace { get; set; }
    public string BeautifiedNamespace { get; set; }
}