using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Creamos el controller de Inicio
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/HTML");
        }
    }
}