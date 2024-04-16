using Infrastructure.Models;

namespace Silicon_MVC.ViewModels;

public class ContactViewModel
{
    public string Title { get; set; } = "Contact Us";
    public ContactModel Form { get; set; } = new ContactModel();
}
