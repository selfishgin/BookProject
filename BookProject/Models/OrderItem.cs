using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookProject.Models;

public class OrderItem
{
    [Key]
    public int OrderItemId { get; set; }

    [Required]
    public int OrderId { get; set; } // Not nullable

    public int? BookId { get; set; } // Nullable (if course is selected)

    public int? CourseId { get; set; } // Nullable (if book is selected)

    [Required]
    public int Quantity { get; set; } // Not nullable

    [Required, Column(TypeName = "decimal(6,2)")]
    public decimal Price { get; set; } // Not nullable

    [ForeignKey("OrderId")]
    public Order Order { get; set; }

    [ForeignKey("BookId")]
    public Book? Book { get; set; }

    [ForeignKey("CourseId")]
    public Course? Course { get; set; }
}
