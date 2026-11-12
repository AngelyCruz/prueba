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
using WPF_Vehitanques.Utilidades;

namespace WPF_Vehitanques.Presentacion.Controles
{
    /// <summary>
    /// Lógica de interacción para AdmVehiculosReportados.xaml
    /// </summary>
    public partial class AdmVehiculosReportados : UserControl
    {
        #region declaraciones
        Numeric numeric = new Numeric();
        Boolean boolhabilitareportados;
        VetoBLL Vetobll = new VetoBLL();
        MainWindow main;
        #endregion
        #region Load
        public AdmVehiculosReportados()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarPrivilegios();
             DelegarPrivilegios();           
            if (IsLoaded)
            { 

            }
        }

        #endregion
        
        #region Métodos
        public void RecibirPadre(MainWindow padre)
        {
            main = padre;
        }
        private void DelegarPrivilegios()
        {
            if (boolhabilitareportados  == true)
            {
                rdbhabilitado.IsEnabled = true ;
              //  rdbhabilitado.Visibility = Visibility.Visible;
               // btn_Nuevo.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                rdbhabilitado.IsEnabled = false;
              //  rdbhabilitado.Visibility = Visibility.Collapsed;
            }
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
            foreach (var item in listaprivi)
            {
                switch (item.nombreMenu)
                {
                    case "Habilitar Reportados":
                        if (item.Autorizar  == 1)
                        {
                            boolhabilitareportados = true;
                        }
                        else
                        {
                            boolhabilitareportados = false;
                        }

                        break;
                    //case "Editar Emp Satelital":
                    //    if (item.Actualizar == 1)
                    //    {
                    //        booleditar = true;
                    //    }
                    //    else
                    //    {
                    //        booleditar = false;
                    //    }
                    //    break;
                    //case "Crear Emp Satelital":
                    //    if (item.Actualizar == 1)
                    //    {
                    //        boolcrear = true;
                    //    }
                    //    else
                    //    {
                    //        boolcrear = false;
                    //    }
                    //    break;
                }
            }
        }
        private void limpiarcamposvehiculo()
        {
            txtplaca.Text = "";
            txtpropietario.Text = "";

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
        private void limpiarcampospersona()
        {
            txtnit.Text = "";
            txtnombrepropi1.Text = "";
            txtnombrepropi2.Text = "";
            txtapellidopropi1.Text = "";
            txtapellidopropi2.Text = "";
            chkconductor.IsChecked = false;
            chkpropietario.IsChecked = false;
        }
        public  void GuardarDatos()
        {
            if (rdbpersona.IsChecked == true)
            {
                GuardarDatosPersona();
            }
            else if (rdbvehiculo.IsChecked == true)
            {
                GuardarDatosVehiculo();
            }

        }
        private void ConsultaDatosreportado(string nit)
        {
            var lista = Vetobll.GetDatosPersonaReportada(nit);
            if (lista.Count > 0)
            {
                MessageBoxResult result1 = MessageBox.Show("Esta persona  está reportada desea cargar datos para modificarlo?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result1 == MessageBoxResult.Yes)
                {
                    limpiarcampospersona();

                    foreach (var item in lista)
                    {
                        txtnit.IsEnabled = false;
                        txtnit.Text = item.Nit;                        
                        switch (item.Estado)
                        {
                            case 1:
                                rdbinhabilitado.IsChecked = true;
                                break;
                            case 2:
                                rdbhabilitado.IsChecked = true;
                                break;
                        }
                        if (item.Conductor == 1)
                        {
                            chkconductor.IsChecked = true;
                        }
                        else
                        {
                            chkconductor.IsChecked = false; ;
                        }
                        if (item.Propietario == 1)
                        {
                            chkpropietario .IsChecked = true;
                        }
                        else
                        {
                            chkpropietario.IsChecked = false ;
                        }
                        txtnombrepropi1.Text = item.Nombre1;
                        txtnombrepropi2.Text = item.Nombre2;
                        txtapellidopropi1.Text = item.Apellido1;
                        txtapellidopropi2.Text = item.Apellido2;
                        txtobservacionestenedor.Text = item.Observaciones;
                        break;
                    }
                }
                else
                {
                    limpiarcampospersona();
                }

            }
            else
            {
                MessageBoxResult result1 = MessageBox.Show("Esta persona  no está reportada desea reportarlo?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result1 == MessageBoxResult.Yes)
                {
                    string id = txtplaca.Text;
                    limpiarcamposvehiculo();
                    txtplaca.Text = id;
                }
            }     
        }
        private void llenarDatosplaca(string placa)
        {
            var lista = Vetobll.GetDatosDatosvehiculoReportado(placa );
            if (lista.Count > 0)
            {
                MessageBoxResult result1 = MessageBox.Show("Este vehículo  está reportado desea cargar datos para modificarlo?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result1 == MessageBoxResult.Yes)
                {
                    limpiarcamposvehiculo();
                    foreach (var item in lista)
                    {
                        txtplaca.IsEnabled = false;
                        txtplaca.Text = item.Placa;
                        switch (item.Estado)
                        {
                            case 1:
                                rdbinhabilitado.IsChecked = true;
                                break;
                            case 2:
                                rdbhabilitado.IsChecked = true;
                                break;
                        }
                        txtpropietario.Text = item.Propietario;

                        txtobservacionestenedor.Text = item.Observaciones;
                        break;
                    }
                }
                else
                {
                    limpiarcamposvehiculo();
                 //   txtplaca.IsEnabled = true ;
                }

                
            }
            else
            {
                MessageBoxResult result1 = MessageBox.Show("Este vehículo no está reportado desea reportarlo?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result1 == MessageBoxResult.Yes)
                {
                    string id = txtplaca.Text;
                    limpiarcamposvehiculo();
                    txtplaca.Text = id;
                }
            }            
        }
        private void GuardarDatosPersona()
        {

            if (validarDatosPersona() == false)
            {
                string usuario = "";
                   string aprobador = "";
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
                short propietario,conductor,estado;
              
                if(rdbhabilitado.IsChecked==true )
                {
                    estado=2;
                    aprobador=usuario;
                }else{
                     estado=1;                    
                }
                if(chkpropietario.IsChecked ==true ){
                    propietario=1;
                }
                else{
                    propietario=0;
                }
                 if(chkconductor .IsChecked ==true ){
                     conductor=1;
                }
                else{
                     conductor=0;
                }
                var lista = Vetobll.GetDatosPersonaReportada(txtnit.Text);
                if (lista.Count > 0)
                {
                        MessageBoxResult result1 = MessageBox.Show("Esta persona ya  está reportado desea  modificar sus datos?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result1 == MessageBoxResult.Yes)
                        {
                            Vetobll.UpdatePersonaVeto(txtnit.Text, txtnombrepropi1.Text, txtnombrepropi2.Text, txtapellidopropi1.Text, txtapellidopropi2.Text, propietario, conductor, usuario, estado,txtobservacionestenedor.Text ,aprobador  );
                        }
                }
                else
                {
                    Vetobll.GuardarPersonaVeto(txtnit.Text, txtnombrepropi1.Text, txtnombrepropi2.Text, txtapellidopropi1.Text, txtapellidopropi2.Text, propietario, conductor, usuario, estado, txtobservacionestenedor.Text);

                }
            }
        }

        private bool validarDatosPersona()
        {
            Boolean respuesta = false;
            if (txtnit.Text == "")
            {
                lblerror_nit.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_nit.Content = "";
            }
            if (txtnombrepropi1.Text == "")
            {
                lblerror_nombrepropi1.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_nombrepropi1.Content = "";
            }
            if (txtapellidopropi1 .Text == "")
            {
                lblerror_apellidopropi1.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_apellidopropi1.Content = "";
            }

            return respuesta;
        }
        private void GuardarDatosVehiculo()
        {
            if (validarDatosvehiculo() == false)
            {
                string usuario = "";
                string aprobador = "";
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
                short estadovehi;

                if (rdbhabilitado.IsChecked == true)
                {
                    estadovehi = 2;
                    aprobador = usuario;
                }
                else
                {
                    estadovehi = 1;
                }
                var lista = Vetobll.GetDatosDatosvehiculoReportado(txtplaca.Text);
                if (lista.Count > 0)
                {
                    MessageBoxResult result1 = MessageBox.Show("Este vehículo ya  está reportado desea  modificar sus datos?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result1 == MessageBoxResult.Yes)
                    {
                        Vetobll.UpdateVehiculoVeto(txtplaca.Text, txtpropietario.Text, usuario, estadovehi, txtobservacionestenedor.Text, aprobador);
                    }
                }
                else
                {
                    Vetobll.GuardarVehiculoVeto(txtplaca.Text, txtpropietario.Text, usuario, estadovehi, txtobservacionestenedor.Text);
                }
            }
        }

        private bool validarDatosvehiculo()
        {
            Boolean respuesta = false;
            if (txtplaca.Text == "")
            {
                lblerror_placa.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_placa.Content = "";
            }
            if (txtpropietario .Text == "")
            {
                lblerror_placa_propietario.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_placa_propietario.Content = "";
            }
            return respuesta;
        }
        #endregion
        #region Eventos
        private void txtNumdocumentopropi_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (validarCaracteresespeciales(e, txt) == true)
            {


                if (e.Key == Key.Enter || e.Key == Key.Tab)
                {
                    ConsultaDatosreportado(txtnit.Text);
                    //Finalizado la consulta
                    //Movemos el foco al siguiente
                    //TextBox
                    txt.MoveFocus(new TraversalRequest(
                        FocusNavigationDirection.Next));
                }
            }
        }

        
        private void rdbvehiculo_Checked(object sender, RoutedEventArgs e)
        {
            pnlpersona.Visibility = Visibility.Collapsed;
            pnlvehiculo.Visibility = Visibility.Visible;
            limpiarcampospersona();
            limpiarcamposvehiculo();
        }

        private void rdbpersona_Checked(object sender, RoutedEventArgs e)
        {
            pnlpersona.Visibility = Visibility.Visible;
            pnlvehiculo.Visibility = Visibility.Collapsed;
            limpiarcamposvehiculo();
            limpiarcampospersona();
        }
        private void chkobservacionestenedor_Checked(object sender, RoutedEventArgs e)
        {
            txtobservacionestenedor.IsEnabled = true;
        }

        private void chkobservacionestenedor_Unchecked(object sender, RoutedEventArgs e)
        {
            txtobservacionestenedor.IsEnabled = false;
        }
        private void txtplaca_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if (validarCaracteresespeciales(e, txt) == true)
            {
                if (e.Key == Key.Enter || e.Key == Key.Tab)
                {
                    llenarDatosplaca(txtplaca.Text);
                    //  txt.MoveFocus(new TraversalRequest(
                    //   FocusNavigationDirection.Next));
                }
            }
            else
            {
                txt.Text = "";
            }
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
        /// <summary>
        /// Valida tipos numéricos en los texbox, en impide caracteres string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
       
        #endregion








  
    }
}
