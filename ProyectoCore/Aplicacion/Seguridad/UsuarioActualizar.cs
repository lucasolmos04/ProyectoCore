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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class UsuarioActualizar 
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string NombreCompleto { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
            public ImagenGeneral ImagenPerfil { get; set; }
        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>
        {
            public EjecutaValidador()
            {
                RuleFor(x => x.NombreCompleto).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnlineContext _cursosOnlineContext;
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly IPasswordHasher<Usuario> _passwordHasher;

            public Manejador(CursosOnlineContext cursosOnlineContext, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IPasswordHasher<Usuario> passwordHasher)
            {
                _cursosOnlineContext = cursosOnlineContext;
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
                _passwordHasher = passwordHasher;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuarioIden = await _userManager.FindByNameAsync(request.Username);
                if (usuarioIden == null)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, new { mensaje = "no existe un usuario con este Username" });
                }

                var result = await _cursosOnlineContext.Users.Where(x => x.Email == request.Email && x.UserName != request.Username).AnyAsync();
                if (result)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.InternalServerError, new { mensaje = "Este mail pertenece a otro usuario" });
                }

                // Buscamos la imagen por el id del Usuario
                var resultImagen = await _cursosOnlineContext.Documento.Where(x => x.ObjetoReferencia == new Guid(usuarioIden.Id)).FirstOrDefaultAsync();
                if (resultImagen == null)
                {
                    var imagen = new Documento
                    {
                        Contenido = Convert.FromBase64String(request.ImagenPerfil.Data),
                        Nombre = request.ImagenPerfil.Nombre,
                        Extension = request.ImagenPerfil.Extension,
                        ObjetoReferencia = new Guid(usuarioIden.Id),
                        DocumentoId = Guid.NewGuid(),
                        FechaCreacion = DateTime.UtcNow
                    };

                    _cursosOnlineContext.Documento.Add(imagen);
                }
                else
                {
                    resultImagen.Contenido = request.ImagenPerfil != null ?  Convert.FromBase64String(request.ImagenPerfil.Data) : resultImagen.Contenido;
                    resultImagen.Nombre = request.ImagenPerfil != null ? request.ImagenPerfil.Nombre : resultImagen.Nombre;
                    resultImagen.Extension = request.ImagenPerfil != null ? request.ImagenPerfil.Extension : resultImagen.Extension;
                }

                usuarioIden.NombreCompleto = request.NombreCompleto;
                usuarioIden.PasswordHash = _passwordHasher.HashPassword(usuarioIden, request.Password);
                usuarioIden.Email = request.Email;

                var resultadoUpdate = await _userManager.UpdateAsync(usuarioIden);
                var resultRoles = await _userManager.GetRolesAsync(usuarioIden);
                var listRoles = new List<string>(resultRoles);

                var imagePerfil = await _cursosOnlineContext.Documento.Where(x => x.ObjetoReferencia == new Guid(usuarioIden.Id)).FirstAsync();
                ImagenGeneral imagenGeneral = null;

                if (imagePerfil != null)
                {
                    imagenGeneral = new ImagenGeneral
                    {
                        Data = Convert.ToBase64String(imagePerfil.Contenido),
                        Nombre = imagePerfil.Nombre,
                        Extension = imagePerfil.Extension
                    };
                }


                if (resultadoUpdate.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuarioIden.NombreCompleto,
                        Username = usuarioIden.UserName,
                        Email = usuarioIden.Email,
                        Token = _jwtGenerador.CrearToken(usuarioIden, listRoles),
                        ImagenPerfil = imagenGeneral
                    };
                }

                throw new Exception("No se pudo actualizar el usuario");
            }
        }
    }
}
