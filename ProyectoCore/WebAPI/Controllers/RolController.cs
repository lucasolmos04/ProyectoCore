using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace WebAPI.Controllers
{
    public class RolController : MiControllerBase
    {
        [HttpPost("crear")]
        public async Task<ActionResult<Unit>> Crear(RolNuevo.Ejecuta param)
        {
            return await Mediator.Send(param);
        }

        [HttpDelete("eliminar")]
        public async Task<ActionResult<Unit>> Elminar(RolEliminar.Ejecuta param)
        {
            return await Mediator.Send(param);
        }
        
        [HttpGet("lista")]
        public async Task<ActionResult<List<IdentityRole>>> Lista()
        {
            return await Mediator.Send(new RolLista.Ejecuta());
        }

        [HttpPost("agregarRolUsuario")]
        public async Task<ActionResult<Unit>> AgrearRolUsuario(UsuarioRolAgregar.Ejecuta param)
        {
            return await Mediator.Send(param);
        }

        [HttpPost("eliminarRolUsuario")]
        public async Task<ActionResult<Unit>> EliminarRolUsuario(UsuarioRolEliminar.Ejecuta param)
        {
            return await Mediator.Send(param);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<List<string>>> ObtenerRolesPorUsuario(string username)
        {
            return await Mediator.Send(new ObtenerRolesPorUsuario.Ejecuta { Username = username });
        }
    }
}
