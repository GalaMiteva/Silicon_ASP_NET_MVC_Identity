using Infrastructure.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Silicon_MVC.ViewModels;
using System.Security.Claims;

namespace Silicon_MVC.Controllers;

public class AuthController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager) : Controller
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;


    public IActionResult Index()
    {
        ViewData["Title"] = "Profile";
        return View();
    }

    #region SignUp
    [Route("/signup")]
    [HttpGet]

    public IActionResult SignUp()
    {


        if (_signInManager.IsSignedIn(User))

            return RedirectToAction("Details", "Account");

        return View(new SignUpViewModel());
    }


    //public IActionResult SignUp()
    //{
    //    var viewModel = new SignUpViewModel();
    //    return View(viewModel);
    //}



    [HttpPost]
    [Route("/signup")]
    public async Task<IActionResult> SignUp(SignUpViewModel viewModel)
    {
        var standartRole = "User";

        if (ModelState.IsValid)
        {

            if (!await _userManager.Users.AnyAsync())
            {
                standartRole = "SuperAdmin";
            }

            var exist = await _userManager.FindByEmailAsync(viewModel.Form.Email);
            if (exist != null)
            {
                ModelState.AddModelError("Email", "Email already exists");
               // ViewData["ErrorMessage"] = "User with the same email already exists";
                ViewData["StatusMessage"] = "danger|User with the same email already exists";

                return View(viewModel);
            }

            var userEntity = new UserEntity
            {
                FirstName = viewModel.Form.FirstName,
                LastName = viewModel.Form.LastName,
                Email = viewModel.Form.Email,
                UserName = viewModel.Form.Email,
                Created = DateTime.Now
            };

            var result = await _userManager.CreateAsync(userEntity, viewModel.Form.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userEntity, standartRole);
                return RedirectToAction("SignIn", "Auth");
            }
        }

        return View(viewModel);
    }
    #endregion




    #region Sign In
    [HttpGet]
    [Route("/signin")]

    public IActionResult SignIn()
    {
        if (_signInManager.IsSignedIn(User))
            return RedirectToAction("Details", "Account");

        return View(new SignInViewModel());
    }

    //public IActionResult SignIn(string returnUrl)
    //{
    //    if (_signInManager.IsSignedIn(User))
    //        return RedirectToAction("Details", "Account");
    //    ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/");

    //    return View(new SignInViewModel());
    //}


    [Route("/signin")]
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(viewModel.Form.Email, viewModel.Form.Password, viewModel.Form.RememberMe, false);
            if (result.Succeeded)
                return RedirectToAction("Details", "Account");
        }

        ModelState.AddModelError("Email", "Incorrect email or password");
        viewModel.ErrorMessage = "Incorrect email or password";
        return View(viewModel);


    }

   

    #endregion

    #region Sign Out
    [HttpGet]
    [Route("/signout")]
    public new async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction( "Home", "Default");
    }
    #endregion




    #region External Account  | Facebook
    [HttpGet]

    public IActionResult Facebook()
    {
        var authProps = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", Url.Action("FacebookCallback"));
        return new ChallengeResult("Facebook", authProps);

    }


    [HttpGet]
    public async Task<IActionResult> FacebookCallback()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info != null)
        {
            var userEntity = new UserEntity
            {
                Email = info.Principal.FindFirstValue(ClaimTypes.Email)!,
                UserName = info.Principal.FindFirstValue(ClaimTypes.Email)!,
                FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!,
                LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!,
                IsExternalAccount = true,
                Created = DateTime.Now!
            };

            var user = await _userManager.FindByEmailAsync(userEntity.Email);
            if (user == null)
            {
                var result = await _userManager.CreateAsync(userEntity);
                if (result.Succeeded)
                    user = await _userManager.FindByEmailAsync(userEntity.Email);
            }

            if (user != null)
            {
                if (user.FirstName != userEntity.FirstName || user.LastName != userEntity.LastName || user.Email != userEntity.Email)
                {
                    user.FirstName = userEntity.FirstName;
                    user.LastName = userEntity.LastName;
                    user.Email = userEntity.Email;

                    await _userManager.UpdateAsync(user);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Details", "Account");
            }

        }

        ModelState.AddModelError("Facebook", "Failed to sign in with Facebook");
        //ViewData["ErrorMessage"] = "Failed to sign in with Facebook";
        ViewData["StatusMessage"] = "danger|Failed to sign in with Facebook";
        return RedirectToAction("SignIn", "Auth");
    }



    #endregion



    #region External Account  | Coogle

    [HttpGet]
    public IActionResult Google()
    {
        var authProps = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action("GoogleCallback"));
        return new ChallengeResult("Google", authProps);
    }


    [HttpGet]
    public async Task<IActionResult> GoogleCallback()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info != null)
        {
            var userEntity = new UserEntity
            {
                Email = info.Principal.FindFirstValue(ClaimTypes.Email)!,
                UserName = info.Principal.FindFirstValue(ClaimTypes.Email)!,
                FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!,
                LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!,
                IsExternalAccount = true,
                Created = DateTime.Now
            };

            var user = await _userManager.FindByEmailAsync(userEntity.Email);
            if (user == null)
            {
                var result = await _userManager.CreateAsync(userEntity);
                if (result.Succeeded)
                    user = await _userManager.FindByEmailAsync(userEntity.Email);
            }

            if (user != null)
            {
                if (user.FirstName != userEntity.FirstName || user.LastName != userEntity.LastName || user.Email != userEntity.Email)
                {
                    user.FirstName = userEntity.FirstName;
                    user.LastName = userEntity.LastName;
                    user.Email = userEntity.Email;

                    await _userManager.UpdateAsync(user);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Details", "Account");
            }

        }

        //ModelState.AddModelError("InvalidAutentcation", "danger|Failed to authenticate with Google.");
        ViewData["StatusMessage"] = "danger|Failed to authenticate with Google.";
        return RedirectToAction("SignIn", "Auth");
    }


    #endregion

}
