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
    public class RemolqueBLL
    {
        ComercialPEntities contex = new ComercialPEntities();
        MensajeBLL mensajebll;
        VentanaMensaje ventmensaje;

        public List<VehiT_ConsultaRemolqueInternacional_Result> GetREmolqueinternacional(string remolque)
        {
            var lista = contex.VehiT_ConsultaRemolqueInternacional(remolque).ToList();
            return  lista;
        }
        public List<VehiT_ConsultaRemolqueXPlaca_Result> GetRemolquePorPlaca(string placaremolque)
        {
            var lista = contex.VehiT_ConsultaRemolqueXPlaca(placaremolque);
            return lista.ToList();
        }

        public Boolean GuardarDAtosRemolque(string IdPlacaRO, short TipoAfiliacion, string Modelo, int Capacidad,
            short TipoRemolque, short NumeroEjes, short Unidad, string Usuario, string Marca,
            int PesoVehicular, string GrupoLlantas, int IdMarca, int IdClase,
            Nullable<Decimal> IdProp, int PesoVacio, Nullable<DateTime> VencePruebaHidro, string TarjetadePropiedad,
            Nullable<DateTime> FechaAforo, string placavehiculo, Nullable<DateTime> Vencekingpin,int idpaisoperacion)
    {
            try
            {
                DS.DSRemolqueTableAdapters.VehiT_GuardarDatosRemolqueTableAdapter dsremolque = new DS.DSRemolqueTableAdapters.VehiT_GuardarDatosRemolqueTableAdapter();


                dsremolque.Get_VehiT_GuardarDatosRemolque(IdPlacaRO, TipoAfiliacion, Modelo, Capacidad, TipoRemolque, NumeroEjes, Unidad,
                    Usuario, Marca, PesoVehicular, GrupoLlantas, IdMarca, IdClase, IdProp, PesoVacio, VencePruebaHidro, TarjetadePropiedad,
                    FechaAforo, placavehiculo, Vencekingpin, idpaisoperacion);
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


        public Boolean UpdateDatosRemolque(string IdPlacaRO, short TipoAfiliacion, string Modelo,
                  int Capacidad, short TipoRemolque, short NumeroEjes, short Unidad,
                  string Marca, int PesoVehicular, string GrupoLlantas, int IdMarca,
                  int IdClase, Nullable<decimal> IdProp, int PesoVacio, Nullable<DateTime> VencePruebaHidro,
                  string TarjetadePropiedad, Nullable<DateTime> FechaAforo, string placavehiculo,
            string usuario, Nullable<DateTime> Vencekingpin, int idpaisoperacion)
        {
            try
            {
                //DS.DSVehiculoTableAdapters.VehiT_UpdateDatosRemolqueTableAdapter DSupdateremolque = new DS.DSVehiculoTableAdapters.VehiT_UpdateDatosRemolqueTableAdapter();
                //DSupdateremolque.Get_VehiT_UpdateDatosRemolque(IdPlacaRO, TipoAfiliacion, Modelo, Capacidad,
                //    TipoRemolque, NumeroEjes, Unidad, Marca, PesoVehicular, GrupoLlantas, IdMarca, IdClase,
                //    IdProp, PesoVacio, VencePruebaHidro, TarjetadePropiedad, FechaAforo,
                //    placavehiculo, usuario);
                DS.DSRemolqueTableAdapters.VehiT_UpdateDatosRemolqueTableAdapter DSupdateremolque = new DS.DSRemolqueTableAdapters.VehiT_UpdateDatosRemolqueTableAdapter();
                DSupdateremolque.GetData_VehiT_UpdateDatosRemolque (IdPlacaRO, TipoAfiliacion, Modelo, Capacidad,
                    TipoRemolque, NumeroEjes, Unidad, Marca, PesoVehicular, GrupoLlantas, IdMarca, IdClase,
                    IdProp, PesoVacio, VencePruebaHidro, TarjetadePropiedad, FechaAforo,
                    placavehiculo, usuario, Vencekingpin, idpaisoperacion);
                //DSupdateremolque.Get_UpdateDatosRemolque(IdPlacaRO, TipoAfiliacion, Modelo,
                //    Capacidad, TipoRemolque, NumeroEjes, Unidad, Marca, PesoVehicular, GrupoLlantas, IdMarca,
                //    IdClase, IdProp, PesoVacio, VencePruebaHidro, TarjetadePropiedad, FechaAforo, placavehiculo,
                //   usuario);

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
