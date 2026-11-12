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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Vehitanques.BLL;
using WPF_Vehitanques.Modelo;
using WPF_Vehitanques.Presentacion.Ventanas;

namespace WPF_Vehitanques.Presentacion.Controles
{
    /// <summary>
    /// Lógica de interacción para AdmTipoVehiculo.xaml
    /// </summary>
    public partial class AdmTipoVehiculo : UserControl
    {
       

        #region Declaraciones

        private int iStar = 1;
        private int iEnd = 10;
        private int iAdd = 10;
        private Nullable<Boolean> boolinfobancaria, boolcambioestado,boolinfovehiculo;
        VehiculoBLL Vehibll = new VehiculoBLL();
        ConductorBLL conducbll = new ConductorBLL();
        VetoBLL vetobll = new VetoBLL();
        string placa;
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj = new VentanaMensaje();
        InicioVehiculos IniVehi = new InicioVehiculos();
        CambiarEstadoVehiculo ventanaestado = new CambiarEstadoVehiculo();
        admInfoBancariaConductor infobancaria = new admInfoBancariaConductor();
        List<seguridad_consultarMenuPermisos2_Result> resultPerfilUsuario;
        VehiT_ConsultarVehiculoXplaca_Result rowplaca = new VehiT_ConsultarVehiculoXplaca_Result ();

        MainWindow main;
        #endregion
        # region load
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarPrivilegios();
             if (IsLoaded)
            {
                placa = "";
                CargarTipoVehiculos();
           // llenargrilla();
            }
        }

        public AdmTipoVehiculo()
        {
            InitializeComponent();
        }
        #endregion
        # region Métodos
        private void CargarTipoVehiculos()
        {
            var lista = Vehibll.GetTiposVehiculo();
            cbbtipovehiculo.ItemsSource = lista;
            cbbtipovehiculo.SelectedValuePath = "Codigo";
            cbbtipovehiculo.DisplayMemberPath = "Descripcion";
            cbbtipovehiculo.SelectedIndex = 0;
        }
        public  void ValidarPrivilegios()
        {
            //var lista = SecurityVariables.PPerfilUsuario;
            //var listaprivi = (from rmenu in lista
            //                  where rmenu.idMenuPadre == 230 & rmenu.alterno == 1 & rmenu.orden==3
            //                  select rmenu).ToList();
            //foreach (var item in listaprivi)
            //{
            //    switch (item.nombreMenu)
            //    {
            //        case "Cambiar Estado":
            //            //if ( item.Actualizar == 1)
            //            //{
            //            //    boolcambioestado = true;
            //            //}
            //            //else
            //            //{
            //            //    boolcambioestado = false;
            //            //}
            //            boolcambioestado = false;
            //            break;
            //        case "Información Bancaria":
            //            if ( item.Autorizar == 1)
            //            {
            //                boolinfobancaria = true;
            //            }
            //            else
            //            {
            //                boolinfobancaria = false;
            //            }
            //            break;
            //        case "Ver Info Vehiculo":
            //            if (item.Autorizar == 1)
            //            {
            //                boolinfovehiculo = true;
            //            }
            //            else
            //            {
            //                boolinfovehiculo = false;
            //            }
            //            break;
                        
            //    }
            //}
        }
        public void RecibirPadre(MainWindow padre)
        {
            main = padre;
        }

