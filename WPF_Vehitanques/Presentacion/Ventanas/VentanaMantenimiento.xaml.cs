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

namespace WPF_Vehitanques.Presentacion.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaMantenimiento.xaml
    /// </summary>
    public partial class VentanaMantenimiento : Window
    {
        MainWindow main;
        public VentanaMantenimiento()
        {
            InitializeComponent();
        }

        private void btn_ventanamensaje_Click(object sender, RoutedEventArgs e)
        {
            main.CerrarApp();
        }

        internal void recibirpadre(MainWindow mainWindow)
        {
            main = mainWindow;
            main.IsEnabled = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            main.CerrarApp();

        }
    }
}
