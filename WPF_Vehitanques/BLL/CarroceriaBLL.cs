using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;


namespace WPF_Vehitanques.BLL
{
   public  class CarroceriaBLL
    {

        ComercialPEntities contex = new ComercialPEntities();

        public List<VehiT_ConsultaTipoCarroceria_Result> GetTipoCarroceria()
        {
            var lista = contex.VehiT_ConsultaTipoCarroceria();
            return lista.ToList();
        }

    }
}
