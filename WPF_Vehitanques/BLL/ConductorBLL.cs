using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;
using WPF_Vehitanques.BLL;
using WPF_Vehitanques.Presentacion.Ventanas;
using System.Windows.Media;

namespace WPF_Vehitanques.BLL
{
    public class ConductorBLL
    {

        ComercialPEntities contex = new ComercialPEntities();
        MensajeBLL mensajebll;
        VentanaMensaje ventmensaje;
        DS.DSConductorTableAdapters.VehiT_GuardarDatosConductorTableAdapter DSconductor = new DS.DSConductorTableAdapters.VehiT_GuardarDatosConductorTableAdapter();

        
            public Boolean UpdateExamenconductor(string IdConductor, DateTime FechaExamen)
        {
            try
            {
                DS.DSConductorTableAdapters.Vehit_UpdateExamenConductorPropioTableAdapter DSudpcondpropio = new DS.DSConductorTableAdapters.Vehit_UpdateExamenConductorPropioTableAdapter();

                DSudpcondpropio.Get_Vehit_UpdateExamenConductorPropio (IdConductor,FechaExamen );

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

        public List<VehiT_ConsultaConductoresPropiosMasivoMSG_Result> ConsultaConductorPropioEnvioMAsivoMSG( int inicio, int final,Boolean Estado)
        {
            var lista = contex.VehiT_ConsultaConductoresPropiosMasivoMSG (inicio ,final,Estado);
            return lista.ToList();

        }
        public int contadorconductorespropiosEnvioMAsivoMSG(Boolean Estado)
        {
            int retorno = 0;
            var lista = contex.VehiT_ConsultaContadorConductoresPropiosMasivoMSG(Estado);
            var lista2 = lista.ToList();
            if (lista2.Count > 0)
            {

                retorno = Convert.ToInt32(lista2[0].Value);
            }
            return retorno;
        }


        public List<VehiT_ConsultaConductor_Propio_Result> ConsultaConductorPropio(string idconductor, string nombre, int inicio, int final)
        {
            var lista = contex.VehiT_ConsultaConductor_Propio( idconductor, nombre,inicio, final);
            return lista.ToList();

        }
        public int contadorconductorespropios(string idconductor, string nombre)
        {
            int retorno = 0;
            var lista = contex.VehiT_ContadorConductor_Propio(idconductor, nombre);
            var lista2 = lista.ToList();
            if (lista2.Count > 0)
            {

                retorno = Convert.ToInt32(lista2[0].Value);
            }
            return retorno;
        }

        public Boolean UpdateConductoresPropios(string IdConductor, short IdAgencia, string Nombres,
   string Licencia, int Categoria, DateTime FechaVencimiento, string Direccion,
   string Telefono, string CelPre, string CiudadResidencia, DateTime FechaNacimiento, Boolean chkAfiliado,
   string Usuario, string Arp, string Eps, string Afp, int Experiencia,  Nullable <DateTime>  FechaPagoSegSoc,
           string Apellido1, string Apellido2, int Afinidad, int Analista, short Estado, string RH, bool chkpatio, string indicativotel)
        {
            try
            {
                DS.DSConductorTableAdapters.VehiT_UpdateDatosConductorPropioTableAdapter DSudpcondpropio = new DS.DSConductorTableAdapters.VehiT_UpdateDatosConductorPropioTableAdapter();

                DSudpcondpropio.Get_VehiT_UpdateDatosConductorPropio(IdConductor, IdAgencia, Nombres, Licencia, Categoria,
                    FechaVencimiento, Direccion, Telefono, CelPre, CiudadResidencia, FechaNacimiento, chkAfiliado, Usuario, Arp,
                    Eps, Afp, Experiencia, FechaPagoSegSoc, Apellido1, Apellido2, Afinidad, Analista, Estado, RH, chkpatio, indicativotel);

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



        public Boolean GuardarConductoresPropios(string IdConductor ,	short  IdAgencia ,	string Nombres ,
	string Licencia  ,int  Categoria ,DateTime  FechaVencimiento ,string	Direccion ,
	string Telefono   ,string CelPre ,string CiudadResidencia ,DateTime 	FechaNacimiento ,Boolean chkAfiliado ,	
	string Usuario  ,	string Arp ,string Eps  ,string Afp   ,int 	Experiencia ,Nullable <DateTime > FechaPagoSegSoc ,
            string Apellido1, string Apellido2, int Afinidad, int Analista, short Estado, string RH, bool chkpatio, string indicativotel)
        {
            try
            {
                DS.DSConductorTableAdapters.VehiT_GuardarDatosConductorPropioTableAdapter DSguardacondpropio = new DS.DSConductorTableAdapters.VehiT_GuardarDatosConductorPropioTableAdapter();

                DSguardacondpropio.Get_VehiT_GuardarDatosConductorPropio(IdConductor,IdAgencia,Nombres,Licencia,Categoria,
                    FechaVencimiento,Direccion,Telefono,CelPre,CiudadResidencia,FechaVencimiento,chkAfiliado,Usuario,Arp,
                    Eps, Afp, Experiencia, FechaPagoSegSoc, Apellido1, Apellido2, Afinidad, Analista, Estado, RH, chkpatio, indicativotel);

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
        public IList<VehiT_ConsultaConductorAsociadoXPlaca_Result> GetconductorXPlaca(string placa)
        {
            var lista = contex.VehiT_ConsultaConductorAsociadoXPlaca(placa);
            return lista.ToList();

        }
        public List<VehiT_ConsultaConductor_Result> GetDAtosconductor(string idconductor)
        {
            var lista = contex.VehiT_ConsultaConductor(idconductor);
            return lista.ToList();
        }
        public List<VehiT_ConsultaConductorAsociadoXPlaca_IdConductor_Result> ConsultaConductorAsociadoPlacaXPlaca_Idcondutor(string placa, string conductor)
        {
            var lista = contex.VehiT_ConsultaConductorAsociadoXPlaca_IdConductor(placa, conductor);
            return lista.ToList();
        }
        public Boolean GuardarDatosConductor(string IdConductor, short IdAgencia, string Nombres, string Licencia,
                                    int Categoria, DateTime FechaVencimientolicen, string Direccion, string Telefono, string CelPre,
                                string CiudadResidencia, string NombrePariente, string TelefonoPariente, DateTime FechaNacimiento,
                            Boolean chkAfiliado, string Usuario, string Observ, string Arp, string Eps, int Experiencia,
                            Nullable<DateTime> FechaPagoSegSoc, Boolean ListaRiesgo, Nullable<DateTime> FechaEstudio,
                        string ObservaRuta, short TipoCarga, string Apellido1, string Apellido2, string Empresa1,
                        string Telefono1, string TipoServicio1, string TipoCarga1, string Empresa2, string Telefono2,
                        string TipoServicio2, string TipoCarga2, string placa, string IdBanco, string TipoCuenta, string NitCuenta,
    string IdIdenTar, string numcuenta, string indicativotel, int tipopago, int IdTipoAfiliacion, DateTime dtFechaPagoPlanilla, DateTime dtFechaultExamenMedico)
        {
            try
            {
                DSconductor.GetVehiT_GuardarDatosConductor(IdConductor, IdAgencia, Nombres, Licencia, Categoria, FechaVencimientolicen, Direccion,
                    Telefono, CelPre, CiudadResidencia, NombrePariente, TelefonoPariente, FechaNacimiento, chkAfiliado, Usuario,
                    Observ, Arp, Eps, Experiencia, FechaPagoSegSoc, ListaRiesgo, FechaEstudio, ObservaRuta, TipoCarga, Apellido1, Apellido2,
                    Empresa1, Telefono1, TipoServicio1, TipoCarga1, Empresa2, Telefono2, TipoServicio2, TipoCarga2, placa, IdBanco,
                    TipoCuenta, NitCuenta, IdIdenTar, numcuenta, indicativotel, tipopago, IdTipoAfiliacion, dtFechaPagoPlanilla, dtFechaultExamenMedico);

                return true;
            }
            catch (Exception ex)
            {
                ventmensaje = new VentanaMensaje();
                ventmensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                ventmensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                ventmensaje.cargarmensaje("Error", ex.Message.ToString()  );
                ventmensaje.ShowDialog();
                return false;
            }
        }


        public Boolean ActualizaDatosConductor(string IdConductor, short IdAgencia, string Nombres, string Licencia,
                                    int Categoria, DateTime FechaVencimientolicen, string Direccion, string Telefono, string CelPre,
                                string CiudadResidencia, string NombrePariente, string TelefonoPariente, DateTime FechaNacimiento,
                            Boolean chkAfiliado, string Usuario, string Observ, string Arp, string Eps, int Experiencia,
                            Nullable<DateTime> FechaPagoSegSoc, Boolean ListaRiesgo, Nullable<DateTime> FechaEstudio,
                        string ObservaRuta, short TipoCarga, string Apellido1, string Apellido2, string Empresa1,
                        string Telefono1, string TipoServicio1, string TipoCarga1, string Empresa2, string Telefono2,
                        string TipoServicio2, string TipoCarga2, string placa, string idicativotel, int tipopago, int IdTipoAfiliacion, DateTime dtPagoPlanillaSalud, DateTime dtFechaUltExamenMedico)
        {

            try
            {
                DS.DSConductorTableAdapters.VehiT_UpdateConductor_PlacaVehiculoTableAdapter DSconducupdate = new DS.DSConductorTableAdapters.VehiT_UpdateConductor_PlacaVehiculoTableAdapter();

                DSconducupdate.GetVehiT_UpdateConductor_PlacaVehiculo(IdConductor, IdAgencia, Nombres, Licencia, Categoria, FechaVencimientolicen,Direccion ,
                    Telefono, CelPre, CiudadResidencia, NombrePariente, TelefonoPariente, FechaNacimiento, chkAfiliado, Usuario,
                    Observ, Arp, Eps, Experiencia, FechaPagoSegSoc, ListaRiesgo, FechaEstudio, ObservaRuta, TipoCarga, Apellido1, Apellido2,
                    Empresa1, Telefono1, TipoServicio1, TipoCarga1, Empresa2, Telefono2, TipoServicio2, TipoCarga2, placa,
                    idicativotel, tipopago, IdTipoAfiliacion, dtPagoPlanillaSalud, dtFechaUltExamenMedico);
                   

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

        public Boolean UpdateInfoBancariaConductor(string IdIdenTar, string IdBanco, string NitCuenta,
            string TipoCuenta, string ObservacionCuenta, string IdConductor, string num_cuenta, string usuario, int tipopago)
        {

            try
            {
                DS.DSConductorTableAdapters.VehiT_UpdateConductor_InfoBancariaTableAdapter DSinfobancaria = new DS.DSConductorTableAdapters.VehiT_UpdateConductor_InfoBancariaTableAdapter();

                DSinfobancaria.Get_UpdateConductor_InfoBancaria(IdIdenTar, IdBanco, NitCuenta, TipoCuenta, ObservacionCuenta, IdConductor, num_cuenta, usuario, tipopago);

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



    }
}
