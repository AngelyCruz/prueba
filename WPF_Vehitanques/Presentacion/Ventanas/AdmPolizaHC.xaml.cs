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
    /// Lógica de interacción para AdmPolizaHC.xaml
    /// </summary>
    public partial class AdmPolizaHC : Window
    {
        #region Declaraciones
        AseguradoraBLL AseBll = new AseguradoraBLL();
        PolizaBLL polizabll = new PolizaBLL();
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj = new VentanaMensaje();
        private int iStar = 1;
        private int iEnd = 10;
        private int iAdd = 10;
        private string placavehiculo, placaremolque, usuario;
        AdmDocumentos Ventanadoc;
        private Boolean Actualizar = false;
        Boolean boolcambioestadoHC=false;

        Numeric numeric = new Numeric();
        MainWindow main;
        #endregion
        #region  load_closed
        private void Window_Closed(object sender, EventArgs e)
        {
            if (Ventanadoc != null)
                Ventanadoc.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargarAseguradoras();
            ValidarPrivilegios();
            if (placavehiculo != "")
                llenargrilla();
            if (placaremolque != "")
                llenarremolque();
            //  fechaactual = DateTime.Now;
        }
        public AdmPolizaHC()
        {
            InitializeComponent();
        }
        #endregion
        #region Métodos
        private void ValidarPrivilegios()
        {
            var lista = SecurityVariables.PPerfilUsuario;
            var listaprivi = (from rmenu in lista
                              where rmenu.idMenuPadre == 230 & rmenu.alterno == 1 & rmenu.orden == 3
                              select rmenu).ToList();
            foreach (var item in listaprivi)
            {
                switch (item.nombreMenu)
                {
                    case "Autoriza borrar póliza HC":
                        if (item.Autorizar == 1)
                        {
                            boolcambioestadoHC = true;
                        }
                        else
                        {
                            boolcambioestadoHC = false;
                        }                       
                        break;
                //    case "Editar Emp Satelital":
                //        if (item.Actualizar == 1)
                //        {
                //            booleditar = true;
                //        }
                //        else
                //        {
                //            booleditar = false;
                //        }
                //        break;
                //    case "Crear Emp Satelital":
                //        if (item.Actualizar == 1)
                //        {
                //            boolcrear = true;
                //        }
                //        else
                //        {
                //            boolcrear = false;
                //        }
                //        break;
                }
            }
        }
        public void RecibirPadre(MainWindow padre)
        {
            main = padre;
        }
        public void CargarPlaca(string placa, string strusuario)
        {
            usuario = strusuario;
            txtplaca.Text = placa;
            placavehiculo = placa;
        }
        public void CargarPlacaRemolque(string placa, string strusuario)
        {
            usuario = strusuario;
            txtplacaremolque.Text = placa;
            placaremolque = placa;
        }
        private void CargarAseguradoras()
        {
            var lista = AseBll.GetASeguradora();
            cbbciaseguradora.ItemsSource = lista;
            cbbciaseguradora.SelectedValuePath = "ID";
            cbbciaseguradora.DisplayMemberPath = "Nombre";
            cbbciaseguradora.SelectedIndex = 0;
        }
        private void CargarGrilla(List<Modelo.VehiT_ConsultaHistoricoPolizasHC_Result> lista)
        {
            gridlistadovehiculos.ItemsSource = null;

            //var listaaux = lista;
            gridlistadovehiculos.ItemsSource = lista;
            if (boolcambioestadoHC == true)
            {
                gridlistadovehiculos.Columns[0].Visibility = System.Windows.Visibility.Visible;

            }
            else
            {
                gridlistadovehiculos.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        private void Cargarremolque(List<Modelo.VehiT_ConsultaHistoricoPolizasHC_Result> lista)
        {
            gridlistadoremolque.ItemsSource = null;
           // var listaaux = lista;
            gridlistadoremolque.ItemsSource = lista;
            if (boolcambioestadoHC == true)
            {
                gridlistadoremolque.Columns[0].Visibility = System.Windows.Visibility.Visible;

            }
            else
            {
                gridlistadoremolque.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        private void llenargrilla()
        {
            try
            {
                pnl_Paginacion.Visibility = Visibility.Visible;
                gridlistadovehiculos.ItemsSource = null;
                gridlistadovehiculos.Visibility = Visibility.Visible;
                lbl_Inicio.Text = iStar.ToString();

                lbl_Total.Text = Convert.ToString(polizabll.contadorpolizaHc(txtplaca.Text));
                if (Convert.ToInt32(lbl_Total.Text) <= iEnd)
                {
                    lbl_Fin.Text = lbl_Total.Text;
                    btn_sig.IsEnabled = false;
                    pnl_Paginacion.Visibility = Visibility.Collapsed;
                }
                else
                {
                    pnl_Paginacion.Visibility = Visibility.Visible;
                    lbl_Fin.Text = iEnd.ToString();
                    //'linea nueva de código
                    btn_sig.IsEnabled = true;
                }
                var lista = polizabll.ConsultaHistoricopolizas(txtplaca.Text, iStar, iEnd);
                if (lista.Count > 0)
                {
                    // gridlistadovehiculos.ItemsSource = lista;
                    CargarGrilla(lista);
                }
                else
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(2,"");
                    Venmsj = new VentanaMensaje();
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                Venmsj = new VentanaMensaje();
                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                Venmsj.cargarmensaje(ex.ToString(), ex.Message.ToString());
                Venmsj.ShowDialog();
            }
        }
        private void llenarremolque()
        {
            if (txtplacaremolque.Text != "")
            {
                try
                {
                    pnl_Paginacion1.Visibility = Visibility.Visible;
                    gridlistadoremolque.ItemsSource = null;
                    gridlistadoremolque.Visibility = Visibility.Visible;
                    lbl_Inicio1.Text = iStar.ToString();

                    lbl_Total1.Text = Convert.ToString(polizabll.contadorpolizaHc(txtplacaremolque.Text));
                    if (Convert.ToInt32(lbl_Total1.Text) <= iEnd)
                    {
                        lbl_Fin1.Text = lbl_Total1.Text;
                        btn_sig1.IsEnabled = false;
                        pnl_Paginacion1.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        pnl_Paginacion1.Visibility = Visibility.Visible;
                        lbl_Fin1.Text = iEnd.ToString();
                        //'linea nueva de código
                        btn_sig1.IsEnabled = true;
                    }
                    var lista = polizabll.ConsultaHistoricopolizas(txtplacaremolque.Text, iStar, iEnd);
                    if (lista.Count > 0)
                    {
                        // gridlistadovehiculos.ItemsSource = lista;
                        Cargarremolque(lista);
                    }
                    else
                    {
                        Mensabll = new MensajeBLL();
                        Mensabll.tipomensajes(2,"");
                        Venmsj = new VentanaMensaje();
                        Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                        Venmsj.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(ex.ToString(), ex.Message.ToString());
                    Venmsj.ShowDialog();
                }
            }

        }
        private void CargarDatospoliza(List<Modelo.VehiT_ConsultaPolizaXNumPoliza_Result> lista)
        {
            Actualizar = true;
            txtnumpoliza.Text = lista[0].Poliza;
            dtfechainicial.SelectedDate = lista[0].Fechainicial;
            dtfechafinal.SelectedDate = lista[0].Fechafinal;
            cbbciaseguradora.SelectedValue = lista[0].Aseguradora;
        }
        private void limpiarcampos()
        {
            Actualizar = false;
            txtnumpoliza.Text = "";
            dtfechainicial.SelectedDate = null;
            dtfechafinal.SelectedDate = null;
            cbbciaseguradora.SelectedIndex = 0;
            rdio_remolque.IsChecked = false;
            rdio_vehiculo.IsChecked = false;
        }
        private Boolean validarDatos()
        {
            Boolean Respuesta = false;
            if (txtnumpoliza.Text != "")
            {
                lblerror_poliza.Content = "";
            }
            else
            {
                lblerror_poliza.Content = "***";
                Respuesta = true;
            }
            if (dtfechafinal.SelectedDate != null)
            {
                lblerror_fechafinal.Content = "";
            }
            else
            {
                lblerror_fechafinal.Content = "***";
                Respuesta = true;
            }
            if (dtfechainicial.SelectedDate != null)
            {
                lblerror_fechaini.Content = "";
            }
            else
            {
                lblerror_fechaini.Content = "***";
                Respuesta = true;
            }
            if (cbbciaseguradora.SelectedIndex < 0)
            {
                lblerror_aseguradora.Content = "***";
                Respuesta = true;
            }
            else
            {
                lblerror_aseguradora.Content = "";
            }
            if (rdio_remolque.IsChecked == true || rdio_vehiculo.IsChecked == true)
            {
                lblerror_checkbox.Content = "";
            }
            else
            {
                lblerror_checkbox.Content = "***";
                Respuesta = true;
            }
            return Respuesta;
        }
        private List<Modelo.VehiT_ConsultaPolizaXNumPoliza_Result> validarExistepoliza(string numpoliza, string placa)
        {
            var lista = polizabll.GetPolizaPorPlaca_numPoliza(numpoliza, placa);
            return lista;
        }
        #endregion
        #region Eventos
        private void btn_cancelar_Click(object sender, RoutedEventArgs e)
        {
            limpiarcampos();
        }
        private void gridlistadovehiculos_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            switch (gridlistadovehiculos.CurrentCell.Column.DisplayIndex)
            {
                case 0:
                    e.ClipboardRowContent.Clear();
                    break;
             
                default:
                    var currentCell = e.ClipboardRowContent[gridlistadovehiculos.CurrentCell.Column.DisplayIndex];
                    e.ClipboardRowContent.Clear();
                    e.ClipboardRowContent.Add(currentCell);
                    break;
            }           

        }
        private void gridlistadoremolque_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            switch (gridlistadoremolque.CurrentCell.Column.DisplayIndex)
            {
                case 0:
                    e.ClipboardRowContent.Clear();
                    break;

                default:
                    var currentCell = e.ClipboardRowContent[gridlistadoremolque.CurrentCell.Column.DisplayIndex ];
                    e.ClipboardRowContent.Clear();
                    e.ClipboardRowContent.Add(currentCell);
                    break;
            }
        }
        private void btn_ant_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int iRes = 0;
                if (Convert.ToInt32(lbl_Fin.Text) == Convert.ToInt32(lbl_Total.Text))
                {
                    iRes = Convert.ToInt32(lbl_Fin.Text) - Convert.ToInt32(lbl_Inicio.Text);
                }
                else
                {
                    iRes = iAdd;
                }
                lbl_Inicio.Text = Convert.ToString(Convert.ToInt32(lbl_Inicio.Text) - iAdd);
                lbl_Fin.Text = Convert.ToString(Convert.ToInt32(lbl_Fin.Text) - iRes);
                lbl_Total.Text = Convert.ToString(polizabll.contadorpolizaHc(txtplaca.Text));
                if (Convert.ToInt32(lbl_Inicio.Text) > 1)
                {
                    btn_sig.IsEnabled = true;
                }
                else
                {
                    lbl_Inicio.Text = Convert.ToString(iStar);
                    lbl_Fin.Text = Convert.ToString(iEnd);
                    btn_ant.IsEnabled = false;
                    btn_sig.IsEnabled = true;
                }
                gridlistadovehiculos.ItemsSource = null;
                var lista = polizabll.ConsultaHistoricopolizas(txtplaca.Text, Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
                if (lista.Count > 0)
                {
                    CargarGrilla(lista);
                }
                else
                {

                }
                gridlistadovehiculos.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
        private void btn_sig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lbl_Inicio.Text = Convert.ToString(Convert.ToInt32(lbl_Inicio.Text) + iAdd);
                lbl_Fin.Text = Convert.ToString(Convert.ToInt32(lbl_Fin.Text) + iAdd);
                lbl_Total.Text = Convert.ToString(polizabll.contadorpolizaHc(txtplaca.Text));
                if (Convert.ToInt32(lbl_Fin.Text) < Convert.ToInt32(lbl_Total.Text))
                {
                    btn_ant.IsEnabled = true;
                }
                else
                {
                    lbl_Fin.Text = lbl_Total.Text;
                    btn_sig.IsEnabled = false;
                    btn_ant.IsEnabled = true;
                }
                gridlistadovehiculos.ItemsSource = null;
                var lista = polizabll.ConsultaHistoricopolizas(txtplaca.Text, Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
                if (lista.Count > 0)
                {
                    CargarGrilla(lista);
                }
                gridlistadovehiculos.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
        private void btn_ant1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int iRes = 0;
                if (Convert.ToInt32(lbl_Fin1.Text) == Convert.ToInt32(lbl_Total1.Text))
                {
                    iRes = Convert.ToInt32(lbl_Fin1.Text) - Convert.ToInt32(lbl_Inicio1.Text);
                }
                else
                {
                    iRes = iAdd;
                }
                lbl_Inicio1.Text = Convert.ToString(Convert.ToInt32(lbl_Inicio1.Text) - iAdd);
                lbl_Fin1.Text = Convert.ToString(Convert.ToInt32(lbl_Fin1.Text) - iRes);

                lbl_Total1.Text = Convert.ToString(polizabll.contadorpolizaHc(txtplacaremolque.Text));

                if (Convert.ToInt32(lbl_Inicio1.Text) > 1)
                {
                    btn_sig1.IsEnabled = true;
                }
                else
                {
                    lbl_Inicio1.Text = Convert.ToString(iStar);
                    lbl_Fin1.Text = Convert.ToString(iEnd);
                    btn_ant1.IsEnabled = false;
                    btn_sig1.IsEnabled = true;
                }
                gridlistadoremolque.ItemsSource = null;
                var lista = polizabll.ConsultaHistoricopolizas(txtplacaremolque.Text, Convert.ToInt32(lbl_Inicio1.Text), Convert.ToInt32(lbl_Fin1.Text));
                if (lista.Count > 0)
                {
                    // gridlistadovehiculos.ItemsSource = lista;
                    Cargarremolque(lista);
                }
                else
                {

                }

                gridlistadoremolque.Visibility = Visibility.Visible;

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
        private void btn_sig1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lbl_Inicio1.Text = Convert.ToString(Convert.ToInt32(lbl_Inicio1.Text) + iAdd);
                lbl_Fin1.Text = Convert.ToString(Convert.ToInt32(lbl_Fin1.Text) + iAdd);

                lbl_Total1.Text = Convert.ToString(polizabll.contadorpolizaHc(txtplacaremolque.Text));
                if (Convert.ToInt32(lbl_Fin1.Text) < Convert.ToInt32(lbl_Total1.Text))
                {
                    btn_ant1.IsEnabled = true;
                }
                else
                {
                    lbl_Fin1.Text = lbl_Total1.Text;
                    btn_sig1.IsEnabled = false;
                    btn_ant1.IsEnabled = true;
                }
                gridlistadoremolque.ItemsSource = null;
                var lista = polizabll.ConsultaHistoricopolizas(txtplacaremolque.Text, Convert.ToInt32(lbl_Inicio1.Text), Convert.ToInt32(lbl_Fin1.Text));
                if (lista.Count > 0)
                {
                    // gridlistadovehiculos.ItemsSource = lista;
                    Cargarremolque(lista);
                }


                gridlistadoremolque.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
        private void btn_aceptar_Click(object sender, RoutedEventArgs e)
        {
            Boolean boolpolizaremolque = false;
            if (validarDatos() == false)
            {
                DateTime fechaini, fechafinal;
                fechaini = (DateTime)dtfechainicial.SelectedDate;
                fechafinal = (DateTime)dtfechafinal.SelectedDate;
                string placa = "";
                Boolean respuesta;
                if (rdio_remolque.IsChecked == true & txtplacaremolque.Text != "")
                {
                    boolpolizaremolque=true;
                    placa = txtplacaremolque.Text;
                }                    
                else
                {
                    placa = txtplaca.Text;
                }
                    
                if (placa != "")
                {
                    var lista1 = validarExistepoliza(txtnumpoliza.Text, placa);
                    if (lista1.Count > 0 )
                    {
                        MessageBoxResult result1 = MessageBox.Show("La póliza esta por ser modificada, Desea realizar ésta actualización?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result1 == MessageBoxResult.Yes)
                        {
                            respuesta = polizabll.ActualizaDatosPoliza(txtnumpoliza.Text, fechaini, fechafinal, placa, cbbciaseguradora.SelectedValue.ToString(), usuario);
                            if (respuesta == true)
                            {
                                Mensabll = new MensajeBLL();
                                Mensabll.tipomensajes(12,"");
                                Venmsj = new VentanaMensaje();
                                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                                Venmsj.ShowDialog();
                                llenargrilla();
                                llenarremolque();
                                limpiarcampos();
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
                        }
                        else
                        {
                            limpiarcampos();
                        }
                    }
                    else
                    {
                        respuesta = polizabll.GuardarDAtosPoliza(txtnumpoliza.Text, fechaini, fechafinal, placa, cbbciaseguradora.SelectedValue.ToString(), usuario);
                        if (respuesta == true)
                        {
                            Mensabll = new MensajeBLL();
                            Mensabll.tipomensajes(12,"");
                            Venmsj = new VentanaMensaje();
                            Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                            Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                            Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                            Venmsj.ShowDialog();
                            llenargrilla();
                            llenarremolque();
                            limpiarcampos();
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
                        
                    }
                }
                else
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(31,"");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes + placa);
                    Venmsj.ShowDialog();
                    limpiarcampos();
                }
            }
        }
        private void dtfechainicial_CalendarClosed(object sender, RoutedEventArgs e)
        {
            DateTime fechaini, fechafutura;
            Nullable<DateTime> fechafinal = null;
            fechaini = Convert.ToDateTime(dtfechainicial.SelectedDate);
            fechafutura = fechaini.AddYears(1);
            if (dtfechafinal.SelectedDate != null)
            {
                fechafinal = (DateTime)dtfechafinal.SelectedDate;
            }
            if (fechafinal != null)
            {
                if (fechafinal > fechafutura)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(13,"");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfechafinal.SelectedDate = null;
                }
            }
        }
        private void dtfechafinal_CalendarClosed(object sender, RoutedEventArgs e)
        {
            DateTime fechafinal, fechaanterior;
            Nullable<DateTime> fechaini = null;
            fechafinal = Convert.ToDateTime(dtfechafinal.SelectedDate);
            fechaanterior = fechafinal.AddYears(-1);
            if (dtfechainicial.SelectedDate != null)
            {
                fechaini = (DateTime)dtfechainicial.SelectedDate;
            }
            if (fechaini != null)
            {
                if (fechaini < fechaanterior)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(13,"");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfechafinal.SelectedDate = null;
                }
            }
        }
        private void btn_poliza_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
                Ventanadoc.Close();
            string placa = "";
            if (rdio_remolque.IsChecked == true & txtplacaremolque.Text != "")
                placa = txtplacaremolque.Text;
            else
                placa = txtplaca.Text;
            if (placa != "")
            {
                if (Ventanadoc != null)
                {
                    Ventanadoc.Close();
                }
                Ventanadoc = new AdmDocumentos(main);
                Ventanadoc.TipoDocumento(17);
                Ventanadoc.IdDocumento(placa);
                Ventanadoc.Show();
            }
        }
        private void btnverDatos_Click(object sender, RoutedEventArgs e)
        {
            string idpoliza = ((Button)sender).CommandParameter.ToString();
            var lista = validarExistepoliza(idpoliza, txtplaca.Text);
            rdio_vehiculo.IsChecked = true;
            CargarDatospoliza(lista);
        }
        private void btnverDatosremolque_Click(object sender, RoutedEventArgs e)
        {
            string idpoliza = ((Button)sender).CommandParameter.ToString();
            var lista = validarExistepoliza(idpoliza, txtplacaremolque.Text);
            rdio_remolque.IsChecked = true;
            CargarDatospoliza(lista);
        }
        private void txtnumpoliza_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                var lista = validarExistepoliza(txt.Text, "");
                if (lista.Count > 0)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(30,"");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes + " para la placa: " + lista[0].Placa);
                    Venmsj.ShowDialog();
                    Actualizar = false;
                    txtnumpoliza.Text = "";
                    dtfechainicial.SelectedDate = null;
                    dtfechafinal.SelectedDate = null;
                    cbbciaseguradora.SelectedIndex = 0;
                    txt.MoveFocus(new TraversalRequest(
                   FocusNavigationDirection.First));
                }
                else
                {
                    txt.MoveFocus(new TraversalRequest(
                       FocusNavigationDirection.Next));
                }
            }
        }
        #endregion

        private void btndelpolizavehi_Click(object sender, RoutedEventArgs e)
        {
            var currentCell = gridlistadovehiculos.SelectedItem;
            var itemcell = (VehiT_ConsultaHistoricoPolizasHC_Result)currentCell;
            limpiarcampos();
            Updateestadopoliza(itemcell);
            llenargrilla();
        }
        private void mostrar_mensaje(int idmensaje)
        {
            Mensabll = new MensajeBLL();
            Mensabll.tipomensajes(idmensaje,"");
            Venmsj = new VentanaMensaje();
            Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
            Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
            Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
            Venmsj.ShowDialog();
        }
        private void Updateestadopoliza(VehiT_ConsultaHistoricoPolizasHC_Result itemcell)
        {
            var respuesta = polizabll.UpdateEstadoPolizaHC(itemcell.Poliza, itemcell.Placa, main.txtusuario.Text.Trim(), 1);
            if (respuesta == true)
            {
                mostrar_mensaje(12);
            }
            else
            {
                mostrar_mensaje(23);
            }
        }

        private void btndelpolizaremol_Click(object sender, RoutedEventArgs e)
        {
            var currentCell = gridlistadoremolque.SelectedItem;
            var itemcell = (VehiT_ConsultaHistoricoPolizasHC_Result)currentCell;
            limpiarcampos();
            Updateestadopoliza(itemcell);
            llenarremolque();
        }

        


    }
}
