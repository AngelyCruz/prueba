using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Vehitanques.Utilidades
{
 public    class Permisos
    {
        public string hostName { get; set; }
        public string myIP { get; set; }
        public Permisos()
        {
            hostName = Dns.GetHostName();
            myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
        }
    }
}
