using Microsoft.EntityFrameworkCore;
using ShimanoTask.Models;

namespace ShimanoTask.Data;

public class MyAppContext : DbContext
{
	public MyAppContext(DbContextOptions<MyAppContext> options) 
		: base(options)
	{
	}

	public DbSet<User> Users { get; set; }
	public DbSet<Group> Groups { get; set; } 
	public DbSet<Menu> Menus { get; set; }
}
