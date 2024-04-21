using Infrastructure.Models;
using Silicon_MVC.Models.Views;

namespace Silicon_MVC.ViewModels.Admin;

public class AdminCoursesViewModel
{
    public IEnumerable<CourseModel> Courses { get; set; } = [];
    public CourseModel Course { get; set; } = new();
    public CreateCourseModel CreateCourse { get; set; } = new();
    public IEnumerable<CategoryModel> Categories { get; set; } = [];

}
