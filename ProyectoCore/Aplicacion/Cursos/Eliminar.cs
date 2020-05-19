using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    /// <summary>
    /// Curso
    /// Clase para eliminar un curso
    /// </summary>
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // Eliminamos los instructores
                var instructoresDB = _context.CursoInstructor.Where(x => x.CursoId == request.Id);

                foreach (var instructor in instructoresDB)
                {
                    _context.CursoInstructor.Remove(instructor);
                }

                // Eliminamos los comentarios
                var comentariosDB = _context.Comentario.Where(x => x.CursoId == request.Id);
                foreach (var cmt in comentariosDB)
                {
                    _context.Comentario.Remove(cmt);
                }

                // Eliminamos los precios
                var precioDB = _context.Precio.Where(x => x.CursoId == request.Id).FirstOrDefault();
                if (precioDB != null)
                {
                    _context.Precio.Remove(precioDB);
                }

                // Eliminamos el Curso
                var curso = await _context.Curso.FindAsync(request.Id);
                if (curso == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { curso = "No se encontro el curso" });
                }

                _context.Remove(curso);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudiedo eliminar el curso");
            }
        }
    }
}
