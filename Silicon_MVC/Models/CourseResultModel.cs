using Silicon_MVC.Models;

namespace Silicon_MVC.Models;

public class CourseResultModel
{
    public bool Succeeded { get; set; }
    public int TotlaItems { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<CourseModel> ? Courses { get; set; }
}
