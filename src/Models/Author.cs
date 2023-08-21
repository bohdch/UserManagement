using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookVerse.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }

        public string Name { get; set; }

        [JsonPropertyName("birth_year")]
        public int? BirthYear { get; set; }

        [JsonPropertyName("death_year")]
        public int? DeathYear { get; set; }
    }
}
