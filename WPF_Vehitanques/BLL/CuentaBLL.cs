using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;

namespace WPF_Vehitanques.BLL
{
    public class CuentaBLL
    {
        ComercialPEntities contex = new ComercialPEntities();

        public List<VehiT_ConsultaTipoCuenta_Result> GetTiposcuenta()
        {
            var lista = contex.VehiT_ConsultaTipoCuenta();
            return lista.ToList();
        }


    }
}
