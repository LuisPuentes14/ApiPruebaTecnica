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
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly BaseDeDatosPruebaTecnicaContext _context;

        public SubjectsController(BaseDeDatosPruebaTecnicaContext context)
        {
            _context = context;
        }       

        // GET: api/Subjects/5
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
