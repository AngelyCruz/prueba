using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;
using WPF_Vehitanques.Presentacion.Ventanas;
using System.Windows.Media;


namespace WPF_Vehitanques.BLL
{
    public class ContratoBLL
    {
        MensajeBLL mensajebll;
        VentanaMensaje ventmensaje;
        ComercialPEntities contex = new ComercialPEntities();
        public Boolean GuardarContrato(int IdContrato, string Placa, string IdConductor, DateTime FechaContrato, string Usuario, int Agencia)
        {
            try
            {
                DS.DSContratoTableAdapters.VehiT_GuardaContratoColtanquesTableAdapter DScontrato = new DS.DSContratoTableAdapters.VehiT_GuardaContratoColtanquesTableAdapter();
                DScontrato.Get_GuardaContratoColtanques(IdContrato, Placa, IdConductor, FechaContrato, Usuario, Agencia);

                return true;
            }

            catch (Exception ex)
            {
                ventmensaje = new VentanaMensaje();
                ventmensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                ventmensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                ventmensaje.cargarmensaje("Error", ex.Message.ToString());
                ventmensaje.ShowDialog();
                // ex.Message.ToString();
                return false;

            }

        }

        public List<VehiT_ConsultaContrato_Result> GetContratoX_placa_Conductor(string placa, string conductor)
        {
            var lista = contex.VehiT_ConsultaContrato(placa,conductor );
            return lista.ToList();
        }

        public List<VehiT_ConsultaExistenciadeContrato_Result> GetExistenciaContrato(int idcontrato)
        {
            var lista = contex.VehiT_ConsultaExistenciadeContrato(idcontrato);
            return lista.ToList();

        }
        public Boolean GuardarAprobacioncontrato(int idcontrato,string placa,string usuario)
        {
           
            try
            {
                DS.DSContratoTableAdapters.VehiT_GuardarAprobacioncontratoColtanquesTableAdapter DSaprobarContrato = new DS.DSContratoTableAdapters.VehiT_GuardarAprobacioncontratoColtanquesTableAdapter();
                DSaprobarContrato.Get_GuardarAprobacioncontratoColtanques(idcontrato, placa, usuario);
                //DS.DSContratosTableAdapters.VehiT_GuardarAprobacioncontratoColtanquesTableAdapter DScontratos = new DS.DSContratosTableAdapters.VehiT_GuardarAprobacioncontratoColtanquesTableAdapter();
                //DScontratos.Get_GuardarAprobacioncontratoColtanques(idcontrato,placa,usuario);
                return true;
            }

            catch (Exception ex)
            {
                ventmensaje = new VentanaMensaje();
                ventmensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                ventmensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                ventmensaje.cargarmensaje("Error", ex.Message.ToString());
                ventmensaje.ShowDialog();
                // ex.Message.ToString();
                return false;

            }

        }
        public Boolean GuardarNoAprobacioncontrato(int idcontrato, string placa)
        {

            try
            {
                DS.DSContratoTableAdapters.VehiT_GuardarNoAprobacioncontratoColtanquesTableAdapter DSaprobarContrato = new DS.DSContratoTableAdapters.VehiT_GuardarNoAprobacioncontratoColtanquesTableAdapter();
                DSaprobarContrato.Get_GuardarNoAprobacioncontratoColtanques(idcontrato, placa);
                //DS.DSContratosTableAdapters.VehiT_GuardarAprobacioncontratoColtanquesTableAdapter DScontratos = new DS.DSContratosTableAdapters.VehiT_GuardarAprobacioncontratoColtanquesTableAdapter();
                //DScontratos.Get_GuardarAprobacioncontratoColtanques(idcontrato,placa,usuario);
                return true;
            }

            catch (Exception ex)
            {
                ventmensaje = new VentanaMensaje();
                ventmensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                ventmensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                ventmensaje.cargarmensaje("Error", ex.Message.ToString());
                ventmensaje.ShowDialog();
                // ex.Message.ToString();
                return false;

            }

        }
    }
}

