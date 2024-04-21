using Infrastructure.Entities;
using Infrastructure.Helpers;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
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
public class AdminController(UserManager<UserEntity> userManager, HttpClient http,IConfiguration configuration, CategoryIdAssigner categoryIdAssigner, IHttpClientFactory httpClientFactory) : Controller
{
    private readonly IConfiguration _configuration = configuration;
    private readonly CategoryIdAssigner _categoryIdAssigner = categoryIdAssigner;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly HttpClient _http = http;
    private readonly UserManager<UserEntity> _userManager = userManager;

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
            var categoryResponse = await _http.GetAsync($"https://localhost:7029/api/Category/{viewModel.Course.Category}?key={_configuration["ApiKey:Secret"]}");

            if (categoryResponse.IsSuccessStatusCode)
            {
                var category = await _categoryIdAssigner.assignIdAsync(categoryResponse);
                viewModel.Course.CategoryId = category.Id;
                viewModel.Course.Category = category.CategoryName;
            }
            
            
            var contentCourse = new StringContent(JsonConvert.SerializeObject(viewModel.Course), Encoding.UTF8, "application/json");
            var response = await _http.PutAsync($"https://localhost:7029/api/Courses?key={_configuration["ApiKey:Secret"]}", contentCourse);
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








    public async Task<IActionResult> Courses(string category = "", string searchQuery = "")
    {
        var viewModel = new AdminCoursesViewModel();

        try
        {
            var responseCategory = await _http.GetAsync($"https://localhost:7029/api/Courses?category={Uri.EscapeDataString(category)}&searchQuery={Uri.EscapeDataString(searchQuery)}&key={_configuration["ApiKey:Secret"]}");
            if (responseCategory.IsSuccessStatusCode)
            {
                var jsonStrings = await responseCategory.Content.ReadAsStringAsync();
                var courses = JsonConvert.DeserializeObject<CourseResultModel>(jsonStrings);
                if (courses != null)
                {
                    viewModel.Courses = courses.Courses!;
                    return View(viewModel);
                }
            }

            var contentCourse = new StringContent(JsonConvert.SerializeObject(viewModel.Course), Encoding.UTF8, "application/json");
            var response = await _http.PutAsync($"https://localhost:7029/api/Courses?key={_configuration["ApiKey:Secret"]}", contentCourse);
            if (response.IsSuccessStatusCode)
            {
                ViewData["Status"] = "Success"; ;
            }
            else
            {
                ViewData["Status"] = "ConnectionFailed";
                ViewData["StatusCode"] = (int)response.StatusCode;
            }
        }
        catch
        {
            ViewData["Status"] = "ConnectionFailed";
        }
        return View();


    }






    //public async Task<IActionResult> Delete(AdminCoursesViewModel viewModel)
    //{
    //    //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

    //    var Id = viewModel.Course.Id;
    //    var cancellationTokenSource = new CancellationTokenSource();
    //    var cancellationToken = cancellationTokenSource.Token;

    //    var response = await _httpClient.DeleteAsync($"https://localhost:7029/api/Courses/{Id}?key={_configuration["ApiKey:Secret"]}", cancellationToken);
    //    if (response.IsSuccessStatusCode)
    //    {
    //        ViewData["CourseStatus"] = "Course deleted succesfully";
    //        return RedirectToAction("Courses");
    //    }
    //    else
    //    {
    //        ViewData["CourseStatus"] = response.StatusCode;
    //        return RedirectToAction("Courses");
    //    }
    //}

    [HttpPost]
    public async Task<IActionResult> Delete(int courseId)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user!.Id;



            var url = $"https://localhost:7029/api/UserCourses/{userId}/{courseId}?key={_configuration["ApiKey:Secret"]}";


            var response = await _http.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Course removed successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to remove course.";
            }

            return RedirectToAction(nameof(Courses));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            TempData["ErrorMessage"] = "Failed to remove course. Please try again later.";
            return RedirectToAction(nameof(Courses));
        }
    }

}
