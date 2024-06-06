namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class LanguageCreateDto
{
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public int CountryId { get; set; }
    public bool? IsDefault { get; set; }

}