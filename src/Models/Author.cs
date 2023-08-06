using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Author
{
    public int Id { get; set; }

    // Foreign key to the Book
    public int BookId { get; set; }

    public string Name { get; set; }
}
