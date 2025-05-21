using System;
using System.Collections.Generic;

namespace ApiPruebaTecnica.Models;

public partial class Teacher
{
    public int IdTeacher { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
}
