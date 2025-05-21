using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiPruebaTecnica.Context;
using ApiPruebaTecnica.Models;
using ApiPruebaTecnica.Dtos;

namespace ApiPruebaTecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly BaseDeDatosPruebaTecnicaContext _context;

        public StudentsController(BaseDeDatosPruebaTecnicaContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetStudents()
        {
            var students = await _context.Students
                .Include(x => x.IdCreditProgramNavigation)
                .Select(x => new
                {
                    x.IdStudent,
                    x.Name,
                    x.NumberDocument,
                    x.IdCreditProgram,
                    CreditProgramName = x.IdCreditProgramNavigation != null
                        ? x.IdCreditProgramNavigation.Name
                        : null
                })
                .ToArrayAsync();

            return students;
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetStudent(int id)
        {
            var student = await _context.Students
                .Include(x => x.IdCreditProgramNavigation)
                .Where(x => x.IdStudent == id)
                .Select(x => new
                {
                    x.IdStudent,
                    x.Name,
                    x.NumberDocument,
                    x.IdCreditProgram,
                    CreditProgramName = x.IdCreditProgramNavigation != null
                        ? x.IdCreditProgramNavigation.Name
                        : null
                })
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, DtoStudent student)
        {
            if (id != student.IdStudent)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(DtoStudent student)
        {

            _context.Students.Add(new Student() { Name = student.Name, NumberDocument = student.NumberDocument, IdCreditProgram = student.IdCreditProgram });
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.IdStudent }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.IdStudent == id);
        }
    }
}
