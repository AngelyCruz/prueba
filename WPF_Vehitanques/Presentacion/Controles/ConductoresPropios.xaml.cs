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
using WPF_Vehitanques.Utilidades;

namespace WPF_Vehitanques.Presentacion.Controles
{
    /// <summary>
    /// Lógica de interacción para ConductoresPropios.xaml
    /// </summary>
    public partial class ConductoresPropios : UserControl
    {
        #region Declaraciones
        private int iStar = 1;
        private int iEnd = 15;
        private int iAdd = 15;
        Nullable<short> Estado = null;
        Numeric numeric = new Numeric();
        ConductorBLL conducbll = new ConductorBLL();       
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj = new VentanaMensaje();        
        List<seguridad_consultarMenuPermisos2_Result> resultPerfilUsuario;
        MainWindow main;
        #endregion
        #region load
        public ConductoresPropios()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarPrivilegios();
            if (IsLoaded)
            {
                
                // llenargrilla();
            }
        }

        #endregion
        #region Métodos
        internal void ValidarPrivilegios()
        {
            var lista = SecurityVariables.PPerfilUsuario;
            var listaprivi = (from rmenu in lista
                              where rmenu.idMenuPadre == 230 & rmenu.alterno == 1 & rmenu.orden == 3
                              select rmenu).ToList();    
            /*
            foreach (var item in listaprivi)
            {
                switch (item.nombreMenu)
                {
                    
                    case "Cambiar Estado":
                        //if ( item.Actualizar == 1)
                        //{
                        //    boolcambioestado = true;
                        //}
                        //else
                        //{
                        //    boolcambioestado = false;
                        //}
                        boolcambioestado = false;
                        break;
                       

                }
            } 
             */
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
                gridlistadovehiculos.ItemsSource = null;
                gridlistadovehiculos.Visibility = Visibility.Visible;
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

                

                lbl_Total.Text = Convert.ToString(conducbll.contadorconductorespropios(txtbusqueda.Text.Trim(),txtnombre.Text.Trim()));
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

        private void CargarGrilla(List<Modelo.VehiT_ConsultaConductor_Propio_Result> lista)
        {
            var listaaux = lista;
            gridlistadovehiculos.ItemsSource = lista;
            //if (boolinfobancaria == true)
            //{
            //    gridlistadovehiculos.Columns[0].Visibility = System.Windows.Visibility.Visible;

            //}
            //else
            //{
            //    gridlistadovehiculos.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
            //}
            //if (boolcambioestado == true)
            //{
            //    gridlistadovehiculos.Columns[1].Visibility = System.Windows.Visibility.Visible;

            //}
            //else
            //{
            //    gridlistadovehiculos.Columns[1].Visibility = System.Windows.Visibility.Collapsed;
            //}
            //if (boolinfovehiculo == true)
            //{
            //    gridlistadovehiculos.Columns[2].Visibility = System.Windows.Visibility.Visible;

            //}
            //else
            //{
            //    gridlistadovehiculos.Columns[2].Visibility = System.Windows.Visibility.Collapsed;
            //}


        }
        #endregion
        #region Eventos
        private void gridlistadovehiculos_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            int cantidad = 0;
            //if (boolinfovehiculo == false)
            //{
            //    cantidad = cantidad + 1;
            //}
            //if (boolinfobancaria == false)
            //{
            //    cantidad = cantidad + 1;
            //}
            //if (boolcambioestado == false)
            //{
            //    cantidad = cantidad + 1;
            //}
            //switch (gridlistadovehiculos.CurrentCell.Column.DisplayIndex)
            //{
            //    case 0:

            //        e.ClipboardRowContent.Clear();

            //        break;
            //    case 1:
            //        e.ClipboardRowContent.Clear();
            //        break;
            //    case 2:
            //        e.ClipboardRowContent.Clear();
            //        break;
            //    default:
            //        var currentCell = e.ClipboardRowContent[gridlistadovehiculos.CurrentCell.Column.DisplayIndex - cantidad];
            //        e.ClipboardRowContent.Clear();
            //        e.ClipboardRowContent.Add(currentCell);
            //        break;
            //}
            var currentCell = e.ClipboardRowContent[gridlistadovehiculos.CurrentCell.Column.DisplayIndex - cantidad];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
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
                gridlistadovehiculos.ItemsSource = null;
                var lista = conducbll.ConsultaConductorPropio(txtbusqueda.Text.Trim(), txtnombre.Text.Trim(), Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
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
                gridlistadovehiculos.ItemsSource = null;
                var lista = conducbll.ConsultaConductorPropio(txtbusqueda.Text.Trim(), txtnombre.Text.Trim(), Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
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

        private void btn_buscar_Click(object sender, RoutedEventArgs e)
        {
            llenargrilla();

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
                    FocusNavigationDirection.Next ));
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


    }
}
