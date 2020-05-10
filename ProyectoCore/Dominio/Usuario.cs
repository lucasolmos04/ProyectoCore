using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    /// <summary>
    /// Clase que contendra los usuarios del sistema
    /// </summary>
    public class Usuario : IdentityUser
    {
        public string NombreCompleto { get; set; }
    }
}
