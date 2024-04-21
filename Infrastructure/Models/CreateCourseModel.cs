
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class CreateCourseModel
{
    [Required]
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public int Hours { get; set; }
    public bool IsBestseller { get; set; }
    public decimal LikesInNumbers { get; set; }
    public decimal LikesInProcent { get; set; }
    public string? Author { get; set; }
    public string? Img { get; set; }

    public string? Category { get; set; } = null!;
    public int? CategoryId { get; set; }
}
