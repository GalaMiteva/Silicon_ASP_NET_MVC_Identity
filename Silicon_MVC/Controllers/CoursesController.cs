using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Silicon_MVC.Models;
using Silicon_MVC.ViewModels;

namespace Silicon_MVC.Controllers;

[Authorize]
public class CoursesController : Controller
{
    private readonly HttpClient _http;

    public CoursesController(HttpClient http)
    {
        _http = http;
    }


    
    public async Task<IActionResult> Index()
    //public async Task<IActionResult> Index(ActionExecutedContext context)
    {
        var viewModel = new CoursesIndexModel();
        var apiKey = "dbee8814-f79e-4790-8ac0-8d29775d9545";

        //var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
        //var apiKey = configuration!.GetValue<string>("ApiKey");


        var url = $"https://localhost:7029/api/courses?key={apiKey}";



        try
        {
            

            
            var response = await _http.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                viewModel.Courses = JsonConvert.DeserializeObject<IEnumerable<CourseModel>>(await response.Content.ReadAsStringAsync())!;
            }
            else
            {
                ViewData["Status"] = "ConnectionFailed";
            }
        }
        catch
        {
            ViewData["Status"] = "ConnectionFailed";
        }

        return View(viewModel);
    }


    public async Task<IActionResult> Details(int id)

    //public async Task<IActionResult> Details(int id, ActionExecutedContext context)
    {
        var viewModel = new CoursesIndexModel();

        //var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
        //var apiKey = configuration!.GetValue<string>("ApiKey");
       var apiKey = "dbee8814-f79e-4790-8ac0-8d29775d9545";

        var url = $"https://localhost:7029/api/courses/{id}?key={apiKey}";

        try
        {
            
            var response = await _http.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var course = JsonConvert.DeserializeObject<CourseModel>(await response.Content.ReadAsStringAsync())!;
                return View(course);
            }
            else
            {
                ViewData["Status"] = "ConnectionFailed";
            }
        }
        catch
        {
            ViewData["Status"] = "ConnectionFailed";
        }

        return View(viewModel);
    }
}
