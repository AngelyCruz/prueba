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
    /// Lógica de interacción para ComprobantesPropietario.xaml
    /// </summary>
    public partial class ComprobantesPropietario : UserControl
    {
        #region Declaraciones
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj = new VentanaMensaje();
        private int iStar = 1;
        private int iEnd = 10;
        private int iAdd = 10;
        ComprobantesBLL ComproBll= new ComprobantesBLL ();
        string nombreemp, nit;
        List<seguridad_consultarMenuPermisos2_Result> resultPerfilUsuario;
        MainWindow main;
       // private Nullable<Boolean> booleditar, boolcrear, boolcambioestado;
        #endregion
        #region load
        public ComprobantesPropietario()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarPrivilegios();
            DelegarPrivilegios();
            if (IsLoaded)
            {
                //LimpiarCamposInfirmacion();
                //LimpiarCamposAprobacion();
                //llenargrilla();
            }
        }
        
        #endregion
        #region Métodos
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
        private void DelegarPrivilegios()
        {
            ////if (boolcrear == true)
            ////{
            ////    btn_Nuevo.Visibility = System.Windows.Visibility.Visible;
            ////}
            ////else
            ////{
            ////    btn_Nuevo.Visibility = System.Windows.Visibility.Collapsed;
            ////}
            //if (boolcambioestado == true)
            //{
            //    exp_aprobacion.IsEnabled = true;
            //}
            //else
            //{

            //    exp_aprobacion.IsEnabled = false;

            //}
            //if (booleditar == true)
            //{
            //    exp_informacion.IsEnabled = true;
            //}
            //else
            //{
            //    exp_informacion.IsEnabled = false;

            //}

        }
        internal void ValidarPrivilegios()
        {
            var lista = SecurityVariables.PPerfilUsuario;
            var listaprivi = (from rmenu in lista
                              where rmenu.idMenuPadre == 230 & rmenu.alterno == 1 & rmenu.orden == 3
                              select rmenu).ToList();
            //foreach (var item in listaprivi)
            //{
            //    switch (item.nombreMenu)
            //    {
            //        case "Estado Emp Satelital":
            //            if (item.Actualizar == 1)
            //            {
            //                boolcambioestado = true;
            //            }
            //            else
            //            {
            //                boolcambioestado = false;
            //            }

            //            break;
            //        case "Editar Emp Satelital":
            //            if (item.Actualizar == 1)
            //            {
            //                booleditar = true;
            //            }
            //            else
            //            {
            //                booleditar = false;
            //            }
            //            break;
            //        case "Crear Emp Satelital":
            //            if (item.Actualizar == 1)
            //            {
            //                boolcrear = true;
            //            }
            //            else
            //            {
            //                boolcrear = false;
            //            }
            //            break;
            //    }
            //}
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
        public void RecibirPadre(MainWindow padre)
        {
            main = padre;
        }
        private void llenargrilla()
        {
            try
            {
                pnl_Paginacion.Visibility = Visibility.Visible;
                gridlistadocomprobantes.ItemsSource = null;
                gridlistadocomprobantes.Visibility = Visibility.Visible;
                lbl_Inicio.Text = iStar.ToString();
                string idpropietario = txtbusqueda.Text;
                //if (cbbitemnombre.IsSelected == true)
                //{
                //    nombreemp = txtbusqueda.Text;
                //    nit = "";

                //}
                //else if (cbbitemnit.IsSelected == true)
                //{
                //    nombreemp = "";
                //    nit = txtbusqueda.Text;
                //}

                lbl_Total.Text = Convert.ToString(ComproBll.ContadorComprobantes(idpropietario));
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
                var lista = ComproBll.GetComprobantesXpropietario (idpropietario, iStar, iEnd);
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
        private void CargarGrilla(List<VehiT_ConsultaComprobantesPropietarioXCedula_Result > lista)
        {
            var listaaux = lista;
            gridlistadocomprobantes.ItemsSource = lista;
            //if (boolcambioestado == true)
            //{
            //    gridlistadocomprobantes.Columns[0].Visibility = System.Windows.Visibility.Visible;
            //    exp_aprobacion.IsEnabled = true;
            //}
            //else
            //{
            //    gridlistadocomprobantes.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
            //    exp_aprobacion.IsEnabled = false;
            //}
            //if (booleditar == true)
            //{
            //    gridlistadocomprobantes.Columns[1].Visibility = System.Windows.Visibility.Visible;
            //    exp_informacion.IsEnabled = true;

            //}
            //else
            //{
            //    gridlistadocomprobantes.Columns[1].Visibility = System.Windows.Visibility.Collapsed;
            //    exp_informacion.IsEnabled = false;
            //}
        }
        #endregion
        #region Eventos
        private void gridlistadovehiculos_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            var currentCell = e.ClipboardRowContent[gridlistadocomprobantes.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
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
                string idpropietario = txtbusqueda.Text;
                lbl_Total.Text = Convert.ToString(ComproBll.ContadorComprobantes(idpropietario));

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
                gridlistadocomprobantes.ItemsSource = null;
                var lista = ComproBll.GetComprobantesXpropietario(idpropietario, Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
                if (lista.Count > 0)
                {

                    CargarGrilla(lista);
                }
                else
                {

                }

                gridlistadocomprobantes.Visibility = Visibility.Visible;

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
                string idpropietario = txtbusqueda.Text;
                //if (cbbitemnombre.IsSelected == true)
                //{
                //    nombreemp = txtbusqueda.Text;
                //    nit = "";

                //}
                //else if (cbbitemnit.IsSelected == true)
                //{
                //    nombreemp = "";
                //    nit = txtbusqueda.Text;
                //}
                lbl_Total.Text = Convert.ToString(ComproBll.ContadorComprobantes(idpropietario));
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
                gridlistadocomprobantes.ItemsSource = null;
                var lista = ComproBll.GetComprobantesXpropietario(idpropietario, Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
                if (lista.Count > 0)
                {
                    // gridlistadovehiculos.ItemsSource = lista;
                    CargarGrilla(lista);
                }


                gridlistadocomprobantes.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        #endregion
      
       

        
    }
}
