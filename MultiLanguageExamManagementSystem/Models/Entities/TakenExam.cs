using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MultiLanguageExamManagementSystem.Models.Entities;
public class TakenExam
{
    [Key]
    public int TakenExamId { get; set; }
    public int ExamId { get; set; }
    [ForeignKey("ExamId")]
    public Exam Exam { get; set; }

    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }

    public bool IsCompleted { get; set; }
}
