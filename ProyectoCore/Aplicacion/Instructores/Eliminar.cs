using MediatR;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class Eliminar
    {
        /// <summary>
        /// Elimina un Instructor
        /// </summary>
        public class Ejecuta : IRequest
        {
            public Guid InstructorId { get; set; }
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
                var result = await _instructorRepository.Eliminar(request.InstructorId);
                if (result > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el instructor");
            }
        }
    }
}
