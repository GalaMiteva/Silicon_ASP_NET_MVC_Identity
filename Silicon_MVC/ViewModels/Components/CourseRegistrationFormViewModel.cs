using System.ComponentModel.DataAnnotations;

namespace Silicon_MVC.ViewModels.Components;

public class CourseRegistrationFormViewModel
{
    [Required]
    [Display(Name = "Title")]
    public string Title { get; set; } = null!;
    [Display(Name = "Price")]
    public decimal Price { get; set; }
    [Display(Name = "Discount Price")]
    public decimal DiscountPrice { get; set; }
    [Display(Name = "Hours")]
    public int Hours { get; set; }
    [Display(Name = "Is Bestseller")]


    public bool IsBestseller = false;
    [Display(Name = "Likes In Numbers")]
    public decimal LikesInNumbers { get; set; }
    [Display(Name = "Likes In Procent")]
    public decimal LikesInProcent { get; set; }
    [Display(Name = "Author")]
    public string? Author { get; set; }
    [Display(Name = "Image Url")]
    public string? Img { get; set; }
    [Display(Name = "Category")]
    public string? Category { get; set; }
}
