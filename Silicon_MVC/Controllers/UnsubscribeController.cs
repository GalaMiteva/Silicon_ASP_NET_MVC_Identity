using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Silicon_MVC.ViewModels;

namespace Silicon_MVC.Controllers;

public class UnsubscribeController : Controller
{
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
                using var http = new HttpClient();
                var apiKey = "dbee8814-f79e-4790-8ac0-8d29775d9545";
                var url = $"https://localhost:7029/api/subscribers?key={apiKey}&email={viewModel.Email}";
                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                var response = await http.SendAsync(request);

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
