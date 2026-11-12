using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;
using WPF_Vehitanques.Presentacion.Ventanas;

namespace WPF_Vehitanques.BLL
{
  public   class ComprobantesBLL
    {
        ComercialPEntities contex = new ComercialPEntities();

        MensajeBLL mensajebll;
        VentanaMensaje ventmensaje;

        public List<VehiT_ConsultaComprobantesPropietarioXCedula_Result > GetComprobantesXpropietario(string idpropietario, int inicio, int final)
        {
            var lista = contex.VehiT_ConsultaComprobantesPropietarioXCedula(idpropietario, inicio, final);
            return lista.ToList();
        }

        public int ContadorComprobantes (string idpropietario)
        {
            int retorno = 0;
            var lista = contex.VehiT_ContadorComprobantesPropietarioXCedula (idpropietario );
            var lista2 = lista.ToList();
            if (lista2.Count > 0)
            {

                retorno = Convert.ToInt32(lista2[0].Value);
            }
            return retorno;
        }
    }
}
