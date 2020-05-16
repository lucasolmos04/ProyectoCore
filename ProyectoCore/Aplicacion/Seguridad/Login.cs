using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        /// <summary>
        /// Clase para manejar el loggeo de los usuarios
        /// </summary>
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Email { get; set; }
            public string Password { get; set; }

            public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
            {
                private readonly UserManager<Usuario> _userManager;
                private readonly SignInManager<Usuario> _signInManager;
                private readonly IJwtGenerador _jwyGenerador;

                public class EjecutaValidacion : AbstractValidator<Ejecuta>
                {
                    public EjecutaValidacion()
                    {
                        RuleFor(x => x.Email).NotEmpty();
                        RuleFor(x => x.Password).NotEmpty();
                    }
                }
                public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador)
                {
                    _userManager = userManager;
                    _signInManager = signInManager;
                    _jwyGenerador = jwtGenerador;
                }

                public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
                {
                    var usuario = await _userManager.FindByEmailAsync(request.Email);
                    if (usuario == null)
                    {
                        throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                    }

                    var result = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

                    if (result.Succeeded)
                    {
                        return new UsuarioData {
                            NombreCompleto = usuario.NombreCompleto,
                            Token = _jwyGenerador.CrearToken(usuario),
                            Username = usuario.UserName,
                            Email = usuario.Email,
                            Imagen = null
                        };
                    }

                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}
