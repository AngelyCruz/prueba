using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;
using WPF_Vehitanques.DS.DSVetoTableAdapters ;
using WPF_Vehitanques.Presentacion.Ventanas;
using System.Windows.Media;



namespace WPF_Vehitanques.BLL
{
    
  public   class VetoBLL
    {
        MensajeBLL mensajebll;
        VentanaMensaje ventmensaje;
      ComercialPEntities contex = new ComercialPEntities();
      Vehit_GuardarDatosPersonaVetoTableAdapter dsguardapersonaveto = new Vehit_GuardarDatosPersonaVetoTableAdapter();
      Vehit_GuardarDatosVehiculoVetoTableAdapter dsguardavehiculoveto = new Vehit_GuardarDatosVehiculoVetoTableAdapter();
      Vehit_UpdateDatosPersonaVetoTableAdapter dsupdatepersonaveto = new Vehit_UpdateDatosPersonaVetoTableAdapter();
      Vehit_UpdateDatosVehiculoVetoTableAdapter dsupdatevehiculoveto = new Vehit_UpdateDatosVehiculoVetoTableAdapter();

      public Boolean UpdatePersonaVeto(string Nit, string Nombre1, string Nombre2, string
                   Apellido1, string Apellido2, short Propietario, short Conductor, string Usuario,
         short Estado, string Observaciones,string autorizador)
      {
          try
          {


              dsupdatepersonaveto.Get_Vehit_UpdateDatosPersonaVeto(Nit, Nombre1, Nombre2,
                  Apellido1, Apellido2, Propietario, Conductor, Usuario, Estado, Observaciones, autorizador);
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
      public Boolean UpdateVehiculoVeto(string Placa, string
           Propietario, string Usuario, short Estado, string Observaciones, string Autorizador)
      {
          try
          {
              dsupdatevehiculoveto.Get_Vehit_UpdateDatosVehiculoVeto(Placa, Propietario, Usuario, Estado, Observaciones,Autorizador );
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


      public Boolean GuardarVehiculoVeto(	    string   Placa, string
            Propietario, string Usuario, short Estado, string Observaciones)
      {
          try
          {

              dsguardavehiculoveto.Get_Vehit_GuardarDatosVehiculoVeto(Placa,Propietario,Usuario,Estado,Observaciones );
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
      public Boolean GuardarPersonaVeto(	string Nit	,string Nombre1	,string Nombre2	,string
                    Apellido1, string Apellido2, short Propietario, short Conductor, string Usuario, 
          short Estado	, string Observaciones	)
      {
          try
          {


              dsguardapersonaveto.Get_Vehit_GuardarDatosPersonaVeto(Nit,Nombre1,Nombre2,
                  Apellido1,Apellido2,Propietario,Conductor,Usuario,Estado,Observaciones );
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


      public List<Vehit_ConsultaDatosPersonaReportada_Result > GetDatosPersonaReportada(string nit)
      {
          var lista = contex.Vehit_ConsultaDatosPersonaReportada(nit );
          return lista.ToList();
      }


      public List<Vehit_ConsultaDatosvehiculoReportado_Result> GetDatosDatosvehiculoReportado(string placa)
      {
          var lista = contex.Vehit_ConsultaDatosvehiculoReportado(placa);
          return lista.ToList();
      }
    }
}
