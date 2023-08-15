using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookVerse.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }

        public string Name { get; set; }
    }
}
