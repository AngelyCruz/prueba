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
using WPF_Vehitanques.DS;
using WPF_Vehitanques.Presentacion.Ventanas;
using WPF_Vehitanques.Utilidades;

namespace WPF_Vehitanques.Presentacion.Controles
{
    /// <summary>
    /// Lógica de interacción para MsgConsdutoresPropios.xaml
    /// </summary>
    public partial class MsgConsdutoresPropios : UserControl
    {
        
        #region Declaraciones
        private int iStar = 1;
        private int iEnd = 400;
        private int iAdd = 400;
        ConductorBLL Conducll = new ConductorBLL();
        AgenciaBLL agenbll = new AgenciaBLL();
      //  EnturnamientoBLL EnturBll = new EnturnamientoBLL();
        DSMensajesTexto.DSPlacasMSGDataTable DSplacas = new DSMensajesTexto.DSPlacasMSGDataTable();
       // DSEnturnamiento.DSNumerosXPlacaDataTable DSNumerosXPlaca = new DSEnturnamiento.DSNumerosXPlacaDataTable();
        VentanaMensaje Venmsj;
        MensajeBLL Mensabll = new MensajeBLL();
        MainWindow main;
        MSGBLL msgbll = new MSGBLL();
        private Boolean enturnar;
        private Boolean Agencias;
        private Boolean Corredores;
        private int CuotaMensajes = 0;
        UserMsg UsrMsg = new UserMsg();
        
        #endregion
        #region load
        public MsgConsdutoresPropios()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarPrivilegios();
            if (IsLoaded)
            {
                
                //CargarTiposVehiculo();
                CargarDatos();
                CargarCuotaDeMensajes();
            }
        }

