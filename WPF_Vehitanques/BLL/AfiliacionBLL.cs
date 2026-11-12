using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;

namespace WPF_Vehitanques.BLL
{
 public    class AfiliacionBLL
    {
        ComercialPEntities contex = new ComercialPEntities();

        public List<TipoAfiliacion> GetTipoAfiliacion()
        {
            var lista = (from Tp in contex.TipoAfiliacion
                         select Tp);
            return lista.ToList();
        }
    }
}
