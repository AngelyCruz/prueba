using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;


namespace WPF_Vehitanques.BLL
{
    public class LineaBLL
    {

        ComercialPEntities contex = new ComercialPEntities();

        public List<VehiT_ConsultaLineaVehiculo_Result> GetlineaPorMarca(int idmarca)
        {
            var lista = contex.VehiT_ConsultaLineaVehiculo(idmarca);
            return lista.ToList();
        }
    }
}
