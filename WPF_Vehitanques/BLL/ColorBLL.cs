using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;
namespace WPF_Vehitanques.BLL
{
  public   class ColorBLL
    {

        ComercialPEntities contex = new ComercialPEntities();
        public List<VehiT_ConsultaColoresVehiculo_Result> Getcolores()
        {
            var lista = contex.VehiT_ConsultaColoresVehiculo();
            return lista.ToList();
        }
    }
}
