using System.Diagnostics;
using ShimanoTask.Models;
using ShimanoTask.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShimanoTask.Services.BCrypt;
using Microsoft.AspNetCore.Mvc;

namespace ShimanoTask.Controllers;

public class UserFormat
{
    public int? UserId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int GroupId { get; set; }
}

public class UserController : Controller
{
    private readonly MyAppContext db;

    public UserController(MyAppContext db)
    {
        this.db = db;
    }

    // IndexPage Contains list of Users
    public IActionResult Index()
    {
        var users = db.Users
            .Where(r => r.GroupId != 1) // We must hide the admin account
            .Include(r => r.Group)
            .ToList();
        return View(users);
    }

    public IActionResult AddUser()
    {
        try {
            var options = db.Groups.Where(r => r.GroupId != 1).ToList();

            // Create a new list to store the transformed objects
            List<SelectListItem> selectList = new List<SelectListItem>();

            // Loop through each object in the original list and transform it
            foreach (var obj in options)
            {
                // Create a new SelectListItem and set its properties
                SelectListItem item = new SelectListItem
                {
                    Value = obj.GroupId.ToString(),  // Set the value of the SelectListItem to the object's Id
                    Text = obj.GroupName             // Set the text of the SelectListItem to the object's Name
                };

                // Add the transformed object to the new list
                selectList.Add(item);
            }

            ViewBag.GroupOptions = selectList;
            return View();

        } 
        catch (Exception ex) {
            TempData["error"] = ex.Message;
            return View();
        }

    }

    [HttpPost]
    public IActionResult AddUser(UserFormat user)
    {
        if(!ModelState.IsValid){
            return View();
        }
        try {
            PasswordManager passwordManager = new PasswordManager();
            var userInput = new User { UserName=user.UserName, Password=passwordManager.HashPassword(user.Password), GroupId=Convert.ToInt32(user.GroupId)}; 
            db.Add(userInput);
            db.SaveChanges();
            TempData["message"] = "User Successfully Added";
            return RedirectToAction("Index");
        }
        catch (Exception ex){
            TempData["error"] = ex.Message;
            return View();
        }
    }

    public IActionResult EditUser(int id)
    {
        Console.WriteLine(id);
        try {
            var userFind = db.Users.FirstOrDefault(r => r.UserId == id);
            Console.WriteLine("This is the username");
            Console.WriteLine(userFind.UserName);
            if (userFind == null){
                TempData["error"] = "User not exist";
                return RedirectToAction("Index");
            }
            ViewBag.User = userFind;

            var options = db.Groups.Where(r => r.GroupId != 1).ToList();

            // Create a new list to store the transformed objects
            List<SelectListItem> selectList = new List<SelectListItem>();

            // Loop through each object in the original list and transform it
            foreach (var obj in options)
            {
                if (obj.GroupId == userFind.GroupId) {
                    SelectListItem item = new SelectListItem
                    {
                        Value = obj.GroupId.ToString(),  // Set the value of the SelectListItem to the object's Id
                        Text = obj.GroupName,             // Set the text of the SelectListItem to the object's Name
                        Selected = true
                    };
                    // Add the transformed object to the new list
                    selectList.Add(item);

                } else {
                    // Create a new SelectListItem and set its properties
                    SelectListItem item = new SelectListItem
                    {
                        Value = obj.GroupId.ToString(),  // Set the value of the SelectListItem to the object's Id
                        Text = obj.GroupName,             // Set the text of the SelectListItem to the object's Name
                        Selected = false
                    };   
                    // Add the transformed object to the new list
                    selectList.Add(item);
                }

            }

            ViewBag.GroupOptions = selectList;


            return View();
        } catch(Exception ex){
            TempData["error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult EditUser(UserFormat user)
    {
        if(!ModelState.IsValid){
            return View();
        }
        try {
            PasswordManager passwordManager = new PasswordManager();
            var userFind = db.Users.FirstOrDefault(r => r.UserId == user.UserId);
            if (userFind == null){
                TempData["error"] = "User not exist";
                return RedirectToAction("Index");
            }
            userFind.UserName = user.UserName;
            userFind.GroupId = user.GroupId;
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
    public IActionResult DeleteUser(int id)
    {
        try {
            var userFind = db.Users.Find(id);
            if (userFind == null){
                TempData["error"] = "User not exist";
                return RedirectToAction("Index");
            }
            db.Users.Remove(userFind);
            db.SaveChanges();
            TempData["message"] = "Successfully Deleted User";
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
