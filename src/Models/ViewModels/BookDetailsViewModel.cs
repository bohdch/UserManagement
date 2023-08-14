using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookVerse.Models.ViewModels
{
    public class BookDetailsViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string[] Subjects { get; set; }

        public string[] Bookshelves { get; set; }

        public string[] Languages { get; set; }
    }
}
