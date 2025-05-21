using ApiPruebaTecnica.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiPruebaTecnica.Dtos
{
    public class DtoStudent
    {      
        public int IdStudent { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string NumberDocument { get; set; } = null!;

        [Required]
        public int IdCreditProgram { get; set; }      

    }
}
