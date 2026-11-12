using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;

namespace WPF_Vehitanques.BLL
{
    class CorreoBLL
    {
        ComercialPEntities contex = new ComercialPEntities();

        public IList<string> GetCorreosAgencia()
        {
            var lista = contex.VehiT_ConsultaCorreosCopia();

            return lista.ToList();
        }
    }
}
