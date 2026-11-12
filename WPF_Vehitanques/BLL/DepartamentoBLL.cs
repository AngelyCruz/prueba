using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;


namespace WPF_Vehitanques.BLL
{
    public class DepartamentoBLL
    {

        ComercialPEntities contex = new ComercialPEntities();
        public List<VehiT_ConsultaDepartamento_Result> GetDepartamentos(int pais)
        {
            var lista = contex.VehiT_ConsultaDepartamento(pais);
            return lista.ToList();
        }
    }
}
