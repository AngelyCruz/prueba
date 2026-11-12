using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;

namespace WPF_Vehitanques.BLL
{
   public  class AnalistaBLL
    {

       ComercialPEntities context = new ComercialPEntities();
       public List<VehiT_ConsultaAnalistas_Result > GetAnalistasConductor()
       {
           var lista = context.VehiT_ConsultaAnalistas();
           return lista.ToList();
       }

    }
}
