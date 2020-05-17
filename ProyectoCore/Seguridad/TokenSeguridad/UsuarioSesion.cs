using Aplicacion.Contratos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Seguridad.TokenSeguridad
{
    /// <summary>
    /// Capa se Seguridad
    /// </summary>
    public class UsuarioSesion : IUsuarioSesion
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsuarioSesion(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Obtenemos el usuario que se encuentra en Sesion
        /// </summary>
        /// <returns></returns>
        public string ObtenerUsuarioSesion()
        {
            // HttpContext => Valores en sesion
            // UserName viene desde los claims
            var userName = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return userName;
        }
    }
}
