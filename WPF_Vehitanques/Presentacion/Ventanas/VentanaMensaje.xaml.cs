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
    /// Lógica de interacción para VentanaMensaje.xaml
    /// </summary>
    public partial class VentanaMensaje : Window
    {
        public VentanaMensaje()
        {
            InitializeComponent();
        }
        public void cargarmensaje(String titulo, String mensaje)
        {
            lblTitulo.Content = titulo;
            lblmensaje.Text = mensaje;
        }

        private void btn_ventanamensaje_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
