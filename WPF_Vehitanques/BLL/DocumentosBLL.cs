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
    public class DocumentosBLL
    {
        ComercialPEntities contex = new ComercialPEntities();
        MensajeBLL mensajebll;
        VentanaMensaje ventmensaje;

        public List<Vehit_ConsultaDocumentosTipoPoliza_Result> GetDocumentosTipoPoliza()
        {
            var lista = contex.Vehit_ConsultaDocumentosTipoPoliza();
            return lista.ToList();
        }

        public List<VehiT_ConsultaRutaDocumentos_Result> GetRutaDocumentos(int idruta)
        {
            var lista = contex.VehiT_ConsultaRutaDocumentos(idruta);
            return lista.ToList();
        }
        public List<VehiT_ConsultaTamanioDocumentos_Result> GetTamaniodocumentos()
        {
            var lista = contex.VehiT_ConsultaTamanioDocumentos();
            return lista.ToList();
        }
        public List<VehiT_ConsultaTipoDocumentos_Result> GetTipoDocumentos(int tipodoc)
        {
            var lista = contex.VehiT_ConsultaTipoDocumentos(tipodoc);
            return lista.ToList();
        }

        public Boolean UpdateEStadoDocumentos(int CodigoDoc, string Placa_Identificado)
        {
            try
            {
                DS.DSDocumentosTableAdapters.VehiT_UpdateDocumentosHojaDeVidaVEhiculoTableAdapter Dsguardadoc = new DS.DSDocumentosTableAdapters.VehiT_UpdateDocumentosHojaDeVidaVEhiculoTableAdapter();
                Dsguardadoc.Get_VehiT_UpdateDocumentosHojaDeVidaVEhiculo(CodigoDoc,Placa_Identificado );
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

        public Boolean GuardarDocumento(String HisDoc_Nombre, string HisDoc_Tipo, string HisDoc_Peso,
            int CodigoDoc, String Doc_Ubicacion, string Placa_Identificado,string _idconductor,int _idruta,string usuario)
        {
            try
            {
                DS.DSDocumentosTableAdapters.VehiT_GuardarDocumentosHojaDeVidaVEhiculoTableAdapter Dsguardadoc = new DS.DSDocumentosTableAdapters.VehiT_GuardarDocumentosHojaDeVidaVEhiculoTableAdapter();
                Dsguardadoc.Get_GuardarDocumentosHojaDeVidaVEhiculo(HisDoc_Nombre, HisDoc_Tipo, HisDoc_Peso,
                    CodigoDoc, Doc_Ubicacion, Placa_Identificado, _idconductor, _idruta, usuario);
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
        public List<VehiT_ConsultaDocumentoXPlaca_Identificador_Result> GetDocumentoXPlaca_identificador(int tipodocumento, string placaId)
        {
            var lista = contex.VehiT_ConsultaDocumentoXPlaca_Identificador(tipodocumento, placaId);
            return lista.ToList();
        }
    }
}
