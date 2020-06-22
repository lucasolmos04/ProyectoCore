using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class ObtenerRolesPorUsuario
    {
        public class Ejecuta : IRequest<List<string>>
        {
            public string Username { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, List<string>>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }
            public async Task<List<string>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(request.Username);
                if (usuario == null)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, new { mensaje = "No existe el usuario" });
                }

                var result = await _userManager.GetRolesAsync(usuario);
                return new List<string>(result);
            }
        }
    }
}
