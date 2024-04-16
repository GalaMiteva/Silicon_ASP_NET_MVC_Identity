

using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class ContactModel
{
    [DataType(DataType.Text)]
    [Display(Name = "Full Name", Prompt = "Enter your full name", Order = 0)]
    [Required(ErrorMessage = "Full name is required")]
    [MinLength(4, ErrorMessage = "Full name must be at least 4 characters long")]
    public string FullName { get; set; } = null!;

    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Enter your email address", Order = 1)]
    [Required(ErrorMessage = "Email is required")]
    [RegularExpression(@"^(?=.*[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,})[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",
    ErrorMessage = "The email format is not valid.")]
    public string Email { get; set; } = null!;

    [Display(Name = "Service (optional)", Prompt = "Choose the service you are interested in", Order = 2)]
    public string? SelectedService { get; set; }

    [Display(Name = "Message", Prompt = "Enter your message here", Order = 3)]
    [Required(ErrorMessage = "Message is required")]
    [MinLength(2, ErrorMessage = "Message must be at least 2 characters long")]
    public string Message { get; set; } = null!;
}
