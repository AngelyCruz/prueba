using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para AdminBeneficiarioPropietario.xaml
    /// </summary>
    public partial class AdminBeneficiarioPropietario : UserControl
    {
        #region Declaraciones#
        Numeric numeric = new Numeric();
        PropietarioBeneficiarioBLL PropiBeneBll = new PropietarioBeneficiarioBLL();
        CiudadBLL CiuBll = new CiudadBLL();
        VetoBLL vetobll = new VetoBLL();
        DepartamentoBLL DeparBll = new DepartamentoBLL();
        MainWindow main;
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj = new VentanaMensaje();
        private DatePicker FechaActual = new DatePicker();
        TipoDocumentoBLL TiDocBll = new TipoDocumentoBLL();
        AgenciaBLL AgenBll = new AgenciaBLL();
        #endregion


        #region Load
        public AdminBeneficiarioPropietario()
        {
            InitializeComponent();
            FechaActual.SelectedDate = DateTime.Now.Date;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarPrivilegios();
            DelegarPrivilegios();
          
            if (IsLoaded)
            {               
                cargarDepartamentosBenefi();
                cargarDepartamentosPropi();
                CargarTipoDocumentos();
                cargarAgencias();
            }



        }
        internal void GetListausuario(List<seguridad_consultarMenuPermisos2_Result> list)
        {
            var lista = list;
            Boolean boolcambioestadoobser = false;

            Boolean boolcambioestadolistasri = false;
            var listaprivi = (from rmenu in lista
                              where rmenu.idMenuPadre == 230 & rmenu.alterno == 1
                              select rmenu).ToList();
            foreach (var item in listaprivi)
            {
                switch (item.nombreMenu)
                {
                    case "Cambiar Observaciones":
                        if (item.alterno == 1 & item.Actualizar == 1)
                        {
                            boolcambioestadoobser = true;
                        }
                        break;                   
                    case "Listas de Riesgo":
                        if (item.alterno == 1 & item.Actualizar == 1)
                        {
                            boolcambioestadolistasri = true;
                        }
                        break;
                }
            }

            Habilitar_Observaciones(boolcambioestadoobser);            
            Habilita_listasRiesgo(boolcambioestadolistasri);          
        
        }
        #endregion

        #region Métodos#
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
        private   void ValidarPrivilegios()
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
        private void cargarAgencias()
        {
            var lista = AgenBll.GetAgencias();
            cbbagenciabenefi.ItemsSource = lista;
            cbbagenciabenefi.SelectedValuePath = "Codigo";
            cbbagenciabenefi.DisplayMemberPath = "Nombre";
            cbbagenciabenefi.SelectedIndex = 0;
        }
        private void Habilita_listasRiesgo(Boolean habilitar)
        {

            pnl_listasriesgobeneficiario.IsEnabled = habilitar;
            dtfechaestudiobenefi.IsEnabled = habilitar;        
            pnl_listasriesgopropietario.IsEnabled = habilitar;
            dtfechaestudiopropi.IsEnabled = habilitar;           
        }
        private void Habilitar_Observaciones(Boolean habilitar)
        {
            if (habilitar == false)
            {
               
                chkobservacionestenedor.IsEnabled = habilitar;
                chkobservacionestenedor.Visibility = Visibility.Collapsed;

            }
            else
            {
                chkobservacionestenedor.IsEnabled = habilitar;
                chkobservacionestenedor.Visibility = Visibility.Visible;
            }


        }
        bool invalid = false;
        public bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

       
        public Boolean ValidarDatosBeneficiario()
        {
            Boolean respuesta = false;
            if (txtemail.Text.Trim() != "")
            {
                if (IsValidEmail(txtemail.Text.Trim()) == false)
                {
                    MessageBox.Show("el formato digitado en el email no es correcto");
                    return true;
                }
            }
            if (txtNumdocumentobenefi.Text != "")
            {
                if (numeric.IsNumeric(txtNumdocumentobenefi.Text) == true)
                {
                    lblerror_Numdocumentobenef.Content = "";
                }
                else
                {
                    lblerror_Numdocumentobenef.Content = "***";
                    respuesta = true;
                }
                // lblerror_Numdocumentobenef.Content = "";
                //respuesta = false;
            }
            else
            {
                lblerror_Numdocumentobenef.Content = "***";
                respuesta = true;
            }

            if (txtnombresbenefi.Text != "")
            {
                lblerror_nombresbenefi.Content = "";
                // respuesta = false;
            }
            else
            {
                lblerror_nombresbenefi.Content = "***";
                respuesta = true;
            }
            if (cbbtipodocumentobenefi.SelectedValue == null)
            {
                cbbtipodocumentobenefi.SelectedIndex = 0;
            }
            //else
            //{
            //    lblerror_apellidobenefi1.Content = "";
            //}
            if (cbbtipodocumentobenefi.SelectedValue.ToString() != "N")
            {
                if (txtapellidobenefi1.Text != "")
                {
                    lblerror_apellidobenefi1.Content = "";
                    // respuesta = false;
                }
                else
                {
                    lblerror_apellidobenefi1.Content = "***";
                    respuesta = true;
                }
                if (txtapellidobenefi2.Text != "")
                {
                    lblerror_apellidobenefi2.Content = "";
                    // respuesta = false;
                }
                else
                {
                    lblerror_apellidobenefi2.Content = "***";
                    respuesta = true;
                }

            }
            else
            {
                if (txtNitVerificacionBenefi.Text.Trim() == "")
                {
                    MessageBox.Show("Debe Digitar el nit de verificación del Beneficiario para los documentos tipo NIT");
                    respuesta = true;
                }
                else
                {
                    int digito = CalculaNit(txtNumdocumentobenefi.Text.Trim());
                    if (digito != Convert.ToInt32(txtNitVerificacionBenefi.Text.Trim()))
                    {
                        MessageBox.Show("El Dígito de Verificación del Beneficiario no corresponde al Nit digitado, por favor verificar.");
                        respuesta = true;
                    }
                }
            }

            if (txtNumdocumentobenefi.Text.Trim() != "" & txtNumdocumentopropi.Text.Trim() != "")
            {
                if (txtNumdocumentobenefi.Text.Trim() == txtNumdocumentopropi.Text.Trim())
                {
                    if (cbbtipodocumentopropi.SelectedValue.ToString() != cbbtipodocumentobenefi.SelectedValue.ToString())
                    {
                        MessageBox.Show("No puede  seleccionar diferentes tipos de documento para un mismo número de identificación.");
                        respuesta = true;
                    }
                }
            }
            if (txttelefonobenefi.Text.Trim() == "" && txtcelbenefi.Text.Trim() == "")
            {
                lblerror_celbenefi.Content = "***";
                MessageBox.Show("Debe digitar al menos un número de teléfono o de celular del beneficiario.");
                respuesta = true;
            }
            else
            {
                if (txtcelbenefi.Text.Trim() != "")
                {
                    if (txtcelbenefi.Text.Trim().Length != 10)
                    {
                        lblerror_celbenefi.Content = "***";
                        MessageBox.Show("El número de celular del beneficiario no tiene la longitud adecuada.");
                        respuesta = true;
                    }
                    else
                    {
                        if (txttelefonobenefi.Text.Trim() != "")
                        {
                            if (txttelefonobenefi.Text.Trim().Length != 7)
                            {
                                MessageBox.Show("El número de teléfono del beneficiario no tiene la longitud adecuada.");
                                respuesta = true;
                            }
                            else
                            {
                                if (txtIndicativobenefi.Text.Trim().Length != 3)
                                {
                                    MessageBox.Show("El número Indicativo del teléfono del beneficiario no tiene la longitud adecuada.");
                                    respuesta = true;
                                }
                                else
                                {
                                    lblerror_celbenefi.Content = "";
                                }

                            }
                        }
                        else
                        {
                            txtIndicativobenefi.Text = "";
                            lblerror_celbenefi.Content = "";

                        }

                    }
                }
                else
                {
                    if (txttelefonobenefi.Text.Trim().Length != 7)
                    {
                        MessageBox.Show("El número de teléfono del beneficiario no tiene la longitud adecuada.");
                        respuesta = true;
                    }
                    else
                    {
                        if (txtIndicativobenefi.Text.Trim().Length != 3)
                        {
                            MessageBox.Show("El número Indicativo del teléfono del beneficiario no tiene la longitud adecuada.");
                            respuesta = true;
                        }
                        else
                        {
                            lblerror_celbenefi.Content = "";
                        }

                    }
                }
            }
            if (txtdireccionbenefi.Text.Trim() == "")
            {
                lblerror_direccionbenefi.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_direccionbenefi.Content = "";
            }
          
            return respuesta;
        }
        public Boolean ValidarDatosPropietario()
        {
            Boolean respuesta = false;
            if (txtNumdocumentopropi.Text != "")
            {
                if (numeric.IsNumeric(txtNumdocumentopropi.Text) == true)
                {
                    lblerror_Numdocumentopropi.Content = "";
                    //if (txtNumdocumentobenefi.Text != "")
                    //{
                    //    if (txtNumdocumentobenefi.Text != txtNumdocumentopropi.Text)
                    //    {
                    //        //btn_contratoBeneficiario.Background = new SolidColorBrush(Colors.Red);
                    //        //btn_contratoBeneficiario.Focus();
                            
                    //    }

                    //}
                    //else
                    //{
                    //    if (numeric.IsNumeric(txtNumdocumentobenefi.Text) == true)
                    //    {
                    //        lblerror_Numdocumentobenef.Content = "";
                    //    }
                    //    else
                    //    {
                    //        lblerror_Numdocumentobenef.Content = "***";
                    //        respuesta = true;
                    //    }

                    //}
                }
                else
                {
                    lblerror_Numdocumentopropi.Content = "***";
                    respuesta = true;
                }

                if (txtnombrespropi.Text != "")
                {
                    lblerror_nombrespropi.Content = "";
                    //respuesta = false;
                }
                else
                {
                    lblerror_nombrespropi.Content = "***";
                    respuesta = true;
                }
                if (cbbtipodocumentopropi.SelectedValue == null)
                {
                    cbbtipodocumentopropi.SelectedIndex = 0;
                }
                //if (cbbtipodocumentopropi.SelectedValue.ToString() == null)
                //{
                //    cbbtipodocumentopropi.SelectedIndex = 1;
                //}

                if (cbbtipodocumentopropi.SelectedValue.ToString() != "N")
                {
                    if (txtapellidopropi1.Text != "")
                    {
                        lblerror_apellidopropi1.Content = "";
                        //respuesta = false;
                    }
                    else
                    {
                        lblerror_apellidopropi1.Content = "***";
                        respuesta = true;
                    }
                    if (txtapellidopropi2.Text != "")
                    {
                        lblerror_apellidopropi2.Content = "";
                        // respuesta = false;
                    }
                    else
                    {
                        lblerror_apellidopropi2.Content = "***";
                        respuesta = true;
                    }
                }
                else
                {
                    if (txtNitVerificacionPropi.Text.Trim() == "")
                    {
                        MessageBox.Show("Debe Digitar el nit de verificación del propietario para los documentos tipo NIT");
                        respuesta = true;
                    }
                    else
                    {
                        int digito = CalculaNit(txtNumdocumentopropi.Text.Trim());
                        if (digito != Convert.ToInt32(txtNitVerificacionPropi.Text.Trim()))
                        {
                            MessageBox.Show("El Dígito de Verificación del propietario no corresponde al Nit digitado, por favor verificar.");
                            respuesta = true;
                        }
                    }
                }
                // lblerror_Numdocumentopropi.Content = "";
                // respuesta = false;

            }
            else
            {
                lblerror_Numdocumentopropi.Content = "***";
                respuesta = true;
            }

            if (txtNumdocumentobenefi.Text.Trim() != "" & txtNumdocumentopropi.Text.Trim() != "")
            {
                if (txtNumdocumentobenefi.Text.Trim() == txtNumdocumentopropi.Text.Trim())
                {
                    if (cbbtipodocumentopropi.SelectedValue.ToString() != cbbtipodocumentobenefi.SelectedValue.ToString())
                    {
                        MessageBox.Show("No puede  seleccionar diferentes tipos de documento para un mismo número de identificación.");
                        respuesta = true;
                    }
                }
            }

            if (txtdireccionpropi .Text.Trim() == "")
            {
                lblerror_direccionpropi .Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_direccionpropi .Content = "";
            }
            if (txttelefonopropi.Text.Trim() != "")
            {
                if (txtcelbenefi.Text.Trim().Length == 7 || txtcelbenefi.Text.Trim().Length == 10)
                {
                    lblerror_telefonopropi.Content = "";
                }
                else
                {
                    lblerror_telefonopropi.Content = "***";
                    MessageBox.Show("El número de teléfono o celular del propietario no tiene la longitud adecuada.");
                    respuesta = true;
                 
                }
            }
            else
            {
                lblerror_telefonopropi.Content = "***";
                MessageBox.Show("El número de teléfono o celular del propietario no tiene la longitud adecuada.");
                respuesta = true;

            }

            return respuesta;
        }
        private void CargarTipoDocumentos()
        {
            var lista = TiDocBll.GetTipoDocumentoIdentificacion();
          

            cbbtipodocumentobenefi.ItemsSource = lista;
            cbbtipodocumentobenefi.SelectedValuePath = "Codigo";
            cbbtipodocumentobenefi.DisplayMemberPath = "TipoDocumento";
            cbbtipodocumentobenefi.SelectedIndex = 1;

            cbbtipodocumentopropi.ItemsSource = lista;
            cbbtipodocumentopropi.SelectedValuePath = "Codigo";
            cbbtipodocumentopropi.DisplayMemberPath = "TipoDocumento";
            cbbtipodocumentopropi.SelectedIndex = 1;
        }
        private void cargarDepartamentosBenefi()
        {
            var lista = DeparBll.GetDepartamentos(1);

            cbbdeparbenefi.ItemsSource = lista;
            cbbdeparbenefi.SelectedValuePath = "Codigo";
            cbbdeparbenefi.DisplayMemberPath = "Nombre";
            cbbdeparbenefi.SelectedIndex = 0;

        }
        private void cargarDepartamentosPropi()
        {
            var lista = DeparBll.GetDepartamentos(1);

            cbbdeparpropi.ItemsSource = lista;
            cbbdeparpropi.SelectedValuePath = "Codigo";
            cbbdeparpropi.DisplayMemberPath = "Nombre";
            cbbdeparpropi.SelectedIndex = 0;



        }
        private void cargarCiudadespropietario(int departamento)
        {
            var lista = CiuBll.GetCiudades(departamento);
            cbbciudadpropi.ItemsSource = lista;
            cbbciudadpropi.SelectedValuePath = "CodigoReducido";
            cbbciudadpropi.DisplayMemberPath = "Nombre";
            cbbciudadpropi.SelectedIndex = 0;
        }

        private void cargarCiudadesbeneficiario(int departamento)
        {
            var lista = CiuBll.GetCiudades(departamento);

            cbbciudadbenefi.ItemsSource = lista;
            cbbciudadbenefi.SelectedValuePath = "CodigoReducido";
            cbbciudadbenefi.DisplayMemberPath = "Nombre";
            cbbciudadbenefi.SelectedIndex = 0;

        }
        private void limpiarcamposbeneficiario()
        {
            cbbtipodocumentobenefi.SelectedIndex = 1;
            txtNumdocumentobenefi.Text = "";
            txtnombresbenefi.Text = ""; ;
            txtapellidobenefi1.Text = "";
            txtapellidobenefi2.Text = "";
            txtdireccionbenefi.Text = "";
            txttelefonobenefi.Text = "";
            cbbagenciabenefi.SelectedIndex = 1;
            txtcelbenefi.Text = "";
            cbbdeparbenefi.SelectedIndex = 1;
            cargarCiudadesbeneficiario(Convert.ToInt32(cbbdeparbenefi.SelectedValue));
            cbbciudadbenefi.SelectedIndex = 1;
            rdblistaNoapruebabenefi.IsChecked = true;
            dtfechaestudiobenefi.SelectedDate = null;
            txtobservacionestenedor.Text = "";
            txtNitVerificacionBenefi.Text = "";
        }
        private void Habilitarcamposbeneficiario(Boolean autrotizacion)
        {

            cbbtipodocumentobenefi.IsEnabled = autrotizacion;

            txtNumdocumentobenefi.IsEnabled = autrotizacion;

            txtnombresbenefi.IsEnabled = autrotizacion; ;
            txtapellidobenefi1.IsEnabled = autrotizacion;
            txtapellidobenefi2.IsEnabled = autrotizacion;


            txtdireccionbenefi.IsEnabled = autrotizacion;
            txttelefonobenefi.IsEnabled = autrotizacion;

            cbbagenciabenefi.IsEnabled = autrotizacion;

            txtcelbenefi.IsEnabled = autrotizacion;

            cbbdeparbenefi.IsEnabled = autrotizacion;
            //      cargarCiudadesbeneficiario(Convert.ToInt32(cbbdeparbenefi.SelectedValue));
            cbbciudadbenefi.IsEnabled = autrotizacion;

            rdblistaNoapruebabenefi.IsChecked = false;
            //dtfechaestudiobenefi.SelectedDate  = autrotizacion;
            dtfechaestudiobenefi.IsEnabled = autrotizacion;


        }
        private void ConsultaDatosBeneficiario(string idbeneficiario)
        {
            var lista = PropiBeneBll.GetDatosBeneficiarioXID(idbeneficiario);
            if (lista.Count > 0)
            {
                MessageBox.Show(" Este Beneficiario ya existe en la base de datos,usted no tiene permitido modificarlo ");
                txtNumdocumentobenefi.Text = "";
            }
            else
            {
                var listareportado = vetobll.GetDatosPersonaReportada(idbeneficiario);
                Boolean reportado = false;
                if (listareportado.Count > 0)
                {
                    foreach (var item in listareportado)
                    {
                        if (item.Estado == 1)
                        {
                            reportado = true;
                            break;
                        }
                    }
                    if (reportado == true)
                    {
                        mostrar_mensaje_reportado(39);
                        limpiarcamposbeneficiario();
                    }
                    else
                    {                   

                        MessageBoxResult result1 = MessageBox.Show("Este Beneficiario no existe en la base de datos desea crealo?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result1 == MessageBoxResult.Yes)
                        {
                            string id = txtNumdocumentobenefi.Text;
                            limpiarcamposbeneficiario();
                            txtNumdocumentobenefi.Text = id;

                        }
                    }

                }
                else
                {
                   

                    MessageBoxResult result1 = MessageBox.Show("Este Beneficiario no existe en la base de datos desea crealo?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result1 == MessageBoxResult.Yes)
                    {
                        string id = txtNumdocumentobenefi.Text;
                        limpiarcamposbeneficiario();
                        txtNumdocumentobenefi.Text = id;

                    }
                }



            }

        }

        private void ConsultaDatosPropietario(string idpropietario)
        {
            var lista = PropiBeneBll.GetDatosPropietarioXID(idpropietario);
            if (lista.Count > 0)
            {
                MessageBox.Show(" Este Propietario ya existe en la base de datos,usted no tiene permitido modificarlo ");

            }
            else
            {

                var listareportado = vetobll.GetDatosPersonaReportada(idpropietario);
                Boolean reportado = false;
                if (listareportado.Count > 0)
                {
                    foreach (var item in listareportado)
                    {
                        if (item.Estado == 1)
                        {
                            reportado = true;
                            break;
                        }
                    }
                    if (reportado == true)
                    {
                        mostrar_mensaje_reportado(39);
                        limpiarcampospropietario();
                    }
                    else
                    {
                       
                        MessageBoxResult result1 = MessageBox.Show("Este Propietario no existe en la base de datos desea crealo?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result1 == MessageBoxResult.Yes)
                        {
                            string id = txtNumdocumentopropi.Text;
                            limpiarcampospropietario();
                            txtNumdocumentopropi.Text = id;

                        }
                    }

                }
                else
                {
                   
                    MessageBoxResult result1 = MessageBox.Show("Este Propietario no existe en la base de datos desea crealo?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result1 == MessageBoxResult.Yes)
                    {
                        string id = txtNumdocumentopropi.Text;
                        limpiarcampospropietario();
                        txtNumdocumentopropi.Text = id;

                    }
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
        private void limpiarcampospropietario()
        {
            cbbtipodocumentopropi.SelectedIndex = 1;
            txtNumdocumentopropi.Text = "";
            txtnombrespropi.Text = "";
            txtapellidopropi1.Text = "";
            txtapellidopropi2.Text = "";

            txtdireccionpropi.Text = "";
            txttelefonopropi.Text = "";

            txttelefonopropi.Text = "";

            cbbdeparpropi.SelectedIndex = 1;
            cargarCiudadespropietario(Convert.ToInt32(cbbdeparpropi.SelectedValue));
            cbbciudadpropi.SelectedIndex = 1;


            rdblistaNoapruebapropi.IsChecked = true;

            dtfechaestudiopropi.SelectedDate = null;

            txtNitVerificacionPropi.Text = "";

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
        private void GuardarDatosBeneficiario()
        {
            Boolean Existebeneficiario;
            var lista = PropiBeneBll.GetDatosBeneficiarioXID(txtNumdocumentobenefi.Text.Trim());
            if (lista.Count > 0)
            {
                Existebeneficiario = true;
            }

            else
            {
                Existebeneficiario = false;
            }

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
            Nullable<DateTime> FechaEstudio;
            Nullable<Boolean> ListaRiesgo;
            Nullable<short> PagoCierre;
            Boolean respuesta = false;
            if (dtfechaestudiobenefi.SelectedDate != null)
            {
                FechaEstudio = Convert.ToDateTime((dtfechaestudiobenefi.SelectedDate));
            }
            else
                FechaEstudio = null;
            if (rdblistaNoapruebabenefi.IsChecked == true)
                ListaRiesgo = false;
            else
            {
                if (rdblistaapruebabenefi.IsChecked == true)
                {
                    ListaRiesgo = true;
                }
                else
                    ListaRiesgo = null;
            }

            PagoCierre = Convert.ToInt16(cbbagenciabenefi.SelectedValue);
            Nullable<short> DigitVerif;
            if (txtNitVerificacionBenefi.Text.Trim() == "")
            {
                DigitVerif = null;
            }
            else
            {
                DigitVerif =Convert.ToInt16 ( txtNitVerificacionBenefi.Text.Trim());
            }

            if (Existebeneficiario == true)
            {
                MessageBox.Show(" Este Beneficiario ya existe en la base de datos,usted no tiene permitido modificarlo ");
                txtNumdocumentobenefi.Text = "";
            }
            else
            {


                respuesta = PropiBeneBll.GuardarDatosBeneficiario(txtNumdocumentobenefi.Text, txtnombresbenefi.Text.TrimStart(),
                                   txtapellidobenefi1.Text, txtapellidobenefi2.Text, cbbtipodocumentobenefi.SelectedValue.ToString(),
                                   cbbciudadbenefi.SelectedValue.ToString(), txtcelbenefi.Text, txttelefonobenefi.Text, txtdireccionbenefi.Text,
                                   ListaRiesgo, FechaEstudio, PagoCierre, usuario, "", txtobservacionestenedor.Text, DigitVerif, txtIndicativobenefi.Text.Trim(),txtemail.Text.Trim ());
                PropiBeneBll.GuardaAuditoriaCambioBeneficiario_Propietario(usuario, txtNumdocumentopropi.Text, "", txtNumdocumentobenefi.Text.Trim(), "guardó beneficiario   nuevo","");
                mostrarmensaje(respuesta);

                if (respuesta == true)
                {
                    limpiarcamposbeneficiario();
                }
            }
        }
        private void GuardarDatosPropietario()
        {
            Boolean Existepropietario;
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
            Nullable<DateTime> FechaEstudio;
            Nullable<Boolean> ListaRiesgo;
            Boolean respuesta = false;
            if (dtfechaestudiopropi.SelectedDate != null)
            {
                FechaEstudio = Convert.ToDateTime((dtfechaestudiopropi.SelectedDate));
            }
            else
                FechaEstudio = null;

            if (rdblistaNoapruebapropi.IsChecked == true)
                ListaRiesgo = false;
            else
            {
                if (rdblistaapruebapropi.IsChecked == true)
                {
                    ListaRiesgo = true;
                }
                else
                    ListaRiesgo = null;
            }
            var lista = PropiBeneBll.GetDatosPropietarioXID(txtNumdocumentopropi.Text);
            if (lista.Count > 0)
            {
                Existepropietario = true;
            }

            else
            {
                Existepropietario = false;
            }

            if (Existepropietario == true)
            {
                MessageBox.Show(" Este Propietario ya existe en la base de datos,usted no tiene permitido modificarlo ");
                txtNumdocumentopropi.Text = "";

            }
            else
            {
                Nullable<short> DigitVerif;
                if (txtNitVerificacionPropi.Text.Trim() == "")
                {
                    DigitVerif = null;
                }
                else
                {
                    DigitVerif = Convert.ToInt16(txtNitVerificacionPropi.Text.Trim());
                }
                respuesta = PropiBeneBll.GuardarDatosPropietario(Convert.ToDecimal(txtNumdocumentopropi.Text.Trim()),
               txtnombrespropi.Text.TrimStart(), txtapellidopropi1.Text.Trim(), txtapellidopropi2.Text.Trim(), cbbtipodocumentopropi.SelectedValue.ToString(),
               cbbciudadpropi.SelectedValue.ToString(), txttelefonopropi.Text, txtdireccionpropi.Text, ListaRiesgo, FechaEstudio, "", usuario, DigitVerif);
                PropiBeneBll.GuardaAuditoriaCambioBeneficiario_Propietario(usuario, txtNumdocumentopropi.Text, "", txtNumdocumentobenefi.Text.Trim(), "guardó propietario nuevo", "");
                //PropiBeneBll.GuardaAuditoriaCambioBeneficiario_Propietario(usuario, txtNumdocumentopropi.Text, "", txtNumdocumentopropi.Text.Trim(), "guardó propietario nuevo", "");

                

                mostrarmensaje(respuesta);
                if (respuesta == true)
                {
                    limpiarcampospropietario ();
                }

            }

        }
        public int CalculaNit(string Numero)
        {
            Numero = LimpiarSeparadores(Numero);
            Decimal valor = Convert.ToDecimal(Numero);
            string format = "000000000000000";
            string mDocumento = valor.ToString(format);
            //string mDocumento = String.Format(Numero, "000000000000000");
            int mDigito = CalcularDigito(mDocumento);
            return mDigito;
        }

        private int CalcularDigito(string mDocumento)
        {
            int mDigito;
            long Total;
            int Residuo;
            Total = Convert.ToInt32(mDocumento.Substring(0, 1)) * 71;
            Total += Convert.ToInt32(mDocumento.Substring(1, 1)) * 67;
            Total += Convert.ToInt32(mDocumento.Substring(2, 1)) * 59;
            Total += Convert.ToInt32(mDocumento.Substring(3, 1)) * 53;
            Total += Convert.ToInt32(mDocumento.Substring(4, 1)) * 47;
            Total += Convert.ToInt32(mDocumento.Substring(5, 1)) * 43;
            Total += Convert.ToInt32(mDocumento.Substring(6, 1)) * 41;
            Total += Convert.ToInt32(mDocumento.Substring(7, 1)) * 37;
            Total += Convert.ToInt32(mDocumento.Substring(8, 1)) * 29;
            Total += Convert.ToInt32(mDocumento.Substring(9, 1)) * 23;
            Total += Convert.ToInt32(mDocumento.Substring(10, 1)) * 19;
            Total += Convert.ToInt32(mDocumento.Substring(11, 1)) * 17;
            Total += Convert.ToInt32(mDocumento.Substring(12, 1)) * 13;
            Total += Convert.ToInt32(mDocumento.Substring(13, 1)) * 7;
            Total += Convert.ToInt32(mDocumento.Substring(14, 1)) * 3;
            Residuo = Convert.ToInt32(Total % 11);
            switch (Residuo)
            {
                case 0:
                    mDigito = 0;
                    break;
                case 1:
                    mDigito = 1;
                    break;
                default:
                    mDigito = 11 - Residuo;
                    break;
            }

            return mDigito;

        }

        private string LimpiarSeparadores(string Numero)
        {
            Numero = Numero.Replace(",", "");
            Numero = Numero.Replace(".", "");
            Numero = Numero.Replace("-", "");
            Numero = Numero.Replace(" ", "");
            return Numero;
        }

          
        public void RecibirPadre(MainWindow padre)
        {
            main = padre;
        }
        #endregion

        #region Eventos#

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
        private void txtNumdocumentopropi_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (validarCaracteresespeciales(e, txt) == true)
            {


                if (e.Key == Key.Enter || e.Key == Key.Tab)
                {
                    ConsultaDatosPropietario(txtNumdocumentopropi.Text);
                    //Finalizado la consulta
                    //Movemos el foco al siguiente
                    //TextBox
                    txt.MoveFocus(new TraversalRequest(
                        FocusNavigationDirection.Next));
                }
            }

        }
        private void txtNumdocumentobenefi_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            TextBox txt = (TextBox)sender;
            if (validarCaracteresespeciales(e, txt) == true)
            {
                if (e.Key == Key.Enter || e.Key == Key.Tab)
                {
                    ConsultaDatosBeneficiario(txtNumdocumentobenefi.Text);
                    //Finalizado la consulta
                    //Movemos el foco al siguiente
                    //TextBox
                    txt.MoveFocus(new TraversalRequest(
                        FocusNavigationDirection.Next));
                }
            }
        }
        private void cbbdeparbenefi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = Convert.ToInt32(cbbdeparbenefi.SelectedValue);
            cargarCiudadesbeneficiario(id);
        }
        private void cbbdeparpropi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = Convert.ToInt32(cbbdeparpropi.SelectedValue);
            cargarCiudadespropietario(id);
        }
        private void cbbagenciabenefi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbagenciabenefi.SelectedValue != null)
            {
                txtagenciabenefi.Text = cbbagenciabenefi.SelectedValue.ToString();
            }

        }
        /// <summary>
        /// la fecha no puede ser superior  a la fecha actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtfechaestudiobenefi_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtfechaestudiobenefi.SelectedDate != null)
            {
                DateTime fechafutura;
                DateTime fechaaux = (DateTime)dtfechaestudiobenefi.SelectedDate;
                fechafutura = DateTime.Now;
                fechafutura = fechafutura.AddYears(1);

                if (dtfechaestudiobenefi.SelectedDate > FechaActual.SelectedDate)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(15,"");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfechaestudiobenefi.SelectedDate = null;
                    dtfechaestudiobenefi.DisplayDateEnd = DateTime.Now;
                }

            }
        }
        /// <summary>
        /// la fecha no puede ser superior  a la fecha actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtfechaestudiopropi_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtfechaestudiopropi.SelectedDate != null)
            {
                DateTime fechafutura;
                DateTime fechaaux = (DateTime)dtfechaestudiopropi.SelectedDate;
                fechafutura = DateTime.Now;
                fechafutura = fechafutura.AddYears(1);

                if (dtfechaestudiopropi.SelectedDate > FechaActual.SelectedDate)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(15,"");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfechaestudiopropi.SelectedDate = null;
                    dtfechaestudiopropi.DisplayDateEnd = DateTime.Now;
                }

            }
        }
        private void dtfechaestudiobenefi_CalendarOpened(object sender, RoutedEventArgs e)
        {
            dtfechaestudiobenefi.SelectedDate = null;
            dtfechaestudiobenefi.DisplayDateStart = DateTime.Now;
        }

        private void dtfechaestudiopropi_CalendarOpened(object sender, RoutedEventArgs e)
        {
            dtfechaestudiopropi.SelectedDate = null;
            dtfechaestudiopropi.DisplayDateStart = DateTime.Now;
        }

        #endregion










        internal void GuardarInformacion()
        {
            if (ValidarDatosPropietario() == false)
            {                
                 
                    GuardarDatosPropietario();                
            }
            if (ValidarDatosBeneficiario() == false)
            {
                GuardarDatosBeneficiario();
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

        private void cbbtipodocumentopropi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbtipodocumentopropi.SelectedValue != null)
            {
                if (cbbtipodocumentopropi.SelectedValue.ToString() == "N")
                {
                    txtNitVerificacionPropi.Visibility = Visibility.Visible;
                    txtapellidopropi1.Visibility = Visibility.Collapsed;
                    txtapellidopropi2.Visibility = Visibility.Collapsed;
                }
                else
                {
                    txtNitVerificacionPropi.Visibility = Visibility.Collapsed;
                    txtapellidopropi1.Visibility = Visibility.Visible;
                    txtapellidopropi2.Visibility = Visibility.Visible;
                }
            }
        }

        private void cbbtipodocumentobenefi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbtipodocumentobenefi.SelectedValue != null)
            {
                if (cbbtipodocumentobenefi.SelectedValue.ToString() == "N")
                {
                    txtNitVerificacionBenefi.Visibility = Visibility.Visible;
                    txtapellidobenefi1.Visibility = Visibility.Collapsed;
                    txtapellidobenefi2.Visibility = Visibility.Collapsed;
                }
                else
                {
                    txtNitVerificacionBenefi.Visibility = Visibility.Visible;
                    txtapellidobenefi1.Visibility = Visibility.Visible;
                    txtapellidobenefi2.Visibility = Visibility.Visible;
                }
            }
        }

        private void chkobservacionestenedor_Checked(object sender, RoutedEventArgs e)
        {
            txtobservacionestenedor.IsEnabled = true;
        }

        private void chkobservacionestenedor_Unchecked(object sender, RoutedEventArgs e)
        {
            txtobservacionestenedor.IsEnabled = false;
        }


     
        
    }
}
