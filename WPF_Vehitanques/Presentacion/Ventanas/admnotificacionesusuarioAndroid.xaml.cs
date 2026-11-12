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

namespace WPF_Vehitanques.Presentacion.Ventanas
{
    /// <summary>
    /// Lógica de interacción para admnotificacionesusuarioAndroid.xaml
    /// </summary>
    public partial class admnotificacionesusuarioAndroid : Window
    {
        #region Declaraciones
        private int iStar = 1;
        private int iEnd = 10;
        private int iAdd = 10;
        TipoDocumentoBLL TiDocBll = new TipoDocumentoBLL();
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj = new VentanaMensaje();
        VehiT_ConsultaConductor_Result row_conductor = null;
        AndroidBLL _AndroidBLL = new AndroidBLL();
        Andr_NotificacionesUsuarioBLL NotifiBll = new Andr_NotificacionesUsuarioBLL();
        MainWindow main;
        string usuario="", agencia="";
        Boolean boolmsgandroid = false,updnotifi=false ,deletenotifi=false ;
        #endregion
        #region Load
        public admnotificacionesusuarioAndroid()
        {
            InitializeComponent();
        }
        private void admnotificacionesuserAndroid_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarPrivilegios();
            DelegarPrivilegios();
            if (IsLoaded)
            {
                CargarTipoDocumentos();                
                GetDAtosUserAndroid();
                limpiarCampos();
                llenargrilla();
            }
        }
        //public admnotificacionesusuarioAndroid()
        //{
           
        //} 
        #endregion
        #region Métodos
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
                    case "Agregar Noficaciones usuario Android":
                        if (item.Autorizar == 1)
                        {
                            boolmsgandroid  = true;
                        }
                        else
                        {
                            boolmsgandroid = false;
                        }

                        break;
                         case "Delete Notif User":
                        if (item.Autorizar == 1)
                        {
                            deletenotifi  = true;
                        }
                        else
                        {
                            deletenotifi = false;
                        }

                        break;
                         case "Upd Notif User":
                        if (item.Autorizar == 1)
                        {
                            updnotifi = true;
                        }
                        else
                        {
                            updnotifi = false;
                        }

