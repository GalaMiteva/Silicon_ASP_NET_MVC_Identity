using Infrastructure.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Silicon_MVC.Controllers;

[Authorize]
public class AccountController(UserManager<UserEntity> userManager, AddressManager addressManager) : Controller
{
    
}
