using System.ComponentModel.DataAnnotations;

namespace BookProject.Models;

public class Order
{
    [Key]
    public int OrderId { get; set; }

    [Required]
    public int UserId { get; set; } // Not nullable

    [Required]
    public decimal TotalAmount { get; set; } // Not nullable

    [Required]
    public DateTime OrderDate { get; set; } = DateTime.Now; // Default value

    public User User { get; set; }
}
