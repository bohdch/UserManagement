using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookVerse.Models;

namespace BookVerse.Models
{
    public class Collection
    {
        public int Id { get; set; }

        // Which user owns a collection
        public Guid UserId { get; set; }
        /*public User user { get; set; }*/

        // Implement the description property

        public string Title { get; set; }

        public List<Book>? Books { get; set; }
    }
}
