using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_Vehitanques.DS;


namespace WPF_Vehitanques.Presentacion.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaGrillaAlertas.xaml
    /// </summary>
    public partial class VentanaGrillaAlertas : Window
    {
        public VentanaGrillaAlertas()
        {
            InitializeComponent();
        }
        DSMensajes.DSMensajesHvVehiculoDataTable DSmensa = new DSMensajes.DSMensajesHvVehiculoDataTable();
        public void GetDatosGrilla(DSMensajes.DSMensajesHvVehiculoDataTable DS)
        {
            DSmensa = DS;
            // gridlistamensajes.ItemsSource = null;
            gridlistamensajes.ItemsSource = DSmensa;
        }



    }
}
