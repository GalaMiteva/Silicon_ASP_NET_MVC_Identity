using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using Silicon_MVC.ViewModels;
using Silicon_MVC.ViewModels.Account;

using System.Diagnostics;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace Silicon_MVC.Controllers;

[Authorize]
public class AccountController(UserManager<UserEntity> userManager,AddressManager addressManager, CategoryService categoryService, CourseService courseService, IConfiguration configuration) : Controller
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly AddressManager _addressManager = addressManager; 
    private readonly CategoryService _categoryService = categoryService;
    private readonly CourseService _courseService = courseService;
    private readonly IConfiguration _configuration = configuration;
    
    private readonly HttpClient _http = new HttpClient();
    



    #region Details
    [HttpGet]
    [Route("/account/details")]
    public async Task<IActionResult> Details()
    {
        var claims = HttpContext.User.Identities.FirstOrDefault();
        var viewModel = new AccountDetailsViewModel
        {
            ProfileInfo = await PopulateProfileInfoAsync()
        };
        viewModel.BasicInfoForm ??= await PopulateBasicInfoAsync();
        viewModel.AddressInfoForm ??= await PopulateAddressInfoAsync();

        return View(viewModel);
    }
    #endregion


    #region [HttpPost] Details
    [HttpPost]
    [Route("/account/details")]
    public async Task<IActionResult> Details(AccountDetailsViewModel viewModel)
    {
        if (viewModel.BasicInfoForm != null)
        {
            if (viewModel.BasicInfoForm.FirstName != null && viewModel.BasicInfoForm.LastName != null && viewModel.BasicInfoForm.Email != null)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    user.FirstName = viewModel.BasicInfoForm.FirstName;
                    user.LastName = viewModel.BasicInfoForm.LastName;
                    user.Email = viewModel.BasicInfoForm.Email;
                    user.PhoneNumber = viewModel.BasicInfoForm.PhoneNumber;
                    user.Bio = viewModel.BasicInfoForm.Biography;

                    var result = await _userManager.UpdateAsync(user);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("IncorrectValues", "Something went wrong! Unable to save data!");
                        ViewData["StatusMessage"] = "danger|Unable to save basic information";
                    }
                    else
                    {
                        ViewData["StatusMessage"] = "success|Basic info was saved successfully";
                    }
                }
            }
        }

        if (viewModel.AddressInfoForm != null)
        {
            if (viewModel.AddressInfoForm.Addressline_1 != null && viewModel.AddressInfoForm.PostalCode != null && viewModel.AddressInfoForm.City != null)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    var address = await _addressManager.GetAddressAsync(user.Id);

                    if (address != null)
                    {
                        address.Addressline_1 = viewModel.AddressInfoForm.Addressline_1;
                        address.Addressline_2 = viewModel.AddressInfoForm.Addressline_2;
                        address.PostalCode = viewModel.AddressInfoForm.PostalCode;
                        address.City = viewModel.AddressInfoForm.City;

                        var result = await _addressManager.UpdateAddressAsync(address);


                        if (!result)
                        {
                            ModelState.AddModelError("IncorrectValues", "Something went wrong! Unable to save data!");
                            ViewData["StatusMessage"] = "danger|Unable to save address information";
                        }
                        else
                        {
                            ViewData["StatusMessage"] = "success|Address info was saved successfully";
                        }

                    }

                    else
                    {
                        address = new AddressEntity
                        {
                            UserId = user.Id,
                            Addressline_1 = viewModel.AddressInfoForm.Addressline_1,
                            Addressline_2 = viewModel.AddressInfoForm.Addressline_2,
                            PostalCode = viewModel.AddressInfoForm.PostalCode,
                            City = viewModel.AddressInfoForm.City,
                        };

                        var result = await _addressManager.CreateAddressAsync(address);
                        if (!result)


                        {
                            ModelState.AddModelError("IncorrectValues", "Something went wrong! Unable to save data!");
                            ViewData["StatusMessage"] = "danger|Unable to save address information";
                        }
                        else
                        {
                            ViewData["StatusMessage"] = "success|Address info was saved successfully";
                        }


                    }
                }
            }
        }

        viewModel.ProfileInfo = await PopulateProfileInfoAsync();
        viewModel.BasicInfoForm ??= await PopulateBasicInfoAsync();
        viewModel.AddressInfoForm ??= await PopulateAddressInfoAsync();



        return View(viewModel);
    }
    #endregion


    private async Task<ProfileInfoViewModel> PopulateProfileInfoAsync()
    {
        var user = await _userManager.GetUserAsync(User);

        return new ProfileInfoViewModel()
        {
            FirstName = user!.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            IsExternalAccount = user.IsExternalAccount,
        };
    }

    private async Task<BasicInfoFormViewModel> PopulateBasicInfoAsync()
    {
        var user = await _userManager.GetUserAsync(User);

       if(user != null)
        {
            return new BasicInfoFormViewModel()
            {
                UserId = user!.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                Biography = user.Bio,
            };
        }
       return null!;

    }

    private async Task<AddressInfoFormViewModel> PopulateAddressInfoAsync()
    {

        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            var address = await _addressManager.GetAddressAsync(user.Id);
            if (address != null)
            {
                return new AddressInfoFormViewModel()
                {

                    Addressline_1 = address.Addressline_1,
                    Addressline_2 = address.Addressline_2,
                    PostalCode = address.PostalCode,
                    City = address.City,
                };
            }
        }

        return new AddressInfoFormViewModel();

    }

    #region Security

    [HttpGet]
    [Route("/account/security")]
    public async Task<IActionResult> Security()
    {
        var viewModel = new AccountSecurityViewModel
        {
            PassForm = await PopulateSecurityPassFormAsync()
        };

        viewModel.BasicInfoForm ??= await PopulateBasicInfoAsync();
        viewModel.ProfileInfo ??= await PopulateProfileInfoAsync();
        return View(viewModel);
    }

    [HttpPost]
    [Route("/account/security")]
    public async Task<IActionResult> Security(AccountSecurityViewModel viewModel)
    {
        if (viewModel.PassForm != null)
        {
            if (viewModel.PassForm.CurrentPassword != null &&
                viewModel.PassForm.NewPassword != null &&
                viewModel.PassForm.ConfirmNewPassword != null)
            {
                if (viewModel.PassForm.NewPassword == viewModel.PassForm.ConfirmNewPassword)
                {
                    var user = await _userManager.GetUserAsync(User);

                    if (user != null)
                    {
                        var result = await _userManager.ChangePasswordAsync(user, viewModel.PassForm.CurrentPassword, viewModel.PassForm.NewPassword);

                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("IncorrectValues", "Failed to update user information");
                            ViewData["StatusMessage"] = "danger|Failed to update password";
                        }
                        else
                        {
                            ViewData["StatusMessage"] = "success|Password updated successfully!";
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("PasswordMismatch", "New password and confirm password do not match.");

                    ViewData["ErrorMessage"] = "New password and confirm password do not match.";
                }
            }
        }

        viewModel.BasicInfoForm = await PopulateBasicInfoAsync();
        viewModel.ProfileInfo = await PopulateProfileInfoAsync();
        viewModel.PassForm = await PopulateSecurityPassFormAsync();

        return View(viewModel);
    }


    private async Task<PassFormViewModel> PopulateSecurityPassFormAsync()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user != null)
        {
            return new PassFormViewModel()
            {
                UserId = user!.Id,
                ConfirmNewPassword = user.PasswordHash!,
                CurrentPassword = user.PasswordHash!,
                NewPassword = user.PasswordHash!,
            };
        }

        return null!;

    }






    [HttpPost]
    [Route("/account/delete")]
    


    public async Task<IActionResult> DeleteAccount(AccountSecurityViewModel viewModel)
    {
        if (ModelState.IsValid || viewModel.DeleteForm.IsDeleted)

        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    await HttpContext.SignOutAsync();
                    TempData["SuccessMessage"] = "Account deleted successfully!";
                    await Task.Delay(1000);
                    return RedirectToAction("Index", "Subscribers");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete the account";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "User not found";
            }
        }
        else
        {
            if (!viewModel.DeleteForm.IsDeleted)
            {
                ModelState.AddModelError("SecurityDeleteForm.IsDeleted", "Please check the box before deleting the account.");
            }
            return View("Security", viewModel);
        }

        return View("Security", viewModel);
    }


    #endregion

    //[HttpPost]

    //public async Task<IActionResult> UploadImage(IFormFile file)
    //{
    //    var result = await _accountManager.UploadUserProfilImageAsync(User, file);
    //    return RedirectToAction("Details", "Account");
    //}
    
    



    public async Task<IActionResult> GetSavedCourses()
    {
        try
        {
            
            var user = await _userManager.GetUserAsync(User);
            var userId = user!.Id;
            //var apiKey = "dbee8814-f79e-4790-8ac0-8d29775d9545";

            //var uri = $"https://localhost:7029/api/UserCourses/{userId}?key={apiKey}";
            var url = $"https://localhost:7029/api/UserCourses/{userId}?key=dbee8814-f79e-4790-8ac0-8d29775d9545";



            var response = await _http.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var coursesResultJson = await response.Content.ReadAsStringAsync();
            var coursesResult = JsonConvert.DeserializeObject<List<UserSavedCourseModel>>(coursesResultJson);

            var viewModel = new SavedCourseViewModel
            {
                Courses = coursesResult!
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            var statusCode = ex is HttpRequestException httpEx && httpEx.StatusCode.HasValue ?
                ((int)httpEx.StatusCode.Value).ToString() :
                "Unknown"; 

            TempData["StatusCode"] = statusCode;

            var viewModel = new SavedCourseViewModel();
            return View(viewModel);
        }
    }





    [HttpPost]
    public async Task<IActionResult> DeleteSavedCourse(int courseId)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user!.Id;
            //var apiKey = "dbee8814-f79e-4790-8ac0-8d29775d9545";


            //var uri = $"https://localhost:7029/api/UserCourses/{userId}/{courseId}?key={apiKey}";
            var uri = $"https://localhost:7029/api/UserCourses/{userId}/{courseId}?key=dbee8814-f79e-4790-8ac0-8d29775d9545";

            var response = await _http.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Course removed successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to remove course.";
            }

            return RedirectToAction(nameof(GetSavedCourses));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            TempData["ErrorMessage"] = "Failed to remove course. Please try again later.";
            return RedirectToAction(nameof(GetSavedCourses));
        }
    }


    [HttpPost]
    public async Task<IActionResult> DeleteAllSavedCourses()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user!.Id;
            //var apiKey = "dbee8814-f79e-4790-8ac0-8d29775d9545";


            //var url = $"https://localhost:7029/api/UserCourses/all/{userId}?key={apiKey}";
            var url = $"https://localhost:7029/api/UserCourses/all/{userId}?key=dbee8814-f79e-4790-8ac0-8d29775d9545";


            var response = await _http.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "All courses removed successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to remove all courses.";
            }

            return RedirectToAction(nameof(GetSavedCourses));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            TempData["ErrorMessage"] = "Failed to remove all courses. Please try again later.";

            return RedirectToAction(nameof(GetSavedCourses));
        }
    }

    [HttpPost]
    public async Task<IActionResult> SavedCourses(UserCourseModel userCourse)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var response = await _courseService.AddCourseToSavedAsync(user.Id, userCourse.CourseId);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Course added to saved courses successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Course already exists for this user.";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            TempData["ErrorMessage"] = "Failed to add course to saved courses. Please try again later.";
        }


        return RedirectToAction("GetSavedCourses", "Account");
    }


    [HttpPost]
    public async Task<IActionResult> UploadProfilImage(IFormFile file)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user != null && file != null && file.Length != 0)
        {
            var fileName = $"p_{user.Id}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/uploads", fileName);

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            user.ProfileImageUrl = fileName;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Image uploaded successfully!";
            }

        }
        else
        {
            TempData["ErrorMessage"] = "Failed to upload the image";
        }

        return RedirectToAction("Details", "Account");

    }
}

