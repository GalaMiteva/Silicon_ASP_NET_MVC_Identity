using System.ComponentModel.DataAnnotations;

namespace Silicon_MVC.ViewModels;

public class SubscribeViewModel
{
    [Display(Name = "Daily Newsletter", Order = 0)]
    public bool Circle1 { get; set; }

    [Display(Name = "Event Updates", Order = 1)]
    public bool Circle2 { get; set; }

    [Display(Name = "Advertising Updates", Order = 2)]
    public bool Circle3 { get; set; }

    [Display(Name = "Startups Weekly", Order = 3)]
    public bool Circle4 { get; set; }

    [Display(Name = "Week in Review", Order = 4)]
    public bool Circle5 { get; set; }

    [Display(Name = "Podcasts", Order = 5)]
    public bool Circle6 { get; set; }

    [Display(Prompt = "Enter your email", Order = 6)]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;
    public bool IsSubscribed { get; set; } = false;
}
