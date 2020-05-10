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
        public class Ejecuta : IRequest<Usuario>
        {
            public string Email { get; set; }
            public string Password { get; set; }

            public class Manejador : IRequestHandler<Ejecuta, Usuario>
            {
                private readonly UserManager<Usuario> _userManager;
                private readonly SignInManager<Usuario> _signInManager;

                public class EjecutaValidacion : AbstractValidator<Ejecuta>
                {
                    public EjecutaValidacion()
                    {
                        RuleFor(x => x.Email).NotEmpty();
                        RuleFor(x => x.Password).NotEmpty();
                    }
                }
                public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
                {
                    _userManager = userManager;
                    _signInManager = signInManager;
                }

                public async Task<Usuario> Handle(Ejecuta request, CancellationToken cancellationToken)
                {
                    var usuario = await _userManager.FindByEmailAsync(request.Email);
                    if (usuario == null)
                    {
                        throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                    }

                    var result = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

                    if (result.Succeeded)
                    {
                        return usuario;
                    }

                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}
