using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;


namespace WPF_Vehitanques
{

    class SecurityVariables
    {
        static string pStrUsuario;
        static Int32 pIntAgencia;
        static List<seguridad_consultarMenuPermisos2_Result> pPerfilUsuario;
        static List<string> pStrBotones = new List<string>();

        public static String PStrUsuario
        {
            get { return pStrUsuario; }
            set { pStrUsuario = value; }
        }

        public static Int32 PIntAgencia
        {
            get { return pIntAgencia; }
            set { pIntAgencia = value; }
        }

        public static List<seguridad_consultarMenuPermisos2_Result> PPerfilUsuario
        {
            get { return pPerfilUsuario; }
            set { pPerfilUsuario = value; }
        }

    }
}
