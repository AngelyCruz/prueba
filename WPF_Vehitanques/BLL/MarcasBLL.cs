using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;

namespace WPF_Vehitanques.BLL
{
    public class MarcasBLL
    {
        ComercialPEntities contex = new ComercialPEntities();
        public IList<VehiT_ConsultaMarcaVEhiculoXID_Result> GetMarcaPorID(int marca)
        {
            var lista = contex.VehiT_ConsultaMarcaVEhiculoXID(marca);
            return lista.ToList();

        }

        public List<VehiT_ConsultMarcasRemolque_Result> GetMarcasRemolque()
        {
            var lista = contex.VehiT_ConsultMarcasRemolque();
            return lista.ToList();
        }

        public List<VehiT_ConsultMarcasVehiculo_Result> GetMarcasVehiculo()
        {
            var lista = contex.VehiT_ConsultMarcasVehiculo();
            return lista.ToList();
        }


    }
}
