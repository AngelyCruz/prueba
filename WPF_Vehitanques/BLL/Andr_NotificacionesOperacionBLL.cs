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
  public   class Andr_NotificacionesOperacionBLL
    {
        AndroidEntities context = new AndroidEntities();
        MensajeBLL mensajebll;
        VentanaMensaje ventmensaje;


        public List<And_ConsultaNotificacionOperPorId_Result> GetNotificacionOperPorId(decimal idnotifi)
        {
            var lista = context.And_ConsultaNotificacionOperPorId(idnotifi);

            return lista.ToList();
        }
    
        public Boolean UpdateEstadoNotificacionesOperacion(string usuarioactua, decimal idnotificacion, int idestado)
        {
            try
            {
                DS.DSAndroidTableAdapters.And_UpdateEstadoNotificacionesOperacionTableAdapter DSudpEstadoNotOper = new DS.DSAndroidTableAdapters.And_UpdateEstadoNotificacionesOperacionTableAdapter();
                DSudpEstadoNotOper.Get_And_UpdateEstadoNotificacionesOperacion(usuarioactua, idnotificacion, idestado);

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

        public Boolean UpdateNotificacionesOperacion(string descripnotifi, string usuarioactua, decimal idnotificacion)
        {
            try
            {
                DS.DSAndroidTableAdapters.And_UpdateNotificacionesOperacionTableAdapter DSudpNotifiUser = new DS.DSAndroidTableAdapters.And_UpdateNotificacionesOperacionTableAdapter();
                DSudpNotifiUser.Get_And_UpdateNotificacionesOperacion(descripnotifi, usuarioactua, idnotificacion);

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


        public Boolean GuardarNotificacionesOperacion(decimal NitOper, string descripnotifi, string usuariocrea)
        {
            try
            {
                DS.DSAndroidTableAdapters.And_GuardaNotificacionesOperacionTableAdapter DSguardaNotifiUser = new DS.DSAndroidTableAdapters.And_GuardaNotificacionesOperacionTableAdapter();
                DSguardaNotifiUser.Get_And_GuardaNotificacionesOperacion(NitOper, descripnotifi, usuariocrea);

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



        public List<And_ConsultaTotalNofiticacionesOperacionActivas_Result > GetNotificacionesOperacion(decimal nitoper, int istar, int iend)
        {
            var lista = context.And_ConsultaTotalNofiticacionesOperacionActivas(nitoper, istar, iend);
            return lista.ToList();
        }

        public int ContadorNotificacionesOperacion(decimal nitoper)
        {
            int retorno = 0;
            var lista = context.And_ConsultaContadorTotalNofiticacionesOperacionActivas(nitoper);
            var lista2 = lista.ToList();
            if (lista2.Count > 0)
            {

                retorno = Convert.ToInt32(lista2[0].Value);
            }
            return retorno;
        }

    }
}
