using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ShimanoTask.Models
{
	public class Group
	{
		public int GroupId { get; set; }
		public string GroupName { get; set; }
		public List<User> Users { get; set; }
		public List<Menu> Menus { get; set; }
	}
}