using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class RolEliminar
    {
        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }
        }

        public class EjecutaValida : AbstractValidator<Ejecuta>
        {
            public EjecutaValida()
            {
                RuleFor(x => x.Nombre).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly RoleManager<IdentityRole> _rolManager;
            public Manejador(RoleManager<IdentityRole> roleManager)
            {
                _rolManager = roleManager;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var role = await _rolManager.FindByNameAsync(request.Nombre);
                if (role == null)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.BadRequest, new { mensaje = "No existe el rol" });
                }

                var result = await _rolManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el rol");
            }
        }
    }
}
