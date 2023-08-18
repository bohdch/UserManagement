using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookVerse.Models;

namespace BookVerse.Models
{
    public class Collection
    {
        [Key]
        public int Id { get; set; }

        public User User { get; set; }

        // Implement the description property

        public string Title { get; set; }

        public List<Book>? Books { get; set; }
    }
}
