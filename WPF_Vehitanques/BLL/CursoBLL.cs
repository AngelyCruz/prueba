using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using WPF_Vehitanques.Modelo;
using WPF_Vehitanques.Presentacion.Ventanas;

namespace WPF_Vehitanques.BLL
{

   public  class CursoBLL
    {
       ComercialPEntities contex = new ComercialPEntities();
       MensajeBLL mensajebll;
       VentanaMensaje ventmensaje;
       public List<Vehit_ConsultaCursosPorGrupo_Result> GetCursoporModulo(int idmodulo)
       {
           var lista = contex.Vehit_ConsultaCursosPorGrupo(idmodulo);
           return lista.ToList();
       }

       public List<Vehit_ConsultaGrupoCursosConductor_Result> GetModuloCursos()
       {
           var lista = contex.Vehit_ConsultaGrupoCursosConductor();
           return lista.ToList();
       }

       public Boolean UpdateEstadoDocumentoCursosConductor( string UsuarioRegistro, decimal CurC_Id,Boolean Estado)
       {
           try
           {
               DS.DSCursosTableAdapters.VehiT_UpdateEstadoDocumentoCursosConductorTableAdapter DSupdEstaDoccurso = new DS.DSCursosTableAdapters.VehiT_UpdateEstadoDocumentoCursosConductorTableAdapter();
               DSupdEstaDoccurso.Get_VehiT_UpdateEstadoDocumentoCursosConductor(UsuarioRegistro, CurC_Id, Estado);

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
       public Boolean UpdateCursoConductor(Nullable<DateTime> FechaVigencia, Nullable<int> VigenciaEnMeses, DateTime FechaCurso, string UsuarioRegistro, decimal CurC_Id)
       {
           try
           {
               DS.DSCursosTableAdapters.Vehit_UpdateCursosConductorTableAdapter DSupdcurso = new DS.DSCursosTableAdapters.Vehit_UpdateCursosConductorTableAdapter();
               DSupdcurso.Get_Vehit_UpdateCursosConductor(FechaVigencia, VigenciaEnMeses, FechaCurso, UsuarioRegistro,CurC_Id );

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
       public Boolean GuardarCursoConductor(int idcurso, string IdConductor, Nullable<DateTime> FechaVigencia, Nullable<int> VigenciaEnMeses, DateTime FechaCurso, string UsuarioRegistro)
       {
           try
           {
               DS.DSCursosTableAdapters.Vehit_GuardarCursosConductorTableAdapter DSinsertcurso = new DS.DSCursosTableAdapters.Vehit_GuardarCursosConductorTableAdapter();
               DSinsertcurso.Get_Vehit_GuardarCursosConductor(idcurso, IdConductor,FechaVigencia,VigenciaEnMeses,FechaCurso,UsuarioRegistro );

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


       public List<Vehit_ConsultaParametrosPorCursoconductor_Result> GetParametrosCurso(int idcurso)
       {
           var  lista = contex.Vehit_ConsultaParametrosPorCursoconductor(idcurso);
           return lista.ToList();

       }
       public Boolean updateEstadoCursoConductor(decimal idcurso,string usuarioupd)
       {
           try
           {
               DS.DSCursosTableAdapters.VehiT_UpdateEstadoCursoConductorTableAdapter DSudpEstadocurso = new DS.DSCursosTableAdapters.VehiT_UpdateEstadoCursoConductorTableAdapter();
               DSudpEstadocurso.Get_VehiT_UpdateEstadoCursoConductor(idcurso,usuarioupd );

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
       public List<VehiT_ConsultaCursosConductor_Result> GetListaCursos()
       {
           var lista = contex.VehiT_ConsultaCursosConductor();
           
           return lista.ToList();
       }

       public List<VehiT_ConsultaCursosPorConductor_Result > GetCursosConductor(string  idconductor, int istar, int iend)
       {
           var lista = contex.VehiT_ConsultaCursosPorConductor(idconductor, istar, iend);
           return lista.ToList();
       }

       public int ContadorCursosConductor(string idconductor)
       {
           int retorno = 0;
           var lista = contex.VehiT_ConsultaContadorCursosPorConductor(idconductor);
           var lista2 = lista.ToList();
           if (lista2.Count > 0)
           {

               retorno = Convert.ToInt32(lista2[0].Value);
           }
           return retorno;
       }

    }
}