                        break;
                        
                            

                }
            }
        }
        private void DelegarPrivilegios()
        {
            if (boolmsgandroid == true)
            {
                btn_guardar.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                btn_guardar.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        public void RecibirPadre(MainWindow padre)
        {
            if (main == null)
                main = padre;
        }
        private void CargarTipoDocumentos()
        {
            var lista = TiDocBll.GetTipoDocumentoIdentificacion();
            cbbtipodocumento.ItemsSource = lista;
            cbbtipodocumento.SelectedValuePath = "Codigo";
            cbbtipodocumento.DisplayMemberPath = "TipoDocumento";
            cbbtipodocumento.SelectedIndex = 0;
        }
        private void GetDAtosUserAndroid()
        {
            var lista = _AndroidBLL.GetDatosUsuarioAndroid(Convert.ToDecimal(row_conductor.IdConductor));
            if (lista.Count > 0)
            {
                foreach (var item in lista)
                {
                    if (item.IsEstado != 1)
                    {
                        MessageBox.Show("El conductor no se encuentra activo en la aplicación  ANDROID, por lo tanto no puede agregarle notificaciones");
                        this.Close();
                    }
                    break;
                }
            }
            else
            {
                MessageBox.Show("El conductor no posee un usuario ANDROID creado, por lo tanto no puede agregarle notificaciones");
                this.Close();
            }            


        }
        
       
        public void RecibirConductor(Modelo.VehiT_ConsultaConductor_Result _row_conductor)
        {
            row_conductor = _row_conductor;
            txtnombres.Text = _row_conductor.Nombres;
            txtapellido1.Text = _row_conductor.Apellido1;
            txtapellido2.Text = _row_conductor.Apellido2;
            txtNumdocumento.Text = row_conductor.IdConductor;
           // txtusername.Text = row_conductor.IdConductor.Trim();
            // cbbtipodocumento.SelectedValue=_row_conductor.
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
                limpiarCampos();
                llenargrilla();
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
        private void limpiarCampos()
        {
            lblidnotificacion.Content = "";
            txtobservaciones.Text = "";
        }
        private void llenargrilla()
        {
            try
            {
                pnl_Paginacion.Visibility = Visibility.Visible;
                gridlistanotificaciones.ItemsSource = null;
                gridlistanotificaciones.Visibility = Visibility.Visible;
                lbl_Inicio.Text = iStar.ToString();

                lbl_Total.Text = Convert.ToString(NotifiBll.ContadorNotificacionesUsuario(Convert.ToDecimal(row_conductor.IdConductor)));
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
                var lista = NotifiBll.GetNotificacionesUsuario(Convert.ToDecimal(row_conductor.IdConductor), iStar, iEnd);
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
        private void CargarGrilla(List<And_ConsultaTotalNofiticacionesUsuarioActivas_Result> lista)
        {
            gridlistanotificaciones.ItemsSource = null;
            gridlistanotificaciones.ItemsSource = lista;
            if (updnotifi  == true)
            {
                gridlistanotificaciones .Columns[0].Visibility = System.Windows.Visibility.Visible;

            }
            else
            {
                gridlistanotificaciones.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
            }
            if (deletenotifi == true)
            {
                gridlistanotificaciones.Columns[1].Visibility = System.Windows.Visibility.Visible;

            }
            else
            {
                gridlistanotificaciones.Columns[1].Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        private Boolean validarcampos()
        {
            Boolean respuesta = false;
            if (txtobservaciones.Text.Trim() == "")
            {
                respuesta = true;
                MessageBox.Show("Es necesario que registre una observación para enviarla al usuario");
                return true;
            }

            return respuesta;

        }
        #endregion
        #region Eventos

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
                lbl_Total.Text = Convert.ToString(NotifiBll.ContadorNotificacionesUsuario(Convert.ToDecimal(row_conductor.IdConductor)));
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
                gridlistanotificaciones.ItemsSource = null;
                var lista = NotifiBll.GetNotificacionesUsuario(Convert.ToDecimal(row_conductor.IdConductor), Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
                if (lista.Count > 0)
                {
                    CargarGrilla(lista);
                }
                else
                {

                }
                gridlistanotificaciones.Visibility = Visibility.Visible;
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
                lbl_Total.Text = Convert.ToString(NotifiBll.ContadorNotificacionesUsuario(Convert.ToDecimal(row_conductor.IdConductor)));
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
                gridlistanotificaciones.ItemsSource = null;
                var lista = NotifiBll.GetNotificacionesUsuario(Convert.ToDecimal(row_conductor.IdConductor), Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
                if (lista.Count > 0)
                {
                    CargarGrilla(lista);
                }
                gridlistanotificaciones.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
        private void btn_Editar_Click(object sender, RoutedEventArgs e)
        {
            limpiarCampos();
            var currentCell = gridlistanotificaciones.SelectedItem;
            var item = (And_ConsultaTotalNofiticacionesUsuarioActivas_Result )currentCell;
            if (item != null)
            {
                txtobservaciones.Text = item.DescripcionNotificacion;
                lblidnotificacion.Content = item.IdNotificacion.ToString();
            }
           
        }

        private void btn_Eliminar_Click(object sender, RoutedEventArgs e)
        {
            var currentCell = gridlistanotificaciones.SelectedItem;
            var item = (And_ConsultaTotalNofiticacionesUsuarioActivas_Result)currentCell;
            if (item != null)
            {                
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
                                case "txtagencia":
                                    agencia = textblock.Text;
                                    break;
                            }
                            if (usuario != "" & agencia != "")
                            {
                                break;
                            }

                        }

                    } 
                Boolean respuesta = NotifiBll.UpdateEstadoNotificacionesUsuario (usuario, item.IdNotificacion,2);
                mostrarmensaje(respuesta);
            }
        }
      
        private void btn_guardar_Click(object sender, RoutedEventArgs e)
        {
            if (validarcampos() == false)
            {
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
                            case "txtagencia":
                                agencia = textblock.Text;
                                break;
                        }
                        if (usuario != "" & agencia != "")
                        {
                            break;
                        }
                    }
                }
                if (lblidnotificacion.Content.ToString().Trim() != "")
                {
                    var lista = NotifiBll.GetNotificacionUsrPorId(Convert.ToDecimal(lblidnotificacion.Content.ToString().Trim()));
                    if (lista.Count > 0)
                    {
                        MessageBoxResult result1 = MessageBox.Show("Ésta notificación ya  Existe. Desea Actualizar sus datos?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result1 == MessageBoxResult.Yes)
                        {
                            Boolean respuesta = NotifiBll.UpdateNotificacionesUsuario(txtobservaciones.Text, usuario, Convert.ToDecimal(lblidnotificacion.Content.ToString().Trim()));
                            mostrarmensaje(respuesta);
                        }
                        else
                        {
                            limpiarCampos();
                        }
                    }
                    else
                    {
                        MessageBoxResult result1 = MessageBox.Show("Se presentó un error en la consulta de la notificación. desea crearla como una completamente nueva?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result1 == MessageBoxResult.Yes)
                        {
                            Boolean respuesta = NotifiBll.GuardarNotificacionesUsuario(Convert.ToDecimal(txtNumdocumento.Text.Trim()), txtobservaciones.Text, usuario);
                            mostrarmensaje(respuesta);
                        }
                        else
                        {
                            limpiarCampos();
                        }
                    }                    
                }
                else
                {
                    Boolean respuesta = NotifiBll.GuardarNotificacionesUsuario(Convert.ToDecimal(txtNumdocumento.Text.Trim()), txtobservaciones.Text, usuario);
                    mostrarmensaje(respuesta);
                }
            }  
        }
        
        #endregion

        
    
      

      
        
    }
}
