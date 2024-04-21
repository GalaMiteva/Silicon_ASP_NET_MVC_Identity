using Infrastructure.Helpers;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Silicon_MVC.Models;
using Silicon_MVC.Models.Views;
using Silicon_MVC.ViewModels.Admin;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Silicon_MVC.Controllers;

[Authorize(Policy = "Admins")]
public class AdminController(HttpClient http,IConfiguration configuration, HttpClient httpClient, CategoryIdAssigner categoryIdAssigner, IHttpClientFactory httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;
    private readonly CategoryIdAssigner _categoryIdAssigner = categoryIdAssigner;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly HttpClient _http = http;


    public IActionResult Index()
    {
        return View();
    }


    [Authorize(Policy = "CIO")]

    public IActionResult Settings()
    {
        return View();
    }


    public async Task<IActionResult> Create(CourseRegistrationFormViewModel viewModel)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(viewModel);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync($"https://localhost:7029/api/courses?key={_configuration["ApiKey:Secret"]}", content);

                if (response.IsSuccessStatusCode)
                {
                    ViewData["Status"] = "Success";
                }
                else
                {
                    ViewData["Status"] = "ConnectionFailed";
                    ViewData["StatusCode"] = (int)response.StatusCode;
                }
            }
        }
        catch
        {
            ViewData["Status"] = "ConnectionFailed";
        }

        return View();
    }





    //[HttpPost("/courses/update")]
    public async Task<IActionResult> Update(AdminCoursesViewModel viewModel)
    {

      try
      {
            
                var json = JsonConvert.SerializeObject(viewModel);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                //var response = await _http.PostAsync($"https://localhost:7029/api/courses?key={_configuration["ApiKey:Secret"]}", content);
                var categoryResponse = await _httpClient.GetAsync($"https://localhost:7029/api/Category/{viewModel.Course.Category}?key={_configuration["ApiKey:Secret"]}");
                
                if (categoryResponse.IsSuccessStatusCode)
                {
                    var category = await _categoryIdAssigner.assignIdAsync(categoryResponse);
                       viewModel.Course.CategoryId = category.Id;
                       viewModel.Course.Category = category.CategoryName;
                }
                var contentCourse = new StringContent(JsonConvert.SerializeObject(viewModel.Course), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"https://localhost:7029/api/Courses?key={_configuration["ApiKey:Secret"]}", contentCourse);
                if (response.IsSuccessStatusCode)
                {
                    ViewData["CourseStatus"] = "Course updated succesfully";
                    return RedirectToAction("Courses");
                }
                else
                {
                    ViewData["CourseStatus"] = response.StatusCode;
                    return RedirectToAction("Courses");
                }
            
        }
        catch
        {
            ViewData["Status"] = "ConnectionFailed";
        }

        return View();
    }







    [HttpGet("/admin/courses")]
    public async Task<IActionResult> Courses(string category = "", string searchQuery = "")
    {
        var viewModel = new AdminCoursesViewModel();

        var categoriesResponse = await _httpClient.GetAsync($"https://localhost:7029/api/Category?key={_configuration["ApiKey:Secret"]}");
        if (categoriesResponse.IsSuccessStatusCode)
        {
            var jsonStrings = await categoriesResponse.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<IEnumerable<CategoryModel>>(jsonStrings);
            if (categories != null)
            {
                viewModel.Categories = categories!;
            }
        }

        var response = await _httpClient.GetAsync($"https://localhost:7029/api/Courses?category={Uri.EscapeDataString(category)}&searchQuery={Uri.EscapeDataString(searchQuery)}&key={_configuration["ApiKey:Secret"]}");
        if (response.IsSuccessStatusCode)
        {
            var jsonStrings = await response.Content.ReadAsStringAsync();
            var courses = JsonConvert.DeserializeObject<CourseResultModel>(jsonStrings);
            if (courses != null)
            {
                viewModel.Courses = courses.Courses!;
                return View(viewModel);
            }
        }

        ViewData["CourseStatus"] = response.StatusCode;
        return View(viewModel);
    }




    [HttpPost("/admin/courses/delete")]
    public async Task<IActionResult> Delete(AdminCoursesViewModel viewModel)
    {
        //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

        var Id = viewModel.Course.Id;
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        var response = await _httpClient.DeleteAsync($"https://localhost:7029/api/Courses/{Id}?key={_configuration["ApiKey:Secret"]}", cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            ViewData["CourseStatus"] = "Course deleted succesfully";
            return RedirectToAction("Courses");
        }
        else
        {
            ViewData["CourseStatus"] = response.StatusCode;
            return RedirectToAction("Courses");
        }
    }



}
