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
    /// Controlador para gestionar la asignación de materias a estudiantes.
    /// </summary>
    /// <remarks>
    /// Nota: Esta clase puede beneficiarse de una refactorización siguiendo los principios de arquitectura limpia
    /// para separar la lógica de validación y negocio del controlador, favoreciendo la mantenibilidad y pruebas unitarias.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class StudentSubjectsController : ControllerBase
    {
        private readonly BaseDeDatosPruebaTecnicaContext _context;

        /// <summary>
        /// Constructor del controlador.
        /// </summary>
        /// <param name="context">Contexto de base de datos inyectado.</param>
        public StudentSubjectsController(BaseDeDatosPruebaTecnicaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todas las materias asignadas a los estudiantes, incluyendo información del estudiante y del profesor.
        /// </summary>
        /// <returns>Lista de asignaciones con datos relacionados.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetStudentSubjects()
        {
            return await _context.StudentSubjects
                .Include(x => x.Subject)
                .Include(x => x.IdStudentNavigation)
                .Include(x => x.Subject.TeacherSubjects)
                .Select(x => new
                {
                    x.IdStudentSubject,
                    x.IdStudent,
                    x.SubjectId,
                    StudentName = x.IdStudentNavigation != null ? x.IdStudentNavigation.Name : null,
                    StudentNumberDocument = x.IdStudentNavigation != null ? x.IdStudentNavigation.NumberDocument : null,
                    SubjectName = x.Subject != null ? x.Subject.Name : null,
                    TeacherName = x.Subject.TeacherSubjects != null ? x.Subject.TeacherSubjects.FirstOrDefault().Teacher.Name : null
                })
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los nombres de los estudiantes que están viendo una materia específica.
        /// </summary>
        /// <param name="subjectId">ID de la materia.</param>
        /// <returns>Lista de nombres de estudiantes.</returns>
        [HttpGet("GetStudentSubjectsBySubjectId/{subjectId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetStudentSubjectsBySubjectId(int subjectId)
        {
            var result = await _context.StudentSubjects
                .Where(x => x.SubjectId == subjectId)                
                .Include(x => x.IdStudentNavigation)
                .Select(x => new
                {                    
                    StudentName = x.IdStudentNavigation != null ? x.IdStudentNavigation.Name : null
                })
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// Obtiene las materias asignadas a un estudiante por su ID.
        /// </summary>
        /// <param name="id">ID del estudiante.</param>
        /// <returns>Lista de materias con información del profesor y del estudiante.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<object>>> GetStudentSubject(int id)
        {
            var studentSubject = await _context.StudentSubjects.Include(x => x.Subject)
                .Include(x => x.IdStudentNavigation)
                .Include(x => x.Subject.TeacherSubjects)
                .Where(x => x.IdStudent == id)
                .Select(x => new
                {
                    x.IdStudentSubject,
                    x.IdStudent,
                    x.SubjectId,
                    StudentName = x.IdStudentNavigation != null ? x.IdStudentNavigation.Name : null,
                    StudentNumberDocument = x.IdStudentNavigation != null ? x.IdStudentNavigation.NumberDocument : null,
                    SubjectName = x.Subject != null ? x.Subject.Name : null,
                    TeacherName = x.Subject.TeacherSubjects != null ? x.Subject.TeacherSubjects.FirstOrDefault().Teacher.Name : null
                }).ToListAsync();           

            return studentSubject;
        }

        /// <summary>
        /// Asigna una nueva materia a un estudiante, validando condiciones específicas.
        /// </summary>
        /// <param name="dto">Datos de la asignación.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPost]
        public async Task<ActionResult<StudentSubject>> PostStudentSubject(DtoStudentSubject dto)
        {
            // Validar si la materia ya está asignada al estudiante
            bool alreadyAssigned = await _context.StudentSubjects
                .AnyAsync(x => x.SubjectId == dto.SubjectId && x.IdStudent == dto.IdStudent);

            if (alreadyAssigned)
            {
                return BadRequest("La materia ya está asignada al estudiante");
            }

            // Validar que el estudiante no tenga más de 3 materias
            int subjectCount = await _context.StudentSubjects
                .CountAsync(x => x.IdStudent == dto.IdStudent);

            if (subjectCount >= 3)
            {
                return BadRequest("El estudiante ya tiene 3 materias asignadas");
            }

            // Validar si la materia tiene profesor asignado
            var teacherBySubject = await _context.TeacherSubjects
                .FirstOrDefaultAsync(x => x.SubjectId == dto.SubjectId);

            if (teacherBySubject == null)
            {
                return BadRequest("No hay profesor asignado a la materia");
            }

            // Validar si el estudiante ya tiene una materia con el mismo profesor
            bool hasSameTeacher = await _context.StudentSubjects
                .Join(_context.TeacherSubjects,
                      ss => ss.SubjectId,
                      ts => ts.SubjectId,
                      (ss, ts) => new { ss, ts })
                .AnyAsync(x => x.ss.IdStudent == dto.IdStudent && x.ts.TeacherId == teacherBySubject.TeacherId);

            if (hasSameTeacher)
            {
                return BadRequest("El estudiante ya tiene una materia asignada con el mismo profesor");
            }

            // Crear y guardar la asignación
            var studentSubject = new StudentSubject
            {
                IdStudent = dto.IdStudent,
                SubjectId = dto.SubjectId
            };

            _context.StudentSubjects.Add(studentSubject);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentSubject", new { id = studentSubject.IdStudentSubject }, studentSubject);
        }


        /// <summary>
        /// Elimina una asignación de materia a estudiante por ID.
        /// </summary>
        /// <param name="id">ID de la asignación.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentSubject(int id)
        {
            var studentSubject = await _context.StudentSubjects.FindAsync(id);
            if (studentSubject == null)
            {
                return NotFound();
            }

            _context.StudentSubjects.Remove(studentSubject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentSubjectExists(int id)
        {
            return _context.StudentSubjects.Any(e => e.IdStudentSubject == id);
        }
    }
}
