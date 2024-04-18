using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Silicon_MVC.ViewModels;
using static System.Net.WebRequestMethods;

namespace Silicon_MVC.Controllers;

public class UnsubscribeController(HttpClient http, IConfiguration configuration) : Controller
{

    private readonly IConfiguration _configuration = configuration;
    private readonly HttpClient _http = http;
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(UnsubscriberViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var uri = $"https://localhost:7029/api/subscribers?key={_configuration["ApiKey:Secret"]}&email={viewModel.Email}";
                var request = new HttpRequestMessage(HttpMethod.Delete, uri);
                var response = await _http.SendAsync(request);

                ViewData["StatusCode"] = (int)response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    ViewData["Status"] = "Success";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ViewData["Status"] = "NotFound";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ViewData["Status"] = "Unauthorized";
                }
                else
                {
                    ViewData["Status"] = "ServerSent";
                }
            }
            catch
            {
                ViewData["Status"] = "ConnectionFailed";
            }
        }

        return View(viewModel);
       
    }
}
