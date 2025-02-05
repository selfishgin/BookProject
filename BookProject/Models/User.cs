using System.ComponentModel.DataAnnotations;

namespace BookProject.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    // Nullable property for the profile picture
    public byte[]? UserProfilePicturePath { get; set; }

    [Required, StringLength(100)]
    public string UserFirstname { get; set; } // Not nullable

    [Required, StringLength(100)]
    public string UserLastname { get; set; } // Not nullable

    [Required]
    public int UserAge { get; set; } // Not nullable

    // Nullable property for the user's speciality
    [StringLength(255)]
    public string? UserSpeciality { get; set; }
}
