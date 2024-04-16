

using Silicon_MVC.Models.Views;

namespace Infrastructure.Models;

public class CourseResultModel
{
    public bool Succeeded { get; set; }
    public int TotlaItems { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<CourseModel>? Courses { get; set; }
}
