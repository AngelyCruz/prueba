using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;


namespace WPF_Vehitanques.BLL
{
    public class TanqueBLL
    {
        ComercialPEntities contex = new ComercialPEntities();
        public List<ClaseTanque> GetClaseTanque()
        {
            var lista = from b in contex.ClaseTanque
                        select b;
            return lista.ToList();
        }


    }
}
