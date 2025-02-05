using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookProject.Models;

public class Book
{
    public int BookId { get; set; }
    public byte[]? BookPicturePath { get; set; } // Nullable byte array for varbinary(max)


    [Required]
    [StringLength(100)]
    public string BookName { get; set; } // Not nullable

    [Required]
    [StringLength(100)]
    public string BookAuthor { get; set; } // Not nullable

    [StringLength(100)]
    public string BookGenre { get; set; } // Nullable

    [StringLength(255)]
    public string BookDesc { get; set; } // Nullable

    [Required]
    public int BookPages { get; set; } // Not nullable

    [Required]
    public decimal BookPrice { get; set; } // Not nullable

    public int? BookReadCount { get; set; } // Nullable
}
