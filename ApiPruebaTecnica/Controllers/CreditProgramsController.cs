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
    [Route("api/[controller]")]
    [ApiController]
    public class CreditProgramsController : ControllerBase
    {
        private readonly BaseDeDatosPruebaTecnicaContext _context;

        public CreditProgramsController(BaseDeDatosPruebaTecnicaContext context)
        {
            _context = context;
        }

        // GET: api/CreditPrograms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCreditPrograms()
        {
            return await _context.CreditPrograms
                .Select( x=> new { IdCreditProgram = x.IdCreditProgram, Name = x.Name, Description =x.Description })
                .ToListAsync();
        }

    }
}
