using System.ComponentModel.DataAnnotations;

namespace ApiPruebaTecnica.Dtos
{
    public class DtoStudentSubject
    {
        public int IdStudentSubject { get; set; }

        [Required]
        public int? IdStudent { get; set; }

        [Required]
        public int? SubjectId { get; set; }
    }
}
