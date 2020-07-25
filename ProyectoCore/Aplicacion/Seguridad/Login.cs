using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
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
                private readonly CursosOnlineContext _cursosOnlineContext;

                public class EjecutaValidacion : AbstractValidator<Ejecuta>
                {
                    public EjecutaValidacion()
                    {
                        RuleFor(x => x.Email).NotEmpty();
                        RuleFor(x => x.Password).NotEmpty();
                    }
                }
                public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador, CursosOnlineContext cursosOnlineContext)
                {
                    _userManager = userManager;
                    _signInManager = signInManager;
                    _jwyGenerador = jwtGenerador;
                    _cursosOnlineContext = cursosOnlineContext;
                }

                public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
                {
                    var usuario = await _userManager.FindByEmailAsync(request.Email);
                    if (usuario == null)
                    {
                        throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                    }

                    var result = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);
                    var resultRoles = await _userManager.GetRolesAsync(usuario); // Roles del usuario
                    var listaRoles = new List<string>(resultRoles);

                    var imagenPerfil = await _cursosOnlineContext.Documento.Where(x => x.ObjetoReferencia == new Guid(usuario.Id)).FirstOrDefaultAsync();

                    if (result.Succeeded)
                    {
                        if (imagenPerfil != null)
                        {
                            var imagenCliente = new ImagenGeneral
                            {
                                Data = Convert.ToBase64String(imagenPerfil.Contenido),
                                Extension = imagenPerfil.Extension,
                                Nombre = imagenPerfil.Nombre
                            };

                            return new UsuarioData
                            {
                                NombreCompleto = usuario.NombreCompleto,
                                Token = _jwyGenerador.CrearToken(usuario, listaRoles),
                                Username = usuario.UserName,
                                Email = usuario.Email,
                                ImagenPerfil = imagenCliente
                            };
                        }
                        else
                        {
                            return new UsuarioData
                            {
                                NombreCompleto = usuario.NombreCompleto,
                                Token = _jwyGenerador.CrearToken(usuario, listaRoles),
                                Username = usuario.UserName,
                                Email = usuario.Email,
                                Imagen = null
                            };
                        }

                    }

                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}