        public void RecibePrivilegios(Nullable<Boolean> Cambioestado, Nullable<Boolean> infobancaria)
        {
            boolinfobancaria = infobancaria;
            boolcambioestado = false;
        }
        private void llenargrilla()
        {
            try
            {
                pnl_Paginacion.Visibility = Visibility.Visible;
                gridlistadovehiculos.ItemsSource = null;
                gridlistadovehiculos.Visibility = Visibility.Visible;
                lbl_Inicio.Text = iStar.ToString();
                placa = txtbusqueda.Text.Trim();              

                lbl_Total.Text = Convert.ToString(Vehibll.contadorVehiculoSinDespacho(placa));
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
                var lista = Vehibll.GetVehiculosinDespacho(placa, iStar, iEnd);
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
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes + " o el vehículo ya tuvo algún despacho asociado.");
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

        private void CargarGrilla(List<Modelo.VehiT_ConsultarVehiculoXplaca_Result > lista)
        {
            var listaaux = lista;
            gridlistadovehiculos.ItemsSource = lista;

        }
        public void recibirordenestadoplaca(Boolean orden)
        {
            if (orden == true)
                llenargrilla();

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
        private void mostrar_mensaje_reportado(int idmensaje)
        {
            Mensabll = new MensajeBLL();
            Mensabll.tipomensajes(idmensaje,"");
            Venmsj = new VentanaMensaje();
            Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
            Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
            Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
            Venmsj.ShowDialog();
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
        private void mostrar_mensaje_reportado(int idmensaje, string mensaje)
        {
            Mensabll = new MensajeBLL();
            Mensabll.tipomensajes(idmensaje,"");
            Venmsj = new VentanaMensaje();
            Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
            Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
            Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes + mensaje);
            Venmsj.ShowDialog();
        }
        private Boolean ValidarDatosReportados(VehiT_ConsultarVehiculoXplaca_condu_propi_Result item)
        {
            Boolean Respuesta = false;
            var lista = vetobll.GetDatosDatosvehiculoReportado(item.IdPlaca);
            var listareportado = vetobll.GetDatosPersonaReportada(item.NitPropietario);
            var listacoundreportado = vetobll.GetDatosPersonaReportada(item.Conductor);

            if (listareportado.Count > 0)
            {
                foreach (var itemre in listareportado)
                {
                    if (itemre.Estado == 1)
                    {
                        Respuesta = true;


                        mostrar_mensaje_reportado(45, item.NitPropietario + " SE ENCUENTRA REPORTADO POR SEGURIDAD");

                    }

                    break;
                }
            }
            if (listacoundreportado.Count > 0)
            {
                foreach (var itemre in listacoundreportado)
                {
                    if (itemre.Estado == 1)
                    {
                        Respuesta = true;
                        mostrar_mensaje_reportado(44, item.Conductor + " SE ENCUENTRA REPORTADO POR SEGURIDAD");
                    }
                    break;
                }
            }
            if (lista.Count > 0)
            {
                foreach (var itemre in lista)
                {
                    if (itemre.Estado == 1)
                    {
                        Respuesta = true;
                        mostrar_mensaje_reportado(43, item.IdPlaca + " SE ENCUENTRA REPORTADO POR SEGURIDAD");
                    }

                    break;
                }
            }


            return Respuesta;

        }
        private void limpiarcampos()
        {
            cbbtipovehiculo.SelectedIndex = 0;
            txtplaca.Text = "";
            rowplaca = null;
        }
        #endregion


        #region Eventos
              
        private void gridlistadovehiculos_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            int cantidad = 0;
            if (boolinfovehiculo == false)
            {
                cantidad = cantidad + 1;
            }
            if (boolinfobancaria == false)
            {
                cantidad = cantidad + 1;
            }
            if (boolcambioestado == false)
            {
                cantidad = cantidad + 1;
            }
            switch (gridlistadovehiculos.CurrentCell.Column.DisplayIndex)
            {
                case 0:

                    e.ClipboardRowContent.Clear();

                    break;
                case 1:
                    e.ClipboardRowContent.Clear();
                    break;
                case 2:
                    e.ClipboardRowContent.Clear();
                    break;
                default:
                    var currentCell = e.ClipboardRowContent[gridlistadovehiculos.CurrentCell.Column.DisplayIndex - cantidad];
                    e.ClipboardRowContent.Clear();
                    e.ClipboardRowContent.Add(currentCell);
                    break;
            }  
        }
        private void btn_buscar_Click(object sender, RoutedEventArgs e)
        {
            llenargrilla();

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
                placa = txtbusqueda.Text.Trim();

                lbl_Total.Text = Convert.ToString(Vehibll.contadorVehiculoSinDespacho(placa));

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
                var lista = Vehibll.GetVehiculosinDespacho (placa,  Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
                if (lista.Count > 0)
                {
                    // gridlistadovehiculos.ItemsSource = lista;
                    CargarGrilla(lista);
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
                placa = txtbusqueda.Text.Trim();

                lbl_Total.Text = Convert.ToString(Vehibll.contadorVehiculoSinDespacho(placa));
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
                var lista = Vehibll.GetVehiculosinDespacho(placa,Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
                if (lista.Count > 0)
                {
                    // gridlistadovehiculos.ItemsSource = lista;
                    CargarGrilla(lista);
                }


                gridlistadovehiculos.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }      
        
        private void btnverDatos_Click(object sender, RoutedEventArgs e)
        {
            limpiarcampos();
            
            string placabtn = ((Button)sender).CommandParameter.ToString();
            var currentCell = gridlistadovehiculos.SelectedItem;
            var item = (VehiT_ConsultarVehiculoXplaca_Result)currentCell;
            if (item != null)
            {
                rowplaca = item;
                cbbtipovehiculo.SelectedValue = item.TipoVehiculo;
                txtplaca.Text = item.IdPlaca.Trim();
                pnlinfoplaca.Visibility = Visibility.Visible;
            }
            else
            {
                rowplaca = null;
                pnlinfoplaca.Visibility = Visibility.Collapsed;
            }

            //if (ValidarDatosReportados(item) == false)
            //{
            //    IniVehi = new InicioVehiculos();
            //    IniVehi.GetPlaca(placabtn);
            //    IniVehi.RecibirPadre(main);
            //    IniVehi.RecibeRedirect(this);
            //    // IniVehi.GetListausuario(SecurityVariables.PPerfilUsuario);
            //    main.pnl_inicio.Children.Clear();
            //    main.pnl_inicio.Children.Add(IniVehi);
                
            //}
            //else
            //{
            //    mostrar_mensaje_reportado(43);
            //}           
        }
      
        private void txtbusqueda_PreviewKeyDown(object sender, KeyEventArgs e)
       {
            TextBox txt = (TextBox)sender;

            if (validarCaracteresespeciales(e, txt) == true)
            {
                if (e.Key == Key.Enter || e.Key == Key.Tab)
                {
                    llenargrilla();
                    //  txt.MoveFocus(new TraversalRequest(
                    //   FocusNavigationDirection.Next));
                }
            }
            else
            {
                txt.Text = "";
            }

        }
        private void btn_aceptar_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result1 = MessageBox.Show("Está seguro que quiere cambiar el tipo de vehículo a esta placa?", "Alerta", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result1 == MessageBoxResult.Yes)
            {
                if (rowplaca != null)
                {
                    Boolean respuesta = Vehibll.UpdateTipoVehiculo(rowplaca.IdPlaca, rowplaca.TipoVehiculo, Convert.ToInt32(cbbtipovehiculo.SelectedValue), main.txtusuario.Text.Trim());
                    if (respuesta == true)
                    {
                        mostrar_mensaje(12);
                        limpiarcampos();
                        pnlinfoplaca.Visibility = Visibility.Collapsed;
                        llenargrilla();
                    }
                    else
                    {
                        mostrar_mensaje(23);
                        limpiarcampos();
                    }
                }
                else
                {
                    limpiarcampos();
                    MessageBox.Show("Se presento una anomalia en la aplicación, por favor repita el proceso. ");
                }

            }

        }
        #endregion

     

        #region Eventos Seguridad
        
        #endregion
    }
}
