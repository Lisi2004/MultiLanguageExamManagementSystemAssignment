using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MultiLanguageExamManagementSystem.Models.Entities;
public class Exam
{
    [Key]
    public int ExamId { get; set; }
    public string Title { get; set; }
    public int CreatorId { get; set; }
    [ForeignKey("CreatorId")]
    public LocalUser Creator { get; set; }

    public ICollection<ExamQuestion> ExamQuestions { get; set; }
    public ICollection<TakenExam> TakenExams { get; set; }
}
