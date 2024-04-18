using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Silicon_MVC.ViewModels;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Silicon_MVC.Controllers;

public class ContactController (HttpClient http, IConfiguration configuration) : Controller
{

    private readonly IConfiguration _configuration = configuration;
    private readonly HttpClient _http = http;
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
                
                
                var uri = $"https://localhost:7029/api/contact?key={_configuration["ApiKey:Secret"]}";



                var json = JsonSerializer.Serialize(viewModel.Form);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(uri, content);

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