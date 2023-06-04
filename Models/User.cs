using System;
using System.ComponentModel.DataAnnotations;

namespace ShimanoTask.Models
{
	public class User
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		
		public int GroupId { get; set; }
		public Group Group { get; set; } = null;
	}
}