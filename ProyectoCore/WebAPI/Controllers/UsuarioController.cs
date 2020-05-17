using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [AllowAnonymous] // Declaramos a Api de Login para que no requiera autorizacion
    public class UsuarioController : MiControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>> Login(Login.Ejecuta param)
        {
            return await Mediator.Send(param);
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioData>> Registrar(Registrar.Ejecuta param)
        {
            return await Mediator.Send(param);
        }

        [HttpGet]
        public async Task<ActionResult<UsuarioData>> GetUsuario()
        {
            return await Mediator.Send(new UsuarioActual.Ejecutar());
        }
    }
}