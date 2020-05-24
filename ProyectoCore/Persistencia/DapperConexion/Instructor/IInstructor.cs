using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructor
{
    /// <summary>
    /// Operaciones a realizar sobre un Instructor
    /// </summary>
    public interface IInstructor
    {
        Task<IEnumerable<InstructorModel>> ObtenerLista();
        Task<InstructorModel> ObtenerPorId(Guid id);
        Task<int> Nuevo(InstructorModel param);
        Task<int> Actualizar(InstructorModel param);
        Task<int> Eliminar(Guid id);
    }
}
