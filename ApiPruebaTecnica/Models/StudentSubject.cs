using System;
using System.Collections.Generic;

namespace ApiPruebaTecnica.Models;

public partial class StudentSubject
{
    public int IdStudentSubject { get; set; }

    public int? IdStudent { get; set; }

    public int? SubjectId { get; set; }

    public virtual Student? IdStudentNavigation { get; set; }

    public virtual Subject? Subject { get; set; }
}
