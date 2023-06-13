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
public class TaskController : Controller
{
    private readonly AppDbContext context;

    public TaskController(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IActionResult> AddTask(Guid id)
    {
        ViewBag.TFanId = id;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddTask(Guid schoolid, [FromForm] TaskDto dto)
    {
        var task = new TaskEntity()
        {
            Title = dto.Title,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = "active",
            MaxBall = dto.MaxBall,
            FanId = schoolid
        };

        context.Tasks.Add(task);
        
        await context.SaveChangesAsync();

        return RedirectToAction("Index", new { id = schoolid });
    }


    public async Task<IActionResult> Index(Guid id)
    {
        ViewBag.TFanId = id;
        var tasks = await context.Tasks.Where(p=>p.FanId == id).ToListAsync();
        return View(tasks);
    }

    public async Task<IActionResult> View(Guid id)
    {
        var task = context.Tasks.Where(p=>p.Id == id).Include(p=>p.Fan).Include(p=>p.UserTasks).ThenInclude(p=>p.User);
        return View(task);
    }

}