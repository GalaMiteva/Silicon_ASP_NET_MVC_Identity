







using Infrastructure.Models;




public class CoursesIndexViewModel
{
    // public IEnumerable<Models.CourseModel> Courses { get; set; } = [];

    public IEnumerable<CourseModel> Courses { get; set; } = [];
    public IEnumerable<CategoryModel> Categories { get; set; } = null!;
    public Pagination? Pagination { get; set; }
}
