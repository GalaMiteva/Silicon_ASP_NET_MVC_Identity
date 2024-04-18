

using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Silicon_MVC.Models.Views;
using System.Text;

namespace Infrastructure.Services;

public class CourseService(HttpClient http, IConfiguration configuration)
{
    private readonly HttpClient _http = http;
    private readonly IConfiguration _configuration = configuration;

    public async Task<CourseResultModel> GetCoursesAsync(string category = "", string searchQuery = "", int pageNumber = 1, int pageSize = 10)
    {
        var response = await _http.GetAsync($"{_configuration["ApiUris:Courses"]}?key=dbee8814-f79e-4790-8ac0-8d29775d9545&category={Uri.UnescapeDataString(category)}&searchQuery={Uri.UnescapeDataString(searchQuery)}&pageNumber={pageNumber}&pageSize={pageSize}");


        if (response.IsSuccessStatusCode)
        {
            var result = JsonConvert.DeserializeObject<CourseResultModel>(await response.Content.ReadAsStringAsync());
            if (result != null && result.Succeeded)
                return result;

        }

        return null!;

    }

    public async Task<CourseModel> GetCourseByIdAsync(int courseId)
    {
        var response = await _http.GetAsync($"{_configuration["ApiUris:Courses"]}/{courseId}?key=dbee8814-f79e-4790-8ac0-8d29775d9545");

        if (response.IsSuccessStatusCode)
        {
            var course = JsonConvert.DeserializeObject<CourseModel>(await response.Content.ReadAsStringAsync());
            return course!;
        }

        return null!;
    }


    public async Task<HttpResponseMessage> CreateCourseAsync(CourseModel course)
    {

        var jsonCourse = JsonConvert.SerializeObject(course);

        var content = new StringContent(jsonCourse, Encoding.UTF8, "application/json");


        var response = await _http.PostAsync(_configuration["ApiUris:Courses"], content);


        return response;
    }


    public async Task<HttpResponseMessage> AddCourseToSavedAsync(string userId, int courseId)
    {
        try
        {

            var userCourseModel = new UserCourseModel
            {
                UserId = userId,
                CourseId = courseId
            };


            var jsonUserCourse = JsonConvert.SerializeObject(userCourseModel);


            var content = new StringContent(jsonUserCourse, Encoding.UTF8, "application/json");


            var response = await _http.PostAsync(_configuration["ApiUris:UserCourses"], content);


            return response;
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<List<UserSavedCourseModel>> GetSavedCoursesAsync(string userId)
    {
        try
        {

            var response = await _http.GetAsync($"{_configuration["ApiUris:UserCourses"]}?userId={userId}");

            response.EnsureSuccessStatusCode();

            var savedCoursesJson = await response.Content.ReadAsStringAsync();
            var savedCourses = JsonConvert.DeserializeObject<List<UserSavedCourseModel>>(savedCoursesJson);

            return savedCourses!;
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
}
