using Microsoft.AspNetCore.Identity;

namespace MultiLanguageExamManagementSystem.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
