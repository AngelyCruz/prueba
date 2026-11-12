using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WPF_Vehitanques.Modelo;
using WPF_Vehitanques.Presentacion.Ventanas;

namespace WPF_Vehitanques.BLL
{
    public   class AndroidBLL
    {
        AndroidEntities context = new AndroidEntities();
        MensajeBLL mensajebll;
        VentanaMensaje ventmensaje;
        DS.DSAndroidTableAdapters.And_CrearUsuarioTableAdapter DSCreauser = new DS.DSAndroidTableAdapters.And_CrearUsuarioTableAdapter();
        DS.DSAndroidTableAdapters.And_UpdateDatosUsuarioTableAdapter DSUpdate = new DS.DSAndroidTableAdapters.And_UpdateDatosUsuarioTableAdapter();

        public Boolean UpdateDatosUsuario(decimal IdUsuario, string Clave, string Nombre,
           string apellidos, decimal nitoperacion, int estado, string username)
        {
            try
            {
                DSUpdate.Get_And_UpdateDatosUsuario(IdUsuario, Clave, Nombre, apellidos, nitoperacion, estado,username);
                return true;
            }
            catch (Exception ex)
            {
                ventmensaje = new VentanaMensaje();
                ventmensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                ventmensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                ventmensaje.cargarmensaje("Error", ex.Message.ToString());
                ventmensaje.ShowDialog();
                return false;
            }
        }

        public Boolean GuardarDatosUsuario(decimal IdUsuario	,string Clave,string Nombre,
            string apellidos, decimal nitoperacion, int estado, string username)
        {
            try
            {
                DSCreauser.Get_And_CrearUsuario(IdUsuario, Clave, Nombre, apellidos, nitoperacion, estado, username);
                return true;
            }
            catch (Exception ex)
            {
                ventmensaje = new VentanaMensaje();
                ventmensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                ventmensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                ventmensaje.cargarmensaje("Error", ex.Message.ToString());
                ventmensaje.ShowDialog();
                return false;
            }
        }

        public List<And_ConsultaUsuario_Result > GetDatosUsuarioAndroid(decimal idusario)
        {
            var lista = context.And_ConsultaUsuario (idusario );
            return lista.ToList();
        }
        public List<And_ConsultaEstados_Result> GetEstadosAndroid()
        {
            var lista = context.And_ConsultaEstados();
            return lista.ToList();
        }
        public List<And_ConsultaOperacion_Result > GetOperacionAndroid()
        {
            var lista = context.And_ConsultaOperacion();
            return lista.ToList();
        }
    }
}
