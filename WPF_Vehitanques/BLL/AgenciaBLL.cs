using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;


namespace WPF_Vehitanques.BLL
{
 public    class AgenciaBLL
    {
        ComercialPEntities contex = new ComercialPEntities();

        public IList<VehiT_ConsultaAgenciaPorID_Result> GetAgenciaPoxID(short  idagencia)
        {
            var lista = contex.VehiT_ConsultaAgenciaPorID (idagencia);
            return lista.ToList();
        }

        public List<VehiT_ConsultaAgencias_Result> GetAgencias()
        {
            var lista = contex.VehiT_ConsultaAgencias();
            return lista.ToList();
        }
    }
}
