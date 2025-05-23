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
    /// <summary>
    /// Controlador para gestionar las operaciones CRUD de los estudiantes.
    /// </summary>
    /// <remarks>
    /// Nota: Esta implementación puede ser refactorizada utilizando el patrón de arquitectura limpia (Clean Architecture),
    /// separando responsabilidades en capas como Aplicación, Dominio e Infraestructura, para mejorar la escalabilidad,
    /// mantenibilidad y facilitar pruebas unitarias.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly BaseDeDatosPruebaTecnicaContext _context;

        /// <summary>
        /// Constructor del controlador de estudiantes.
        /// </summary>
        /// <param name="context">Contexto de base de datos inyectado.</param>
        public StudentsController(BaseDeDatosPruebaTecnicaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los estudiantes con su respectivo programa de crédito.
        /// </summary>
        /// <returns>Una lista de estudiantes con información del programa de crédito.</returns>
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

        /// <summary>
        /// Obtiene los detalles de un estudiante específico por su ID.
        /// </summary>
        /// <param name="id">ID del estudiante.</param>
        /// <returns>Información del estudiante si existe; de lo contrario, 404.</returns>
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

        /// <summary>
        /// Actualiza un estudiante existente.
        /// </summary>
        /// <param name="id">ID del estudiante a actualizar.</param>
        /// <param name="student">DTO con los datos actualizados del estudiante.</param>
        /// <returns>Respuesta HTTP que indica el resultado de la operación.</returns>
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

        /// <summary>
        /// Crea un nuevo estudiante.
        /// </summary>
        /// <param name="student">DTO con los datos del nuevo estudiante.</param>
        /// <returns>Estudiante creado con su ubicación.</returns>
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(DtoStudent student)
        {

            _context.Students.Add(new Student() { Name = student.Name, NumberDocument = student.NumberDocument, IdCreditProgram = student.IdCreditProgram });
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.IdStudent }, student);
        }

        /// <summary>
        /// Elimina un estudiante por su ID.
        /// </summary>
        /// <param name="id">ID del estudiante a eliminar.</param>
        /// <returns>Respuesta HTTP indicando el resultado de la eliminación.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.StudentSubjects.RemoveRange(_context.StudentSubjects.Where(x => x.IdStudent == id));

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
