using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Vehitanques.BLL
{
  public   class Andr_Operaciones
    {
        public  DS.DSAndroid.DSOperacionDataTable GetOperaciones(List<Modelo.seguridad_consultarMenuPermisos2_Result> lista)
        {
            DS.DSAndroid.DSOperacionDataTable DSOper = new DS.DSAndroid.DSOperacionDataTable();
            var listaprivi = (from rmenu in lista
                              where rmenu.idMenuPadre == 230 & rmenu.alterno == 1 & rmenu.orden == 4
                              select rmenu).ToList();

            DSOper.AddDSOperacionRow(1, "Seleccione una Operación");
            foreach (var item in listaprivi)
            {
                switch (item.nombreMenu)
                {
                    case "CRYOGAS":
                        if (item.Autorizar == 1)
                        {
                            DSOper.AddDSOperacionRow(860013704, "CRYOGAS");
                        }
                        break;
                    case "COLTANQUES":
                        if (item.Autorizar == 1)
                        {
                            DSOper.AddDSOperacionRow(860040576, "Coltanques propios");
                        }
                        break;
                }
            }
            return DSOper;
        }
    }
}
