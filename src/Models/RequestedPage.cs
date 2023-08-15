using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookVerse.Models
{
    public class RequestedPage
    {
        [Key]
        public int Id { get; set; }

        public int PageNumber { get; set; }

        public string Category { get; set; }
    }
}

