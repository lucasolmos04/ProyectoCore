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
    public class Registrar
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string UserName { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }

        }

        // Validaciones
        public class EjecutaValidador : AbstractValidator<Ejecuta>
        {
            public EjecutaValidador()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnlineContext _context;
            private readonly UserManager<Usuario> _userManeger;
            private readonly IJwtGenerador _jwtGenerador;
            public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador)
            {
                _context = context;
                _userManeger = userManager;
                _jwtGenerador = jwtGenerador;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // AnyAsync Retorna un boolean
                // Validamos si existe un usuario con ese Email
                var existe = await _context.Users.Where(x => x.Email == request.Email).AnyAsync();

                if (existe)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.BadRequest, new { mensaje = "Existe un usuario con este Email" });
                }

                // Validamos si existe un usuario con ese UserName
                var existeUserName = await _context.Users.Where(x => x.UserName == request.UserName).AnyAsync();

                if (existeUserName)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.BadRequest, new { mensaje = "Existe ya el usuario con este userName" });
                }

                // Creamos el usuario
                var usuario = new Usuario
                {
                    NombreCompleto = request.Nombre + " " + request.Apellido,
                    Email = request.Email,
                    UserName = request.UserName
                };

                var result = await _userManeger.CreateAsync(usuario, request.Password);

                // Si se insertó, lo mostramos
                if (result.Succeeded)
                {
                    return new UsuarioData {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = _jwtGenerador.CrearToken(usuario),
                        Username = usuario.UserName,
                        Email = usuario.Email
                    };
                }

                throw new Exception("No se pudo agregar el nuevo usuario");
            }
        }
    }
}
