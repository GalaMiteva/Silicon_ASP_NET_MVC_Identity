using Microsoft.AspNetCore.Mvc;
using Silicon_MVC.ViewModels;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Silicon_MVC.Controllers;

public class ContactController : Controller
{
    public IActionResult Index()

    {
        ViewData["Title"] = "Contact Us";
        return View(new ContactViewModel());
    }


    [HttpPost]
    public async Task<IActionResult> Index(ContactViewModel viewModel)
    {
        if (viewModel != null && ModelState.IsValid)
        {
            try
            {
                using var http = new HttpClient();
                var apiKey = "dbee8814-f79e-4790-8ac0-8d29775d9545";
                var url = $"https://localhost:7029/api/contact?key={apiKey}";




                var json = JsonSerializer.Serialize(viewModel.Form);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Status"] = "Success";
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    TempData["StatusCode"] = (int)response.StatusCode;
                    TempData["Status"] = "Unauthorized";
                }
                else
                {
                    TempData["StatusCode"] = (int)response.StatusCode;
                    TempData["Status"] = "Error";
                }
            }
            catch (HttpRequestException)
            {
                TempData["Status"] = "ConnectionFailed";
            }
        }
        else
        {
            TempData["Status"] = "InvalidInput";
        }

        return RedirectToAction("Index", "Contact");
    }
}