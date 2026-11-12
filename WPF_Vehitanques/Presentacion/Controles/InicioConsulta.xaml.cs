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
using WPF_Vehitanques.Presentacion.Ventanas;
using WPF_Vehitanques.Modelo;

namespace WPF_Vehitanques.Presentacion.Controles
{
    public partial class InicioConsulta : UserControl
    {
        # region Declaraciones

        private int iStar = 1;
        private int iEnd = 10;
        private int iAdd = 10;
        private Nullable<Boolean> boolinfobancaria, boolcambioestado,boolinfovehiculo,consultaoperacionpaises;
        VehiculoBLL Vehibll = new VehiculoBLL();
        ConductorBLL conducbll = new ConductorBLL();
        VetoBLL vetobll = new VetoBLL();
        string placa, conductor, propietario,remolque;
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj = new VentanaMensaje();
        InicioVehiculos IniVehi = new InicioVehiculos();
        CambiarEstadoVehiculo ventanaestado = new CambiarEstadoVehiculo();
        admInfoBancariaConductor infobancaria = new admInfoBancariaConductor();
        List<seguridad_consultarMenuPermisos2_Result> resultPerfilUsuario;
        MainWindow main;
        #endregion
        # region load
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarPrivilegios();
             if (IsLoaded)
            {
                placa = "";
                conductor = "";
                propietario = "";
           // llenargrilla();
            }
        }
        public InicioConsulta()
        {
            InitializeComponent();
           // GetListausuario(SecurityVariables.PPerfilUsuario);
        }

        #endregion
        # region Métodos
        internal void ValidarPrivilegios()
        {
            var lista = SecurityVariables.PPerfilUsuario;
            var listaprivi = (from rmenu in lista
                              where rmenu.idMenuPadre == 230 & rmenu.alterno == 1 & rmenu.orden==3
                              select rmenu).ToList();
            foreach (var item in listaprivi)
            {
                switch (item.nombreMenu)
                {
                    case "Consulta Operación países":
                        if (item.Autorizar == 1)
                        {
                            consultaoperacionpaises = true;
                        }
                        else
                        {
                            consultaoperacionpaises = false;
                        }
                        break;
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
                    case "Información Bancaria":
                        if ( item.Autorizar == 1)
                        {
                            boolinfobancaria = true;
                        }
                        else
                        {
                            boolinfobancaria = false;
                        }
                        break;
                    case "Ver Info Vehiculo":
                        if (item.Autorizar == 1)
                        {
                            boolinfovehiculo = true;
                        }
                        else
                        {
                            boolinfovehiculo = false;
                        }
                        break;
                   
                        
                        
                }
            }
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
                if (cbbitemplaca.IsSelected == true)
                {
                    placa = txtbusqueda.Text;
                    conductor = "";
                    propietario = "";
                    remolque = "";
                }
                else if (cbbitempropi.IsSelected == true)
                {
                    placa = "";
                    conductor = "";
                    propietario = txtbusqueda.Text;
                    remolque = "";
                }
                else if (cbbitemcond.IsSelected == true)
                {
                    placa = "";
                    propietario = "";
                    conductor = txtbusqueda.Text;
                    remolque = "";

                }
                else if (cbbitemremolque.IsSelected == true)
                {
                    placa = "";
                    propietario = "";
                    conductor = "";
                    remolque = txtbusqueda.Text;
                }


                lbl_Total.Text = Convert.ToString(Vehibll.contadorVehiculo(placa, conductor, propietario, remolque));
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
                var lista = Vehibll.GetVehiculo(placa, conductor, propietario, remolque, iStar, iEnd);
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

        private void CargarGrilla(List<Modelo.VehiT_ConsultarVehiculoXplaca_condu_propi_Result> lista)
        {
            var listaaux = lista;
            gridlistadovehiculos.ItemsSource = lista;
            if (boolinfobancaria == true)
            {
                gridlistadovehiculos.Columns[0].Visibility = System.Windows.Visibility.Visible;
                
            }
            else
            {
                gridlistadovehiculos.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
            }
            if (boolcambioestado == true)
            {
                gridlistadovehiculos.Columns[1].Visibility = System.Windows.Visibility.Visible;

            }
            else
            {
                gridlistadovehiculos.Columns[1].Visibility = System.Windows.Visibility.Collapsed;
            }
            if (boolinfovehiculo  == true)
            {
                gridlistadovehiculos.Columns[2].Visibility = System.Windows.Visibility.Visible;

            }
            else
            {
                gridlistadovehiculos.Columns[2].Visibility = System.Windows.Visibility.Collapsed;
            }
          

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
        #endregion


        # region Eventos

        private void btnActivar_Click(object sender, RoutedEventArgs e)
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
            //            break;
            //    }
            //}
            //string placabtn = ((Button)sender).CommandParameter.ToString();
            //ventanaestado.Close();
            //ventanaestado = new CambiarEstadoVehiculo();
            //ventanaestado.GetPadre(this);
            //ventanaestado.GetPlaca(placabtn, usuario);
            //ventanaestado.Show();
        }

        private void btninforbancaria_Click(object sender, RoutedEventArgs e)
        {
            var currentCell = gridlistadovehiculos.SelectedItem;
            var itemcell = (VehiT_ConsultarVehiculoXplaca_condu_propi_Result)currentCell;
            if (ValidarDatosReportados(itemcell) == false)
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
                            break;
                    }
                }

