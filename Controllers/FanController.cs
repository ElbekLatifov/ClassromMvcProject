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

[Authorize]
public class FanController : Controller
{
    private readonly AppDbContext context;
    private readonly GetUserId _userid;

    public FanController(AppDbContext context, GetUserId userid)
    {
        this.context = context;
        _userid = userid;
    }

    public async Task<IActionResult> AddFan(Guid schoolid)
    {
        ViewBag.SchoolId = schoolid;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddFan(Guid schoolid, [FromForm] FanDto dto)
    {
        if(!ModelState.IsValid)
        {
            return View(dto);
        }
        var fans = await context.Fans.Where(p=>p.SchoolId == schoolid).ToListAsync();

        if(fans.Any(p=>p.Name == dto.Name))
        {
            ModelState.AddModelError("Name", "this scince already have in this school");
            return View();
        }

        var fan = new Fan()
        {
            Name = dto.Name,
            Discription = dto.Description,
            SchoolId = schoolid
        };
        var userid = _userid.UserId;
        fan.FanUsers = new List<UserFan>()
        {
            new UserFan()
            {
                UserId = userid,
                Type = EUserFan.Teacher
            }
        };
        var school = await context.SchoolSet.Include(p=>p.Fans).FirstOrDefaultAsync(p=>p.Id == schoolid);
        school.Fans.Add(fan);
        context.Fans.Add(fan);
        await context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> View(Guid id)
    {
        var fan = await context.Fans.Include(p=>p.FanUsers).ThenInclude(p=>p.User).FirstOrDefaultAsync(p=>p.Id == id);
        return View(fan);
    }
    [HttpGet]
    public async Task<IActionResult> Share(Guid fanid)
    {
        ViewBag.FanId = fanid;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Share(Guid fanid, [FromForm] TaklifnomaDto join)
    {
        var userid = _userid.UserId;
        var touser = await context.Users.FirstOrDefaultAsync(p=>p.UserName == join.UserName);

        var IsJoined = await context.Requests.AnyAsync(p=>p.ToUserId == touser.Id && p.FanId == fanid);
        var IsAgainJoined = await context.UsersFans.AnyAsync(p=>p.UserId == touser.Id && p.FanId == fanid);

        if(IsJoined || IsAgainJoined)
        {
            return RedirectToAction("View", new {id = fanid});
        }

        var jn = new Taklifnoma()
        {
            FanId = fanid,
            ToUserId = touser.Id,
            FromUserId = userid,
            IsJoined = false
        };
        context.Requests.Add(jn);
        await context.SaveChangesAsync();
        return RedirectToAction("View", new {id = fanid});
    }

    public async Task<IActionResult> JoinR(Guid reqid, bool j)
    {
        var join = await context.Requests.FirstOrDefaultAsync(p=>p.Id == reqid);
        if(j)
        {
            var taklif = new UserFan()
            {
                FanId = join.FanId,
                UserId = join.ToUserId,
                Type = EUserFan.Student
            };
            join.IsJoined = true;
            context.UsersFans.Add(taklif);
        }
        else
        {
            context.Requests.Remove(join);
        }
        await context.SaveChangesAsync();
        return RedirectToAction("Profile", "User");
    }

    public async Task<IActionResult> SearchFan(string name)
    {
        //if(name == null) return RedirectToAction("View");
        if(context.Fans.Any(p=>p.Name.Contains(name)))
        {
            var sch = await context.Fans.Where(p=>p.Name.Contains(name)).Include(p=>p.FanUsers).ToListAsync();
            return View(sch);
        }
        return RedirectToAction("NotFound");
    }

    public async Task<IActionResult> NotFound()
    {
        return View();
    }
}