using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;


namespace WPF_Vehitanques.BLL
{
    public  class CiudadBLL
    {

        ComercialPEntities contex = new ComercialPEntities();
        public List<VehiT_ConsultaCiudad_Result> GetCiudades(int departamento)
        {
            var lista = contex.VehiT_ConsultaCiudad(departamento);
            return lista.ToList();
        }
    }
}
