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
    public class VehiculoBLL
    {

        ComercialPEntities contex = new ComercialPEntities();
        MensajeBLL mensajebll;
        VentanaMensaje ventmensaje;
        DS.DSVehiculoTableAdapters.VehiT_GuardarDatosVehiculoTableAdapter dsvehi = new DS.DSVehiculoTableAdapters.VehiT_GuardarDatosVehiculoTableAdapter();
        //DSVehiculo.VehiT_GuardarDatosVehiculoDataTable   DSguardahvvehiculo = new DSVehiculo.VehiT_GuardarDatosVehiculoDataTable();

        public Boolean UpdateSoatXplaca(string numsoat,string ciasoat,string placa,  DateTime FechavencePolizaSoat,string usuario)
        {
            try
            {
                DS.DSVehiculoTableAdapters.Vehit_UpdateSoatXPlacaTableAdapter   dsvehiculo = new DS.DSVehiculoTableAdapters.Vehit_UpdateSoatXPlacaTableAdapter ();
                dsvehiculo.Get_Vehit_UpdateSoatXPlaca(numsoat, ciasoat, placa, FechavencePolizaSoat, usuario);
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


        public Boolean GuardaAprobacionPolizasRC(string placa, Boolean Poliza_RC_Activa,  string usuario, string IdPoliza,DateTime  FechavencePoliza, string IdAseguradora)
        {
            try
            {
                DS.DSVehiculoTableAdapters.VehiT_UpdateAprobacionPolizaRCTableAdapter dsupdaproolRC = new DS.DSVehiculoTableAdapters.VehiT_UpdateAprobacionPolizaRCTableAdapter();
                dsupdaproolRC.Get_VehiT_UpdateAprobacionPolizaRC(placa, Poliza_RC_Activa, usuario, IdPoliza, FechavencePoliza, IdAseguradora);

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

        public int GetContadorProgManteMsg(decimal idconcepto)
        {
            int respuesta = 0;
            try
            {
                var lista = contex.VehiT_ConsultaContadorVehiProgMantenimeinto(idconcepto);
                var lista2 = lista.ToList();
                respuesta = lista2[0].Value;

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return respuesta;
        }
        public List<VehiT_ConsultaTotalVehiProgMantenimeinto_Result> GetProgManteMsg(int iStar, int iEnd, decimal idconcepto)
        {
            var lista = contex.VehiT_ConsultaTotalVehiProgMantenimeinto(iStar, iEnd, idconcepto);
            return lista.ToList();
        }
        public List<VehiT_ConsultaExistenciadeVehiculo_Result> ConsultaExistenciaVehiculo(string placa)
        {
            var lista = contex.VehiT_ConsultaExistenciadeVehiculo(placa);
            return lista.ToList();

        }
        public Boolean GuardarDatosVehiculo(string IdPlaca, int Marca, string Modelo, string Color, int TipoMotor,
            int TipoCarroceria, string NumeroMotor, string TarjetadePropiedad,
            string SeguroObligatorio, DateTime VencimientoSeguroObligatorio,
            int TipoAfiliacion, Boolean GrupoPropios, string Usuario, int TipoVehiculo,
             string Conductor, string Afiliadora1, string NitPropietario,
            string Afiliadora, string TelAfiliadora,
             string ChaSerie, string Observaciones, string GrupoLlantas, short TipoCarga,
           DateTime VigenciaRevisionMecan, short? Categoria,
           string Linea, Nullable<short> ModRepot,
             int PesoVacio, string CiaSoat, Nullable<decimal> Propietario,
            string EmpresaSatelital, string PolizaRC, Nullable<DateTime> VencimientoPolizaRC,
             decimal CodEmpresaSat, string IdAsegRCE,
            string UsuarioGPS, string PasswordGPS, Nullable<DateTime> VenceQuintaRueda, string colorRNDC, 
            int LineaRNDC, Nullable<DateTime> FechaAforoSencillo,
            Nullable<DateTime> FechaHidroSencillo, int peso_cabezote, int idpaisoperacion, Nullable<DateTime> VencimientoContrato, Nullable<DateTime> FechaEstudio)
        {
            try
            {
                dsvehi.GetVehiT_GuardarDatosVehiculo(IdPlaca, Marca, Modelo, Color,
                    TipoMotor, TipoCarroceria, NumeroMotor, TarjetadePropiedad, SeguroObligatorio, VencimientoSeguroObligatorio,
                    TipoAfiliacion, GrupoPropios, Usuario, TipoVehiculo, Conductor,
                    Afiliadora1, NitPropietario, Afiliadora, TelAfiliadora, ChaSerie, Observaciones, GrupoLlantas, TipoCarga,
                    VigenciaRevisionMecan, Categoria, Linea, ModRepot, PesoVacio, CiaSoat, Propietario,
                    EmpresaSatelital, PolizaRC, VencimientoPolizaRC, CodEmpresaSat, IdAsegRCE,
                    UsuarioGPS, PasswordGPS, VenceQuintaRueda, colorRNDC, LineaRNDC,
                    FechaAforoSencillo, FechaHidroSencillo, peso_cabezote, idpaisoperacion, VencimientoContrato,FechaEstudio);
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
        public List<VehiT_ConsultaHojadeVidaVehiculo_Result> GetHojaDeVidaVehiculo(string placa)
        {
            var lista = contex.VehiT_ConsultaHojadeVidaVehiculo(placa);
            return lista.ToList();
        }
        public List<VehiT_ConsultarVehiculoXplaca_Result> GetVehiculosinDespacho(string placa, int inicio, int final)
        {
            var lista = contex.VehiT_ConsultarVehiculoXplaca(placa,  inicio, final);
            return lista.ToList();
        }
        public List<VehiT_ConsultarVehiculoXplaca_condu_propi_Result> GetVehiculo(string placa,
            string conductor, string propietario, string remolque, int inicio, int final)
        {
            var lista = contex.VehiT_ConsultarVehiculoXplaca_condu_propi(placa, conductor, propietario,remolque, inicio, final);
            return lista.ToList();
        }
        public int contadorVehiculoSinDespacho(string placa)
        {
            int retorno = 0;
            var lista = contex.VehiT_ContadorVehiculoXplaca(placa);
            var lista2 = lista.ToList();
            if (lista2.Count > 0)
            {
                retorno = Convert.ToInt32(lista2[0].Value);
            }
            return retorno;
        }
        public int contadorVehiculo(string placa, string conductor, string propietario,string remolque)
        {
            int retorno = 0;
            var lista = contex.VehiT_ContadorVehiculoXplaca_condu_propi(placa, conductor, propietario,remolque );
            var lista2 = lista.ToList();
            if (lista2.Count > 0)
            {
                retorno = Convert.ToInt32(lista2[0].Value);
            }
            return retorno;
        }

        public List<VehiT_ConsultaTiposVehiculo_Result> GetTiposVehiculo()
        {
            var lista = contex.VehiT_ConsultaTiposVehiculo();
            return lista.ToList();
        }
        public List<EjesVehiculo> GetEjesVehiculo()
        {
            //var lista = from Ej in contex.EjesVehiculo
            //            //where Ej.IdEje <= 3
            //            select Ej;
            var lista = from Ej in contex.EjesVehiculo                       
                        select Ej;
            return lista.ToList();
        }
        public List<EjesVehiculo> GetEjesVehiculo4()
        {
            var lista = from Ej in contex.EjesVehiculo                        
                        select Ej;
            return lista.ToList();
        }

        public Boolean UpdateTipoVehiculo(string placa,int oldtipovehi,int newtipovehi,string usuario)
        {            
                 try
            {
                DS.DSVehiculoTableAdapters.VehiT_UpdateCambioTipoVehiculoTableAdapter dsupdtipovehi = new DS.DSVehiculoTableAdapters.VehiT_UpdateCambioTipoVehiculoTableAdapter();
                dsupdtipovehi.Get_VehiT_UpdateCambioTipoVehiculo(placa, oldtipovehi,newtipovehi , usuario);
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

        public Boolean ActualizaEstadoPlaca(string placa, short estado, string observacion, Boolean habilitaviaje, string usuario)
        {

            try
            {
                DS.DSVehiculoTableAdapters.VehiT_UpdateEstadoPlacaVehiculoTableAdapter dsestadoplaca = new DS.DSVehiculoTableAdapters.VehiT_UpdateEstadoPlacaVehiculoTableAdapter();
                dsestadoplaca.Get_UpdateEstadoPlacaVehiculo(placa, estado, observacion, habilitaviaje, usuario);
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

        //public DS.DSVehiculo.VehiT_ConsultaEstadoPorPlacaDataTable GetEstadoplaca(string placa)
        //{
        //    DS.DSVehiculo dsvehi = new DS.DSVehiculo();

        //    DS.DSVehiculoTableAdapters.VehiT_ConsultaEstadoPorPlacaTableAdapter dsestado = new DS.DSVehiculoTableAdapters.VehiT_ConsultaEstadoPorPlacaTableAdapter();
        //    dsestado.Get_ConsultaEstadoPorPlaca(placa );

        //    return dsestado;
        //}

        public List<VehiT_ConsultaEstadoPorPlaca_Result> GetEstadoPlaca(string placa)
        {
            var lista = contex.VehiT_ConsultaEstadoPorPlaca(placa);
            return lista.ToList();
        }

        public Boolean UpdateDAtosVehiculo(string IdPlaca, int Marca, string Modelo, string Color,
            int TipoMotor, int TipoCarroceria, string NumeroMotor, string TarjetadePropiedad, string SeguroObligatorio,
            DateTime VencimientoSeguroObligatorio, int TipoAfiliacion, int TipoVehiculo, string Afiliadora1,
            string TelAfiliadora, string ChaSerie, string Observaciones, string GrupoLlantas,
            short TipoCarga, DateTime VigenciaRevisionMecan, string Linea, Nullable<short> ModRepot, int PesoVacio,
            string CiaSoat, string EmpresaSatelital, string PolizaRC, Nullable<DateTime> VencimientoPolizaRC,
            decimal CodEmpresaSat, string IdAsegRCE, string UsuarioGPS, string PasswordGPS,
            Nullable<DateTime> VenceQuintaRueda, string colorRNDC, int LineaRNDC, Boolean contrato, string usuario, Nullable<DateTime > FechaAforoSencillo,
            Nullable<DateTime> FechaHidroSencillo, int peso_cabezote, int idpais, Nullable<DateTime> VencimientoContrato, Nullable<DateTime> FechaEstudio)
        {
            try
            {
                DS.DSVehiculoTableAdapters.VehiT_UpdateDatosVehiculoTableAdapter DSupdatevehi = new DS.DSVehiculoTableAdapters.VehiT_UpdateDatosVehiculoTableAdapter();

                DSupdatevehi.GetDataVehiT_UpdateDatosVehiculo(IdPlaca, Marca, Modelo, Color, TipoMotor, TipoCarroceria,
                    NumeroMotor, TarjetadePropiedad, SeguroObligatorio, VencimientoSeguroObligatorio,
                    TipoAfiliacion, TipoVehiculo, Afiliadora1, TelAfiliadora, ChaSerie, Observaciones,
                    GrupoLlantas, TipoCarga, VigenciaRevisionMecan, Linea, ModRepot, PesoVacio,
                    CiaSoat, EmpresaSatelital, PolizaRC, VencimientoPolizaRC, CodEmpresaSat,
                    IdAsegRCE, UsuarioGPS, PasswordGPS, VenceQuintaRueda, colorRNDC, LineaRNDC, contrato, usuario,
                    FechaAforoSencillo, FechaHidroSencillo, peso_cabezote, idpais, VencimientoContrato,FechaEstudio);

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
