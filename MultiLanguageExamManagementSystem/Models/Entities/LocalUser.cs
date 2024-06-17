using MultiLanguageExamManagementSystem.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class LocalUser
{
    [Key]
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; } // "Admin" for Professor, "" for Student

    public ICollection<Exam> CreatedExams { get; set; }
    public ICollection<TakenExam> TakenExams { get; set; }
}
