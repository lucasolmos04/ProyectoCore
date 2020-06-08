using System;
using System.Collections.Generic;
using System.Text;

namespace Persistencia.DapperConexion.Instructor
{
    /// <summary>
    /// Data del modelo de Instructor
    /// </summary>
    public class InstructorModel
    {
        public Guid InstructorId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Grado { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
