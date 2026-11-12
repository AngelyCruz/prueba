using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WPF_Vehitanques.DS.DSPolizaHCTableAdapters;
using WPF_Vehitanques.DS.DsPolizaRCTableAdapters;
using WPF_Vehitanques.Modelo;
using WPF_Vehitanques.Presentacion.Ventanas;

namespace WPF_Vehitanques.BLL
{
    public  class PolizaRCBLL
    {

        ComercialPEntities contex = new ComercialPEntities();
        MensajeBLL mensajebll;
        VentanaMensaje ventmensaje;



        public Boolean GuardarDAtosPolizaRC(string PolizaRC, string IdAsegRCE, DateTime VencimientoPolizaRC, string Placa, string usuario)
        {
            try
            {
                Vehit_UpdatePolizaRCXplacaTableAdapter dspoliza = new Vehit_UpdatePolizaRCXplacaTableAdapter();
                dspoliza.Get_Vehit_UpdatePolizaRCXplaca(PolizaRC, IdAsegRCE, VencimientoPolizaRC, Placa, usuario);
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
