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
  public   class Andr_NotificacionesUsuarioBLL
    {
      AndroidEntities context = new AndroidEntities();
      MensajeBLL mensajebll;
      VentanaMensaje ventmensaje;


      public List<And_ConsultaNotificacionUsrPorId_Result> GetNotificacionUsrPorId(decimal idnotifi)
      {
          var lista = context.And_ConsultaNotificacionUsrPorId(idnotifi);

          return lista.ToList();
      }

      public Boolean UpdateEstadoNotificacionesUsuario( string usuarioactua, decimal idnotificacion,int idestado)
      {
          try
          {
              DS.DSAndroidTableAdapters.And_UpdateEstadoNotificacionesUsuarioTableAdapter DSudpEstadoNotUsr = new DS.DSAndroidTableAdapters.And_UpdateEstadoNotificacionesUsuarioTableAdapter();
              DSudpEstadoNotUsr.Get_And_UpdateEstadoNotificacionesUsuario ( usuarioactua, idnotificacion,idestado );

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


      public Boolean UpdateNotificacionesUsuario( string descripnotifi, string usuarioactua, decimal idnotificacion)
      {
          try
          {
              DS.DSAndroidTableAdapters.And_UpdateNotificacionesUsuarioTableAdapter DSudpNotifiUser = new DS.DSAndroidTableAdapters.And_UpdateNotificacionesUsuarioTableAdapter();
              DSudpNotifiUser.Get_And_UpdateNotificacionesUsuario (descripnotifi, usuarioactua, idnotificacion);

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


      public Boolean GuardarNotificacionesUsuario(decimal iduser,string descripnotifi,string usuariocrea)
      {
          try
          {
              DS.DSAndroidTableAdapters.And_GuardaNotificacionesUsuarioTableAdapter DSguardaNotifiUser = new DS.DSAndroidTableAdapters.And_GuardaNotificacionesUsuarioTableAdapter();
              DSguardaNotifiUser.Get_And_GuardaNotificacionesUsuario(iduser, descripnotifi, usuariocrea);

              return true;
          }
          catch(Exception ex){
              ventmensaje = new VentanaMensaje();
              ventmensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
              ventmensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
              ventmensaje.cargarmensaje("Error", ex.Message.ToString());
              ventmensaje.ShowDialog();
              return false;
          }
      }



      public List<And_ConsultaTotalNofiticacionesUsuarioActivas_Result> GetNotificacionesUsuario(decimal iduser, int istar, int iend)
      {
          var lista = context.And_ConsultaTotalNofiticacionesUsuarioActivas(iduser, istar, iend);
          return lista.ToList();
      }

      public int ContadorNotificacionesUsuario(decimal iduser)
      {
          int retorno = 0;
          var lista = context.And_ConsultaContadorTotalNofiticacionesUsuarioActivas(iduser);
          var lista2 = lista.ToList();
          if (lista2.Count > 0)
          {

              retorno = Convert.ToInt32(lista2[0].Value);
          }
          return retorno;
      }


    }
}
