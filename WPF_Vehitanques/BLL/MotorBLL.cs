using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;

namespace WPF_Vehitanques.BLL
{
    public class MotorBLL
    {

        ComercialPEntities contex = new ComercialPEntities();
        public List<TipoMotorVehiculo> GetTipoMotorVehiculo()
        {
            var lista = (from m in contex.TipoMotorVehiculo
                         select m);
            return lista.ToList();
        }
    }
}
