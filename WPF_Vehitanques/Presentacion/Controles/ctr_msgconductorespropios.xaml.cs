using System;
using System.Collections.Generic;
using System.Data;
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
using WPF_Vehitanques.DS;
using WPF_Vehitanques.Modelo;
using WPF_Vehitanques.Presentacion.Ventanas;
using WPF_Vehitanques.Utilidades;

namespace WPF_Vehitanques.Presentacion.Controles
{
    /// <summary>
    /// Lógica de interacción para ctr_msgconductorespropios.xaml
    /// </summary>
    public partial class ctr_msgconductorespropios : UserControl
    {
        #region Declaraciones
        private int iStar = 1;
        private int iEnd = 10;
        private int iAdd = 10;
        private int CuotaMensajes = 0;
        MainWindow main;       
        Numeric numeric = new Numeric();
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj = new VentanaMensaje();
        private DatePicker FechaActual = new DatePicker();
        ConductorBLL conducbll = new ConductorBLL();
        MSGBLL msgbll = new MSGBLL();

        DSConductor.DtConductorMsgDataTable dtconductorAsignado;
        DSMensajesTexto.DSPlacasMSGDataTable DSplacas = new DSMensajesTexto.DSPlacasMSGDataTable();
        #endregion
        #region Load

        public ctr_msgconductorespropios()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarPrivilegios();
            DelegarPrivilegios();
            if (IsLoaded)
            {
                CargarCuotaDeMensajes();
            }
        } 
        #endregion

