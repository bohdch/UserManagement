using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Book
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int id { get; set; }

    public string Title { get; set; }

    public List<Author> Authors { get; set; }

    public string[] Subjects { get; set; }

    public string[] Languages { get; set; }

    public Dictionary<string, string> Formats { get; set; }

    public int Download_count { get; set; }
}
