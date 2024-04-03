using System.ComponentModel.DataAnnotations;

namespace Silicon_MVC.ViewModels;

public class UnsubscriberViewModel
{



    [Display(Prompt = "Enter your email", Order = 1)]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Email is required")]
    [RegularExpression(@"^(?=.*[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,})[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",
    ErrorMessage = "The email format is not valid.")]


    //[Display(Prompt = "Enter your email", Order = 1)]
    //[DataType(DataType.EmailAddress)]
    //[Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;
}
