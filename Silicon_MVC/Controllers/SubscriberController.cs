using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Silicon_MVC.ViewModels;

namespace Silicon_MVC.Controllers;

public class SubscriberController : Controller
{
    public IActionResult Index()
    {
        return View(new SubscriberViewModel());
    }


    [HttpPost]

    public async Task<IActionResult> Index(SubscriberViewModel viewModel, ActionExecutedContext context)
    {
        if (ModelState.IsValid)
        {
            try
            {
                using var http = new HttpClient();

                var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
                var apiKey = configuration!.GetValue<string>("ApiKey");
                var url = $"https://localhost:7029/api/subscribers?key={apiKey}&email={viewModel.Email}&circle1={viewModel.Circle1}&cirle2={viewModel.Circle2}&circle3={viewModel.Circle3}&circle4={viewModel.Circle4}&circle5={viewModel.Circle5}&circle6={viewModel.Circle6}&isSubscribed={viewModel.IsSubscribed}";

                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var response = await http.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    ViewData["Status"] = "Success";
                    viewModel.IsSubscribed = true;
                    //return RedirectToAction("Index", "Home");
                }

                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    ViewData["Status"] = "AlreadyExist";
                }


                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ViewData["Status"] = "Unauthorized";
                }

            }

            catch
            {
                ViewData["Status"] = "ConnectionFailed";
            }

        }

        //return View(viewModel);
        return RedirectToAction( "Home", "Default");
    }

}
