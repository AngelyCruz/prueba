using Elibom;
using System;
using System.Collections;
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
    /// Lógica de interacción para ProgramadosMantenimiento.xaml
    /// </summary>
    public partial class ProgramadosMantenimiento : UserControl
    {
        #region Declaraciones
        private int iStar = 1;
        private int iEnd = 15;
        private int iAdd = 15;
        DSMensajesTexto.DSPlacasMSGDataTable DSplacas = new DSMensajesTexto.DSPlacasMSGDataTable();
        VentanaMensaje Venmsj;
        MensajeBLL Mensabll = new MensajeBLL();
        VehiculoBLL vehibll = new VehiculoBLL();
        MainWindow main;
        ElibomClient elibom;
        UserMsg UsrMsg = new UserMsg();
        MSGBLL msgbll = new MSGBLL();
        private int CuotaMensajes = 0;
        #endregion
        #region Load
        public ProgramadosMantenimiento()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                CargarCuotaDeMensajes();
            }
        }
        #endregion
        #region Métodos
        private void CargarCuotaDeMensajes()
        {
            var DScuota = msgbll.GetCuotaMensajes(3);
            foreach (var row in DScuota)
            {
                CuotaMensajes = row.SaldoMensajes;

                break;
            }
            MessageBox.Show("Su cuota de mensajes es:" + CuotaMensajes.ToString());
        }
        public void RecibirPadre(MainWindow padre)
        {
            if (main == null)
                main = padre;
        }
        private void CargarGrilla(List<Modelo.VehiT_ConsultaTotalVehiProgMantenimeinto_Result> lista)
        {
            gridlistadovehiculos.ItemsSource = null;
            gridlistadovehiculos.ItemsSource = lista;

        }
        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;

            if (null == itemsSource)
            {
                yield return null;
            }

            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;

                if (null != row)
                {
                    yield return row;
                }
            }
        }
        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null)
            {
                return null;
            }

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;

                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName);

                    if (foundChild != null) break;
                }
                else
                    if (!string.IsNullOrEmpty(childName))
                    {
                        var frameworkElement = child as FrameworkElement;

                        if (frameworkElement != null && frameworkElement.Name == childName)
                        {
                            foundChild = (T)child;
                            break;
                        }
                        else
                        {
                            foundChild = FindChild<T>(child, childName);

                            if (foundChild != null)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        foundChild = (T)child;
                        break;
                    }
            }

            return foundChild;
        }

        public void limpiargrillacheCkpoints()
        {
            if (DSplacas.Count > 0)
            {
                DSplacas.Clear();
                chkSelected.IsChecked = false;
                var row_list = GetDataGridRows(gridlistadovehiculos);
                try
                {
                    foreach (DataGridRow single_row in row_list)
                    {
                        if (single_row != null)
                        {
                            CheckBox Chkgrid = FindChild<CheckBox>(single_row, "chkMsgtexto");
                            Chkgrid.IsChecked = false;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                catch
                {

                }               
                //var row_list1 = GetDataGridRows(gridlistadovehiculos );
                //chkSelected.IsChecked = false;
                //foreach (DataGridRow single_row in row_list1)
                //{
                //    if (single_row != null)
                //    {
                //        CheckBox Chkgrid = FindChild<CheckBox>(single_row, "chkMsgtextoDisp");
                //        Chkgrid.IsChecked = false;
                //    }
                //    else
                //    {
                //        break;
                //    }


                //}

            }
        }
        private Boolean validarCantidadMensajes(int total_registros)
        {
            ElibomClient elibom = new ElibomClient(UsrMsg.GetUsuario(), UsrMsg.GetPassword());
            var lista = elibom.getAccount();
            var creditos = lista["credits"];
            var cuenta = lista["name"];
            var tipoCuenta = lista["type"];

            decimal credits = Convert.ToDecimal(creditos);

            if (credits <= total_registros)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
       
        #region Eventos
        private void btn_buscar_Click(object sender, RoutedEventArgs e)
        {            
            CargarInfoGrilla();
        }
        private void CargarInfoGrilla()
        {
            try
            {
                limpiargrillacheCkpoints();
                pnl_Paginacion.Visibility = Visibility.Visible;
                gridlistadovehiculos.ItemsSource = null; 
                gridlistadovehiculos.Visibility = Visibility.Visible;
                lbl_Inicio.Text = iStar.ToString();
                decimal concepto = Convert.ToDecimal(cbbconcepto.Text);
                lbl_Total.Text = Convert.ToString(vehibll.GetContadorProgManteMsg(concepto));
                //if (validarCantidadMensajes(Convert.ToInt32(lbl_Total.Text)) == true )
                //{
                //    MessageBox.Show ("Lo créditos de la cuenta no son suficientes para la cantidad de registros");
                //    return;
                //}
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
                var lista = vehibll.GetProgManteMsg(iStar, iEnd, concepto );
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
        private void btn_ant_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                limpiargrillacheCkpoints();
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
                decimal concepto = Convert.ToDecimal(cbbconcepto.Text);
                lbl_Total.Text = Convert.ToString(vehibll.GetContadorProgManteMsg(concepto));
                //if (validarCantidadMensajes(Convert.ToInt32(lbl_Total.Text)) == true)
                //{
                //    MessageBox.Show("Lo créditos de la cuenta no son suficientes para la cantidad de registros");
                //    return;
                //}
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
                var lista = vehibll.GetProgManteMsg(Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text), concepto);

                if (lista.Count > 0)
                {
                    CargarGrilla(lista);
                }
                gridlistadovehiculos.Visibility = Visibility.Visible;

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

        private void btn_sig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lbl_Inicio.Text != "")
                {
                    limpiargrillacheCkpoints();
                    lbl_Inicio.Text = Convert.ToString(Convert.ToInt32(lbl_Inicio.Text) + iAdd);
                    lbl_Fin.Text = Convert.ToString(Convert.ToInt32(lbl_Fin.Text) + iAdd);
                    decimal concepto = Convert.ToDecimal(cbbconcepto.Text);
                    lbl_Total.Text = Convert.ToString(vehibll.GetContadorProgManteMsg(concepto));                   
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
                    var lista = vehibll.GetProgManteMsg(Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text), concepto);
                    // var lista = Vehibll.GetVehiculo(placa, conductor, propietario, Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
                    if (lista.Count > 0)
                    {
                        // gridlistadovehiculos.ItemsSource = lista;
                        CargarGrilla(lista);
                    }
                    gridlistadovehiculos.Visibility = Visibility.Visible;
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
        private void btn_VentanaMsg_Click(object sender, RoutedEventArgs e)
        {
            if (validarCantidadMensajes(Convert.ToInt32(DSplacas.Count)) == true)
            {
                MessageBox.Show("Lo créditos de la cuenta no son suficientes para la cantidad de registros");
                return;
            }
            if (DSplacas.Count > 0)
            {
                string usuario = "";
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
                        }
                        if (usuario != "")
                        {
                            break;
                        }
                    }
                }
                ///
                if (CuotaMensajes >= DSplacas.Count)
                {
                    EnvioMsgDeTexto Venmsgtexto = new EnvioMsgDeTexto();
                    Venmsgtexto.GetDatasetDatos(DSplacas, usuario,3);
                    Venmsgtexto.GetMain(this);
                    Venmsgtexto.Show();
                }
                else
                {
                    MessageBox.Show("Su cuota de mensajes no cubre la cantidad de mensajes a enviar");
                }
               
            }
            else
            {
                MessageBox.Show("Debe seleccionar al menos un vehículo para enviar el mensaje de texto.");
            }
        }
        private void chkMsgtexto_Checked(object sender, RoutedEventArgs e)
        {
            string placabtn = ((CheckBox)sender).CommandParameter.ToString();
            var currentCell = gridlistadovehiculos.SelectedItem;
            var itemcell = (VehiT_ConsultaTotalVehiProgMantenimeinto_Result)currentCell;
            if (itemcell != null)
            {
                DSMensajesTexto.DSPlacasMSGRow AdaprowAux;
                DataRow[] dr = DSplacas.Select("placa='" + itemcell.Placa + "'");
                if (dr.Length > 0)
                {
                    AdaprowAux = ((DSMensajesTexto.DSPlacasMSGRow)dr[0]);
                    DSplacas.RemoveDSPlacasMSGRow(AdaprowAux);
                    DSplacas.AcceptChanges();
                }
                DSplacas.AddDSPlacasMSGRow(itemcell.Placa, itemcell.CelPre,itemcell.IdConductor );
            }
            else
            {
               // MessageBox.Show("Se presentó un error al momento de seleccionar esta placa");
            }
            //var lista = EnturBll.GetTelefonoConductorPorPlaca(placabtn);
            //if (lista.Count > 0)
            //{
            //    DSplacas.AddDSPlacasMSGRow(placabtn, lista[0].CelPre);
            //}
        }

        private void chkMsgtexto_Unchecked(object sender, RoutedEventArgs e)
        {
            string placabtn = ((CheckBox)sender).CommandParameter.ToString();

            var currentCell = gridlistadovehiculos.SelectedItem;
            var itemcell = (VehiT_ConsultaTotalVehiProgMantenimeinto_Result)currentCell;
            DSMensajesTexto.DSPlacasMSGRow AdaprowAux;
            DataRow[] dr = DSplacas.Select("placa='" + itemcell.Placa  + "'");
            if (dr.Length > 0)
            {
                AdaprowAux = ((DSMensajesTexto.DSPlacasMSGRow)dr[0]);
                DSplacas.RemoveDSPlacasMSGRow(AdaprowAux);
                DSplacas.AcceptChanges();
            }
        }

        private void chkSelected_Checked(object sender, RoutedEventArgs e)
        {
            DSplacas.Clear();
            for (int i = 0; i < gridlistadovehiculos.Items.Count; i++)
            {
                var item = gridlistadovehiculos.Columns[0].GetCellContent(gridlistadovehiculos.Items[i]);
                var lista = item.DataContext;
                string placa = ((VehiT_ConsultaTotalVehiProgMantenimeinto_Result)(lista)).Placa;
                string celular = ((VehiT_ConsultaTotalVehiProgMantenimeinto_Result)(lista)).CelPre;
                string idconductor = ((VehiT_ConsultaTotalVehiProgMantenimeinto_Result)(lista)).IdConductor;
                DataRow[] dr = DSplacas.Select("placa='" + placa + "'");
                if (dr.Length > 0)
                {
                    DSMensajesTexto.DSPlacasMSGRow AdaprowAux;
                    AdaprowAux = ((DSMensajesTexto.DSPlacasMSGRow)dr[0]);
                    DSplacas.RemoveDSPlacasMSGRow(AdaprowAux);
                    DSplacas.AcceptChanges();
                }
                DSplacas.AddDSPlacasMSGRow(placa, celular, idconductor);
            }
            var row_list = GetDataGridRows(gridlistadovehiculos);
            foreach (DataGridRow single_row in row_list)
            {
                CheckBox Chkgrid = FindChild<CheckBox>(single_row, "chkMsgtexto");
                Chkgrid.IsChecked = true;
            }
        }

        private void chkSelected_Unchecked(object sender, RoutedEventArgs e)
        {
            limpiargrillacheCkpoints();
        }
        #endregion

     
       
       
    }
}
