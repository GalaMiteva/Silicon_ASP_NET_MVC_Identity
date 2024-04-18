using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Silicon_MVC.ViewModels;
using static System.Net.WebRequestMethods;

namespace Silicon_MVC.Controllers;

public class SubscribersController(HttpClient http, IConfiguration configuration) : Controller
{

    private readonly IConfiguration _configuration = configuration;
    private readonly HttpClient _http = http;
    public IActionResult Index()
    {
        return View(new SubscriberViewModel());
    }


    [HttpPost]
    public async Task<IActionResult> Index(SubscriberViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                
                var uri = $"https://localhost:7029/api/subscribers?key={_configuration["ApiKey:Secret"]}&email={viewModel.Email}&circle1={viewModel.Circle1}&cirle2={viewModel.Circle2}&circle3={viewModel.Circle3}&circle4={viewModel.Circle4}&circle5={viewModel.Circle5}&circle6={viewModel.Circle6}&isSubscribed={viewModel.IsSubscribed}";


                var request = new HttpRequestMessage(HttpMethod.Post, uri);
                var response = await _http.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Status"] = "Success";
                    viewModel.IsSubscribed = true;
                }

                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["Status"] = "AlreadyExist";
                }


                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    TempData["Status"] = "Unauthorized";
                }

            }

            catch
            {
                TempData["Status"] = "ConnectionFailed";
            }

        }
        return RedirectToAction("Index", "Home", "dontWant");
    }
}
