using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class Editar
    {
        /// <summary>
        /// Edita un Instructor
        /// </summary>
        public class Ejecuta : IRequest
        {
            public Guid InstructorId { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Titulo { get; set; }
        }

        public class EjecutaValida : AbstractValidator<Ejecuta>
        {
            public EjecutaValida()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
                RuleFor(x => x.Titulo).NotEmpty();
            }

            public class Manejador : IRequestHandler<Ejecuta>
            {
                private readonly IInstructor _instructorRepository;

                public Manejador(IInstructor instructorRepository)
                {
                    _instructorRepository = instructorRepository;
                }
                public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
                {
                    var result = await _instructorRepository.Actualizar(request.InstructorId, request.Nombre, request.Apellido, request.Titulo);
                    if (result > 0)
                    {
                        return Unit.Value;
                    }

                    throw new Exception("No se pudo actualizar la data del instructor");
                }
            }
        }
    }

}
