using System;
using System.Collections.Generic;

namespace ApiPruebaTecnica.Models;

public partial class CreditProgram
{
    public int IdCreditProgram { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
