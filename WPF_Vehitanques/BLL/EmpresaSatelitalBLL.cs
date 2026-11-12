using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;
using WPF_Vehitanques.DS;
using WPF_Vehitanques.Presentacion.Ventanas;
using System.Windows.Media;


namespace WPF_Vehitanques.BLL
{
    public class EmpresaSatelitalBLL
    {
        ComercialPEntities contex = new ComercialPEntities();
        
        MensajeBLL mensajebll;
        VentanaMensaje ventmensaje;


        public Boolean ActualizaEstadoEmpresaSAtelital(decimal id, Boolean Estado, string UsrModifica)
        {
            try
            {
                DS.DSEmpSatelitalTableAdapters.VehiT_UpdateEstadoEmpresaSatelitalTableAdapter DSUpdateSAtelital = new DS.DSEmpSatelitalTableAdapters.VehiT_UpdateEstadoEmpresaSatelitalTableAdapter();
                DSUpdateSAtelital.GetData_VehiT_UpdateEstadoEmpresaSatelital(id, Estado , UsrModifica);
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


        public Boolean ActualizaDAtosEmpresaSAtelital(decimal id, string Nombre, string Nit, string Direccion, string Representante,
            string Telefono1, string Telefono2, string Ciudad, string DirWeb, string UsrModifica)
        {
            try
            {
                DS.DSEmpSatelitalTableAdapters.VehiT_UpdateDatosEmpresaSatelitalTableAdapter DSUpdateSAtelital = new DS.DSEmpSatelitalTableAdapters.VehiT_UpdateDatosEmpresaSatelitalTableAdapter();
                DSUpdateSAtelital.GetData_VehiT_UpdateDatosEmpresaSatelital(id,Nombre, Nit, Direccion, Representante, Telefono1, Telefono2, Ciudad, DirWeb, UsrModifica);
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

        public Boolean GuardarDAtosEmpresaSAtelital( string Nombre, string Nit,string Direccion,string  Representante,
            string Telefono1,string Telefono2,string Ciudad,string DirWeb,string UsrModifica)
        {
            try
            {
                DS.DSEmpSatelitalTableAdapters.VehiT_GuardarDatosEmpresaSatelitalTableAdapter DSSAtelital = new DS.DSEmpSatelitalTableAdapters.VehiT_GuardarDatosEmpresaSatelitalTableAdapter();
                DSSAtelital.GetData_VehiT_GuardarDatosEmpresaSatelital(Nombre, Nit, Direccion, Representante, Telefono1, Telefono2, Ciudad,true , DirWeb,  UsrModifica);
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

        public List<VehiT_ConsultaEmpSAtelitalPor_ID_Result> GetCompaniaPorID(string IDempre)
        {
            var lista = contex.VehiT_ConsultaEmpSAtelitalPor_ID(IDempre);

            return lista.ToList();
        }
        public List<VehiT_ConsultaEmpSAtelitalPor_Nit_Result> GetDatosCompaniaPorNIT(string nit)
        {
            var lista = contex.VehiT_ConsultaEmpSAtelitalPor_Nit(nit);

            return lista.ToList();
        }
        public List<VehiT_ConsultaCompniasatelital_Result> GEtCompaniaSatelital()
        {
            var lista = contex.VehiT_ConsultaCompniasatelital();
            return lista.ToList();
        }

        public List  <VehiT_ConsultaEmpSAtelitalPorNombre_Nit_Result  >GetCompaniasSatelitales(string nombre, string nit, int inicio, int final)

    {
        var lista = contex.VehiT_ConsultaEmpSAtelitalPorNombre_Nit(nombre, nit, inicio, final);
            return lista.ToList();
    }

        public int ContadorEmpresasSatelitales(string nombre, string nit)
        {
            int retorno = 0;
            var lista = contex.VehiT_ContadorEmpSAtelitalPorNombre_Nit (nombre,nit) ;
            var lista2 = lista.ToList();
            if (lista2.Count > 0)
            {

                retorno = Convert.ToInt32(lista2[0].Value);
            }
            return retorno;
        }


    }
}
