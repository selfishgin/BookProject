namespace BookProject.Models;

public class CartItem
{
    public int ItemId { get; set; } // BookId or CourseId
    public string ItemName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ItemType { get; set; } // "Book" or "Course"
}