        #endregion
        #region metodos
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
        private void ValidarPrivilegios()
        {
            var lista = SecurityVariables.PPerfilUsuario;
            var listaprivi = (from rmenu in lista
                              where rmenu.idMenuPadre == 255 & rmenu.alterno == 1 & rmenu.orden == 3
                              select rmenu).ToList();
            foreach (var item in listaprivi)
            {
                switch (item.nombreMenu)
                {
                    case "Enviar Masivos Conductores Inactivos":
                        if (item.Autorizar == 1)
                        {
                            rdbinactivo.IsEnabled = true;                            
                        }
                        else
                        {
                            rdbinactivo.IsEnabled = false ;                            
                        }
                            
                        break;                   

                }
            }
        }
        public void RecibirPadre(MainWindow padre)
        {
            if (main == null)
                main = padre;
        }
        //private void CargarTiposVehiculo()
        //{
        //    var lista = Conducll.GetTiposVehiculo();
        //    cbbbtipovehiculoDisp.ItemsSource = null;
        //    cbbbtipovehiculoDisp.ItemsSource = lista;
        //    cbbbtipovehiculoDisp.SelectedValuePath = "Codigo";
        //    cbbbtipovehiculoDisp.DisplayMemberPath = "Descripcion";
        //    cbbbtipovehiculoDisp.SelectedIndex = 0;
        //}
        private void CargarDatos(IList<Modelo.VehiT_ConsultaConductoresPropiosMasivoMSG_Result> lista)
        {
            lbltotal.Content = lbl_TotalDisp.Text ;
            DSplacas.Clear();
            if (lista.Count > 0)
            {             
                foreach (var item in lista)
                {
                    if (item.CelPre != "")
                    {
                        DSplacas.AddDSPlacasMSGRow(item.IdConductor, item.CelPre,item.IdConductor.ToString().Trim() );
                    }
                }
            }
        }
        private void CargarDatos()
        {
            try
            {
                Boolean  estado = false ;
                if (rdbinactivo.IsChecked == true)
                {
                    estado = true ;
                }
                else
                {
                    estado = false;
                }
                //if (cbbbtipovehiculoDisp.SelectedValue != null)
                //{
                //    int tipovehiculocbb = Convert.ToInt32(cbbbtipovehiculoDisp.SelectedValue);
                //    switch (tipovehiculocbb)
                //    {
                //        case 2:
                //            if (rdbinactivo.IsChecked == true)
                //            {
                //                estado = 2;
                //            }
                //            break;
                //        case 17:
                //            if (rdbinactivo.IsChecked == true)
                //            {
                //                estado = 2;
                //            }
                //            break;
                //        default:
                //            estado = 1;
                //            break;
                //    }
                //}
                pnl_PaginacionDisp.Visibility = Visibility.Visible;
                lbl_InicioDisp.Text = iStar.ToString();
              //  int tipovehiculo = Convert.ToInt32(cbbbtipovehiculoDisp.SelectedValue);

                lbl_TotalDisp.Text = Convert.ToString(Conducll.contadorconductorespropiosEnvioMAsivoMSG(estado));

                if (Convert.ToInt32(lbl_TotalDisp.Text) <= iEnd)
                {
                    lbl_FinDisp.Text = lbl_TotalDisp.Text;
                    btn_sigDisp.IsEnabled = false;
                    pnl_PaginacionDisp.Visibility = Visibility.Collapsed;
                }
                else
                {
                    pnl_PaginacionDisp.Visibility = Visibility.Visible;
                    lbl_FinDisp.Text = iEnd.ToString();
                    //'linea nueva de código
                    btn_sigDisp.IsEnabled = true;
                }
                var lista = Conducll.ConsultaConductorPropioEnvioMAsivoMSG (iStar, iEnd, estado );
                if (lista.Count > 0)
                {
                    // gridlistadovehiculosDisp.ItemsSource = lista;
                    CargarDatos(lista);
                }
                else
                {
                    lbltotal.Content = lista.Count.ToString();
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
        #endregion
        #region eventos

        private void btn_antDisp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Boolean estado = false;
                if (rdbinactivo.IsChecked == true)
                {
                    estado = true;
                }
                else
                {
                    estado = false;
                }
                int iRes = 0;
                if (Convert.ToInt32(lbl_FinDisp.Text) == Convert.ToInt32(lbl_TotalDisp.Text))
                {
                    //<agenbll>ApplyAnimationClock>
                    iRes = Convert.ToInt32(lbl_FinDisp.Text) - Convert.ToInt32(lbl_InicioDisp.Text);
                }
                else
                {
                    iRes = iAdd;
                }
                lbl_InicioDisp.Text = Convert.ToString(Convert.ToInt32(lbl_InicioDisp.Text) - iAdd);
                lbl_FinDisp.Text = Convert.ToString(Convert.ToInt32(lbl_FinDisp.Text) - iRes);


                lbl_TotalDisp.Text = Convert.ToString(Conducll.contadorconductorespropiosEnvioMAsivoMSG(estado));

                if (Convert.ToInt32(lbl_InicioDisp.Text) > 1)
                {
                    btn_sigDisp.IsEnabled = true;
                }
                else
                {
                    lbl_InicioDisp.Text = Convert.ToString(iStar);
                    lbl_FinDisp.Text = Convert.ToString(iEnd);
                    btn_antDisp.IsEnabled = false;
                    btn_sigDisp.IsEnabled = true;
                }
                var lista = Conducll.ConsultaConductorPropioEnvioMAsivoMSG(Convert.ToInt32(lbl_InicioDisp.Text), Convert.ToInt32(lbl_FinDisp.Text), estado );

                if (lista.Count > 0)
                {
                    CargarDatos(lista);
                }
                else
                {
                    lbltotal.Content = lista.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
        private void btn_sigDisp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lbl_InicioDisp.Text != "")
                {
                    Boolean estado = false;
                    if (rdbinactivo.IsChecked == true)
                    {
                        estado = true;
                    }
                    else
                    {
                        estado = false;
                    }
                    lbl_InicioDisp.Text = Convert.ToString(Convert.ToInt32(lbl_InicioDisp.Text) + iAdd);
                    lbl_FinDisp.Text = Convert.ToString(Convert.ToInt32(lbl_FinDisp.Text) + iAdd);

              
                    lbl_TotalDisp.Text = Convert.ToString(Conducll.contadorconductorespropiosEnvioMAsivoMSG(estado));

                    if (Convert.ToInt32(lbl_FinDisp.Text) < Convert.ToInt32(lbl_TotalDisp.Text))
                    {
                        btn_antDisp.IsEnabled = true;
                    }
                    else
                    {
                        lbl_FinDisp.Text = lbl_TotalDisp.Text;
                        btn_sigDisp.IsEnabled = false;
                        btn_antDisp.IsEnabled = true;
                    }
                    var lista = Conducll.ConsultaConductorPropioEnvioMAsivoMSG(Convert.ToInt32(lbl_InicioDisp.Text), Convert.ToInt32(lbl_FinDisp.Text), estado );

                    if (lista.Count > 0)
                    {
                        CargarDatos(lista);
                    }
                    else
                    {
                        lbltotal.Content = lista.Count.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
        private void btn_CArgarDAtos_Click(object sender, RoutedEventArgs e)
        {
            
                CargarDatos();
            
        }
        private void btn_VentanaMsg_Click(object sender, RoutedEventArgs e)
        {
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
                    Venmsgtexto.GetDatasetDatos(DSplacas, usuario,2);
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
        #endregion     
        //private void cbbbtipovehiculoDisp_Selected(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cbbbtipovehiculoDisp.SelectedValue != null)
        //    {
        //        int tipovehiculo = Convert.ToInt32(cbbbtipovehiculoDisp.SelectedValue);
        //        switch (tipovehiculo)
        //        {
        //            case 2:
        //                ValidarPrivilegios();
        //                break;
        //            case 17:
        //                ValidarPrivilegios();
        //                break;
        //            default:
        //                ValidarPrivilegios();
        //                rdbactivo.IsChecked = true;
        //                break;
        //        }
        //    }
        //}
    }
}
