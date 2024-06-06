using System.ComponentModel.DataAnnotations;

namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class LocalizationResourceDto
{
    public string Value { get; set; }
    [MaxLength(200)]
    public string BeautifiedNamespace { get; set; }
}