                string idplaca = ((Button)sender).CommandParameter.ToString();
                string idconductor = "";
                var lista = conducbll.GetconductorXPlaca(idplaca);
                foreach (var item in lista)
                {
                    idconductor = item.IdConductor;
                }
                infobancaria.Close();
                infobancaria = new admInfoBancariaConductor();
                infobancaria.GetPadre(this,main );
               // infobancaria.GetConductor(idconductor, usuario, idplaca,itemcell.NitPropietario );
                infobancaria.cargarDatos(itemcell);
                infobancaria.Show();
            }           
            
        }

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
                if (cbbitemplaca.IsSelected == true)
                {
                    placa = txtbusqueda.Text;
                    conductor = "";
                    propietario = "";
                }
                else if (cbbitempropi.IsSelected == true)
                {
                    placa = "";
                    conductor = "";
                    propietario = txtbusqueda.Text;
                }
                else if (cbbitemcond.IsSelected == true)
                {
                    placa = "";
                    propietario = "";
                    conductor = txtbusqueda.Text;
                }
                lbl_Total.Text = Convert.ToString(Vehibll.contadorVehiculo(placa, conductor, propietario, remolque));

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
                var lista = Vehibll.GetVehiculo(placa, conductor, propietario, remolque, Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
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
                if (cbbitemplaca.IsSelected == true)
                {
                    placa = txtbusqueda.Text;
                    conductor = "";
                    propietario = "";
                }
                else if (cbbitempropi.IsSelected == true)
                {
                    placa = "";
                    conductor = "";
                    propietario = txtbusqueda.Text;
                }
                else if (cbbitemcond.IsSelected == true)
                {
                    placa = "";
                    propietario = "";
                    conductor = txtbusqueda.Text;

                }
                lbl_Total.Text = Convert.ToString(Vehibll.contadorVehiculo(placa, conductor, propietario, remolque));
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
                var lista = Vehibll.GetVehiculo(placa, conductor, propietario, remolque, Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
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
            string placabtn = ((Button)sender).CommandParameter.ToString();
            var currentCell = gridlistadovehiculos.SelectedItem;
            var item = (VehiT_ConsultarVehiculoXplaca_condu_propi_Result)currentCell;
           

            if (ValidarDatosReportados(item) == false)
            {
                IniVehi = new InicioVehiculos();
                IniVehi.GetPlaca(placabtn);
                IniVehi.RecibirPadre(main);
                IniVehi.RecibeRedirect(this);
                // IniVehi.GetListausuario(SecurityVariables.PPerfilUsuario);
                main.pnl_inicio.Children.Clear();
                main.pnl_inicio.Children.Add(IniVehi);
                
            }
            else
            {
                switch (item.TipoAfiliacion)
                {
                    case 2:
                    case 3:
                        mostrar_mensaje_reportado(43);
                        break;
                    default:
                        mostrar_mensaje_reportado(60);
                        IniVehi = new InicioVehiculos();
                        IniVehi.GetPlaca(placabtn);
                        IniVehi.RecibirPadre(main);
                        IniVehi.RecibeRedirect(this);
                        // IniVehi.GetListausuario(SecurityVariables.PPerfilUsuario);
                        main.pnl_inicio.Children.Clear();
                        main.pnl_inicio.Children.Add(IniVehi);
                        break;
                }              
            }           
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
        private void mostrar_mensaje_reportado(int idmensaje, string mensaje)
        {
            Mensabll = new MensajeBLL();
            Mensabll.tipomensajes(idmensaje,"");
            Venmsj = new VentanaMensaje();
            Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
            Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
            Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes + mensaje );
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
                        mostrar_mensaje_reportado(43, item.IdPlaca  + " SE ENCUENTRA REPORTADO POR SEGURIDAD");
                    }
                    
                    break;
                }
            }   
            return Respuesta;
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

        #endregion

        #region Eventos Seguridad
        //internal void GetListausuario(List<seguridad_consultarMenuPermisos2_Result> list)
        //{
        //    var lista = list;
        //    var listaprivi = (from rmenu in lista
        //                      where rmenu.idMenuPadre == 230 & rmenu.alterno == 1
        //                      select rmenu).ToList();
        //    foreach (var item in listaprivi)
        //    {
        //        if (item.nombreMenu == "Cambiar Estado" & item.alterno == 1 & item.Actualizar == 1)
        //        {
        //            boolcambioestado = true;

        //        }
        //        if (item.nombreMenu == "Información Bancaria" & item.alterno == 1 & item.Actualizar == 1)
        //        {
        //            boolinfobancaria = true;

        //        }
        //    }
        //    var listabotones = (from rmenu in lista
        //                        where rmenu.idMenu == 231
        //                        select rmenu).ToList();
        //    // main.VisibilidadBotones(listabotones.ToList());
        //    RecibePrivilegios(boolcambioestado, boolinfobancaria);
        //}
        #endregion

    }
}