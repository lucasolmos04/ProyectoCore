using System;
using System.Collections.Generic;
using System.Text;

namespace Persistencia.DapperConexion.Paginacion
{
    public class PaginacionModel
    {
        public List<IDictionary<string,object>> ListaRecords { get; set; }
        public int TotalRecords { get; set; }
        public int NumerosPaginas { get; set; }
    }
}
