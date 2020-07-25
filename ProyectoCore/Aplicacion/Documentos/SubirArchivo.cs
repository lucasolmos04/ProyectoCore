using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Documentos
{
    public class SubirArchivo
    {
        public class Ejecuta : IRequest
        {
            public Guid ObjetoReferencia { get; set; }
            public string Data { get; set; }
            public string Nombre { get; set; }
            public string Extension { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _cursosOnlineContext;

            public Manejador(CursosOnlineContext cursosOnlineContext)
            {
                _cursosOnlineContext = cursosOnlineContext;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var documento = await _cursosOnlineContext.Documento.Where(x => x.ObjetoReferencia == request.ObjetoReferencia).FirstAsync();
                if (documento == null)
                {
                    var doc = new Documento
                    {
                        Contenido = Convert.FromBase64String(request.Data),
                        Nombre = request.Nombre,
                        Extension = request.Extension,
                        DocumentoId = Guid.NewGuid(),
                        FechaCreacion = DateTime.UtcNow
                    };
                    _cursosOnlineContext.Documento.Add(doc);
                }
                else
                {
                    documento.Contenido = Convert.FromBase64String(request.Data);
                    documento.Nombre = request.Nombre;
                    documento.Extension = request.Extension;
                    documento.FechaCreacion = DateTime.UtcNow;
                }

                var result = await _cursosOnlineContext.SaveChangesAsync();
                if (result > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo guardar el archivo");
            }
        }
    }
}
