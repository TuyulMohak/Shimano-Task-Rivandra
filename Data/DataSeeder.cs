using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShimanoTask.Data;
using ShimanoTask.Models;
using ShimanoTask.Services.BCrypt;

namespace ShimanoTask.Data;

public class DataSeeder
{
	private readonly MyAppContext db;

    public DataSeeder(MyAppContext db)
    {
        this.db = db;
    }
	public void Seed()
    {

    	// Add Group (With Menus)
    	var AdminGroup = new Group {
    		GroupName = "admin"
    	};
    	var SalesGroup = new Group {
    		GroupName = "sales",
    		Menus = new List<Menu>
    		{
    			new Menu { MenuName="Sales Data" }
    		}
    	};
    	var ProductionGroup = new Group () {
    		GroupName = "production",
    		Menus = new List<Menu>
    		{
    			new Menu { MenuName="Production Data" },
    			new Menu { MenuName="Demand Data" }
    		}
    	};
    	bool groupHasRows = db.Groups.Any();
    	if (!groupHasRows) {
        	db.Groups.Add(AdminGroup);
	        db.Groups.Add(SalesGroup);
	        db.Groups.Add(ProductionGroup);
			db.SaveChanges();
			Console.WriteLine("Successfully seed the Group Table");
        } else {
        	Console.WriteLine("Group Seed already planted on Group Table");
        }

    	
    	// Add Users with Group Id
        PasswordManager passwordManager = new PasswordManager();
        // for simplicity, all of the users have same password "marc123"
        bool UserHasRows = db.Users.Any();
        if (!UserHasRows) {
        	db.Users.Add(new User { UserName="Admin123", Password=passwordManager.HashPassword("marc123"), GroupId=1 });
	        db.Users.Add(new User { UserName="Mark", Password=passwordManager.HashPassword("marc123"), GroupId=2 });
	        db.Users.Add(new User { UserName="John", Password=passwordManager.HashPassword("marc123"), GroupId=3 });
	        db.Users.Add(new User { UserName="Oliver", Password=passwordManager.HashPassword("marc123"), GroupId=2 });
			db.SaveChanges();
			Console.WriteLine("Successfully seed the User Table");
        } else {
        	Console.WriteLine("User Seed already planted on User Table");
        }
    }
}
