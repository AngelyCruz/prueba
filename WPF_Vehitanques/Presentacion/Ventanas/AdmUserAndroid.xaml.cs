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
using WPF_Vehitanques.Utilidades;

namespace WPF_Vehitanques.Presentacion.Ventanas
{
    /// <summary>
    /// Lógica de interacción para AdmUserAndroid.xaml
    /// </summary>
    public partial class AdmUserAndroid : Window
    {
        #region Declaraciones
        TipoDocumentoBLL TiDocBll = new TipoDocumentoBLL();
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj = new VentanaMensaje();
        VehiT_ConsultaConductor_Result row_conductor = null;
        AndroidBLL _AndroidBLL = new AndroidBLL();
        Encriptar _encriptar = new Encriptar();
        MainWindow main;
        string usuario, agencia;
        Boolean booluserandroid = false;
        #endregion

        #region load
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarPrivilegios();
            DelegarPrivilegios();
            if (IsLoaded)
            {               
                CargarTipoDocumentos();
                cargarEstados();
                cargarOperacion();
                GetDAtosUserAndroid();
            }
            
        }
        public AdmUserAndroid()
        {
            InitializeComponent();
        }
        #endregion
        #region Métodos
        internal void ValidarPrivilegios()
        {
            var lista = SecurityVariables.PPerfilUsuario;
            var listaprivi = (from rmenu in lista
                              where rmenu.idMenuPadre == 230 & rmenu.alterno == 1 & rmenu.orden == 3
                              select rmenu).ToList();
            foreach (var item in listaprivi)
            {
                switch (item.nombreMenu)
                {
                    case "Agregar Usuarios Android":
                        if (item.Autorizar == 1)
                        {
                            booluserandroid = true;
                        }
                        else
                        {
                            booluserandroid = false;
                        }

                        break;
                    
                }
            }
        }
        private void DelegarPrivilegios()
        {
            if (booluserandroid == true)
            {
                btn_guardar.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                btn_guardar.Visibility = System.Windows.Visibility.Collapsed;
            }
            

            

        }
        public void RecibirPadre(MainWindow padre)
        {
            if (main == null)
                main = padre;
        }
        public Boolean ValidarCamposUsuario()
        {
            Boolean respues = false;
            if (cbboperacion.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar una operación para el conductor");
                respues = true;
                return true; ;
            }
            if (txtclave.Text.Trim() == "")
            {
                MessageBox.Show("Debe agregar una clave para el conductor");
                respues = true;
                return true; ;
            } 
            if (cbbestado.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar un estado para el conductor");
                respues = true;
                return true; ;

            }

            return respues;
        }
        private void GetDAtosUserAndroid()
        {
            var lista = _AndroidBLL.GetDatosUsuarioAndroid(Convert.ToDecimal( row_conductor.IdConductor));
            foreach(var item in lista )
            {
                cbboperacion.SelectedValue = item.NitOperacion;
                cbbestado.SelectedValue = item.IsEstado;
                txtclave.Text = _encriptar.Fun_Encriptar(item.Clave);
                txtclave.IsEnabled = false;
                chkmodificaclave.IsChecked = false;
                break;
            }
            
            
        }
        private void cargarEstados()
        {
            var lista = _AndroidBLL .GetEstadosAndroid ();
            cbbestado.ItemsSource = lista;
            cbbestado.SelectedValuePath = "IdEstado";
            cbbestado.DisplayMemberPath = "DescripEstado";
            cbbestado.SelectedIndex = 0;
        }
        private void cargarOperacion()
        {
            var lista = _AndroidBLL.GetOperacionAndroid ();
            cbboperacion.ItemsSource = lista;
            cbboperacion.SelectedValuePath = "NitOperacion";
            cbboperacion.DisplayMemberPath = "DescripcionOper";
            cbboperacion.SelectedIndex = 0;
        }
        public void RecibirConductor(Modelo.VehiT_ConsultaConductor_Result _row_conductor)
        {
            row_conductor = _row_conductor;
            txtnombres.Text = row_conductor.Nombres;
            txtapellido1.Text = row_conductor.Apellido1;
            txtapellido2.Text = row_conductor.Apellido2;
            txtNumdocumento.Text = row_conductor.IdConductor;
            txtusername.Text = row_conductor.IdConductor.Trim();
           // cbbtipodocumento.SelectedValue=_row_conductor.
        }
        private void CargarTipoDocumentos()
        {
            var lista = TiDocBll.GetTipoDocumentoIdentificacion();
            cbbtipodocumento.ItemsSource = lista;
            cbbtipodocumento.SelectedValuePath = "Codigo";
            cbbtipodocumento.DisplayMemberPath = "TipoDocumento";
            cbbtipodocumento.SelectedIndex = 0;
        }
        private void mostrarmensaje(Boolean respuesta)
        {
            if (respuesta == true)
            {
                Mensabll = new MensajeBLL();
                Mensabll.tipomensajes(12,"");
                Venmsj = new VentanaMensaje();
                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                Venmsj.ShowDialog();
                //inicioconsul.recibirordenestadoplaca(true);
                //this.Close();

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
                //inicioconsul.recibirordenestadoplaca(false);
                //this.Close();
            }
        }
        #endregion
        #region Eventos
        private void btn_generarclave_Click(object sender, RoutedEventArgs e)
        {
            GeneradorPassword Generator = new GeneradorPassword(10, 40, 10, 40);
            //GeneradorPassword Generator = new GeneradorPassword(10);
            string nuevaclave = Generator.GetNewPassword();
            txtclave.Text = nuevaclave;
            // (int longitudCaracteres, int porcentajeMayusculas, int porcentajeSimbolos, int porcentajeNumeros)

        }
        private void chkmodificaclave_Checked(object sender, RoutedEventArgs e)
        {
            txtclave.Text = "";
            btn_generarclave.IsEnabled = true;
        }
        private void chkmodificaclave_Unchecked(object sender, RoutedEventArgs e)
        {
            btn_generarclave.IsEnabled = false;
        }

