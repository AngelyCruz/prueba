using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;

namespace WPF_Vehitanques.BLL
{
    public class LicenciaBLL
    {
        ComercialPEntities contex = new ComercialPEntities();
        public List<VehiT_ConsultaCategorialicencia_Result> GetCategoriaLicencia()
        {
            var lista = contex.VehiT_ConsultaCategorialicencia();
            return lista.ToList();
        }
    }
}
