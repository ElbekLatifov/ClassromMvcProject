using System.Net;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Classrom.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using C.Services;
using ClassRoom.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Classrom.Controllers;

public class UserController : Controller
{
    public UserManager<User> _userManager;
    public SignInManager<User> _signinManager;
    private readonly AppDbContext context;
    private readonly GetUserId _userid;

    public UserController(UserManager<User> userManager, SignInManager<User> signinManager, AppDbContext context, GetUserId userid)
    {
        _userid = userid;
        this.context = context;
        _userManager = userManager;
        _signinManager = signinManager;
    }

    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp([FromForm] UserDto user)
    {
        if(!ModelState.IsValid)
        {
            return View(user);
        }

        var userr = new User()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            UserName= user.UserName
        };

        if(user.PhotoUrl is not null)
            userr.PhotoUrl = await SavePhoto.SaveUsersFile(user.PhotoUrl);

        var result = await _userManager.CreateAsync(userr, user.Password);
        if(!result.Succeeded)
        {
            ModelState.AddModelError("UserName", result.Errors.First().Description);
            return View();
        }
        await _signinManager.SignInAsync(userr, isPersistent: true);

        return RedirectToAction("Profile");
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var requests = await context.Requests.Select(p=>p.ToUserId == _userid.UserId).ToListAsync();
        ViewBag.Requests = requests;
        var user = await _userManager.GetUserAsync(User);
        return View(user);
    }

    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn([FromForm] SignInDto dto)
    {
        if(!ModelState.IsValid)
        {
            return View(dto);
        }
        var result = await _signinManager.PasswordSignInAsync(dto.UserName, dto.Password, true, false);
        if(result.Succeeded)
        {
            ModelState.AddModelError("Password", "Login or password incorrect");
        }
        return RedirectToAction("Profile");
    }

    public async Task<IActionResult> LogOut()
    {
        await _signinManager.SignOutAsync();
        return RedirectToAction("SignIn");
    }

    public async Task<IActionResult> Update()
    {
        return Ok("Update bhozircha mavjud emas");
    }
}