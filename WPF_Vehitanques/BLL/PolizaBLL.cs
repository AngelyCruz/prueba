using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WPF_Vehitanques.DS.DSPolizaHCTableAdapters;
using WPF_Vehitanques.Modelo;
using WPF_Vehitanques.Presentacion.Ventanas;

namespace WPF_Vehitanques.BLL
{
    public class PolizaBLL
    {
        ComercialPEntities contex = new ComercialPEntities();
        MensajeBLL mensajebll;
        VentanaMensaje ventmensaje;


        public Boolean UpdateEstadoPolizaHC(string PolizaRC, string Placa, string usuario, short idestado)
        {
            try
            {
                VehiT_UpdateEstadoPolizaHCTableAdapter dspoliza = new VehiT_UpdateEstadoPolizaHCTableAdapter();
                dspoliza.Get_VehiT_UpdateEstadoPolizaHC(PolizaRC, Placa, usuario, idestado);
                return true;

            }
            catch (Exception ex)
            {
                ventmensaje = new VentanaMensaje();
                ventmensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                ventmensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                ventmensaje.cargarmensaje("Error", ex.Message.ToString());
                return false;
            }
        }
        public List<VehiT_ConsultaHistoricoPolizasHC_Result> ConsultaHistoricopolizas(string placa, int inicio, int final)
        {
            var lista = contex.VehiT_ConsultaHistoricoPolizasHC(placa, inicio, final);

            return lista.ToList();
        }

        public int contadorpolizaHc(string placa)
        {
            int retorno = 0;
            var lista = contex.VehiT_ContadorPolizasHCXplaca(placa);
            var lista2 = lista.ToList();
            if (lista2.Count > 0)
            {
                retorno = Convert.ToInt32(lista2[0].Value);
            }
            return retorno;
        }

        public List<VehiT_ConsultaPolizaXNumPoliza_Result> GetPolizaPorPlaca_numPoliza(string numpoliza, string placa)
        {
            var lista = contex.VehiT_ConsultaPolizaXNumPoliza(numpoliza, placa);
            return lista.ToList();

        }


        public Boolean GuardarDAtosPolizapropios(string poliza, DateTime Fechainicial, DateTime Fechafinal, string Placa, string Aseguradora, string usuario)
        {
            try
            {
                DS.DSPolizaHCTableAdapters.VehiT_GuardarDatosPolizaHC_propiosTableAdapter dspoliza = new DS.DSPolizaHCTableAdapters.VehiT_GuardarDatosPolizaHC_propiosTableAdapter();
                dspoliza.Get_VehiT_GuardarDatosPolizaHC_propios(poliza, Fechainicial, Fechafinal, Placa, Aseguradora, usuario);
                return true;

            }
            catch (Exception ex)
            {
                ventmensaje = new VentanaMensaje();
                ventmensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                ventmensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                ventmensaje.cargarmensaje("Error", ex.Message.ToString());
                return false;
            }
        }
        public Boolean GuardarDAtosPoliza(string poliza, DateTime Fechainicial, DateTime Fechafinal, string Placa, string Aseguradora, string usuario)
        {
            try
            {
                DS.DSPolizaHCTableAdapters.VehiT_GuardarDatosPolizaHCTableAdapter dspoliza = new DS.DSPolizaHCTableAdapters.VehiT_GuardarDatosPolizaHCTableAdapter();
                dspoliza.Get_GuardarDatosPolizaHC(poliza, Fechainicial, Fechafinal, Placa, Aseguradora, usuario);
                return true;

            }
            catch (Exception ex)
            {
                ventmensaje = new VentanaMensaje();
                ventmensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                ventmensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                ventmensaje.cargarmensaje("Error", ex.Message.ToString());
                return false;
            }
        }


        public Boolean ActualizaDatosPoliza(string poliza, DateTime Fechainicial, DateTime Fechafinal, string Placa, string Aseguradora, string usuario)
        {
            try
            {
                DS.DSPolizaHCTableAdapters.VehiT_UpdateDatosPolizaHCTableAdapter dspoliza = new DS.DSPolizaHCTableAdapters.VehiT_UpdateDatosPolizaHCTableAdapter();
                dspoliza.Get_UpdateDatosPolizaHC(poliza, Fechainicial, Fechafinal, Placa, Aseguradora, usuario);
                return true;

            }
            catch (Exception ex)
            {
                ventmensaje = new VentanaMensaje();
                ventmensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                ventmensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                ventmensaje.cargarmensaje("Error", ex.Message.ToString());
                return false;
            }
        }
    }
}