        #region Métodos
        private void CargarCuotaDeMensajes()
        {
            var DScuota = msgbll.GetCuotaMensajes(2);
            foreach (var row in DScuota)
            {
                CuotaMensajes = row.SaldoMensajes;

                break;
            }
            MessageBox.Show("Su cuota de mensajes es:" + CuotaMensajes.ToString());
        }
        private void DelegarPrivilegios()
        {


        }
        private void ValidarPrivilegios()
        {
            //var lista = SecurityVariables.PPerfilUsuario;
            //var listaprivi = (from rmenu in lista
            //                  where rmenu.idMenuPadre == 230 & rmenu.alterno == 1 & rmenu.orden == 3
            //                  select rmenu).ToList();
            //foreach (var item in listaprivi)
            //{
            //    switch (item.nombreMenu)
            //    {
            //        case "Usuario Android":
            //            if (item.Autorizar == 1)
            //            {
            //                booluserandroid = true;
            //            }
            //            else
            //            {
            //                booluserandroid = false;
            //            }

            //            break;

            //    }
            //}
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
        public void RecibirPadre(MainWindow padre)
        {
            main = padre;
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
        private void llenargrilla()
        {
            try
            {
                pnl_Paginacion.Visibility = Visibility.Visible;
                gridlistadoconductor.ItemsSource = null;
                gridlistadoconductor.Visibility = Visibility.Visible;
                lbl_Inicio.Text = iStar.ToString();
                //if (rdiactivo.IsChecked == true)
                //{
                //    Estado = 0;
                //}
                //else
                //{
                //    if (rdiInactivo.IsChecked == true)
                //    {
                //        Estado = 1;
                //    }
                //    else
                //    {
                //        Estado = null;
                //    }
                //}



                lbl_Total.Text = Convert.ToString(conducbll.contadorconductorespropios(txtbusqueda.Text.Trim(), txtnombre.Text.Trim()));
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
                var lista = conducbll.ConsultaConductorPropio(txtbusqueda.Text.Trim(), txtnombre.Text.Trim(), iStar, iEnd);
                if (lista.Count > 0)
                {
                    // gridlistadoconductor.ItemsSource = lista;
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

        private void CargarGrilla(List<Modelo.VehiT_ConsultaConductor_Propio_Result> lista)
        {
            var listaaux = lista;
            gridlistadoconductor.ItemsSource = lista;
            //if (boolinfobancaria == true)
            //{
            //    gridlistadoconductor.Columns[0].Visibility = System.Windows.Visibility.Visible;

            //}
            //else
            //{
            //    gridlistadoconductor.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
            //}
            //if (boolcambioestado == true)
            //{
            //    gridlistadoconductor.Columns[1].Visibility = System.Windows.Visibility.Visible;

            //}
            //else
            //{
            //    gridlistadoconductor.Columns[1].Visibility = System.Windows.Visibility.Collapsed;
            //}
            //if (boolinfovehiculo == true)
            //{
            //    gridlistadoconductor.Columns[2].Visibility = System.Windows.Visibility.Visible;

            //}
            //else
            //{
            //    gridlistadoconductor.Columns[2].Visibility = System.Windows.Visibility.Collapsed;
            //}


        }

        private void llenarGrillaConductoresAsignados(DSConductor.DtConductorMsgDataTable _dtconductorAsignado)
        {
            gridlistadoconductorasignado.ItemsSource = null;
            gridlistadoconductorasignado.ItemsSource = _dtconductorAsignado;
        }

        private DSConductor.DtConductorMsgRow AgregarRowdatatable(VehiT_ConsultaConductor_Propio_Result itemcell, DSConductor.DtConductorMsgDataTable dtconductorAsignado)
        {
            DSConductor.DtConductorMsgRow dtrow = dtconductorAsignado.NewDtConductorMsgRow();
            dtrow.IdConductor = itemcell.IdConductor.Trim();
            dtrow.CelConductor = itemcell.CelPre.Trim();
            dtrow.NombreConductor = itemcell.Nombre.Trim();

            return dtrow;
        }

        private Boolean Validarconductor(VehiT_ConsultaConductor_Propio_Result itemcell)
        {
            if (itemcell.CelPre != null)
            {
                if (itemcell.CelPre.Trim().Length == 10)
                {
                    return false;
                }
                else
                {
                    MessageBox.Show("El número de celular del conductor no cumple con la características de 10  dígitos.");
                    return true;
                }
            }
            else
            {
                MessageBox.Show("No se peude asignar un conductor si número de celular.");
                return true;
            }
        }

        #endregion


        #region Eventos
        private void btnasignaconductor_Click(object sender, RoutedEventArgs e)
        {
            var currentCell = gridlistadoconductor.SelectedItem;
            var itemcell = (VehiT_ConsultaConductor_Propio_Result)currentCell;
            if (Validarconductor(itemcell) == false)
            {
                if (dtconductorAsignado == null)
                {
                    dtconductorAsignado = new DSConductor.DtConductorMsgDataTable();                    
                }
                if (dtconductorAsignado.Count > 0)
                {
                    foreach (var row in dtconductorAsignado)
                    {
                        if (itemcell.IdConductor == row.IdConductor)
                        {
                            MessageBox.Show("El conducto ya ha sido asignado para envío de mensajes");
                            return;
                        }
                    }
                }

                if (dtconductorAsignado.Count > 50)
                {
                    MessageBox.Show("No puede superar el límite de 50 conductores Asignados.");
                    return;
                }
                DSConductor.DtConductorMsgRow dtrow = dtconductorAsignado.NewDtConductorMsgRow();
                dtrow = AgregarRowdatatable(itemcell, dtconductorAsignado);
                dtconductorAsignado.AddDtConductorMsgRow(dtrow);
                llenarGrillaConductoresAsignados(dtconductorAsignado);

            }   
        }

        private void btndeleteconductor_Click(object sender, RoutedEventArgs e)
        {
            DataRowView currentCell = (DataRowView)gridlistadoconductorasignado .SelectedItem;
            var item = (DSConductor.DtConductorMsgRow)currentCell.Row;
            DSConductor.DtConductorMsgDataTable DtAux;
           // DSProducto.DtProductosAlertaReciboDataTable DtAux;
            DtAux = eliminaconductor(item);
            llenarGrillaConductoresAsignados(DtAux);
        }

        private DSConductor.DtConductorMsgDataTable eliminaconductor(DSConductor.DtConductorMsgRow item)
        {            
            DSConductor.DtConductorMsgDataTable DtAux;
            foreach (var row in dtconductorAsignado )
            {
                if (row.IdConductor == item.IdConductor)
                {
                    row.Delete();
                    break;
                }
            }
            dtconductorAsignado.AcceptChanges();
            DtAux = (DSConductor.DtConductorMsgDataTable)dtconductorAsignado.Copy();
            return DtAux;
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
                //if (rdiactivo.IsChecked == true)
                //{
                //    Estado = 0;
                //}
                //else
                //{
                //    if (rdiInactivo.IsChecked == true)
                //    {
                //        Estado = 1;
                //    }
                //    else
                //    {
                //        Estado = null;
                //    }
                //}
                lbl_Total.Text = Convert.ToString(conducbll.contadorconductorespropios(txtbusqueda.Text.Trim(), txtnombre.Text.Trim()));

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
                gridlistadoconductor.ItemsSource = null;
                var lista = conducbll.ConsultaConductorPropio(txtbusqueda.Text.Trim(), txtnombre.Text.Trim(), Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
                if (lista.Count > 0)
                {
                    // gridlistadoconductor.ItemsSource = lista;
                    CargarGrilla(lista);
                }
                gridlistadoconductor.Visibility = Visibility.Visible;

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
                //if (rdiactivo.IsChecked == true)
                //{
                //    Estado = 0;
                //}
                //else
                //{
                //    if (rdiInactivo.IsChecked == true)
                //    {
                //        Estado = 1;
                //    }
                //    else
                //    {
                //        Estado = null;
                //    }
                //}
                lbl_Total.Text = Convert.ToString(conducbll.contadorconductorespropios(txtbusqueda.Text.Trim(), txtnombre.Text.Trim()));
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
                gridlistadoconductor.ItemsSource = null;
                var lista = conducbll.ConsultaConductorPropio(txtbusqueda.Text.Trim(), txtnombre.Text.Trim(), Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
                if (lista.Count > 0)
                {
                    // gridlistadoconductor.ItemsSource = lista;
                    CargarGrilla(lista);
                }


                gridlistadoconductor.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
        private void btn_buscar_Click(object sender, RoutedEventArgs e)
        {
            if (validarcampos() == false)
            {
                llenargrilla();
            }
            

        }

        private Boolean  validarcampos()
        {
            if (txtbusqueda.Text.Trim() == "" && txtnombre.Text.Trim() == "")
            {
                MessageBox.Show("Debe digitar un número de documento o un nombre para efectuar la búsqueda");
                return true;
            }
            return false;
        }
        private void textboxt_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (validarCaracteresespeciales(e, txt) == true)
            {


                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                {
                    //e.Handled = false;
                    if (txt.Text.Length >= 1)
                    {
                        if (numeric.IsNumeric(txt.Text) == true)
                            e.Handled = false;
                        else
                        {
                            e.Handled = true;
                            txt.Text = "";
                        }
                    }


                }
                else
                    e.Handled = true;


            }
            else
            {
                txt.Text = "";
            }
        }
        private void txtbusqueda_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if (validarCaracteresespeciales(e, txt) == true)
            {
                if (e.Key == Key.Enter || e.Key == Key.Tab)
                {
                    txt.MoveFocus(new TraversalRequest(
                    FocusNavigationDirection.Next));
                    if (txt.Text.Trim() == "")
                    {
                        return;
                    }
                    llenargrilla();

                }
            }
            else
            {
                txt.Text = "";
            }

        }

        private void txtnombre_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if (validarCaracteresespeciales(e, txt) == true)
            {
                if (e.Key == Key.Enter || e.Key == Key.Tab)
                {
                    if (txt.Text.Trim() == "")
                    {
                        return;
                    }
                    llenargrilla();
                    //txt.MoveFocus(new TraversalRequest(
                    // FocusNavigationDirection.Next));
                }
            }
            else
            {
                txt.Text = "";
            }
        }
        #endregion

        private void gridlistadoconductorasignado_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            int cantidad = 0;
            switch (gridlistadoconductorasignado.CurrentCell.Column.DisplayIndex)
            {
                //case 0:

                //    e.ClipboardRowContent.Clear();

                //    break;
                //case 1:
                //    e.ClipboardRowContent.Clear();
                //    break;
                //case 2:
                //    e.ClipboardRowContent.Clear();
                //    break;
                default:
                    e.ClipboardRowContent.Clear();
                    break;
            }
            //var currentCell = e.ClipboardRowContent[gridlistadovehiculos.CurrentCell.Column.DisplayIndex - cantidad];
            //e.ClipboardRowContent.Clear();
            //e.ClipboardRowContent.Add(currentCell);
        }

        private void gridlistadoconductor_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            int cantidad = 0;
            switch (gridlistadoconductor.CurrentCell.Column.DisplayIndex)
            {
                case 0:

                    e.ClipboardRowContent.Clear();

                    break;
               
                default:
                    var currentCell = e.ClipboardRowContent[gridlistadoconductor.CurrentCell.Column.DisplayIndex - cantidad];
                    e.ClipboardRowContent.Clear();
                    e.ClipboardRowContent.Add(currentCell);
                    break;
            }
        }

        private void btn_VentanaMsg_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result1 = MessageBox.Show("Tan pronto se abra la ventana mensajes se limpiaran los conductores Seleccionados. Está Usted consciente de continuar  con ésta acción? ", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result1 == MessageBoxResult.Yes)
            {


                DSplacas.Clear();
                if (dtconductorAsignado == null)
                {
                    MessageBox.Show("Debe seleccionar al menos un Conductor para enviar el mensaje de texto.");
                    return;
                }
                if (dtconductorAsignado.Count > 0)
                {
                    foreach (var item in dtconductorAsignado)
                    {
                        if (item.CelConductor != "")
                        {
                            DSplacas.AddDSPlacasMSGRow(item.IdConductor, item.CelConductor, item.IdConductor.ToString().Trim());
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar al menos un Conductor para enviar el mensaje de texto.");
                    return;
                }

                if (DSplacas.Count > 0)
                {
                    string usuario = "";
                    //foreach (FrameworkElement element in main.tolbarinformacion.Items)
                    //{
                    //    if (element is TextBlock)
                    //    {
                    //        var textblock = (TextBlock)element;
                    //        switch (textblock.Name)
                    //        {
                    //            case "txtusuario":
                    //                usuario = textblock.Text;
                    //                break;
                    //        }
                    //        if (usuario != "")
                    //        {
                    //            break;
                    //        }
                    //    }
                    //}
                    /////
                    usuario = main.txtusuario.Text.Trim();

                    if (CuotaMensajes >= DSplacas.Count)
                    {
                        EnvioMsgDeTexto Venmsgtexto = new EnvioMsgDeTexto();
                        Venmsgtexto.GetDatasetDatos(DSplacas, usuario, 2);
                        limpiargrilla();
                        // Venmsgtexto.GetMain(this);
                        Venmsgtexto.Show();
                        
                    }
                    else
                    {
                        MessageBox.Show("Su cuota de mensajes no cubre la cantidad de mensajes a enviar");
                    }

                    // EnvioMsgDeTexto Venmsgtexto = new EnvioMsgDeTexto();
                    // Venmsgtexto.GetDatasetDatos(DSplacas, usuario);
                    //// Venmsgtexto.GetMain(this);
                    // Venmsgtexto.Show();
                    // //  limpiargrillachepoints();
                }
                else
                {
                    //btn_EnviarMsg.Visibility = Visibility.Collapsed;
                    //txtmensajetexto.Visibility = Visibility.Collapsed;
                    MessageBox.Show("Debe seleccionar al menos un Conductor para enviar el mensaje de texto.");
                }
            }
            
        }

        private void limpiargrilla()
        {
            gridlistadoconductorasignado.ItemsSource = null;
            dtconductorAsignado.Clear();
            dtconductorAsignado.Dispose();

        }

       
    }
}
