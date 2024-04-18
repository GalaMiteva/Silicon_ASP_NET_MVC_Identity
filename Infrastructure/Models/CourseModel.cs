namespace Silicon_MVC.Models.Views;

public class CourseModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public int Hours { get; set; }
    public bool IsBestseller { get; set; }
    public decimal LikesInNumbers { get; set; }
    public decimal LikesInProcent { get; set; }
    public string? Author { get; set; }
    public string? Img { get; set; }
    //public int CategoryId { get; set; } 
    public string Category { get; set; } = null!;
}
