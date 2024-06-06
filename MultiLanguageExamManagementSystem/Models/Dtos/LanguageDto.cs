namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class LanguageDto
{
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public bool? IsDefault { get; set; }
    public bool? IsSelected { get; set; }
}