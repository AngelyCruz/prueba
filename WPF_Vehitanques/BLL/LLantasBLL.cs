using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;


namespace WPF_Vehitanques.BLL
{
    public class LLantasBLL
    {
        ComercialPEntities contex = new ComercialPEntities();
        public List<GruposVehiculos> GetGrupollantas()
        {
            var lista = (from GL in contex.GruposVehiculos
                         select GL);
            return lista.ToList();
        }
    }
}
