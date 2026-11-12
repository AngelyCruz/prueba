using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;


namespace WPF_Vehitanques.BLL
{
    public class TipoDocumentoBLL
    {
        ComercialPEntities contex = new ComercialPEntities();
        public List<TipoDocIdentificacion_V1> GetTipoDocumentoIdentificacion()
        {
            var lista = (from TiDoc in contex.TipoDocIdentificacion_V1
                         where TiDoc.Estado==1
                         select TiDoc 
                           );
            return lista.ToList();
        }
    }
}
