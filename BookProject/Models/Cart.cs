using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookProject.Models;

public class Cart
{
    [Key]
    public int CartId { get; set; }

    [Required]
    public int UserId { get; set; } // Not nullable

    public int? BookId { get; set; } // Nullable (if course is selected)

    public int? CourseId { get; set; } // Nullable (if book is selected)

    [Required]
    public int Quantity { get; set; } = 1; // Default value

    [ForeignKey("UserId")]
    public User User { get; set; }

    [ForeignKey("BookId")]
    public Book? Book { get; set; }

    [ForeignKey("CourseId")]
    public Course? Course { get; set; }
}
