using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;


namespace WPF_Vehitanques.BLL
{
    public class UnidadMedidaBLL
    {
        ComercialPEntities contex = new ComercialPEntities();

        public List<VehiT_ConsultaUnidadDeMedidaPesoCarga_Result> GetUnidadMedida()
        {
            var lista = contex.VehiT_ConsultaUnidadDeMedidaPesoCarga();
            return lista.ToList();
        }
    }
}
