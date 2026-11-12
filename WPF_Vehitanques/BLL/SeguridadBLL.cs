using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;


namespace WPF_Vehitanques.BLL
{
    public class SeguridadBLL
    {
        aspnetdb_ColtanquesEntities Contextsecurity = new aspnetdb_ColtanquesEntities();

        public List<seguridad_consultarMenuPermisos2_Result> GetListaPermisos(string username)
        {
            var lista = Contextsecurity.seguridad_consultarMenuPermisos2("wpfVehitanques", username);
            return lista.ToList();

        }
        public Boolean GuardarAuditoriaUsrNoActualizan(string nombreapp, string userapp, string versionpc, string versionpublicada, string ippc, string hostname)
        {
            try
            {
                DS.DSSecurityTableAdapters.Gap_REgsitroUsuariosSinActualizacionesTableAdapter dssecurity = new DS.DSSecurityTableAdapters.Gap_REgsitroUsuariosSinActualizacionesTableAdapter();

                dssecurity.Get_Gap_REgsitroUsuariosSinActualizaciones(nombreapp, userapp, versionpc, versionpublicada, ippc, hostname);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
