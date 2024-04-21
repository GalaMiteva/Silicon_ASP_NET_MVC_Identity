using Infrastructure.Entities;
using Infrastructure.Helpers;
using Infrastructure.Migrations;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Silicin_MVC.Services;
using Silicon_MVC.Models;
using Silicon_MVC.Models.Views;
using Silicon_MVC.ViewModels;
using Silicon_MVC.ViewModels.Admin;
using Silicon_MVC.ViewModels.Components;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Silicon_MVC.Controllers;



[Authorize]
public class CoursesController(CategoryIdAssigner categoryIdAssigner, UserManager<UserEntity> userManager, HttpClient http,CategoryService categoryService, CourseService courseService, IConfiguration configuration) : Controller
{


    private readonly CategoryService _categoryService = categoryService;
    private readonly CourseService _courseService = courseService;
    private readonly IConfiguration _configuration = configuration;
    private readonly HttpClient _http = http;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly CategoryIdAssigner _categoryIdAssigner = categoryIdAssigner;



    public async Task<IActionResult> Index(string category = "", string searchQuery = "", int pageNumber = 1, int pageSize = 6)
    {
        try
        {
            var coursesResult = await _courseService.GetCoursesAsync(category, searchQuery, pageNumber, pageSize);
            var viewModel = new CoursesIndexViewModel
            {
                Categories = await _categoryService.GetCategoriesAsync(),
                Courses = coursesResult.Courses!,
                Pagination = new Pagination
                {
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    TotalPages = coursesResult.TotalPages,
                    TotalItems = coursesResult.TotlaItems
                }
            };

            return View(viewModel);
        }
        catch (Exception)
        {
            ViewData["Status"] = "ConnectionFailed";

            var viewModel = new CoursesIndexViewModel
            {
            };

            return View(viewModel);
        }
    }

    
    public async Task<IActionResult> Details(int id)
    {

        try
        {
            var course = await _courseService.GetCourseByIdAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
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

}
