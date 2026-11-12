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
    /// Lógica de interacción para EmpresaSatelital.xaml
    /// </summary>
    public partial class EmpresaSatelital : UserControl
    {

        #region Declaraciones
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj = new VentanaMensaje();
        private int iStar = 1;
        private int iEnd = 10;
        private int iAdd = 10;
        EmpresaSatelitalBLL EmpeBll = new EmpresaSatelitalBLL();
        string nombreemp, nit;
        List<seguridad_consultarMenuPermisos2_Result> resultPerfilUsuario;
        MainWindow main;
        private Nullable<Boolean> booleditar,boolcrear, boolcambioestado;
        #endregion

        #region load
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarPrivilegios();
            DelegarPrivilegios();
            if (IsLoaded)
            {
                LimpiarCamposInfirmacion();
                LimpiarCamposAprobacion();
                llenargrilla();
            }
        }
        public EmpresaSatelital()
        {
            InitializeComponent();
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
            //if (boolcrear == true)
            //{
            //    btn_Nuevo.Visibility = System.Windows.Visibility.Visible;
            //}
            //else
            //{
            //    btn_Nuevo.Visibility = System.Windows.Visibility.Collapsed;
            //}
            if (boolcambioestado == true)
            {
                exp_aprobacion.IsEnabled=true ;
            }
            else
            {

                exp_aprobacion.IsEnabled = false ;

            }
            if (booleditar  == true)
            {
                exp_informacion .IsEnabled = true ;
            }
            else
            {
                exp_informacion.IsEnabled = false;

            }

        }
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
                    case "Estado Emp Satelital":
                        if (item.Actualizar == 1)
                        {
                            boolcambioestado = true;
                        }
                        else
                        {
                            boolcambioestado = false;
                        }
                        
                        break;
                    case "Editar Emp Satelital":
                        if (item.Actualizar == 1)
                        {
                            booleditar = true;
                        }
                        else
                        {
                            booleditar = false;
                        }
                        break;
                    case "Crear Emp Satelital":
                        if (item.Actualizar == 1)
                        {
                            boolcrear = true;
                        }
                        else
                        {
                            boolcrear = false;
                        }
                        break;
                }
            }
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
        private void CargarInfoCompania(string IDempre)
        {
            
            var lista = EmpeBll.GetCompaniaPorID(IDempre);
            foreach (var item in lista)
            {
                txtnombreemp.Text = item.Nombre;
                txtnitemp.Text = item.Nit;
                txtdireccion.Text = item.Direccion;
                txtrepresentante.Text = item.Representante;
                txttelefono1.Text = item.Telefono1;
                txttelefono2.Text = item.Telefono2;
                txtciudad.Text = item.Ciudad;
                txtpaginaweb.Text = item.DirWeb;
                lblidemprecreacion.Content = item.IdEmpresa;
                if(item.Inactivo==true )
                {
                    cbbiteminactivo.IsSelected = true;
                }
                else
                {
                    cbbitemactivo.IsSelected = true;
                }               
                exp_informacion.IsEnabled  = true;
                exp_informacion.IsExpanded = true;
                
                break;            
            }
        }
        private void llenargrilla()
        {
            try
            {
                pnl_Paginacion.Visibility = Visibility.Visible;
                gridlistadocompanias.ItemsSource = null;
                gridlistadocompanias.Visibility = Visibility.Visible;
                lbl_Inicio.Text = iStar.ToString();
                if (cbbitemnombre .IsSelected == true)
                {
                    nombreemp = txtbusqueda.Text;
                    nit = "";
                   
                }
                else if (cbbitemnit.IsSelected == true)
                {
                    nombreemp  = "";              
                    nit = txtbusqueda.Text;
                }

                lbl_Total.Text = Convert.ToString(EmpeBll.ContadorEmpresasSatelitales(nombreemp , nit));
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
                var lista = EmpeBll.GetCompaniasSatelitales(nombreemp, nit, iStar, iEnd);
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
       private void CargarGrilla(List<VehiT_ConsultaEmpSAtelitalPorNombre_Nit_Result> lista)
        {
            var listaaux = lista;
            gridlistadocompanias.ItemsSource = lista;
            if (boolcambioestado == true)
            {
                gridlistadocompanias.Columns[0].Visibility = System.Windows.Visibility.Visible;
                exp_aprobacion.IsEnabled = true;
            }
            else
            {
                gridlistadocompanias.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
                exp_aprobacion.IsEnabled = false ;
            }
            if (booleditar  == true)
            {
                gridlistadocompanias.Columns[1].Visibility = System.Windows.Visibility.Visible;
                exp_informacion.IsEnabled = true;

            }
            else
            {
                gridlistadocompanias.Columns[1].Visibility = System.Windows.Visibility.Collapsed;
                exp_informacion.IsEnabled = false ;
            }           
        }
        private void LimpiarCamposInfirmacion()
        {
            txtnombreemp.Text = "";
            txtnitemp.Text = "";
            txtdireccion.Text = "";
            txtrepresentante.Text = "";
            txttelefono1.Text = "";
            txttelefono2.Text = "";
            txtciudad.Text = "";
            txtpaginaweb.Text = "";           
            cbbiteminactivo.IsSelected = true;
            lblidemprecreacion.Content = "";
            
        }
        private void LimpiarCamposAprobacion()
        {
            lblidempresaaprob.Content = "";
            lblEmpresa.Content = "";

        }
        private void MostrarpanelAprobacion(string idempresa)
        {
            exp_aprobacion.IsExpanded = true;
            var lista = EmpeBll.GetCompaniaPorID(idempresa);
            foreach (var item in lista)
            {
                lblEmpresa.Content = item.Nombre;
                lblnit.Content = item.Nit;
                lblidempresaaprob.Content = item.IdEmpresa;
                if (item.Inactivo == true)
                {
                    btn_Aprobar.IsEnabled = true;
                    btn_Noaprobar.IsEnabled = false;
                }
                else
                {
                    btn_Aprobar.IsEnabled = false;
                    btn_Noaprobar.IsEnabled = true;
                }
                break;
            }
        }
        private Boolean ValidarCampos()
        {
            Boolean Respuesta = false;
            if (txtnombreemp.Text == "")
            {
                Respuesta = true;
                lblerror_nombreemp.Content = "***";
            }
            else
            {
                lblerror_nombreemp.Content = "";
            }
            if (txtnitemp.Text == "")
            {
                Respuesta = true;
                lblerror_nitemp.Content = "***";
            }
            else
            {
                lblerror_nitemp.Content = "";
            }
            if (txttelefono1.Text == "")
            {
                Respuesta = true;
                lblerror_telefono1.Content = "***";
            }
            else
            {
                lblerror_telefono1.Content = "";
            }
            if (txtciudad.Text == "")
            {
                Respuesta = true;
                lblerror_ciudad.Content = "***";
            }
            else
            {
                lblerror_ciudad.Content = "";
            }

            return Respuesta;

        }
        private void GuardarDatosEmpresa()
        {

            if (ValidarCampos() == false)
            {
                Boolean respuesta = false;
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

                if (lblidemprecreacion.Content != "")
                {

                    respuesta = EmpeBll.ActualizaDAtosEmpresaSAtelital(Convert.ToDecimal(lblidemprecreacion.Content.ToString()), txtnombreemp.Text, txtnitemp.Text,
                        txtdireccion.Text, txtrepresentante.Text, txttelefono1.Text, txttelefono2.Text, txtciudad.Text, txtpaginaweb.Text, usuario);
                    mostrarmensaje(respuesta);

                }
                else
                {
                    var lista = EmpeBll.GetDatosCompaniaPorNIT(txtnitemp.Text);
                    if (lista.Count > 0)
                    {
                        Mensabll = new MensajeBLL();
                        Mensabll.tipomensajes(35,"");
                        Venmsj = new VentanaMensaje();
                        Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                        Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                        Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                        Venmsj.ShowDialog();
                    }
                    else
                    {
                        respuesta = EmpeBll.GuardarDAtosEmpresaSAtelital(txtnombreemp.Text, txtnitemp.Text,
                        txtdireccion.Text, txtrepresentante.Text, txttelefono1.Text, txttelefono2.Text, txtciudad.Text, txtpaginaweb.Text, usuario);
                        mostrarmensaje(respuesta);
                    }
                }
                if (respuesta == true)
                {
                    exp_Busqueda.IsExpanded = true;
                }
            }
        }        
        #endregion

        #region Eventos
        private void btn_Guardar_Click(object sender, RoutedEventArgs e)
        {
            GuardarDatosEmpresa();
        }

        private void btnActivar_Click(object sender, RoutedEventArgs e)
        {
          
            string nitbtn = ((Button)sender).CommandParameter.ToString();
            MostrarpanelAprobacion(nitbtn );           
        }

     
        private void gridlistadovehiculos_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            int cantidad = 0;
            if (booleditar == false)
            {
                cantidad = cantidad + 1;
            }            
            if (boolcambioestado == false)
            {
                cantidad = cantidad + 1;
            }
            switch (gridlistadocompanias.CurrentCell.Column.DisplayIndex)
            {
                case 0:
                   
                     e.ClipboardRowContent.Clear();
              
                    break;
                case 1:
                    e.ClipboardRowContent.Clear();                     
                    break;
                default:
                    var currentCell = e.ClipboardRowContent[gridlistadocompanias.CurrentCell.Column.DisplayIndex - cantidad ];
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
                if (cbbitemnombre.IsSelected == true)
                {
                    nombreemp = txtbusqueda.Text;
                    nit = "";

                }
                else if (cbbitemnit.IsSelected == true)
                {
                    nombreemp = "";
                    nit = txtbusqueda.Text;
                }

                lbl_Total.Text = Convert.ToString(EmpeBll.ContadorEmpresasSatelitales(nombreemp, nit));

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
                gridlistadocompanias.ItemsSource = null;
                var lista = EmpeBll.GetCompaniasSatelitales(nombreemp, nit, Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
                if (lista.Count > 0)
                {
                    
                    CargarGrilla(lista);
                }
                else
                {

                }

                gridlistadocompanias.Visibility = Visibility.Visible;

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
                if (cbbitemnombre.IsSelected == true)
                {
                    nombreemp = txtbusqueda.Text;
                    nit = "";

                }
                else if (cbbitemnit.IsSelected == true)
                {
                    nombreemp = "";
                    nit = txtbusqueda.Text;
                }
                lbl_Total.Text = Convert.ToString(EmpeBll.ContadorEmpresasSatelitales(nombreemp, nit));
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
                gridlistadocompanias.ItemsSource = null;
                var lista = EmpeBll.GetCompaniasSatelitales(nombreemp, nit, Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
                if (lista.Count > 0)
                {
                    // gridlistadovehiculos.ItemsSource = lista;
                    CargarGrilla(lista);
                }


                gridlistadocompanias.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        private void btnverDatos_Click(object sender, RoutedEventArgs e)
        {
            string IDemprebtn = ((Button)sender).CommandParameter.ToString();
            CargarInfoCompania(IDemprebtn );
        

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
        private void exp_Busqueda_Expanded(object sender, RoutedEventArgs e)
        {
            //exp_aprobacion.IsEnabled = false;
            //exp_informacion.IsEnabled = false;
            exp_informacion.IsExpanded = false;
            exp_aprobacion.IsExpanded = false;
            LimpiarCamposInfirmacion();
            LimpiarCamposAprobacion();
            llenargrilla();
        }

        private void exp_informacion_Expanded(object sender, RoutedEventArgs e)
        {

            exp_Busqueda.IsExpanded = false;
            exp_aprobacion.IsExpanded = false;
            LimpiarCamposAprobacion();
        }

        private void exp_aprobacion_Expanded(object sender, RoutedEventArgs e)        {
           
            LimpiarCamposInfirmacion();
            exp_informacion.IsExpanded = false;
            exp_Busqueda.IsExpanded = false;
        }

        public void Nuevaempresa()
        {

            exp_informacion.IsEnabled = true;
            exp_informacion.IsExpanded = true;
        }
        private  void btn_Nuevo_Click(object sender, RoutedEventArgs e)
        {
            Nuevaempresa();
        }

        private void btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCamposInfirmacion();
            LimpiarCamposAprobacion();
            exp_Busqueda.IsExpanded = true;

        }

        private void TextboxtStringShift_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (validarCaracteresespeciales(e, txt) == true)
            {
                e.Handled = false;
            }
            //if (e.Key == Key.LeftCtrl  || e.Key == Key.System)
            //{
            //    e.Handled = true;
            //    txt.Text = "";
            //}           

        }
        private void btn_Noaprobar_Click(object sender, RoutedEventArgs e)
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
            Boolean respuesta = false;
            if (lblidempresaaprob.Content != "")
            {
                respuesta = EmpeBll.ActualizaEstadoEmpresaSAtelital(Convert.ToDecimal(lblidempresaaprob.Content.ToString()), true, usuario);
                exp_Busqueda.IsExpanded = true;
                mostrarmensaje(respuesta);
            }
            ;
        }

        private void btn_Aprobar_Click(object sender, RoutedEventArgs e)
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
            Boolean respuesta = false;
            if (lblidempresaaprob.Content != "")
            {
                respuesta = EmpeBll.ActualizaEstadoEmpresaSAtelital(Convert.ToDecimal(lblidempresaaprob.Content.ToString()), false, usuario);
                exp_Busqueda.IsExpanded = true;
                mostrarmensaje(respuesta);
            }

        }
        #endregion


       
  

       
    }
}
