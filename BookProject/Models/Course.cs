using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookProject.Models;

public class Course
{
    [Key]
    public int CourseId { get; set; }

    // Nullable property for the course picture
    public byte[]? CoursePicturePath { get; set; }

    [Required, StringLength(100)]
    public string CourseName { get; set; } // Not nullable

    [Required, StringLength(100)]
    public string CourseMentor { get; set; } // Not nullable

    // Nullable property for required skills
    [StringLength(255)]
    public string? CourseReqSkill { get; set; }

    [Required]
    public int CourseDuration { get; set; } // Not nullable

    [Required, Column(TypeName = "decimal(6,2)")]
    public decimal CoursePrice { get; set; } // Not nullable
}
