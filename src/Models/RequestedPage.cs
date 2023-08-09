using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class RequestedPage
{
    public int Id { get; set; }
    public int PageNumber { get; set; }
    public string Category { get; set; }
}
