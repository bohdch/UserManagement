using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Book
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    public string Title { get; set; }

    public List<Author> Authors { get; set; }

    public string Description { get; set; } = String.Empty;

    public string[] Subjects { get; set; }

    public string[] Bookshelves { get; set; }

    public string[] Languages { get; set; }

    public Dictionary<string, string> Formats { get; set; }

    [JsonPropertyName("download_count")]
    public int DownloadCount { get; set; }
}
