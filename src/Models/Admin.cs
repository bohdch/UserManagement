using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
	public class Admin
	{
        // Default value (administrator can change account settings if desired)

        [Required]
        public string Name { get; set; } = "admin";

        [Required]
        [Key]
        public string Password { get; set; } = "admin";
    }
}
