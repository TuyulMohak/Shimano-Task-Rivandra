using System.Diagnostics;
using ShimanoTask.Models;
using ShimanoTask.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShimanoTask.Services.BCrypt;
using Microsoft.AspNetCore.Mvc;

namespace ShimanoTask.Controllers;

// public class AccountFormat
// {
//     // I making this optional for a while
//     public string? UserName { get; set; }
//     public string? Password { get; set; }
// }

public class HomeController : Controller
{
    private readonly MyAppContext db;

    public HomeController(MyAppContext db)
    {
        this.db = db;
    }

    // IndexPage Contains list of Users
    public IActionResult Index()
    {
        var groups = db.Groups.Where(r=>r.GroupId != 1).ToList();
        return View(groups);
    }

    public IActionResult Menus(int id)
    {
        var group = db.Groups.Find(id);
        Console.WriteLine("THIS THING HAS MENUS")
        Console.WriteLine(group.Menus)
        return View(group);
    }
}
