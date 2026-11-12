using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Vehitanques.Utilidades
{
  public   class DigitoVerificacion
    {
        public int ValidarDigitoVerificacion(string Numero)
        {

            Numero = LimpiarSeparadores(Numero);
            decimal valor = Convert.ToDecimal(Numero);
            string format = "000000000000000";
            string mDocumento = valor.ToString(format);
            //string mDocumento = String.Format(Numero, "000000000000000");
            int mDigito = CalcularDigito(mDocumento);
            return mDigito;
        }
        private string LimpiarSeparadores(string Numero)
        {
            Numero = Numero.Replace(",", "");
            Numero = Numero.Replace(".", "");
            Numero = Numero.Replace("-", "");
            Numero = Numero.Replace(" ", "");
            return Numero;
        }
        private int CalcularDigito(string mDocumento)
        {
            int mDigito;
            long Total;
            int Residuo;
            Total = Convert.ToInt32(mDocumento.Substring(0, 1)) * 71;
            Total += Convert.ToInt32(mDocumento.Substring(1, 1)) * 67;
            Total += Convert.ToInt32(mDocumento.Substring(2, 1)) * 59;
            Total += Convert.ToInt32(mDocumento.Substring(3, 1)) * 53;
            Total += Convert.ToInt32(mDocumento.Substring(4, 1)) * 47;
            Total += Convert.ToInt32(mDocumento.Substring(5, 1)) * 43;
            Total += Convert.ToInt32(mDocumento.Substring(6, 1)) * 41;
            Total += Convert.ToInt32(mDocumento.Substring(7, 1)) * 37;
            Total += Convert.ToInt32(mDocumento.Substring(8, 1)) * 29;
            Total += Convert.ToInt32(mDocumento.Substring(9, 1)) * 23;
            Total += Convert.ToInt32(mDocumento.Substring(10, 1)) * 19;
            Total += Convert.ToInt32(mDocumento.Substring(11, 1)) * 17;
            Total += Convert.ToInt32(mDocumento.Substring(12, 1)) * 13;
            Total += Convert.ToInt32(mDocumento.Substring(13, 1)) * 7;
            Total += Convert.ToInt32(mDocumento.Substring(14, 1)) * 3;
            Residuo = Convert.ToInt32(Total % 11);
            switch (Residuo)
            {
                case 0:
                    mDigito = 0;
                    break;
                case 1:
                    mDigito = 1;
                    break;
                default:
                    mDigito = 11 - Residuo;
                    break;
            }

            return mDigito;

        }

          
    }
}
