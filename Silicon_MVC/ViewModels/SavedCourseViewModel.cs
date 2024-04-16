using Infrastructure.Models;

namespace Silicon_MVC.ViewModels;

public class SavedCourseViewModel
{

    public IEnumerable<UserSavedCourseModel> Courses { get; set; } = [];
    public UserCourseModel UserCourse { get; set; } = new UserCourseModel();
}
