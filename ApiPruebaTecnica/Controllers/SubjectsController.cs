using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiPruebaTecnica.Context;
using ApiPruebaTecnica.Models;

namespace ApiPruebaTecnica.Controllers
{
    /// <summary>
    /// Controlador para gestionar las materias (Subjects).
    /// </summary>
    /// <remarks>
    /// Nota: Esta clase puede beneficiarse de una refactorización aplicando los principios de la arquitectura limpia,
    /// separando la lógica de acceso a datos y las transformaciones en capas como servicios o casos de uso.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly BaseDeDatosPruebaTecnicaContext _context;

        /// <summary>
        /// Constructor del controlador.
        /// </summary>
        /// <param name="context">Contexto de base de datos inyectado.</param>
        public SubjectsController(BaseDeDatosPruebaTecnicaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todas las materias asociadas a un programa académico específico.
        /// </summary>
        /// <param name="id">ID del programa académico (CreditProgram).</param>
        /// <returns>Lista de materias del programa académico.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<object>>> GetSubjectCreditProgram(int id)
        {
            var subject = await _context.Subjects
              .Where(x => x.IdCreditProgram == id)
              .Select(x => new 
              {
                  IdSubject = x.IdSubject,
                  Name = x.Name,
                  Credits = x.Credits,
                  IdCreditProgram = x.IdCreditProgram
              })
              .ToListAsync();

            return subject;
        }

        
    }
}