        private void btn_guardar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCamposUsuario() == false ) 
            {
                foreach (FrameworkElement element in main.tolbarinformacion.Items)
                {
                    if (element is TextBlock)
                    {
                        var textblock = (TextBlock)element;
                        switch (textblock.Name)
                        {
                            case "txtusuario":
                                usuario = textblock.Text;
                                break;
                            case "txtagencia":
                                agencia = textblock.Text;
                                break;
                        }
                        if (usuario != "" & agencia != "")
                        {
                            break;
                        }


                    }

                }

                Boolean respuesta = false;
                var lista = _AndroidBLL.GetDatosUsuarioAndroid(Convert.ToDecimal(row_conductor.IdConductor));
                if (lista.Count > 0)
                {
                    MessageBoxResult result1 = MessageBox.Show("Este usuario ANDROID ya  Existe. Desea Actualizar sus datos?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result1 == MessageBoxResult.Yes)
                    {
                        if (chkmodificaclave.IsChecked == true)
                        {
                            if (txtclave.Text.Trim() == "")
                            {
                                MessageBox.Show("No puede guardar Datos de un usuario sin clave", "Mensaje");
                                return;
                            }
                            else
                            {
                                if (txtclave.Text.Length > 6)
                                {
                                    respuesta = _AndroidBLL.UpdateDatosUsuario(Convert.ToDecimal(txtNumdocumento.Text), txtclave.Text.Trim(), txtnombres.Text.Trim(), txtapellido1.Text.Trim() +" "+ txtapellido2.Text.Trim(), Convert.ToDecimal(cbboperacion.SelectedValue), 1,usuario );
                                }
                                else
                                {
                                    MessageBox.Show("La contraseña debe tener mínimo 6 caracteres", "Mensaje");
                                    return;
                                }

                            }

                        }
                        else
                        {
                            respuesta = _AndroidBLL.UpdateDatosUsuario(Convert.ToDecimal(txtNumdocumento.Text), lista[0].Clave, txtnombres.Text, txtapellido1.Text + " " + txtapellido2.Text, Convert.ToDecimal(cbboperacion.SelectedValue), Convert.ToInt32(cbbestado.SelectedValue), usuario);
                        }
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    respuesta = _AndroidBLL.GuardarDatosUsuario(Convert.ToDecimal(txtNumdocumento.Text), txtclave.Text, txtnombres.Text, txtapellido1.Text + txtapellido2.Text, Convert.ToDecimal(cbboperacion.SelectedValue), 1,usuario );
                }

                mostrarmensaje(respuesta);
            }
        }
        #endregion

        private void btn_MailAndroid_Click(object sender, RoutedEventArgs e)
        {

        }

       

       

      

    }
}
