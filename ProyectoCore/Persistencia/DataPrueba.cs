using Dominio;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    public class DataPrueba
    {
        /// <summary>
        /// Insertara un usuario solo cuando no existan usuarios dentro de la base de datos
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public static async Task InsertarData(CursosOnlineContext context, UserManager<Usuario> userManager)
        {
            if (!userManager.Users.Any())
            {
                var usuario = new Usuario { NombreCompleto = "Lucas Olmos", UserName = "lucasolmos", Email = "lucasolmos@gmail.com" };
                await userManager.CreateAsync(usuario, "Pass123$");
            }
        }
    }
}
