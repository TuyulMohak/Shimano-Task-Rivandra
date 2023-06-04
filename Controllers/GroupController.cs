using System.Diagnostics;
using ShimanoTask.Models;
using ShimanoTask.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShimanoTask.Services.BCrypt;
using Microsoft.AspNetCore.Mvc;

namespace ShimanoTask.Controllers;

public class GroupFormat
{
    public int? GroupId { get; set; }
    public string GroupName { get; set; }
}

public class GroupController : Controller
{
    private readonly MyAppContext db;

    public GroupController(MyAppContext db)
    {
        this.db = db;
    }

    // IndexPage Contains list of Groups
    public IActionResult Index()
    {
        var groups = db.Groups
            .Where(r => r.GroupId != 1) // We must hide the admin account
            .ToList();
        return View(groups);
    }

    public IActionResult AddGroup()
    {
        try {
            return View();

        } 
        catch (Exception ex) {
            TempData["error"] = ex.Message;
            return View();
        }

    }

    [HttpPost]
    public IActionResult AddGroup(GroupFormat group)
    {
        if(!ModelState.IsValid){
            return View();
        }
        try {
            PasswordManager passwordManager = new PasswordManager();

            var groupInput = new Group { GroupName=group.GroupName }; 
            db.Add(groupInput);
            db.SaveChanges();
            TempData["message"] = "Group Successfully Added";
            return RedirectToAction("Index");
        }
        catch (Exception ex){
            TempData["error"] = ex.Message;
            return View();
        }
    }



    public IActionResult EditGroup(int id)
    {
        Console.WriteLine(id);
        try {
            var groupFind = db.Groups.FirstOrDefault(r => r.GroupId == id);
            Console.WriteLine("This is the groupname");
            Console.WriteLine(groupFind.GroupName);
            if (groupFind == null){
                TempData["error"] = "User not exist";
                return RedirectToAction("Index");
            }
            ViewBag.Group = groupFind;
            return View();
        } catch(Exception ex){
            TempData["error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult EditGroup(GroupFormat group)
    {
        Console.WriteLine(group.GroupId);
        Console.WriteLine(group.GroupName);
        if(!ModelState.IsValid){
            return View();
        }
        try {
            var groupFind = db.Groups.FirstOrDefault(r => r.GroupId == group.GroupId);
            if (groupFind == null){
                TempData["error"] = "User not exist";
                return RedirectToAction("Index");
            }
            groupFind.GroupName = group.GroupName;
            db.SaveChanges();
            TempData["message"] = "User Successfully Updated";
            return RedirectToAction("Index");
        }
        catch (Exception ex){
            TempData["error"] = ex.Message;
            return View();
        }
    } 

    [HttpGet]
    public IActionResult DeleteGroup(int id)
    {
        try {
            var groupFind = db.Groups.Find(id);
            if (groupFind == null){
                TempData["error"] = "User not exist";
                return RedirectToAction("Index");
            }
            db.Groups.Remove(groupFind);
            db.SaveChanges();
            TempData["message"] = "Successfully Deleted Group";
            return RedirectToAction("Index");
        }
        catch(Exception ex) {
            TempData["error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
