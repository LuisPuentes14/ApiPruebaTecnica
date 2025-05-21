using System;
using System.Collections.Generic;

namespace ApiPruebaTecnica.Models;

public partial class Student
{
    public int IdStudent { get; set; }

    public string Name { get; set; } = null!;

    public string NumberDocument { get; set; } = null!;

    public int? IdCreditProgram { get; set; }

    public virtual CreditProgram? IdCreditProgramNavigation { get; set; }

    public virtual ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
}
