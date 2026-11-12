using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;

namespace WPF_Vehitanques.BLL
{
   public  class AseguradoraBLL
    {
        ComercialPEntities contex = new ComercialPEntities();

        public List<Aseguradoras> GetASeguradora()
        {
            var lista = (from Ase in contex.Aseguradoras
                         select Ase).OrderBy(p=>p.Nombre);
            return lista.ToList();
        }
    }
}
