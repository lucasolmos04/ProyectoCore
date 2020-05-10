using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    /// <summary>
    /// Clase que contiene las realciones de Curso -> Instructor
    ///                                      Instructor -> Curso
    /// </summary>
    public class CursoInstructor
    {
        public Guid CursoId { get; set; }
        public Guid InstructorId { get; set; }
        public Curso Curso { get; set; }
        public Instructor Instructor { get; set; }
    }
}
