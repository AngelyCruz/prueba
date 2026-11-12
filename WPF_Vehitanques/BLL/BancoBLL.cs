using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;

namespace WPF_Vehitanques.BLL
{
   public  class BancoBLL
    {

        ComercialPEntities contex = new ComercialPEntities();
        public List<VehiT_ConsultaBancoConductor_Result> GetBancosConductor()
        {
            var lista = contex.VehiT_ConsultaBancoConductor();
            return lista.ToList();
        }
    }
}
