using System;
using System.Collections.Generic;

namespace ApiPruebaTecnica.Models;

public partial class TeacherSubject
{
    public int Id { get; set; }

    public int? TeacherId { get; set; }

    public int? SubjectId { get; set; }

    public virtual Subject? Subject { get; set; }

    public virtual Teacher? Teacher { get; set; }
}
