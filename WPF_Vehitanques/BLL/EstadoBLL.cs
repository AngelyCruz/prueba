using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;

namespace WPF_Vehitanques.BLL
{
  public   class EstadoBLL
    {
        ComercialPEntities contex = new ComercialPEntities();

        public List<VehiT_ConsultaEstadoPlaca_Result> GetEstadoPlaca()
        {
            var lista = contex.VehiT_ConsultaEstadoPlaca();
            return lista.ToList();
        }
    }
}
