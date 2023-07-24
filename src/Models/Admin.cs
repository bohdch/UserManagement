using System;
using System.ComponentModel.DataAnnotations;

namespace BookVerse.Models
{
	public class Admin
	{
        [Required]
        public string Name { get; set; } = "admin";

        [Required]
        [Key]
        public string Password { get; set; } = "admin";
    }
}
