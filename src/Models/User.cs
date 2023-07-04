using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        // TO DO - implement an image field
       

        private string _name;

        [Required]
        public string Name
        {
            get { return _name; }
            set { _name = value?.Trim(); }
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string? Phone { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool Verified { get; set; } = false;
    }
}
