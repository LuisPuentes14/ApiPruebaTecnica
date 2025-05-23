using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiPruebaTecnica.Context;
using ApiPruebaTecnica.Models;
using System.Xml.Linq;

namespace ApiPruebaTecnica.Controllers
{
    /// <summary>
    /// Controlador para gestionar los programas de crédito.
    /// </summary>
    /// <remarks>
    /// Nota: Este controlador puede ser refactorizado utilizando una arquitectura limpia (Clean Architecture),
    /// separando la lógica de negocio de la capa de presentación para mejorar la escalabilidad,
    /// la mantenibilidad y permitir pruebas unitarias más efectivas.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class CreditProgramsController : ControllerBase
    {
        private readonly BaseDeDatosPruebaTecnicaContext _context;

        /// <summary>
        /// Constructor del controlador de programas de crédito.
        /// </summary>
        /// <param name="context">Contexto de base de datos inyectado.</param>
        public CreditProgramsController(BaseDeDatosPruebaTecnicaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los programas de crédito disponibles.
        /// </summary>
        /// <returns>Una lista de programas de crédito con sus propiedades básicas.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCreditPrograms()
        {
            return await _context.CreditPrograms
                .Select( x=> new { IdCreditProgram = x.IdCreditProgram, Name = x.Name, Description =x.Description })
                .ToListAsync();
        }

    }
}
