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
using WPF_Vehitanques.Modelo;
using WPF_Vehitanques.BLL;
using WPF_Vehitanques.Presentacion.Controles;


namespace WPF_Vehitanques.Presentacion.Ventanas
{
    /// <summary>
    /// Lógica de interacción para AprobarContratos.xaml
    /// </summary>
    public partial class AprobarContratos : Window
    {
        private List<VehiT_ConsultaExistenciadeContrato_Result> listacontrato;
        private String Username;
        private ContratoBLL contratobll = new ContratoBLL();
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj = new VentanaMensaje();
        InicioVehiculos inicioconsul;

        public AprobarContratos()
        {
            InitializeComponent();
        }
        internal void SetListaContrato(List<VehiT_ConsultaExistenciadeContrato_Result> _lista, string _usuario)
        {
            listacontrato = _lista;
            Username = _usuario;
            cargarinfo();
        }
        public void GetPadre(InicioVehiculos inicio)
        {
            inicioconsul = inicio;
        }
        private void cargarinfo()
        {
            if (listacontrato.Count > 0)
            {
                txtplaca.Text = listacontrato[0].IdPlaca;
                txtNumdocumento.Text = listacontrato[0].IdConductor;
                txtNumcontrato.Text = listacontrato[0].IdContrato.ToString();
                txtusuarioCreador.Text = listacontrato[0].Usuario;
                lbluser.Content = Username;
                if (listacontrato[0].UsuarioAprobo != "" & listacontrato[0].FechaAprobacion != null)
                {
                    btn_Aprobar.IsEnabled = false;
                    btn_Noaprobar.IsEnabled = true;
                }
                else
                {
                    btn_Aprobar.IsEnabled = true;
                    btn_Noaprobar.IsEnabled = false;
                }
            }
            else
            {
                /// mostrar mensaje de que la lista está vacía.
            }
        }

    

        private void btn_Aprobar_Click(object sender, RoutedEventArgs e)
        {
            Boolean respuesta = true;
            respuesta = contratobll.GuardarAprobacioncontrato(Convert.ToInt32(txtNumcontrato.Text), txtplaca.Text, Username);
            if (respuesta == true)
            {
                Mensabll = new MensajeBLL();
                Mensabll.tipomensajes(12,"");
                Venmsj = new VentanaMensaje();
                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                Venmsj.ShowDialog();
                inicioconsul.CargarContrato();
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

            }
            this.Close();
        }

        private void btn_Noaprobar_Click(object sender, RoutedEventArgs e)
        {
            Boolean respuesta = true;
            respuesta = contratobll.GuardarNoAprobacioncontrato(Convert.ToInt32(txtNumcontrato.Text), txtplaca.Text);
            if (respuesta == true)
            {
                Mensabll = new MensajeBLL();
                Mensabll.tipomensajes(12,"");
                Venmsj = new VentanaMensaje();
                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                Venmsj.ShowDialog();
                inicioconsul.CargarContrato();
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

            }
            this.Close();
        }

    }
}
