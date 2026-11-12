using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;

namespace WPF_Vehitanques.BLL
{
  public   class EmpresaBLL
    {
      ComercialPEntities context = new ComercialPEntities();


      public List<VehiT_ConsultaEmpresaComp_Result> GetEmpresasConductor()
      {
          var lista = context.VehiT_ConsultaEmpresaComp();
          return lista.ToList();
      }



    }
}
