using System;
using System.Collections.Generic;

namespace ApiPruebaTecnica.Models;

public partial class Subject
{
    public int IdSubject { get; set; }

    public string Name { get; set; } = null!;

    public int Credits { get; set; }

    public int? IdCreditProgram { get; set; }

    public virtual CreditProgram? IdCreditProgramNavigation { get; set; }

    public virtual ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();

    public virtual ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
}
