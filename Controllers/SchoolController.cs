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
public class SchoolController : Controller
{
    private readonly AppDbContext context;
    private readonly GetUserId _userid;

    public SchoolController(AppDbContext context, GetUserId userid)
    {
        this.context = context;
        _userid = userid;
    }

    public async Task<IActionResult> CreateSchool()
    {
        return View();
    }
 
    [HttpPost]
    public async Task<IActionResult> CreateSchool([FromForm] SchoolDto dto)
    {
        if(!ModelState.IsValid)
        {
            return View(dto);
        }

        var school = new School()
        {
            Name = dto.Name,
            Description = dto.Description,
            PhotoUrl = await SavePhoto.SaveSchoolsFile(dto.PhotoUrl)
        };

        var userid = _userid.UserId;

        school.Users = new List<UserSchool>()
        {
            new UserSchool()
            {
                UserId = userid,
                Type = EUserSchool.Creater
            }
        };

        context.SchoolSet.Add(school);
        await context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Index(int page = 0, string query = "", bool sort = false)
    {
        ViewBag.Page = page;
        List<School> schools = default;
        if(sort)
        {
            //var queryr = from x in context.SchoolSet select x;
            switch (query)
            {
                case "Named" : schools = await context.SchoolSet.Include(p=>p.Users).OrderBy(x=>x.Name).ToListAsync(); break;
                case "MUsersCount" : schools = await context.SchoolSet.Include(p=>p.Users).OrderByDescending(x=>x.Users.Count).ToListAsync(); break;
                case "mUsersCount" : schools = await context.SchoolSet.Include(p=>p.Users).OrderBy(x=>x.Users.Count).ToListAsync(); break;
            }
            return View("Sorting", schools);
            //schools = context.SchoolSet.GroupBy(p=>p.Users).Select(p=>p.Count);
        }
        schools = await context.SchoolSet.Include(school=>school.Users).ThenInclude(o=>o.User).ToListAsync();
        return View(schools);
    }
    public async Task<IActionResult> Sorting()
    {
        return View();
    }

    public async Task<IActionResult> View(Guid id)
    {
        var school = await context.SchoolSet.Include(school=>school.Users).ThenInclude(userschool=>userschool.User).FirstOrDefaultAsync(p=>p.Id == id);
        return View(school);
    }

    public async Task<IActionResult> Join(Guid id)
    {
        var school = await context.SchoolSet.Include(s=>s.Users).ThenInclude(p=>p.User).FirstOrDefaultAsync(p=>p.Id == id);
        var userid = _userid.UserId;
        if(!school.Users.Any(p=>p.UserId == userid))
        {
            school.Users.Add(new UserSchool()
            {
                UserId = userid,
                Type = EUserSchool.Student
            });
        }
        await context.SaveChangesAsync();
        
        return RedirectToAction("View", new{id = school.Id}); 
    }

    [HttpGet]
    public async Task<IActionResult> UpdateSchool(Guid id)
    {
        var school = await context.SchoolSet.Include(s=>s.Users).ThenInclude(p=>p.User).FirstOrDefaultAsync(p=>p.Id == id);
        return View(new UpdateSchoolDto()
        {
            Name = school.Name,
            Description = school.Description
        });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSchool(Guid id, [FromForm] UpdateSchoolDto dto)
    {
        var school = await context.SchoolSet.Include(s=>s.Users).ThenInclude(p=>p.User).FirstOrDefaultAsync(p=>p.Id == id);
        school.Description = dto.Description;
        school.Name = dto.Name;
        school.PhotoUrl = await SavePhoto.SaveSchoolsFile(dto.PhotoUrl);
        await context.SaveChangesAsync();
        return RedirectToAction("View", new {id = id});
    }
    public async Task<IActionResult> UpdateRole(Guid userid, Guid schoolid, EUserSchool role)
    {
        var school = await context.UserSchools.FirstOrDefaultAsync(p=>p.UserId == userid && p.SchoolId == schoolid);
        if(school.Type != EUserSchool.Creater || role != EUserSchool.Creater)
        {
            school.Type = role;
        }
        await context.SaveChangesAsync();
        return RedirectToAction("View", new {id = schoolid});
    }

    public async Task<IActionResult> Students(Guid id)
    {
        var students = context.SchoolSet.Where(p=>p.Id == id);
        return View(students);
    }

    public async Task<IActionResult> SearchSchool(string name)
    {
        if(name == null) return RedirectToAction("Index");
        if(context.SchoolSet.Any(p=>p.Name.Contains(name)))
        {
            var sch = await context.SchoolSet.Where(p=>p.Name.Contains(name)).Include(p=>p.Users).ToListAsync();
            return View(sch);
        }
        return RedirectToAction("NotFound");
    }

    public async Task<IActionResult> NotFound()
    {
        return View();
    }
}