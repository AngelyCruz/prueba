using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Vehitanques.Utilidades
{
    public class Numeric
    {
        public bool IsNumeric(string str)
        {
            double _val;
            long n;
            bool valor = long.TryParse(str, out n);
            //  bool valor = Double.TryParse(str, _val);
            return valor;
        }
    }
}
