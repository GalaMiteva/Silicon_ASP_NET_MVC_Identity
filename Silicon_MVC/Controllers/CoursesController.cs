using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Silicin_MVC.Services;
using Silicon_MVC.Models;
using Silicon_MVC.Models.Views;
using Silicon_MVC.ViewModels;
using Silicon_MVC.ViewModels.Components;
using System.Text;

namespace Silicon_MVC.Controllers;

[Authorize]
public class CoursesController(CategoryService categoryService, CourseService courseService) : Controller
{
    private readonly CategoryService _categoryService = categoryService;
    private readonly CourseService _courseService = courseService;


    


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

    //public async Task<IActionResult> Details(int id, ActionExecutedContext context)
    {
        //var viewModel = new CoursesIndexModel();

        //var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
        //var apiKey = configuration!.GetValue<string>("ApiKey");
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
                using var http = new HttpClient();

                var json = JsonConvert.SerializeObject(viewModel);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PostAsync($"https://localhost:7029/api/courses", content);



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


}
