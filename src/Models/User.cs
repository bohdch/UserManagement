using System;
using System.ComponentModel.DataAnnotations;

namespace BookVerse.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string? Phone { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool Verified { get; set; } = false;
    }
}
