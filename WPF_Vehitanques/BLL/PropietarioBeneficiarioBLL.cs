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
    public class PropietarioBeneficiarioBLL
    {


        ComercialPEntities contex = new ComercialPEntities();
        MensajeBLL mensajebll;
        VentanaMensaje ventmensaje;
        public List<VehiT_ConsultaBeneficiarioPorPlaca_Result> GetBeneficiarioporplaca(string placa)
        {
            var lista = contex.VehiT_ConsultaBeneficiarioPorPlaca(placa);
            return lista.ToList();
        }

        public Boolean GuardaAuditoriaCambioBeneficiario_Propietario(string usuario, string propietario, string idconductor, string idbeneficiario, string mensaje,string placa)
        {
            try
            {
                DS.DSPropietarioBeneficiarioTableAdapters.Vehit_GuardaAuditoriaCambioBeneficiario_PropietarioTableAdapter DSpropieta = new DS.DSPropietarioBeneficiarioTableAdapters.Vehit_GuardaAuditoriaCambioBeneficiario_PropietarioTableAdapter();
                DSpropieta.Get_Vehit_GuardaAuditoriaCambioBeneficiario_Propietario(usuario, propietario, idconductor, idbeneficiario, mensaje,placa );

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

        public List<VehiT_ConsultaBeneficiario_Result> GetDatosBeneficiarioXID(string Idbeneficiario)
        {
            var lista = contex.VehiT_ConsultaBeneficiario(Idbeneficiario);
            return lista.ToList();

        }

        public List<VehiT_ConsultaPropietario_Result> GetDatosPropietarioXID(string Idpropietario)
        {
            var lista = contex.VehiT_ConsultaPropietario(Idpropietario);
            return lista.ToList();
        }
        public Boolean GuardarDatosPropietario(decimal IdProp, string Nombres, string Apellido1,
            string Apellido2, string TipoDoc, string CiuPro, string TelPro,
            string DirPro, Nullable<Boolean> ListaRiesgo, Nullable<DateTime> FechaEstudio, string placa, string usuario, Nullable<short> DigitVerif)
        {
            try
            {
                DS.DSPropietarioBeneficiarioTableAdapters.VehiT_GuardarDatosPropietarioTableAdapter DSpropieta = new DS.DSPropietarioBeneficiarioTableAdapters.VehiT_GuardarDatosPropietarioTableAdapter();
                DSpropieta.Get_GuardarDatosPropietario(IdProp, Nombres,
                    Apellido1, Apellido2, TipoDoc, CiuPro, TelPro, DirPro, ListaRiesgo, FechaEstudio, placa, usuario,DigitVerif );

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

        public Boolean GuardarDatosBeneficiario(string IdNit, string Nombres, string Apellido1,
           string Apellido2, string TipoDoc, string Ciudad, string Ben_TelCel, string Telefono,
           string Direccion, Nullable<Boolean> ListaRiesgo, Nullable<DateTime> FechaEstudio,
            Nullable<short> PagoCierre, string Usuario, string placa, string observaciones, Nullable<short> DigitVerif, string indicativotel,string email)
        {
            try
            {
                DS.DSPropietarioBeneficiarioTableAdapters.VehiT_GuardarDatosBeneficiarioTableAdapter DSbenefi = new DS.DSPropietarioBeneficiarioTableAdapters.VehiT_GuardarDatosBeneficiarioTableAdapter();
                DSbenefi.Get_GuardarDatosBeneficiario(IdNit, Nombres,
                    Apellido1, Apellido2, TipoDoc, Ciudad, Ben_TelCel, Telefono, Direccion, ListaRiesgo,
                    FechaEstudio, PagoCierre, Usuario, placa, observaciones, DigitVerif, indicativotel,email );

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


        public Boolean UpdateDAtosBeneficiario(string IdNit, string Nombres, string Apellido1,
           string Apellido2, string TipoDoc, string Ciudad, string Ben_TelCel, string Telefono,
           string Direccion, Nullable<Boolean> ListaRiesgo, Nullable<DateTime> FechaEstudio,
             string placa, string observaciones, string usuario, Nullable<short> DigitVerif, string indicativotel, string email)
        {
            try
            {
                DS.DSPropietarioBeneficiarioTableAdapters.VehiT_UpdateDatosBeneficiarioTableAdapter dsupdatebeneficiario = new DS.DSPropietarioBeneficiarioTableAdapters.VehiT_UpdateDatosBeneficiarioTableAdapter();

                dsupdatebeneficiario.Get_UpdateDatosBeneficiario(IdNit, Nombres,
                    Apellido1, Apellido2, TipoDoc, Ciudad, Ben_TelCel, Telefono, Direccion, ListaRiesgo,
                    FechaEstudio, placa, observaciones, usuario, DigitVerif, indicativotel,email );

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



        public Boolean UpdateDatosPropietario(decimal IdProp, string Nombres, string Apellido1,
            string Apellido2, string TipoDoc, string CiuPro, string TelPro,
            string DirPro, Nullable<Boolean> ListaRiesgo, Nullable<DateTime> FechaEstudio, string placa, string usuario, Nullable<short> DigitVerif)
        {
            try
            {
                DS.DSPropietarioBeneficiarioTableAdapters.VehiT_UpdateDatosPropietarioTableAdapter DSpropieta = new DS.DSPropietarioBeneficiarioTableAdapters.VehiT_UpdateDatosPropietarioTableAdapter();
                DSpropieta.Get_UpdateDatosPropietario(IdProp, Nombres,
                    Apellido1, Apellido2, TipoDoc, CiuPro, TelPro, DirPro, ListaRiesgo, FechaEstudio, placa, usuario,DigitVerif );

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
