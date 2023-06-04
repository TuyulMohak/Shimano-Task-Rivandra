using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ShimanoTask.Models
{
	public class Menu
	{
		public int MenuId { get; set; }
		public string MenuName { get; set; }
		public List<Group> Groups { get; set; }
	}
}