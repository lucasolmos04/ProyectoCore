using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Persistencia.DapperConexion.Paginacion
{
    public class PaginacionRepositorio : IPaginacion
    {
        public readonly IFactoryConnection _factoryConnection;

        public PaginacionRepositorio(IFactoryConnection factory)
        {
            _factoryConnection = factory;
        }
        public async Task<PaginacionModel> devolverPaginacion(string storeProcedure, int numeroPagina, int cantidadElementos, IDictionary<string, object> parametrosFiltro, string ordenamientoColumna)
        {
            PaginacionModel paginacionModel = new PaginacionModel();
            List<IDictionary<string, object>> listaReporte = new List<IDictionary<string, object>>();
            int totalRecords = 0;
            int totalPaginas = 0;

            try
            {
                var connetion = _factoryConnection.GetConnection();
                DynamicParameters parametros = new DynamicParameters();

                // Filtros
                foreach (var param in parametrosFiltro)
                {
                    parametros.Add("@" + param.Key, param.Value);
                }

                // Paramentros de Entrada
                parametros.Add("@NumeroPagina", numeroPagina);
                parametros.Add("@CantidadElementos", cantidadElementos);
                parametros.Add("@Ordenamiento", ordenamientoColumna);

                // Parametros de Salida
                parametros.Add("@TotalRecords", totalRecords, DbType.Int32, ParameterDirection.Output);
                parametros.Add("@TotalPaginas", totalPaginas, DbType.Int32, ParameterDirection.Output);

                var result = await connetion.QueryAsync(storeProcedure, parametros, commandType: CommandType.StoredProcedure);
                listaReporte = result.Select(x => (IDictionary < string, object>)x).ToList();

                paginacionModel.ListaRecords = listaReporte;
                paginacionModel.NumerosPaginas = parametros.Get<int>("@TotalPaginas");
                paginacionModel.TotalRecords = parametros.Get<int>("@TotalRecords");
            }
            catch (Exception ex)
            {

                throw new Exception("No se pudo ejecutar el procedimiento almacenado");
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }

            return paginacionModel;
        }
    }
}
