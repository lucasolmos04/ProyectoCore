using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
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
    /// Clase para editar un curso
    /// </summary>
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal? Precio { get; set; }
            public decimal? Promocion { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                //Ejecutamos una regla para las propiedades
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
            }
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
                var curso = await _context.Curso.FindAsync(request.CursoId);

                if (curso == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { curso = "No se encontro el curso" });
                }

                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

                // Actulizar Precio del Curso
                var precioEntidad = _context.Precio.Where(x => x.CursoId == curso.CursoId).FirstOrDefault();
                if (precioEntidad != null)
                {
                    precioEntidad.Promocion = request.Promocion ?? precioEntidad.Promocion;
                    precioEntidad.PrecioActual = request.Precio ?? precioEntidad.PrecioActual;
                }
                else
                {
                    precioEntidad = new Precio
                    {
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.Precio ?? 0,
                        Promocion = request.Promocion ?? 0,
                        CursoId = curso.CursoId
                    };

                    await _context.Precio.AddAsync(precioEntidad);
                }

                if (request.ListaInstructor != null)
                {
                    if (request.ListaInstructor.Count > 0)
                    {
                        var instrucoresBD = _context.CursoInstructor.Where(x => x.CursoId == request.CursoId);
                        // Eliminados todos los Instructores que estan en la base
                        foreach (var instructor in instrucoresBD)
                        {
                            _context.CursoInstructor.Remove(instructor);
                        }

                        // Insertamos todos los instructores nuevos traidos desde el front
                        foreach (var ids in request.ListaInstructor)
                        {
                            var nuevoInstructor = new CursoInstructor
                            {
                                CursoId = request.CursoId,
                                InstructorId = ids
                            };

                            _context.CursoInstructor.Add(nuevoInstructor);
                        }
                    }
                }

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se guardaron los cambios en el curso");
            }
        }
    }
}
