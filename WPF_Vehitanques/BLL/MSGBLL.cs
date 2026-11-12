using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WPF_Vehitanques.DS;
using WPF_Vehitanques.DS.DSMensajesTextoTableAdapters;
using WPF_Vehitanques.Modelo;
using WPF_Vehitanques.Presentacion.Ventanas;

namespace WPF_Vehitanques.BLL
{
    public class MSGBLL
    {
        MensajeBLL mensajebll;
        VentanaMensaje ventmensaje;

        Entur_GuardarErroresRequesMensajesDETExtoTableAdapter DsMsg = new Entur_GuardarErroresRequesMensajesDETExtoTableAdapter();
        Entur_GuardarHistoricoMsgResponseTableAdapter DShistoricoMsg = new Entur_GuardarHistoricoMsgResponseTableAdapter();
        ComercialPEntities context = new ComercialPEntities();
        public DSMensajesTexto.Entur_ConsultaCuotaMensajesXAreaDataTable GetCuotaMensajes(int idarea)
        {
            Entur_ConsultaCuotaMensajesXAreaTableAdapter DsCuota = new Entur_ConsultaCuotaMensajesXAreaTableAdapter();
            return DsCuota.Get_Entur_ConsultaCuotaMensajesXArea(idarea);
        }
        public IList<Vehit_ConsultaNumerosCopiaMSG_Result > GetNumerosCopia(int idarea)
        {
            var lista = context.Vehit_ConsultaNumerosCopiaMSG(idarea );
            return lista.ToList();
        }
        public void GuardarHisroicoResponseMsg(string IdRequest, string Mensaje, string usuario, int catidadmensajes,int idarea)
        {
            try
            {
                DShistoricoMsg.Get_Entur_GuardarHistoricoMsgResponse(IdRequest, Mensaje, usuario, catidadmensajes,idarea );
            }
            catch (Exception ex)
            {
                ventmensaje = new VentanaMensaje();
                ventmensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                ventmensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                ventmensaje.cargarmensaje("Error", ex.Message.ToString());
                ventmensaje.ShowDialog();
            }
        }

        public void GuardarErroresMsg(string error, int codigoerror)
        {
            try
            {
                DsMsg.Get_Entur_GuardarErroresRequesMensajesDETExto(error, codigoerror);
            }
            catch (Exception ex)
            {
                ventmensaje = new VentanaMensaje();
                ventmensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                ventmensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                ventmensaje.cargarmensaje("Error", ex.Message.ToString());
                ventmensaje.ShowDialog();
            }
        }



    }
}
