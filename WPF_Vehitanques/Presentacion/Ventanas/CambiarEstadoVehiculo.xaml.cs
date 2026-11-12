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
using WPF_Vehitanques.BLL;
using WPF_Vehitanques.Modelo;
using WPF_Vehitanques.Presentacion.Controles;
using WPF_Vehitanques.Utilidades;

namespace WPF_Vehitanques.Presentacion.Ventanas
{
    /// <summary>
    /// Lógica de interacción para CambiarEstadoVehiculo.xaml
    /// </summary>
    public partial class CambiarEstadoVehiculo : Window
    {
        EstadoBLL establl = new EstadoBLL();
        VehiculoBLL vehibll = new VehiculoBLL();
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj = new VentanaMensaje();
        InicioConsulta inicioconsul;
        Numeric numeric = new Numeric();
        private string placa, usuario;
        public CambiarEstadoVehiculo()
        {
            InitializeComponent();

        }

        private void cargarEstadoPlaca()
        {
            var lista = establl.GetEstadoPlaca();
            cbbestado.ItemsSource = lista;
            cbbestado.SelectedValuePath = "Estado";
            cbbestado.DisplayMemberPath = "Descripcion";
            cbbestado.SelectedValue = 2;
        }
        public void GetPlaca(string strplaca, string strusaurio)
        {
            cargarEstadoPlaca();
            placa = strplaca;
            usuario = strusaurio;
            lblplaca.Content = strplaca.ToUpper();
            CargarEstadoplacaVehiculo();
        }

        private void CargarEstadoplacaVehiculo()
        {
            var lista = vehibll.GetEstadoPlaca(placa);
            if (lista.Count > 0)
            {
                cbbestado.SelectedValue = lista[0].EstadoPlaca;
                txtobservaciones.Text = lista[0].Observacionbloqueo;
            }
        }
        public void GetPadre(InicioConsulta inicio)
        {
            inicioconsul = inicio;
        }

        private void btn_aceptar_Click(object sender, RoutedEventArgs e)
        {
            Boolean habilita_viaje = true;
            MessageBoxResult result1 = MessageBox.Show("Está Seguro que desea Modificar el estado a la placa?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result1 == MessageBoxResult.Yes)
            {
                if (txtobservaciones.Text == "")
                {
                    lblerror_observaciones.Foreground = new SolidColorBrush(Colors.Red);
                    lblerror_observaciones.Content = "Debe digitar el motivo del cambio de estado";
                }
                else
                {
                    MessageBoxResult result2 = MessageBox.Show("Desea Habilitar para un sólo viaje?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result2 == MessageBoxResult.Yes)
                    {
                        habilita_viaje = true;
                    }
                    else
                    {
                        habilita_viaje = false;
                    }
                    Boolean respuesta;
                    respuesta = vehibll.ActualizaEstadoPlaca(placa, Convert.ToInt16(cbbestado.SelectedValue), txtobservaciones.Text, habilita_viaje, usuario);
                    if (respuesta == true)
                    {
                        Mensabll = new MensajeBLL();
                        Mensabll.tipomensajes(12,"");
                        Venmsj = new VentanaMensaje();
                        Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                        Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                        Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                        Venmsj.ShowDialog();
                        inicioconsul.recibirordenestadoplaca(true);
                        this.Close();

                    }
                    else
                    {
                        Mensabll = new MensajeBLL();
                        Mensabll.tipomensajes(23,"");
                        Venmsj = new VentanaMensaje();
                        Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                        Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                        Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                        Venmsj.ShowDialog();
                        inicioconsul.recibirordenestadoplaca(false);
                        this.Close();
                    }
                }
            }
            else
            {
                inicioconsul.recibirordenestadoplaca(false);
                this.Close();
            }

        }
        private void TextboxtStringShift_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (validarCaracteresespeciales(e, txt) == true)
            {
                e.Handled = false;
            }
        }
        private Boolean validarCaracteresespeciales(KeyEventArgs e, TextBox txt)
        {
            Boolean respuesta;
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                MessageBox.Show(" la combinación de esta tecla no está pérmitida " + e.Key.ToString());
                txt.Text = "";
                respuesta = false;
            }
            else
            {
                respuesta = true;
            }
            return respuesta;
        }

    }
}
