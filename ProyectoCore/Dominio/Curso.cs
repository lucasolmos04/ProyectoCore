using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    /// <summary>
    /// Clase que contendra todos los cursos online
    /// </summary>
    public class Curso
    {
        public Guid CursoId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public byte[] FotoPortada { get; set; }
        public Precio PrecioPromocion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public ICollection<Comentario> ComentarioList { get; set; }
        public ICollection<CursoInstructor> InstructoresLink { get; set; }
    }
}
