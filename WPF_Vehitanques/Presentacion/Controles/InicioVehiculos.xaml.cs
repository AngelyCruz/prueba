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
using WPF_Vehitanques.Presentacion.Controles;
using WPF_Vehitanques.Presentacion.Ventanas;
using WPF_Vehitanques.BLL;
using WPF_Vehitanques.DS;
using WPF_Vehitanques.Modelo;
using WPF_Vehitanques.Utilidades;
using System.Text.RegularExpressions;
using System.Data;
using System.Globalization;
using WPF_Vehitanques.Modelo;

namespace WPF_Vehitanques.Presentacion.Controles
{
    /// <summary>
    /// Lógica de interacción para InicioVehiculos.xaml
    /// </summary>
    public partial class InicioVehiculos : UserControl
    {
        #region Declaraciones

        Modelo.ComercialPEntities contextVehit = new Modelo.ComercialPEntities();
        bool invalid = false;
        private string strplaca = "";
        private String RutaDocu;
        private DatePicker FechaActual = new DatePicker();
        private Nullable<Boolean> actualizarvehi = null;
        MainWindow main;
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj = new VentanaMensaje();
        EmpresaSatelitalBLL empsatBll = new EmpresaSatelitalBLL();
        VehiculoBLL vehibll = new VehiculoBLL();
        ColorBLL ColBll = new ColorBLL();
        MarcasBLL MarBll = new MarcasBLL();
        TanqueBLL TanBll = new TanqueBLL();
        MotorBLL MotBll = new MotorBLL();
        AfiliacionBLL AfilBll = new AfiliacionBLL();
        CarroceriaBLL CarBLL = new CarroceriaBLL();
        EstadoBLL establl = new EstadoBLL();
        LineaBLL linBll = new LineaBLL();
        AseguradoraBLL AseBll = new AseguradoraBLL();
        UnidadMedidaBLL uniBll = new UnidadMedidaBLL();
        LLantasBLL LlanBll = new LLantasBLL();
        TipoDocumentoBLL TiDocBll = new TipoDocumentoBLL();
        CiudadBLL CiuBll = new CiudadBLL();
        LicenciaBLL licenBll = new LicenciaBLL();
        BancoBLL BanBll = new BancoBLL();
        AgenciaBLL AgenBll = new AgenciaBLL();
        DepartamentoBLL DeparBll = new DepartamentoBLL();
        RemolqueBLL RemolBll = new RemolqueBLL();
        ConductorBLL ConducBll = new ConductorBLL();
        InicioConsulta Iniconsul;
        CuentaBLL CuenBll = new CuentaBLL();
        DocumentosBLL DocuBll = new DocumentosBLL();
        ContratoBLL contratobll = new ContratoBLL();
        AdmDocumentos Ventanadoc;
        VetoBLL vetobll = new VetoBLL();
        CorreoBLL Coreobll = new CorreoBLL();
        PolizaBLL polizabll = new PolizaBLL();
        DigitoVerificacion _DigitoVerificacion = new DigitoVerificacion();
        private Boolean nuevo = false;

        PropietarioBeneficiarioBLL PropiBeneBll = new PropietarioBeneficiarioBLL();
        DSMensajes.DSMensajesHvVehiculoDataTable DSMens = new DSMensajes.DSMensajesHvVehiculoDataTable();
        Numeric numeric = new Numeric();
        AprobarContratos VentanaAprobarContratos = new AprobarContratos();
        AdmPolizaHC ventAdmpolizaHc;
        private Boolean cambiarcontrato;
        private Boolean cambiarestado = false;
        private Boolean udppropietario = false;
        private Boolean udpbeneficiario = false;
        private Boolean udpcolor = false;
        private Boolean modificavehiculo_remolque = true;
        private Boolean modificabenefi_propie = true;
        private Boolean NoCrearConductores = false;
        private Boolean aprobarPolRC = false;
        private Boolean boolhabilitarbotonesBene = false;
        private Boolean autorizaoperacioninternacional = false;
        private Boolean autorizamoftipoAfilremolque = false;
        private Boolean boolAutorizaCambiarRiesgo = false;
        private Boolean boolBotonEstudioSeg = false;
    
       

        //seguridad_consultarMenuPermisos2_Result listaprivilegios= new seguridad_consultarMenuPermisos2_Result();
        private Boolean Existepropietario, ExisteConductor, Existebeneficiario, existeremolque, habilitarbotoncontrato,
            habilitarcontrato, boolmodificatodos, boolmodificapropios, boolmodificaterceros, boolmodificaipoafiliacion,
             boolmodificaConductorestodos, boolmodificaConductorespropios, boolmodificaConductoresterceros;
        ComercialPEntities contex = new ComercialPEntities();

        #endregion

        #region load

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetListausuario(SecurityVariables.PPerfilUsuario);
            if (IsLoaded)
            {
                cargarColores();
                CargarUnidadMedida();
                cargarMarcaVehiculo();
                cargarMarcaRemolque();
                CargarclaseTanque();
                CargarTipoMotor();
                CargarTipoVehiculos();
                CargarTipoAfiliacion();
                CargarTipoCarroceria();
                cargarEstadoPlaca();
                CargarAseguradoras();
                CargarGrupoLLantas();
                CargarTipoDocumentos();
                CargarCategorialicencia();
                cargarBancos();
                cargarAgencias();
                cargarDepartamentos();
                cargarDepartamentosBenefi();
                cargarDepartamentosPropi();
                CargarEjesVehiculo(0);
                cargarTipoCuenta();
                cargarEstadoPlacaAprobacion();
                cargarTipoCuentacompaniasatelital();
                CargarNivelRiesgoCarga();
                habilitarbotonesvehiculo(actualizarvehi);
                Habilitarbotonesconductor(actualizarvehi);
                Habilitarbotonespropietariobeneficiario(actualizarvehi);
                habilitarBotonesLeasing(actualizarvehi);
                Habilitar_BotonesSeguridad(boolBotonEstudioSeg);
                //  habilitarChekOperacionEcuador(SecurityVariables.PStrUsuario);               
                cargarinfo();
                CargarEstadoplacaVehiculo();
                validarpermisosoperacioninternacional();

            }
        }

        public InicioVehiculos()
        {
            InitializeComponent();
            FechaActual.SelectedDate = DateTime.Now.Date;
        }

        #endregion

        #region métodos

        private void validarpermisosoperacioninternacional()
        {
            if (autorizaoperacioninternacional == true)
            {
                if (txtplacaremolque.Text.Trim() != "")
                {
                    var lista = RemolBll.GetREmolqueinternacional(txtplacaremolque.Text.Trim());

                    if (lista.Count > 0)
                    {
                        Chk_Grabar_operEcuador.IsChecked = true;
                        Chk_Grabar_operEcuador.IsEnabled = true;
                        MessageBox.Show("Recuerde que este vehículo está registrado para carga internacional y solicitará menos documentos, a menos que no lo seleccione para dicho transporte");
                    }
                    else
                    {
                        Chk_Grabar_operEcuador.IsChecked = false;
                        Chk_Grabar_operEcuador.IsEnabled = false;
                        MessageBox.Show("Este vehículo NO está registrado para carga internacional y solicitará todos los documentos");
                    }
                }
                else
                {
                    Chk_Grabar_operEcuador.IsChecked = false;
                    Chk_Grabar_operEcuador.IsEnabled = false;
                }
            }
            else
            {
                Chk_Grabar_operEcuador.IsChecked = false;
                Chk_Grabar_operEcuador.IsEnabled = false;
            }
        }

        private Boolean valdiarDAtospolizaRC()
        {
            Boolean respuesta = false;
            if (cbbcomRCE.SelectedIndex < 0)
            {
                MessageBox.Show("debe seleccionar una compañía proveedora de pólizas RC");
                return true;
            }
            if (txtnumpolizaRC.Text.Trim() == "")
            {

                MessageBox.Show("debe ingresar un número de póliza RC");
                return true;
            }
            if (dtfechaRC.SelectedDate == null)
            {
                MessageBox.Show("debe seleccionar una fecha de póliza RC");
                return true;
            }
            return respuesta;
        }

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

        private void mostrarmensajeRemolVehiculo(Boolean respuesta, string vehi_remol)
        {
            if (respuesta == true)
            {
                Mensabll = new MensajeBLL();
                Mensabll.tipomensajes(41, "");
                Venmsj = new VentanaMensaje();
                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                Venmsj.cargarmensaje(Mensabll.titulos, vehi_remol + Mensabll.mensajes);
                Venmsj.ShowDialog();
            }
            else
            {
                Mensabll = new MensajeBLL();
                Mensabll.tipomensajes(23, "");
                Venmsj = new VentanaMensaje();
                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                Venmsj.ShowDialog();
            }
        }

        private void mostrarmensaje(Boolean respuesta)
        {
            if (respuesta == true)
            {
                Mensabll = new MensajeBLL();
                Mensabll.tipomensajes(12, "");
                Venmsj = new VentanaMensaje();
                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                Venmsj.ShowDialog();
            }
            else
            {
                Mensabll = new MensajeBLL();
                Mensabll.tipomensajes(23, "");
                Venmsj = new VentanaMensaje();
                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                Venmsj.ShowDialog();
            }
        }

        #endregion

        #region DatosPrivados del formulario

        public void RecibirPadre(MainWindow padre)
        {
            if (main == null)
                main = padre;
        }

        public void RecibeRedirect(InicioConsulta inicon)
        {
            Iniconsul = inicon;
        }

        public void RecibeOrdenPropietario(Boolean orden)
        {
            Habilitarcampospropietario(orden);
        }

        public void RecibeOrdenBeneficiario(Boolean orden)
        {
            Habilitarcamposbeneficiario(orden);
        }

        public InicioConsulta retornoRedirect()
        {
            return Iniconsul;
        }

        public void GetPlaca(string placa)
        {
            strplaca = placa;
            actualizarvehi = true;
        }

        //public Boolean  Actualizavehiculogtset 
        //    {
        //       get 
        //       { 
        //          return actualizarvehi!=null ; 
        //       }
        //       set 
        //       {
        //          actualizarvehi = value; 
        //       }
        //    }

        #endregion

        #region Validaciones botones documentos

        //private void habilitarChekOperacionEcuador(string strUsuario) 
        //{
        //    if (strUsuario.Trim().ToUpper() == "FFRANCOM")
        //    {
        //        Chk_Grabar.IsEnabled  = true;
        //    }
        //    else
        //    {
        //        Chk_Grabar.IsEnabled = false;
        //    }
        //}

        private void habilitarBotonesLeasing(Boolean? habilitar)
        {
            if (habilitar != true)
            {
                btn_contratoleasing.IsEnabled = false;
            }
            else
            {
                btn_contratoleasing.IsEnabled = true;
            }
        }

        public void Habilitarbotonespropietariobeneficiario(Boolean? habilitar)
        {
            if (habilitar != true)
            {
                btn_contratoBeneficiario.IsEnabled = false;
                btn_docbancariosben.IsEnabled = false;
            }
            else
            {
                if (habilitar == true)
                {
                    btn_contratoBeneficiario.IsEnabled = true;
                    btn_docbancariosben.IsEnabled = true;
                }
                else
                {
                    btn_contratoBeneficiario.IsEnabled = false;
                    btn_docbancariosben.IsEnabled = false;
                }
            }
        }

        public void Habilitarbotonesconductor(Boolean? habilitar)
        {
            if (habilitar != true)
            {
                btn_cedulaconductor.IsEnabled = false;
                btn_licenciaconductor.IsEnabled = false;
                btn_pagoSSocial.IsEnabled = false;
                btn_FotoConductor.IsEnabled = false;
                btn_AutorizaInfo.IsEnabled = false;
                btn_Eps.IsEnabled = false;
                btn_ARL.IsEnabled = false;
                btn_docbancarioscond.IsEnabled = false;
            }
            else
            {
                if (habilitar == true)
                {
                    btn_cedulaconductor.IsEnabled = true;
                    btn_licenciaconductor.IsEnabled = true;
                    btn_pagoSSocial.IsEnabled = true;
                    btn_FotoConductor.IsEnabled = true;
                    btn_AutorizaInfo.IsEnabled = true;
                    btn_Eps.IsEnabled = true;
                    btn_ARL.IsEnabled = true;
                    btn_docbancarioscond.IsEnabled = true;
                }
            }
        }

        public void habilitarbotonesvehiculo(Boolean? habilitar)
        {
            if (habilitar != true)
            {
                btn_tecnicomecanica.IsEnabled = false;
                btn_tpropiedadvehi.IsEnabled = false;
                btn_soat.IsEnabled = false;
                btn_fotovehiculo.IsEnabled = false;
                btn_contratocoltan.IsEnabled = false;
                btn_polizaRC.IsEnabled = false;
                btn_hidrostatica.IsEnabled = false;
                btn_TpropieRemolque.IsEnabled = false;
                btn_fotoremolque.IsEnabled = false;
                btn_aforotanque.IsEnabled = false;
                btn_aforotanqueSencillo.IsEnabled = false;
                btn_hidrostaticaSencillo.IsEnabled = false;
                btn_PolizaHc.IsEnabled = false;
                btn_kingpin.IsEnabled = false;
            }
            else
            {
                if (habilitar == true)
                {
                    btn_tecnicomecanica.IsEnabled = true;
                    btn_tpropiedadvehi.IsEnabled = true;
                    btn_soat.IsEnabled = true;
                    btn_fotovehiculo.IsEnabled = true;
                    btn_contratocoltan.IsEnabled = true;
                    btn_polizaRC.IsEnabled = true;
                    btn_hidrostatica.IsEnabled = true;
                    btn_TpropieRemolque.IsEnabled = true;
                    btn_fotoremolque.IsEnabled = true;
                    btn_aforotanque.IsEnabled = true;
                    btn_aforotanqueSencillo.IsEnabled = true;
                    btn_hidrostaticaSencillo.IsEnabled = true;
                    btn_PolizaHc.IsEnabled = true;
                    btn_kingpin.IsEnabled = true;
                }
            }
        }

        #endregion

        #region Validacion campos

        private void HabilitarAprovacionPolRC(Boolean activar)
        {
            btn_Aprobar.IsEnabled = activar;
            btn_Noaprobar.IsEnabled = activar;
        }

        private void HabilitarInfoBancaria(Boolean activar)
        {
            if (activar == true)
            {
                cbbbanco.IsEnabled = true;
                cbbbanco.SelectedIndex = 1;
                txtnitcuenta.IsEnabled = true;
                cbbtipocuenta.IsEnabled = true;
                txtnumcuenta.IsEnabled = true;
                txtidtarjeta.IsEnabled = true;
            }
            else
            {
                cbbbanco.IsEnabled = false;
                txtnitcuenta.IsEnabled = false;
                cbbtipocuenta.IsEnabled = false;
                txtnumcuenta.IsEnabled = false;
                txtidtarjeta.IsEnabled = false;
            }
        }

        public void validarDatospro_Beneficiario()
        {
            //Boolean respuesta = false;
            if (txtNumdocumentobenefi.Text.Trim() != "")
            {
                if (ValidarDatosBeneficiario() == false)
                {
                    GuardarDatosBeneficiario();
                    //GuardarDatosPropietario();
                }
                else
                {

                }
            }
            if (txtNumdocumentopropi.Text.Trim() != "")
            {
                if (ValidarDatosPropietario() == false)
                {
                    //GuardarDatosBeneficiario();
                    GuardarDatosPropietario();
                }
                else
                {

                }
            }
        }

        public Boolean ValidarDatosPropietarioBeneficiario()
        {
            Boolean respuesta = false;
            if (txtNumdocumentopropi.Text.Trim() != "")
            {
                if (numeric.IsNumeric(txtNumdocumentopropi.Text.Trim()) == true)
                {
                    lblerror_Numdocumentopropi.Content = "";
                    if (txtNumdocumentobenefi.Text != "")
                    {
                        if (txtNumdocumentobenefi.Text != txtNumdocumentopropi.Text)
                        {
                            btn_contratoBeneficiario.Background = new SolidColorBrush(Colors.Red);
                            btn_contratoBeneficiario.Focus();
                            //btn_contratoBeneficiario.TabIndex = TabIndexProperty.AddOwner();
                            //      MoveFocus(new TraversalRequest(
                            //FocusNavigationDirection.Down));
                        }
                    }
                    else
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
                }
                else
                {
                    lblerror_Numdocumentopropi.Content = "***";
                    respuesta = true;
                }
            }
            else
            {
                lblerror_Numdocumentopropi.Content = "***";
                respuesta = true;
            }
            if (txtNumdocumentobenefi.Text.Trim() != "")
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
                            lblerror_celbenefi.Content = "";
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
                            lblerror_celbenefi.Content = "";
                        }
                    }
                }
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
            if (cbbtipodocumentobenefi.SelectedValue.ToString() != "N")
            {
                if (txtapellidobenefi1.Text != "")
                {
                    lblerror_apellidobenefi1.Content = "";
                }
                else
                {
                    lblerror_apellidobenefi1.Content = "***";
                    respuesta = true;
                }
                if (txtapellidobenefi2.Text != "")
                {
                    lblerror_apellidobenefi2.Content = "";
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
                    int digito = _DigitoVerificacion.ValidarDigitoVerificacion(txtNumdocumentobenefi.Text.Trim());
                    if (digito != Convert.ToInt32(txtNitVerificacionBenefi.Text.Trim()))
                    {
                        MessageBox.Show("El Dígito de Verificación del Beneficiario no corresponde al Nit digitado, por favor verificar.");
                        respuesta = true;
                    }
                }
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
                    int digito = _DigitoVerificacion.ValidarDigitoVerificacion(txtNumdocumentopropi.Text.Trim());
                    if (digito != Convert.ToInt32(txtNitVerificacionPropi.Text.Trim()))
                    {
                        MessageBox.Show("El Dígito de Verificación del propietario no corresponde al Nit digitado, por favor verificar.");
                        respuesta = true;
                    }
                }
            }

            return respuesta;
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
                    int digito = _DigitoVerificacion.ValidarDigitoVerificacion(txtNumdocumentobenefi.Text.Trim());
                    if (digito != Convert.ToInt32(txtNitVerificacionBenefi.Text.Trim()))
                    {
                        MessageBox.Show("El Dígito de Verificación del Beneficiario no corresponde al Nit digitado, por favor verificar.");
                        respuesta = true;
                    }
                }
            }

            if (txtNumdocumentobenefi.Text.Trim() != "" && txtNumdocumentopropi.Text.Trim() != "")
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
                        int digito = _DigitoVerificacion.ValidarDigitoVerificacion(txtNumdocumentopropi.Text.Trim());
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

            if (txtdireccionpropi.Text.Trim() == "")
            {
                lblerror_direccionpropi.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_direccionpropi.Content = "";
            }
            if (txttelefonopropi.Text.Trim() != "")
            {
                if (txttelefonopropi.Text.Trim().Length == 7 || txttelefonopropi.Text.Trim().Length == 10)
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

        private Boolean ValidarCAmposConductor()
        {
            Boolean respuesta = false;
            if (txtexperiencia.Text == "")
            {
                lblerror_experiencia.Content = "***";
                respuesta = true;
            }
            else
            {
                if (numeric.IsNumeric(txtexperiencia.Text) == true)
                    lblerror_experiencia.Content = "";
                else
                {
                    lblerror_experiencia.Content = "***";
                    respuesta = true;
                }
            }
            if (txtNumdocumento.Text == "")
            {
                lblerror_documenConductor.Content = "***";
                respuesta = true;
            }
            else
            {
                //lblerror_documenConductor.Content = "";
                if (numeric.IsNumeric(txtNumdocumento.Text) == true)
                {
                    lblerror_documenConductor.Content = "";
                }
                else
                {
                    lblerror_documenConductor.Content = "***";
                    respuesta = true;
                }
                //  respuesta = false;
            }
            if (txtapellido1.Text == "")
            {
                lblerror_apellido1Cond.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_apellido1Cond.Content = "";
                // respuesta = false;
            }
            if (txtapellido2.Text == "")
            {
                lblerror_apellido2Cond.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_apellido2Cond.Content = "";
                // respuesta = false;
            }
            if (dtfechanacimieto.SelectedDate == null)
            {
                lblerror_fechanamcond.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_fechanamcond.Content = "";
                //respuesta = false;
            }
            if (txtdireccioncasa.Text.Trim() == "")
            {
                lblerror_direccioncasa.Content = "***";
                respuesta = true;
            }
            else { lblerror_direccioncasa.Content = ""; }

            if (txtcelular.Text.Trim() == "" && txttelefonocasa.Text.Trim() == "")
            {
                lblerror_celular.Content = "***";
                MessageBox.Show("Debe digitar al menos un número de teléfono o de celular del conductor.");
                respuesta = true;
            }
            else
            {
                if (txtcelular.Text.Trim() != "")
                {
                    if (txtcelular.Text.Trim().Length != 10)
                    {
                        lblerror_celular.Content = "***";
                        MessageBox.Show("El número de celular del conductor no tiene la longitud adecuada.");
                        respuesta = true;
                    }
                    else
                    {
                        if (txttelefonocasa.Text.Trim() != "")
                        {
                            if (txttelefonocasa.Text.Trim().Length != 7)
                            {
                                MessageBox.Show("El número de teléfono del conductor no tiene la longitud adecuada.");
                                respuesta = true;
                            }
                            else
                            {
                                if (txtIndicativo.Text.Trim().Length != 3)
                                {
                                    MessageBox.Show("El número Indicativo del teléfono del conductor no tiene la longitud adecuada.");
                                    respuesta = true;
                                }
                                else
                                {
                                    lblerror_celular.Content = "";
                                }
                            }
                        }
                        else
                        {
                            txtIndicativo.Text = "";
                            lblerror_celular.Content = "";
                        }
                    }
                }
                else
                {
                    if (txttelefonocasa.Text.Trim().Length != 7)
                    {
                        MessageBox.Show("El número de teléfono del conductor no tiene la longitud adecuada.");
                        respuesta = true;
                    }
                    else
                    {
                        if (txtIndicativo.Text.Trim().Length != 3)
                        {
                            MessageBox.Show("El número Indicativo del teléfono del conductor no tiene la longitud adecuada.");
                            respuesta = true;
                        }
                        else
                        {
                            lblerror_celular.Content = "";
                        }
                    }
                }
            }
            if (txtnumlicencia.Text == "")
            {
                lblerror_numlicencia.Content = "***";
                respuesta = true;
            }
            else
            {
                if (numeric.IsNumeric(txtnumlicencia.Text) == true)
                    lblerror_numlicencia.Content = "";
                else
                {
                    lblerror_numlicencia.Content = "***";
                    respuesta = true;
                }
                //lblerror_numlicencia.Content = "";
                //  respuesta = false;
            }
            if (dtfechanacimieto.SelectedDate != null)
            {
                int anioactualnac, anionaci, aniodiferencianaci;
                DateTime fechanaci = (DateTime)dtfechanacimieto.SelectedDate;
                DateTime fechaactual = DateTime.Now;

                anioactualnac = DateTime.Now.Year;
                anionaci = fechanaci.Year;

                aniodiferencianaci = anioactualnac - anionaci;
                if (aniodiferencianaci < 18)
                {
                    respuesta = true;
                    MessageBox.Show("la fecha de nacimiento del conductor, no corresponde a los parámetros del MINISTERIO DE TRANSPORTE,La edad Debe ser superior a 18 años");
                }
            }
            else
            {
                respuesta = true;
                MessageBox.Show("la fecha de nacimiento del conductor, no corresponde a los parámetros del MINISTERIO DE TRANSPORTE,La edad Debe ser superior a 18 años");
            }
            var listapla = vehibll.ConsultaExistenciaVehiculo(txtplaca.Text.Trim());

            if (Convert.ToInt32(listapla[0].TipoAfiliacion) == 3)
            {
                switch (Convert.ToInt32(cbbcateglicencia.SelectedValue))
                {
                    case 7:
                    case 8:
                    case 9:
                        if (dtfechalicencia.SelectedDate == null)
                        {
                            lblerror_fechalicencia.Content = "***";
                            respuesta = true;
                        }
                        else
                        {
                            int anioactual, aniolicencia, aniodiferencia;
                            DateTime fechalince = (DateTime)dtfechalicencia.SelectedDate;
                            DateTime fechaactu = DateTime.Now;
                            if (dtfechalicencia.SelectedDate <= FechaActual.SelectedDate)
                            {
                                lblerror_fechalicencia.Content = "***";
                                MessageBox.Show("la fecha de vencimiento de la licencia no corresponde a los parámetros del MINISTERIO DE TRANSPORTE");
                                respuesta = true;
                            }
                            else
                            {
                                anioactual = DateTime.Now.Year;
                                aniolicencia = fechalince.Year;
                                aniodiferencia = aniolicencia - anioactual;
                                if (aniodiferencia < 0 || aniodiferencia > 3)
                                {
                                    lblerror_fechalicencia.Content = "***";
                                    MessageBox.Show("la fecha de vencimiento de la licencia no corresponde a los parámetros del MINISTERIO DE TRANSPORTE");
                                    respuesta = true;
                                }
                                else
                                {
                                    lblerror_fechalicencia.Content = "";
                                }
                            }
                            //respuesta = false;
                        }
                        break;
                    default:
                        if (dtfechalicencia.SelectedDate == null)
                        {
                            lblerror_fechalicencia.Content = "***";
                            respuesta = true;
                        }
                        else
                        {
                            int anioactual, aniolicencia, aniodiferencia;
                            DateTime fechalince = (DateTime)dtfechalicencia.SelectedDate;
                            DateTime fechaactu = DateTime.Now;
                            if (dtfechalicencia.SelectedDate <= FechaActual.SelectedDate)
                            {
                                lblerror_fechalicencia.Content = "***";
                                MessageBox.Show("la fecha de vencimiento de la licencia no corresponde a los parámetros del MINISTERIO DE TRANSPORTE");
                                respuesta = true;
                            }
                            else
                            {
                                anioactual = DateTime.Now.Year;
                                aniolicencia = fechalince.Year;
                                aniodiferencia = aniolicencia - anioactual;
                                if (aniodiferencia < 0 || aniodiferencia > 10)
                                {
                                    lblerror_fechalicencia.Content = "***";
                                    MessageBox.Show("la fecha de vencimiento de la licencia no corresponde a los parámetros del MINISTERIO DE TRANSPORTE");
                                    respuesta = true;
                                }
                                else
                                {
                                    lblerror_fechalicencia.Content = "";
                                }
                            }
                            //respuesta = false;
                        }
                        break;
                }
            }
            else
            {
                switch (Convert.ToInt32(cbbcateglicencia.SelectedValue))
                {
                    case 7:
                    case 8:
                    case 9:
                        if (dtfechalicencia.SelectedDate == null)
                        {
                            lblerror_fechalicencia.Content = "***";
                            respuesta = true;
                        }
                        else
                        {
                            int anioactual, aniolicencia, aniodiferencia;
                            DateTime fechalince = (DateTime)dtfechalicencia.SelectedDate;
                            DateTime fechaactu = DateTime.Now;
                            if (dtfechalicencia.SelectedDate <= FechaActual.SelectedDate)
                            {
                                lblerror_fechalicencia.Content = "***";
                                MessageBox.Show("la fecha de vencimiento de la licencia no corresponde a los parámetros del MINISTERIO DE TRANSPORTE");
                                respuesta = true;
                            }
                            else
                            {
                                anioactual = DateTime.Now.Year;
                                aniolicencia = fechalince.Year;
                                aniodiferencia = aniolicencia - anioactual;
                                if (aniodiferencia < 0 || aniodiferencia > 3)
                                {
                                    lblerror_fechalicencia.Content = "***";
                                    MessageBox.Show("la fecha de vencimiento de la licencia no corresponde a los parámetros del MINISTERIO DE TRANSPORTE");
                                    respuesta = true;
                                }
                                else
                                {
                                    lblerror_fechalicencia.Content = "";
                                }
                            }
                            //respuesta = false;
                        }
                        break;
                    default:
                        MessageBox.Show("la categoría de licencia seleccionada no es permitida para conductores propios o terceros");
                        respuesta = true;
                        break;
                }
            }

            if (txtnombres.Text == "")
            {
                lblerror_nombresCond.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_nombresCond.Content = "";
                //  respuesta = false;
            }
            var lista = ConducBll.GetDAtosconductor(txtNumdocumento.Text);
            if (lista.Count > 0)
            {
                ExisteConductor = true;
            }
            else
            {
                ExisteConductor = false;
            }
            //if (dtFechaPagoSalud.SelectedDate == null)
            //{
            //    lblerror_PlanillaAporte.Content = "***";
            //    respuesta = true;
            //}
            //else
            //{
            //    DateTime dtFecha = Convert.ToDateTime(dtFechaPagoSalud.SelectedDate);
            //    dtFecha = dtFecha.AddMonths(1);
            //    if (dtFecha < FechaActual.SelectedDate)
            //    {
            //        lblerror_PlanillaAporte.Content = "***";
            //        respuesta = true;
            //    }
            //    else
            //    {
            //        lblerror_PlanillaAporte.Content = "";
            //    }
            //}
            //if (dtFechaExamenes.SelectedDate == null)
            //{
            //    lblerror_ExamenesMedicos.Content = "***";
            //    respuesta = true;
            //}
            //else
            //{
            //    lblerror_ExamenesMedicos.Content = "";
            //}
            return respuesta;
        }

        private Boolean validarcamposremolque()
        {
            Boolean respuesta = false;
            if (txtplacaremolque.Text.Trim() == "")
            {
                lblerror_placaremolque.Content = "***";
                respuesta = true;
            }
            else
            {
                validarpermisosoperacioninternacional();
                string letraR = txtplacaremolque.Text.Trim();
                char letra = letraR[0];
                switch (letra.ToString())
                {
                    case "T":
                        if (txtplacaremolque.Text.Trim().Length == 5)
                        {
                            lblerror_placaremolque.Content = "";
                            // respuesta = false;
                        }
                        else
                        {
                            lblerror_placaremolque.Content = "***";
                            respuesta = true;
                        }
                        break;
                    case "S":
                    case "R":
                        if (txtplacaremolque.Text.Trim().Length == 6)
                        {
                            lblerror_placaremolque.Content = "";
                            // respuesta = false;
                        }
                        else
                        {
                            lblerror_placaremolque.Content = "***";
                            respuesta = true;
                        }
                        break;
                    default:
                        MessageBox.Show("Señor usuario: " + main.txtusuario.Text.Trim() + ", Usted Está tratando de crear un remolque con una placa que no está permitida, auditoría interna será informada de su intento de alterar el proceso.");
                        return true;
                        break;
                }
            }
            if (txtmodeloremolque.Text == "")
            {

                lblerror_modeloremolque.Content = "***";
                respuesta = true;
            }
            else
            {
                if (txtmodeloremolque.Text.Length == 4)
                {
                    if (numeric.IsNumeric(txtmodeloremolque.Text) == true)
                        lblerror_modeloremolque.Content = "";
                    else
                    {
                        lblerror_modeloremolque.Content = "***";
                        respuesta = true;
                    }
                    // lblerror_modeloremolque.Content = "";
                    // respuesta = false;
                }
                else
                {
                    lblerror_modeloremolque.Content = "***";
                    respuesta = true;
                }
            }
            if (txtTPropiedadremolque.Text == "")
            {
                lblerror_Tpropiedadremolque.Content = "***";
                respuesta = true;
            }
            else
            {
                if (numeric.IsNumeric(txtTPropiedadremolque.Text) == true)
                    lblerror_Tpropiedadremolque.Content = "";
                else
                {
                    lblerror_Tpropiedadremolque.Content = "***";
                    respuesta = true;
                }
                // lblerror_Tpropiedadremolque.Content = "";
                // respuesta = false;
            }
            if (txtcapacidadremolque.Text.Trim() == "")
            {
                lblerror_capacidadremolque.Content = "***";
                respuesta = true;
            }
            else
            {
                //lblerror_capacidadremolque.Content = "";
                if (numeric.IsNumeric(txtcapacidadremolque.Text.Trim()) == true)
                {
                    if (Convert.ToInt32(txtcapacidadremolque.Text.Trim()) <= 1000)
                    {
                        MessageBox.Show("La capacidad del remolque no puede ser inferior a mil kilos o galones");
                        lblerror_capacidadremolque.Content = "***";
                        respuesta = true;
                    }
                    else
                    {
                        lblerror_capacidadremolque.Content = "";
                    }
                }
                else
                {
                    lblerror_capacidadremolque.Content = "***";
                    respuesta = true;
                }
                // respuesta = false;
            }
            if (txtpesoremolque.Text.Trim() == "")
            {
                lblerror_Pesoremolque.Content = "***";
                respuesta = true;
            }
            else
            {
                if (numeric.IsNumeric(txtpesoremolque.Text) == true)
                    lblerror_Pesoremolque.Content = "";
                else
                {
                    lblerror_Pesoremolque.Content = "***";
                    respuesta = true;
                }
                // lblerror_Pesoremolque.Content = "";
                //  respuesta = false;
            }
            DateTime fechafutura;
            fechafutura = DateTime.Now;
            if (dtfechaquitanrueda.SelectedDate != null)
            {
                DateTime fechaaux = (DateTime)dtfechaquitanrueda.SelectedDate;
                if (fechaaux < fechafutura)
                {
                    lblerror_QuintaRueda.Content = "***";
                    respuesta = true;
                }
                else
                {
                    lblerror_QuintaRueda.Content = "";
                }
            }
            else
            {
                if (Chk_Grabar_operEcuador.IsChecked == true)
                {
                    lblerror_QuintaRueda.Content = "";
                }
                else
                {
                    lblerror_QuintaRueda.Content = "***";
                    respuesta = true;
                }
            }

            if (Convert.ToInt32(cbbtipocarroceria.SelectedValue) == 67)
            {
                if (Convert.ToInt32(cbbunidadremolque.SelectedValue) == 8)
                {
                    respuesta = true;
                    MessageBox.Show("la unidad de medida del remolque no corresponde al tipo de carrocería");
                }
                if (Convert.ToInt32(cbbclase.SelectedValue) == 999)
                {
                    respuesta = true;
                    MessageBox.Show("se debe Seleccionar la clase del tanque");
                }
            }
            else
            {
                if (Convert.ToInt32(cbbunidadremolque.SelectedValue) == 4)
                {
                    respuesta = true;
                    MessageBox.Show("la unidad de medida del remolque no corresponde al tipo de carrocería");
                }
            }
            return respuesta;
        }

        private Boolean ValidarCamposVehiculo()
        {
            Boolean respuesta = false;
            if (cbblinea.SelectedIndex < 0)
            {
                lblerror_linea.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_linea.Content = "";
            }
            if (cbbgrupollantas.SelectedIndex < 0)
            {
                cbbgrupollantas.SelectedIndex = 1;
            }
            if (txtplaca.Text == "")
            {
                lblerror_Placa.Content = "***";
                respuesta = true;
            }
            else
            {
                if (txtplaca.Text.Trim().Length == 6)
                {
                    lblerror_Placa.Content = "";
                    // respuesta = false;
                }
                else
                {
                    lblerror_Placa.Content = "***";
                    respuesta = true;
                }
            }
            if (txtmodelo.Text == "")
            {
                lblerror_Modelo.Content = "***";
                respuesta = true;
            }
            else
            {
                if (txtmodelo.Text.Length == 4)
                {
                    if (numeric.IsNumeric(txtmodelo.Text) == true)
                        lblerror_Modelo.Content = "";
                    else
                    {
                        lblerror_Modelo.Content = "***"; respuesta = true;
                    }
                }
                else
                {
                    lblerror_Modelo.Content = "***";
                    respuesta = true;
                }
            }
            if (txttarjetapropiedad.Text == "")
            {
                lblerror_TarjetaDePropiedad.Content = "***";
                respuesta = true;
            }
            else
            {
                if (numeric.IsNumeric(txttarjetapropiedad.Text) == true)
                    lblerror_TarjetaDePropiedad.Content = "";
                else
                {
                    lblerror_TarjetaDePropiedad.Content = "***";
                    respuesta = true;
                }
            }
            if (txtchasis.Text == "")
            {
                lblerror_Chasis.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_Chasis.Content = "";
            }
            if (txtnumotor.Text == "")
            {
                lblerror_numotor.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_numotor.Content = "";
            }
            if (Convert.ToInt32(cbbtiporemolque.SelectedValue) == 67)
            {
                if (txtpesovacio.Text.Trim() == "")
                {
                    lblerror_PVacioCabezote.Content = "***";
                    respuesta = true;
                }
                else
                {
                    if (numeric.IsNumeric(txtpesovacio.Text.Trim()) == true)
                        lblerror_PVacioCabezote.Content = "";
                    else
                    {
                        lblerror_PVacioCabezote.Content = "***";
                        respuesta = true;
                    }
                }
            }
            else
            {
                if (txtpesovacio.Text.Trim() == "")
                {
                    txtpesovacio.Text = "0";
                    lblerror_PVacioCabezote.Content = "";
                }
                else
                {
                    if (numeric.IsNumeric(txtpesovacio.Text.Trim()) == true)
                        lblerror_PVacioCabezote.Content = "";
                    else
                    {
                        lblerror_PVacioCabezote.Content = "***";
                        respuesta = true;
                    }
                }
            }
            if (txtpesocabezote.Text.Trim() == "")
            {
                lblerror_PesoCabezote.Content = "***";
                respuesta = true;
            }
            else
            {
                if (numeric.IsNumeric(txtpesocabezote.Text.Trim()) == true)
                    lblerror_PesoCabezote.Content = "";
                else
                {
                    lblerror_PesoCabezote.Content = "***";
                    respuesta = true;
                }
            }
            DateTime fechafutura;
            fechafutura = DateTime.Now;
            int aniovehiculo = 0;
            Boolean anioauxbool = false;
            int anioactual = Convert.ToInt32(fechafutura.Year.ToString());
            if (txtmodelo.Text != "")
            {
                aniovehiculo = Convert.ToInt32(txtmodelo.Text);
            }
            if (anioactual == aniovehiculo)
            {
                fechafutura = fechafutura.AddYears(2);
                anioauxbool = true;
            }
            if (anioactual == (aniovehiculo + 1))
            {
                fechafutura = fechafutura.AddYears(1);
                anioauxbool = true;
            }
            if (anioauxbool == false)
            {
                fechafutura = fechafutura.AddYears(1);
            }
            if (dtfechatmecanica.SelectedDate == null)
            {
                if (anioauxbool == false)
                {
                    lblerror_Tmecanica.Content = "***";
                    respuesta = true;
                }
                else
                {
                    dtfechatmecanica.SelectedDate = fechafutura;
                }
            }
            else
            {
                DateTime tecnoM = (DateTime)dtfechatmecanica.SelectedDate;
                DateTime fechaAc = DateTime.Now;
                if (tecnoM <= fechaAc)
                {
                    lblerror_Tmecanica.Content = "***";
                    respuesta = true;
                    MessageBox.Show("No puede ingresar una fecha de Técnico mecánica Vencida");
                }
                else
                {
                    if (anioauxbool == true)
                    {
                        dtfechatmecanica.SelectedDate = fechafutura;
                    }
                    lblerror_Tmecanica.Content = "";
                }
            }
            if (txtnumsoat.Text == "")
            {
                lblerror_numsoat.Content = "***";
                respuesta = true;
            }
            else
            {
                string cadenaauxsoat = txtnumsoat.Text;
                if (cadenaauxsoat.Contains("A") & cadenaauxsoat.Contains("T"))
                {
                    char[] MyChar = { 'a', 't', 'A', 'T', ' ' };
                    string NewString = cadenaauxsoat.TrimStart(MyChar);

                    if (numeric.IsNumeric(NewString) == true)
                        lblerror_numsoat.Content = "";
                    else
                    {
                        lblerror_numsoat.Content = "***";
                        respuesta = true;
                    }
                    if (dtfechasoat.SelectedDate == null)
                    {
                        lblerror_fechasoat.Content = "***";
                        respuesta = true;
                    }
                    else
                    {
                        lblerror_fechasoat.Content = "";
                    }
                }
                else
                {
                    lblerror_numsoat.Content = "***";
                    respuesta = true;
                }
            }
            if (cbbciasoat.SelectedIndex < 0)
            {
                respuesta = true;
                lblerror_ciasoat.Content = "***";
            }
            else
            {
                lblerror_ciasoat.Content = "";
            }
            //if (dtFechaVenceContrato.SelectedDate == null)
            //{
            //    lblerror_VenceContrato.Content = "***";
            //    respuesta = true;
            //}
            //else
            //{
            //    lblerror_VenceContrato.Content = "";
            //}
            if (chk_remolque.IsChecked == true)
            {
                if (GrBo_inforemolque.Visibility == Visibility.Visible)
                {
                    dtfeAforotanqueSencillo.SelectedDate = null;
                    lblerror_feaforotanqueSencillo.Content = "";
                    dtfeHidrostaticaSencillo.SelectedDate = null;
                    lblerror_feHidrostaticaSencillo.Content = "";
                    if (dtfechaquitanrueda.SelectedDate == null)
                    {
                        if (Chk_Grabar_operEcuador.IsChecked == true) //Se modifica por solicitud de Gerencia. La opción de omitir el campo quinta rueda
                        {
                            lblerror_QuintaRueda.Content = "";
                        }
                        else
                        {
                            lblerror_QuintaRueda.Content = "***";
                            respuesta = true;
                        }
                    }
                    else
                    {
                        lblerror_QuintaRueda.Content = "";
                    }
                    if (txtplacaremolque.Text != "")
                    {
                        lblerror_placaremolque.Content = "";
                    }
                    else
                    {
                        lblerror_placaremolque.Content = "***";
                        respuesta = true;
                    }
                    if (txtmodeloremolque.Text != "")
                    {
                        if (numeric.IsNumeric(txtmodeloremolque.Text) == true)
                            lblerror_modeloremolque.Content = "";
                        else
                        {
                            lblerror_modeloremolque.Content = "***";
                            respuesta = true;
                        }
                    }
                    else
                    {
                        lblerror_modeloremolque.Content = "***";
                        respuesta = true;
                    }
                    if (txtcapacidadremolque.Text != "")
                    {
                        if (numeric.IsNumeric(txtcapacidadremolque.Text) == true)
                            lblerror_capacidadremolque.Content = "";
                        else
                        {
                            lblerror_capacidadremolque.Content = "***";
                            respuesta = true;
                        }
                    }
                    else
                    {
                        lblerror_capacidadremolque.Content = "***";
                        respuesta = true;
                    }
                    if (txtpesoremolque.Text != "")
                    {
                        if (numeric.IsNumeric(txtpesoremolque.Text) == true)
                            lblerror_Pesoremolque.Content = "";
                        else
                        {
                            lblerror_Pesoremolque.Content = "***";
                            respuesta = true;
                        }
                    }
                    else
                    {
                        lblerror_Pesoremolque.Content = "***";
                        respuesta = true;
                    }
                    if (txtTPropiedadremolque.Text != "")
                    {
                        if (numeric.IsNumeric(txtTPropiedadremolque.Text) == true)
                            lblerror_Tpropiedadremolque.Content = "";
                        else
                        {
                            lblerror_Tpropiedadremolque.Content = "***";
                            respuesta = true;
                        }
                    }
                    else
                    {
                        lblerror_Tpropiedadremolque.Content = "***";
                        respuesta = true;
                    }
                    if (Convert.ToInt32(cbbtiporemolque.SelectedValue) == 67)
                    {
                        if (dtfeHidrostatica.SelectedDate == null)
                        {
                            if (Chk_Grabar_operEcuador.IsChecked == true) //Se modifica por solicitud de Gerencia. La opción de omitir vence hidrostatica
                            {
                                lblerror_feHidrostatica.Content = "";
                            }
                            else
                            {
                                lblerror_feHidrostatica.Content = "***";
                                respuesta = true;
                            }

                        }
                        else
                        {
                            lblerror_feHidrostatica.Content = "";
                        }
                        if (dtfeAforotanque.SelectedDate == null)
                        {
                            if (Chk_Grabar_operEcuador.IsChecked == true) //Se modifica por solicitud de Gerencia. La opción de omitir Aforo Tanque
                            {
                                lblerror_feaforotanque.Content = "";
                            }
                            else
                            {
                                lblerror_feaforotanque.Content = "***";
                                respuesta = true;
                            }
                        }
                        else
                        {
                            lblerror_feaforotanque.Content = "";
                        }
                    }
                    else
                    {
                        lblerror_feaforotanque.Content = "";
                        lblerror_feHidrostatica.Content = "";
                    }
                    var item = (EjesVehiculo)cbbejes.SelectedItem;
                    if (item != null)
                    {
                        if (item.IdEje > 3)
                        {
                            if (txtcapacidadremolque.Text.Trim() == "")
                            {
                                MessageBox.Show("Para la cantidad de ejes seleccionada debe existir una capacidad de carga en kilos");
                                respuesta = true;
                            }
                            if (Convert.ToInt32(cbbtipocarroceria.SelectedValue) == 67 || Convert.ToInt32(cbbtiporemolque.SelectedValue) == 67)
                            {
                                MessageBox.Show("Para la cantidad de ejes seleccionada no puede seleccionar este típo de carrocería");
                                respuesta = true;
                            }
                            if (Convert.ToInt32(cbbunidadremolque.SelectedValue) == 4)
                            {
                                MessageBox.Show("Para la cantidad de ejes seleccionada debe existir una unidad de carga en kilos");
                                respuesta = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (Convert.ToInt32(cbbtipocarroceria.SelectedValue) == 67)
                {
                    if (dtfeAforotanqueSencillo.SelectedDate == null)
                    {
                        if (Chk_Grabar_operEcuador.IsChecked == true) //Se modifica por solicitud de Gerencia. La opción de omitir Aforo Tanque Sencillo
                        {
                            lblerror_feaforotanqueSencillo.Content = "";
                        }
                        else
                        {
                            lblerror_feaforotanqueSencillo.Content = "***";
                            respuesta = true;
                        }
                    }
                    else
                    {
                        lblerror_feaforotanqueSencillo.Content = "";
                    }
                    if (dtfeHidrostaticaSencillo.SelectedDate == null)
                    {
                        if (Chk_Grabar_operEcuador.IsChecked == true) //Se modifica por solicitud de Gerencia. La opción de omitir Hidrostatica sencillo
                        {
                            lblerror_feHidrostaticaSencillo.Content = "";
                        }
                        else
                        {
                            lblerror_feHidrostaticaSencillo.Content = "***";
                            respuesta = true;
                        }
                    }
                    else
                    {
                        lblerror_feHidrostaticaSencillo.Content = "";
                    }
                }
                else
                {
                    lblerror_feaforotanqueSencillo.Content = "";
                    lblerror_feHidrostaticaSencillo.Content = "";
                }
            }
            return respuesta;
        }

        private bool ValidarDatosContrato()
        {
            Boolean respuesta = false;
            if (txtnumerocontrato.Text == "")
            {
                respuesta = true;
                lblerror_contrato.Content = "***";
            }
            else
            {
                lblerror_contrato.Content = "";
            }
            if (dtfechacontrato.SelectedDate == null)
            {
                respuesta = true;
                lblerror_fechacontrato.Content = "***";
            }
            else
            {
                lblerror_fechacontrato.Content = "";
            }
            return respuesta;
        }

        #endregion

        #region ValidarExistecia Datos en BD

        private Boolean ValidarConductorAsociadoplaca()
        {
            Boolean respuesta;
            var lista = ConducBll.ConsultaConductorAsociadoPlacaXPlaca_Idcondutor(txtplaca.Text, txtNumdocumento.Text);
            if (lista.Count > 0)
                respuesta = true;
            else
                respuesta = false;
            return respuesta;
        }

        public Boolean ValidarExisteplaca()
        {
            Boolean respuesta;
            if (actualizarvehi != true)
            {
                var lista = vehibll.ConsultaExistenciaVehiculo(txtplaca.Text.Trim());
                if (lista.Count > 0)
                {
                    respuesta = true;
                }
                else
                {
                    respuesta = false;
                }
            }
            else
            {
                respuesta = false;
            }
            return respuesta;
        }

        public Boolean ValidarExisteplacaRemolque()
        {
            Boolean respuesta;
            var lista = RemolBll.GetRemolquePorPlaca(txtplacaremolque.Text.Trim());
            if (lista.Count > 0)
            {
                respuesta = true;
            }
            else
            {
                respuesta = false;
            }
            return respuesta;
        }

        #endregion

        #region metodos guardar Información

        private void GuardarDatosRemolque()
        {
            if (chk_remolque.IsChecked == true & GrBo_inforemolque.Visibility == Visibility.Visible)
            {
                if (validarcamposremolque() == false)
                {
                    Nullable<DateTime> fechahidrostatica, fechaaforo, FechaVencekingpin;
                    Nullable<decimal> idpropietario;
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
                    if (dtfeAforotanque.SelectedDate != null)
                    {
                        fechaaforo = Convert.ToDateTime(dtfeAforotanque.SelectedDate);
                    }
                    else
                    {
                        fechaaforo = null;
                    }
                    if (dtfeHidrostatica.SelectedDate != null)
                    {
                        fechahidrostatica = Convert.ToDateTime(dtfeHidrostatica.SelectedDate);
                    }
                    else
                    {
                        fechahidrostatica = null;
                    }
                    if (txtNumdocumentopropi.Text != "")
                    {
                        idpropietario = Convert.ToDecimal(txtNumdocumentopropi.Text);
                    }
                    else
                    {
                        idpropietario = null;
                    }
                    if (dtfekingpin.SelectedDate != null)
                    {
                        FechaVencekingpin = (DateTime)dtfekingpin.SelectedDate;
                    }
                    else
                    {
                        FechaVencekingpin = null;
                    }
                    Boolean respuesta;
                    int idpais = 1;
                    if (autorizaoperacioninternacional == true)
                    {
                        if (Chk_Grabar_operEcuador.IsChecked == true)
                        {
                            idpais = 2;
                        }
                    }
                    if (existeremolque == true)
                    {
                        var rowafiliacio = (TipoAfiliacion)cbbtipoafiliacion.SelectedItem;
                        var lista = RemolBll.GetRemolquePorPlaca(txtplacaremolque.Text.Trim());
                        if (lista.Count > 0)
                        {
                            if (rowafiliacio != null)
                            {
                                if (autorizamoftipoAfilremolque == false)
                                {
                                    if (rowafiliacio.IdCodigo != lista[0].TipoAfiliacion)
                                    {
                                        MessageBox.Show("Usted no puede asociar un remoque con un tipo de afiliación diferente al del cabezote.");
                                        return;
                                    }
                                }
                            }
                        }

                        respuesta = RemolBll.UpdateDatosRemolque(txtplacaremolque.Text.Trim(), Convert.ToInt16(cbbtipoafiliacion.SelectedValue), txtmodeloremolque.Text, Convert.ToInt32(txtcapacidadremolque.Text),
                            Convert.ToInt16(cbbtipocarroceria.SelectedValue), Convert.ToInt16(cbbejes.SelectedValue), Convert.ToInt16(cbbunidadremolque.SelectedValue),
                            cbbmarcaremolque.Text, Convert.ToInt32(txtpesoremolque.Text), cbbgrupollantas.SelectedValue.ToString(), Convert.ToInt32(cbbmarcaremolque.SelectedValue),
                             Convert.ToInt32(cbbclase.SelectedValue), idpropietario, Convert.ToInt32(txtpesoremolque.Text),
                              fechahidrostatica, txtTPropiedadremolque.Text, fechaaforo, txtplaca.Text, usuario, FechaVencekingpin, idpais);
                    }
                    else
                    {
                        respuesta = RemolBll.GuardarDAtosRemolque(txtplacaremolque.Text, Convert.ToInt16(cbbtipoafiliacion.SelectedValue), txtmodeloremolque.Text, Convert.ToInt32(txtcapacidadremolque.Text),
                       Convert.ToInt16(cbbtipocarroceria.SelectedValue), Convert.ToInt16(cbbejes.SelectedValue), Convert.ToInt16(cbbunidadremolque.SelectedValue), usuario,
                       cbbmarcaremolque.Text, Convert.ToInt32(txtpesoremolque.Text), cbbgrupollantas.SelectedValue.ToString(), Convert.ToInt32(cbbmarcaremolque.SelectedValue),
                       Convert.ToInt32(cbbclase.SelectedValue), idpropietario, Convert.ToInt32(txtpesoremolque.Text),
                       fechahidrostatica, txtTPropiedadremolque.Text, fechaaforo, txtplaca.Text, FechaVencekingpin, idpais);
                    }
                    mostrarmensajeRemolVehiculo(respuesta, "Datos del Remolque ");
                    if (respuesta == true)
                    {
                        habilitarbotonesvehiculo(true);
                        txtplaca.IsEnabled = false;
                    }
                }
                else
                {
                    MessageBox.Show("Datos del remolque no guardados por que la información no está completa");
                }
            }
        }

        private void guardarDatosvehiculo()
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
            short tipocarga = 3;
            if (rdbalto.IsChecked == true)
            {
                tipocarga = 1;
            }
            if (rdbmedio.IsChecked == true)
            {
                tipocarga = 2;
            }
            if (rdbbajo.IsChecked == true)
            {
                tipocarga = 3;
            }
            short categoria = 1;
            Boolean contrato = false;
            Nullable<decimal> propietario;
            if (txtNumdocumentopropi.Text != "")
                propietario = decimal.Parse(txtNumdocumentopropi.Text.Trim());
            else
                propietario = null;
            if (chk_contraro.IsChecked == true)
            {
                contrato = true;
            }
            else
            {
                contrato = false;
            }
            DateTime soat, fechatecmecanica, FechaVenceContrato;
            Nullable<DateTime> fechaRC, fechaquintarueda, fechaaforosencillo, fechaHidroSencillo, FechaEstVeh;
            Nullable<short> ModRepot;
            ModRepot = null;
            fechaquintarueda = null;
            fechaRC = null;
            if (chk_remolque.IsChecked == true)
            {
                fechaaforosencillo = null;
                fechaHidroSencillo = null;
                if (dtfechaquitanrueda.SelectedDate != null)
                {
                    fechaquintarueda = Convert.ToDateTime(dtfechaquitanrueda.SelectedDate);
                }
                else
                {
                    fechaquintarueda = DateTime.Now.AddYears(-1);
                }
            }
            else
            {
                if (cbbtipovehiculo.SelectedValue != null)
                {
                    switch (Convert.ToInt32(cbbtipovehiculo.SelectedValue))
                    {
                        case 4:
                        case 8:
                        case 1:
                            if (Convert.ToInt32(cbbtipocarroceria.SelectedValue) == 67)
                            {

                                if (dtfeAforotanqueSencillo.SelectedDate != null)
                                {
                                    fechaaforosencillo = Convert.ToDateTime(dtfeAforotanqueSencillo.SelectedDate);
                                }
                                else
                                {
                                    fechaaforosencillo = null;
                                }
                                if (dtfeHidrostaticaSencillo.SelectedDate != null)
                                {
                                    fechaHidroSencillo = Convert.ToDateTime(dtfeHidrostaticaSencillo.SelectedDate);
                                }
                                else
                                {
                                    fechaHidroSencillo = null;
                                }
                            }
                            else
                            {
                                fechaaforosencillo = null;
                                fechaHidroSencillo = null;
                            }
                            break;
                        default:
                            fechaaforosencillo = null;
                            fechaHidroSencillo = null;
                            break;
                    }
                }
                else
                {
                    fechaaforosencillo = null;
                    fechaHidroSencillo = null;
                }
            }
            
            soat = Convert.ToDateTime(dtfechasoat.SelectedDate);
            fechatecmecanica = Convert.ToDateTime(dtfechatmecanica.SelectedDate);
            FechaVenceContrato = Convert.ToDateTime(dtFechaVenceContrato.SelectedDate);
            if (dtfeEstudioVehiculo.SelectedDate !=null)
	        {
                FechaEstVeh = Convert.ToDateTime(dtfeEstudioVehiculo.SelectedDate);
	        }
            else
            {
                FechaEstVeh = Convert.ToDateTime(DateTime.Now);
            }
            
            if (txtmodelorepotenciado.Text != "")
            {
                ModRepot = Convert.ToInt16(txtmodelorepotenciado.Text);
            }
            if (dtfechaRC.SelectedDate != null)
            {
                fechaRC = Convert.ToDateTime(dtfechaRC.SelectedDate);
            }
            if (cbblinea.SelectedValue == null)
            {
                cbblinea.SelectedIndex = 1;
            }

            if (cbbcomRCE.SelectedValue == null)
            {
                cbbcomRCE.SelectedIndex = 1;
            }
            int idpais = 1;
            if (autorizaoperacioninternacional == true)
            {
                if (Chk_Grabar_operEcuador.IsChecked == true)
                {
                    idpais = 2;
                }
            }
            Boolean respuesta;
            var rowafiliacio = (TipoAfiliacion)cbbtipoafiliacion.SelectedItem;
            if (rowafiliacio != null)
            {
                switch (rowafiliacio.IdCodigo)
                {
                    case 2:
                    case 3:
                        if (boolmodificaterceros == true || boolmodificatodos == true)
                        {

                        }
                        else
                        {

                            MessageBox.Show("Usted no tiene permitido modificar vehículos con este tipo de Afiliación");
                            return;
                        }
                        break;
                    case 1:
                    case 5:
                        if (boolmodificapropios == true || boolmodificatodos == true)
                        {

                        }
                        else
                        {
                            MessageBox.Show("Usted no tiene permitido modificar vehículos con este tipo de Afiliación");
                            return;
                        }
                        break;
                    default:
                        if (boolmodificatodos == true)
                        {

                        }
                        else
                        {
                            MessageBox.Show("Usted no tiene permitido modificar vehículos con este tipo de Afiliación");
                            return;
                        }
                        break;
                }
            }

            if (actualizarvehi == true)
            {
                respuesta = vehibll.UpdateDAtosVehiculo(txtplaca.Text.Trim(), Convert.ToInt32(cbbmarca.SelectedValue), txtmodelo.Text.Trim(),
                    cbbcolor.Text, Convert.ToInt32(cbbtipomotor.SelectedValue), Convert.ToInt32(cbbtipocarroceria.SelectedValue),
                    txtnumotor.Text, txttarjetapropiedad.Text, txtnumsoat.Text.Trim(), soat, Convert.ToInt32(cbbtipoafiliacion.SelectedValue),
                      Convert.ToInt32(cbbtipovehiculo.SelectedValue), txtciafiliadora.Text, txttelciafiliadora.Text, txtchasis.Text, txtobservaciones.Text,
                    cbbgrupollantas.SelectedValue.ToString(), tipocarga, fechatecmecanica, cbblinea.Text, ModRepot,
                     Convert.ToInt32(txtpesovacio.Text), cbbciasoat.SelectedValue.ToString(), cbbempreSatelital.Text,
                              txtnumpolizaRC.Text.Trim(), fechaRC, Convert.ToDecimal(cbbempreSatelital.SelectedValue),
                               cbbcomRCE.SelectedValue.ToString(), txtusuariogps.Text, txtclavegps.Text,
                                fechaquintarueda,
                                cbbcolor.SelectedValue.ToString(),
                                Convert.ToInt32(cbblinea.SelectedValue),
                                contrato, usuario, fechaaforosencillo, fechaHidroSencillo,
                                Convert.ToInt32(txtpesocabezote.Text.Trim()), idpais, FechaVenceContrato, FechaEstVeh
                      );
            }
            else
            {
                respuesta = vehibll.GuardarDatosVehiculo(txtplaca.Text.Trim(), Convert.ToInt32(cbbmarca.SelectedValue),
                                txtmodelo.Text.Trim(),
                                cbbcolor.Text, Convert.ToInt32(cbbtipomotor.SelectedValue), Convert.ToInt32(cbbtipocarroceria.SelectedValue),
                                txtnumotor.Text, txttarjetapropiedad.Text, txtnumsoat.Text.Trim(), soat,
                                Convert.ToInt32(cbbtipoafiliacion.SelectedValue),
                                contrato, usuario,
                               Convert.ToInt32(cbbtipovehiculo.SelectedValue),
                               txtNumdocumento.Text,
                                 txtciafiliadora.Text, txtNumdocumentobenefi.Text.Trim(),
                                 txtciafiliadora.Text, txttelciafiliadora.Text, txtchasis.Text, txtobservaciones.Text,
                                 cbbgrupollantas.SelectedValue.ToString(),
                                tipocarga, fechatecmecanica, categoria, cbblinea.Text,
                                ModRepot,
                                Convert.ToInt32(txtpesovacio.Text), cbbciasoat.SelectedValue.ToString(),                                //Convert.ToInt32(txtpesovacio.Text), cbbciasoat.Text,

                              propietario, cbbempreSatelital.Text,
                              txtnumpolizaRC.Text.Trim(),
                              fechaRC, Convert.ToDecimal(cbbempreSatelital.SelectedValue),
                              cbbcomRCE.SelectedValue.ToString(),
                                txtusuariogps.Text, txtclavegps.Text, fechaquintarueda,
                                cbbcolor.SelectedValue.ToString(),
                                Convert.ToInt32(cbblinea.SelectedValue), fechaaforosencillo,
                                fechaHidroSencillo, Convert.ToInt32(txtpesocabezote.Text.Trim()),
                                idpais,
                                FechaVenceContrato, FechaEstVeh);
            }
            if (chk_remolque.IsChecked == true & txtplacaremolque.Text != "")
            {
                existeremolque = ValidarExisteplacaRemolque();
                if (existeremolque == true)
                {
                    MessageBoxResult result1 = MessageBox.Show("Este Remolque ya  Existe. Desea Actualizar sus datos?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result1 == MessageBoxResult.Yes)
                    {
                        GuardarDatosRemolque();
                    }
                    else
                    {
                        limpiarCamposremolque();
                    }
                }
                else
                {
                    GuardarDatosRemolque();
                }
            }
            mostrarmensajeRemolVehiculo(respuesta, "Datos del vehículo ");
            if (respuesta == true)
            {
                habilitarbotonesvehiculo(true);
                habilitarBotonesLeasing(true);
                txtplaca.IsEnabled = false;
                cbbtipovehiculo.IsEnabled = false;
                limpiarErroresVehiculo();
            }
            else
            {
                habilitarBotonesLeasing(false);
                txtplaca.IsEnabled = true;
                cbbtipovehiculo.IsEnabled = true;
            }
        }

        private void GuardarDatosPropietario()
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
            var lista = PropiBeneBll.GetDatosPropietarioXID(txtNumdocumentopropi.Text.Trim());
            if (lista.Count > 0)
            {
                Existepropietario = true;
            }
            else
            {
                Existepropietario = false;
            }
            Nullable<short> DigitVerif;
            if (txtNitVerificacionPropi.Text.Trim() == "")
            {
                DigitVerif = null;
                int digito = _DigitoVerificacion.ValidarDigitoVerificacion(txtNumdocumentopropi.Text.Trim());
                DigitVerif = Convert.ToInt16(digito);
            }
            else
            {
                DigitVerif = Convert.ToInt16(txtNitVerificacionPropi.Text.Trim());
            }
            if (Existepropietario == true)
            {
                MessageBoxResult result1 = MessageBox.Show("Este Propietario ya  Existe. Desea Actualizar sus datos?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result1 == MessageBoxResult.Yes)
                {
                    respuesta = PropiBeneBll.UpdateDatosPropietario(Convert.ToDecimal(txtNumdocumentopropi.Text.Trim()),
                        txtnombrespropi.Text.TrimStart(), txtapellidopropi1.Text.Trim(), txtapellidopropi2.Text.Trim(), cbbtipodocumentopropi.SelectedValue.ToString(),
                        cbbciudadpropi.SelectedValue.ToString(), txttelefonopropi.Text, txtdireccionpropi.Text, ListaRiesgo, FechaEstudio, txtplaca.Text, usuario, DigitVerif);
                    mostrarmensaje(respuesta);
                    if (respuesta == true)
                    {
                        Habilitarbotonesconductor(true);
                        txtNumdocumento.IsEnabled = false;
                    }
                }
            }
            else
            {
                respuesta = PropiBeneBll.GuardarDatosPropietario(Convert.ToDecimal(txtNumdocumentopropi.Text.Trim()),
               txtnombrespropi.Text.TrimStart(), txtapellidopropi1.Text.Trim(), txtapellidopropi2.Text.Trim(), cbbtipodocumentopropi.SelectedValue.ToString(),
               cbbciudadpropi.SelectedValue.ToString(), txttelefonopropi.Text, txtdireccionpropi.Text, ListaRiesgo, FechaEstudio, txtplaca.Text, usuario, DigitVerif);
                PropiBeneBll.GuardaAuditoriaCambioBeneficiario_Propietario(usuario, txtNumdocumentopropi.Text, "", txtNumdocumentobenefi.Text.Trim(), "guardó propietario nuevo", txtplaca.Text.Trim());
                mostrarmensaje(respuesta);
                if (respuesta == true)
                {
                    Habilitarbotonesconductor(true);
                    txtNumdocumento.IsEnabled = false;
                }
            }
        }

        private void GuardarDatosBeneficiario()
        {
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
                int digito = _DigitoVerificacion.ValidarDigitoVerificacion(txtNumdocumentobenefi.Text.Trim());
                DigitVerif = Convert.ToInt16(digito);
            }
            else
            {
                DigitVerif = Convert.ToInt16(txtNitVerificacionBenefi.Text.Trim());
            }
            if (Existebeneficiario == true)
            {
                MessageBoxResult result1 = MessageBox.Show("Este Beneficiario ya  Existe. Desea Actualizar sus datos?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result1 == MessageBoxResult.Yes)
                {
                    //agregar método Update
                    respuesta = PropiBeneBll.UpdateDAtosBeneficiario(txtNumdocumentobenefi.Text.Trim(), txtnombresbenefi.Text.TrimStart(),
                                       txtapellidobenefi1.Text.Trim(), txtapellidobenefi2.Text.Trim(), cbbtipodocumentobenefi.SelectedValue.ToString(),
                                       cbbciudadbenefi.SelectedValue.ToString(), txtcelbenefi.Text, txttelefonobenefi.Text, txtdireccionbenefi.Text,
                                       ListaRiesgo, FechaEstudio, txtplaca.Text, txtobservacionestenedor.Text, usuario, DigitVerif, txtIndicativobenefi.Text.Trim(), txtemail.Text.Trim());

                    mostrarmensaje(respuesta);
                    if (respuesta == true)
                    {
                        Habilitarbotonesconductor(true);
                        txtNumdocumento.IsEnabled = false;
                    }
                }
            }
            else
            {
                respuesta = PropiBeneBll.GuardarDatosBeneficiario(txtNumdocumentobenefi.Text.Trim(), txtnombresbenefi.Text.TrimStart(),
                                   txtapellidobenefi1.Text.Trim(), txtapellidobenefi2.Text.Trim(), cbbtipodocumentobenefi.SelectedValue.ToString(),
                                   cbbciudadbenefi.SelectedValue.ToString(), txtcelbenefi.Text, txttelefonobenefi.Text, txtdireccionbenefi.Text,
                                   ListaRiesgo, FechaEstudio, PagoCierre, usuario, txtplaca.Text, txtobservacionestenedor.Text, DigitVerif, txtIndicativobenefi.Text.Trim(), txtemail.Text.Trim());
                PropiBeneBll.GuardaAuditoriaCambioBeneficiario_Propietario(usuario, txtNumdocumentopropi.Text.Trim(), "", txtNumdocumentobenefi.Text.Trim(), "guardó beneficiario   nuevo", txtplaca.Text.Trim());
                mostrarmensaje(respuesta);
                if (respuesta == true)
                {
                    Habilitarbotonesconductor(true);
                    txtNumdocumento.IsEnabled = false;
                }
            }
        }

        private void guardarDatosConductor()
        {
            var item = (TipoAfiliacion)cbbtipoafiliacion.SelectedItem;
            if (item != null)
            {
                if (item.IdCodigo == 1)
                {
                    MessageBox.Show("Los datos de conductores Asosiados a Hojas de vida de vehículos propios, no tienen permitidas modificaciones en vehitanques.");
                    return;
                }
            }
            string usuario = "";
            string agencia = "";
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

            Nullable<DateTime> FechapagoSSocial, fechaestudio;
            DateTime fechanaciemiento, FechaPagoPlanillaSalud, FechaUltExamenMedico;
            //fechanaciemiento = null;
            FechapagoSSocial = null;
            fechaestudio = null;
            Boolean listaRiesgo;
            short tipocarga = 3;
            Boolean afiliado = false, respuesta;
            if (cbbbanco.SelectedValue == null)
            {
                cbbbanco.SelectedIndex = 1;
            }
            string idbanco = cbbbanco.SelectedValue.ToString();
            int Id_TipoAfiliacion = 2;
            if (rdbafiliado.IsChecked == true)
            {
                afiliado = false;
                Id_TipoAfiliacion = 2;
            }
            else
            {
                afiliado = false;
                Id_TipoAfiliacion = 2;
            }
            ///cambio solicitado esperanza becerra 09/05/2019---solo se autoriza a guardar terceros--
            //else
            //{
            //    afiliado = true;
            //    Id_TipoAfiliacion=1;
            //}

            if (rdbescolta.IsChecked == true)
            {
                Id_TipoAfiliacion = 3;
            }
            int tipopago;
            if (rdbeletronico.IsChecked == true)
            {
                tipopago = 1;
            }
            else
            {
                tipopago = 2;
            }
            if (rdbaltocondu.IsChecked == true)
            {
                tipocarga = 1;
            }
            if (rdbmediocondu.IsChecked == true)
            {
                tipocarga = 2;
            }
            if (rdbbajocondu.IsChecked == true)
            {
                tipocarga = 3;
            }
            if (rdblistaaprueba.IsChecked == true)
            {
                listaRiesgo = true;
            }
            else
            {
                listaRiesgo = false;
            }
            if (dtfechaEstudioConductor.SelectedDate != null)
            {
                fechaestudio = Convert.ToDateTime(dtfechaEstudioConductor.SelectedDate);
            }
            if (dtfechaPagoSSocial.SelectedDate != null)
                FechapagoSSocial = Convert.ToDateTime(dtfechaPagoSSocial.SelectedDate);
            if (dtfechanacimieto.SelectedDate != null)
                fechanaciemiento = Convert.ToDateTime(dtfechanacimieto.SelectedDate);
            else
            {
                fechanaciemiento = Convert.ToDateTime(FechaActual); ;
            }
            var lista = ConducBll.GetDAtosconductor(txtNumdocumento.Text);
            if (lista.Count > 0)
            {
                ExisteConductor = true;
            }
            else
            {
                ExisteConductor = false;
            }
            FechaPagoPlanillaSalud = Convert.ToDateTime(dtFechaPagoSalud.SelectedDate);
            FechaUltExamenMedico = Convert.ToDateTime(dtFechaExamenes.SelectedDate);
            if (ExisteConductor == true)
            {
                MessageBoxResult result1 = MessageBox.Show("Este conductor ya  Existe. Desea Actualizar los datos del conductor?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result1 == MessageBoxResult.Yes)
                {
                    if (Convert.ToInt32(cbbtipoafiliacion.SelectedValue) == 1)
                    {
                        ValidarInfobancaria(false);
                    }
                    respuesta = ConducBll.ActualizaDatosConductor(txtNumdocumento.Text, Convert.ToInt16(agencia), txtnombres.Text, txtnumlicencia.Text, Convert.ToInt32(cbbcateglicencia.SelectedValue), Convert.ToDateTime(dtfechalicencia.SelectedDate), txtdireccioncasa.Text,
                  txttelefonocasa.Text, txtcelular.Text, cbbciudadconductor.SelectedValue.ToString(), txtnombreferencia.Text, txttelreferencia.Text,
                  fechanaciemiento, afiliado, usuario, txtobservacionesconductor.Text,
                  txtARL.Text, txteps.Text, Convert.ToInt32(txtexperiencia.Text), FechapagoSSocial, listaRiesgo, fechaestudio,
                  txtobservacionesruta.Text, tipocarga, txtapellido1.Text, txtapellido2.Text, txtnombreempresa1.Text, txttelempresa1.Text, txtantiguedadempresa1.Text,
                  txttipocargaempresa1.Text, txtnombreempresa2.Text, txttelempresa2.Text, txtantiguedadempresa2.Text, txttipocargaempresa2.Text,
                 txtplaca.Text, txtIndicativo.Text.Trim(), tipopago, Id_TipoAfiliacion, FechaPagoPlanillaSalud, FechaUltExamenMedico);
                    mostrarmensaje(respuesta);
                    if (respuesta == true)
                    {
                        Habilitarbotonesconductor(true);
                        txtNumdocumento.IsEnabled = false;
                        HabilitarInfoBancaria(false);
                    }
                }
            }
            else
            {
                if (NoCrearConductores == false)
                {
                    switch (Convert.ToInt32(cbbtipoafiliacion.SelectedValue))
                    {
                        case 2:
                            if (ValidarInfobancaria(true) == false)
                            {
                                respuesta = ConducBll.GuardarDatosConductor(txtNumdocumento.Text, Convert.ToInt16(agencia), txtnombres.Text, txtnumlicencia.Text, Convert.ToInt32(cbbcateglicencia.SelectedValue), Convert.ToDateTime(dtfechalicencia.SelectedDate), txtdireccioncasa.Text,
                                                   txttelefonocasa.Text, txtcelular.Text, cbbciudadconductor.SelectedValue.ToString(), txtnombreferencia.Text, txttelreferencia.Text,
                                                   Convert.ToDateTime(dtfechanacimieto.SelectedDate), afiliado, usuario, txtobservacionesconductor.Text,
                                                   txtARL.Text, txteps.Text, Convert.ToInt32(txtexperiencia.Text), FechapagoSSocial, listaRiesgo, fechaestudio,
                                                   txtobservacionesruta.Text, tipocarga, txtapellido1.Text, txtapellido2.Text, txtnombreempresa1.Text, txttelempresa1.Text, txtantiguedadempresa1.Text,
                                                   txttipocargaempresa1.Text, txtnombreempresa2.Text, txttelempresa2.Text, txtantiguedadempresa2.Text, txttipocargaempresa2.Text,
                                                  txtplaca.Text, idbanco, cbbtipocuenta.SelectedValue.ToString(), txtnitcuenta.Text, txtidtarjeta.Text, txtnumcuenta.Text, txtIndicativo.Text.Trim(), tipopago, Id_TipoAfiliacion, FechaPagoPlanillaSalud, FechaUltExamenMedico);
                                mostrarmensaje(respuesta);
                                if (respuesta == true)
                                {
                                    Habilitarbotonesconductor(true);
                                    txtNumdocumento.IsEnabled = false;
                                    HabilitarInfoBancaria(false);
                                }
                            }
                            break;
                        default:
                            respuesta = ConducBll.GuardarDatosConductor(txtNumdocumento.Text, Convert.ToInt16(agencia), txtnombres.Text, txtnumlicencia.Text, Convert.ToInt32(cbbcateglicencia.SelectedValue), Convert.ToDateTime(dtfechalicencia.SelectedDate), txtdireccioncasa.Text,
                                                       txttelefonocasa.Text, txtcelular.Text, cbbciudadconductor.SelectedValue.ToString(), txtnombreferencia.Text, txttelreferencia.Text,
                                                       Convert.ToDateTime(dtfechanacimieto.SelectedDate), afiliado, usuario, txtobservacionesconductor.Text,
                                                       txtARL.Text, txteps.Text, Convert.ToInt32(txtexperiencia.Text), FechapagoSSocial, listaRiesgo, fechaestudio,
                                                       txtobservacionesruta.Text, tipocarga, txtapellido1.Text, txtapellido2.Text, txtnombreempresa1.Text, txttelempresa1.Text, txtantiguedadempresa1.Text,
                                                       txttipocargaempresa1.Text, txtnombreempresa2.Text, txttelempresa2.Text, txtantiguedadempresa2.Text, txttipocargaempresa2.Text,
                                                      txtplaca.Text, idbanco, cbbtipocuenta.SelectedValue.ToString(), txtnitcuenta.Text, txtidtarjeta.Text, txtnumcuenta.Text, txtIndicativo.Text.Trim(), tipopago, Id_TipoAfiliacion, FechaPagoPlanillaSalud, FechaUltExamenMedico);
                            mostrarmensaje(respuesta);
                            if (respuesta == true)
                            {
                                Habilitarbotonesconductor(true);
                                txtNumdocumento.IsEnabled = false;
                                HabilitarInfoBancaria(false);
                            }
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Su Perfil No Tiene Autorizado Crear Conductores,Para pedir Modificación Comuniquese con el administrador del sistema");
                }
            }
        }

        private Boolean ValidarInfobancaria(Boolean Nuevo)
        {
            Boolean respuesta = false;
            if (Convert.ToInt32(cbbtipoafiliacion.SelectedValue) == 2)
            {
                var listabenefi = PropiBeneBll.GetBeneficiarioporplaca(txtplaca.Text);
                //if (lista[0].NitCuenta != listabenefi[0].IdNit)
                //{
                //    if (lista[0].NitCuenta != idcondoctor)
                //    {
                //        MessageBox.Show("El nit de cuenta del conductor no coincide con Id del conductor ni con Nit del propietario");
                //    }
                //}   
                if (listabenefi.Count > 0)
                {
                    if (txtnitcuenta.Text != "")
                    {
                        if (txtnitcuenta.Text != txtNumdocumento.Text)
                        {
                            string nitcuenta = txtnitcuenta.Text;
                            string idpropietario = listabenefi[0].IdNit;
                            if (idpropietario != "")
                            {
                                if (nitcuenta != idpropietario)
                                {
                                    MessageBox.Show("El número de nit cuenta no coincide con el Id conductor ni con el Id del propietario");
                                    respuesta = true;
                                    if (Nuevo == false)
                                    {
                                        EnviarMailErrorCuentas(txtnitcuenta.Text, listabenefi[0].IdNit, txtNumdocumento.Text, txtplaca.Text);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("El número de nit cuenta no coincide con el Id conductor");
                                respuesta = true;
                                if (Nuevo == false)
                                {

                                    EnviarMailErrorCuentas(txtnitcuenta.Text, listabenefi[0].IdNit, txtNumdocumento.Text, txtplaca.Text);

                                }
                            }
                        }
                    }
                }
            }
            return respuesta;
        }

        private void GuardarContrato()
        {
            string usuario = "";
            int agencia = 0;
            DateTime fechacontrato;
            Boolean respuesta;
            fechacontrato = (DateTime)dtfechacontrato.SelectedDate;
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
                            agencia = Convert.ToInt32(textblock.Text);
                            break;
                    }
                    if (usuario != "" & agencia != 0)
                    {
                        break;
                    }
                }
            }
            respuesta = contratobll.GuardarContrato(Convert.ToInt32(txtnumerocontrato.Text), txtplaca.Text, txtNumdocumento.Text, fechacontrato, usuario, agencia);
            mostrarmensaje(respuesta);
            if (respuesta == true)
            {
                btn_contratocoltan.IsEnabled = true;
            }
        }

        public void GuardarInformacion()
        {
            TabItem ti = tabHVVehiculo.SelectedItem as TabItem;
            switch (ti.Name)
            {
                case "tabvehiremol":
                    if (modificavehiculo_remolque == true)
                    {
                        if (ValidarCamposVehiculo() == false)
                        {
                            if (chk_remolque.IsChecked == true)
                            {
                                if (txtplacaremolque.Text != "")
                                {
                                    if (validarcamposremolque() == false)
                                    {
                                        if (actualizarvehi != true)
                                        {
                                            if (ValidarExisteplaca() == false)
                                            {
                                                guardarDatosvehiculo();
                                            }
                                            else
                                            {
                                                Mensabll = new MensajeBLL();
                                                Mensabll.tipomensajes(22, "");
                                                Venmsj = new VentanaMensaje();
                                                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                                                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                                                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                                                Venmsj.ShowDialog();
                                            }
                                        }
                                        else
                                        {
                                            guardarDatosvehiculo();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (actualizarvehi != true)
                                {
                                    if (ValidarExisteplaca() == false)
                                    {
                                        guardarDatosvehiculo();
                                    }
                                    else
                                    {
                                        Mensabll = new MensajeBLL();
                                        Mensabll.tipomensajes(22, "");
                                        Venmsj = new VentanaMensaje();
                                        Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                                        Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                                        Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                                        Venmsj.ShowDialog();
                                    }
                                }
                                else
                                {
                                    guardarDatosvehiculo();
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Esta función no se encuentra habilitada para este panel.");
                    }
                    break;
                case "tabconducto":
                    var listaplaca = vehibll.ConsultaExistenciaVehiculo(txtplaca.Text.Trim());
                    if (listaplaca.Count > 0)
                    {
                        if (ValidarCAmposConductor() == false)
                        {
                            if (actualizarvehi != true)
                            {
                                if (ValidarConductorAsociadoplaca() == false)
                                {
                                    guardarDatosConductor();
                                }
                                else
                                {
                                    MessageBoxResult result1 = MessageBox.Show("Este conductor ya está asociado a esta placa. Desea Actualizar los datos del conductor?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                    if (result1 == MessageBoxResult.Yes)
                                    {
                                        ExisteConductor = true;
                                        guardarDatosConductor();
                                        Habilitarbotonesconductor(true);
                                    }
                                }
                            }
                            else
                            {
                                if (actualizarvehi == true)
                                {
                                    guardarDatosConductor();
                                    Habilitarbotonesconductor(true);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No puede asociar o crear un conductor con una placa que no Registra en base de datos", "ADVERTENCIA", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    break;
                case "tabpropie":
                    if (modificabenefi_propie == true)
                    {
                        validarDatospro_Beneficiario();
                        //if (ValidarDatosPropietarioBeneficiario() == false)
                        //{
                        //    GuardarDatosBeneficiario();
                        //    GuardarDatosPropietario();
                        //    //if (actualizarvehi != true)
                        //    //{
                        //    //    GuardarDatosBeneficiario();
                        //    //    GuardarDatosPropietario();
                        //    //}
                        //    //else
                        //    //{
                        //    //    GuardarDatosBeneficiario();
                        //    //    GuardarDatosPropietario();
                        //    //}
                        //}
                    }
                    else
                    {
                        MessageBox.Show("Esta función no se encuentra habilitada para este panel.");
                    }
                    break;
                case "tabcontrato":
                    if (ValidarDatosContrato() == false)
                    {
                        var lista = contratobll.GetExistenciaContrato(Convert.ToInt32(txtnumerocontrato.Text));
                        if (lista.Count <= 0)
                        {
                            MessageBoxResult result1 = MessageBox.Show("Está seguro que desea guardar información del contrato COLTANQUES?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result1 == MessageBoxResult.Yes)
                            {
                                GuardarContrato();
                            }
                        }
                        else
                        {
                            Mensabll = new MensajeBLL();
                            Mensabll.tipomensajes(33, "");
                            Venmsj = new VentanaMensaje();
                            Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                            Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                            Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes + lista[0].IdPlaca);
                            Venmsj.ShowDialog();
                        }
                    }
                    break;
                default:
                    //MessageBox.Show("Esta función no se encuentra habilitada para esta ventana.");
                    break;
            }
        }

        #endregion

        #region CargarListas desplegables Desde Base de datos

        private void cargarEstadoPlacaAprobacion()
        {
            var lista = establl.GetEstadoPlaca();
            lista = lista.Where(p => p.Estado != 3).ToList();
            cbbestadoaprobacion.ItemsSource = lista;
            cbbestadoaprobacion.SelectedValuePath = "Estado";
            cbbestadoaprobacion.DisplayMemberPath = "Descripcion";
            cbbestadoaprobacion.SelectedValue = 2;
        }

        private void CargarLinea(int idmarca)
        {
            int idmatca2 = GetIDminTransmarcaPorID(idmarca);
            var lista = linBll.GetlineaPorMarca(idmatca2);
            if (lista.Count > 0)
            {
                cbblinea.ItemsSource = lista;
                cbblinea.SelectedValuePath = "IdLinea";
                cbblinea.DisplayMemberPath = "descripcion";
                cbblinea.SelectedIndex = 0;
            }
            else
            {
                cbblinea.ItemsSource = null;
                Mensabll = new MensajeBLL();
                Mensabll.tipomensajes(3, "");
                Venmsj = new VentanaMensaje();
                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                Venmsj.ShowDialog();
            }
        }

        private void CargarUnidadMedida()
        {
            var lista = uniBll.GetUnidadMedida();
            cbbunidadremolque.ItemsSource = lista;
            cbbunidadremolque.SelectedValuePath = "Codigo";
            cbbunidadremolque.DisplayMemberPath = "Nombre";
            cbbunidadremolque.SelectedIndex = 0;
        }

        private void CargarGrupoLLantas()
        {
            var lista = LlanBll.GetGrupollantas();
            cbbgrupollantas.ItemsSource = lista;
            cbbgrupollantas.SelectedValuePath = "Cod";
            cbbgrupollantas.DisplayMemberPath = "Des";
            cbbgrupollantas.SelectedIndex = 0;
        }

        private void CargarTipoDocumentos()
        {
            var lista = TiDocBll.GetTipoDocumentoIdentificacion();
            cbbtipodocumento.ItemsSource = lista;
            cbbtipodocumento.SelectedValuePath = "Codigo";
            cbbtipodocumento.DisplayMemberPath = "TipoDocumento";
            cbbtipodocumento.SelectedIndex = 0;

            cbbtipodocumentobenefi.ItemsSource = lista;
            cbbtipodocumentobenefi.SelectedValuePath = "Codigo";
            cbbtipodocumentobenefi.DisplayMemberPath = "TipoDocumento";
            cbbtipodocumentobenefi.SelectedIndex = 1;

            cbbtipodocumentopropi.ItemsSource = lista;
            cbbtipodocumentopropi.SelectedValuePath = "Codigo";
            cbbtipodocumentopropi.DisplayMemberPath = "TipoDocumento";
            cbbtipodocumentopropi.SelectedIndex = 1;
        }

        private void CargarCategorialicencia()
        {
            var lista = licenBll.GetCategoriaLicencia();
            cbbcateglicencia.ItemsSource = lista;
            cbbcateglicencia.SelectedValuePath = "Cat_Id";
            cbbcateglicencia.DisplayMemberPath = "Cat_Descripcion";
            cbbcateglicencia.SelectedIndex = 0;
        }

        private void cargarTipoCuentacompaniasatelital()
        {
            var lista = empsatBll.GEtCompaniaSatelital();
            cbbempreSatelital.ItemsSource = lista;
            cbbempreSatelital.SelectedValuePath = "IdEmpresa";
            cbbempreSatelital.DisplayMemberPath = "Nombre";
            cbbempreSatelital.SelectedIndex = 0;
        }

        private void cargarTipoCuenta()
        {
            var lista = CuenBll.GetTiposcuenta();
            cbbtipocuenta.ItemsSource = lista;
            cbbtipocuenta.SelectedValuePath = "IdTipoCuenta";
            cbbtipocuenta.DisplayMemberPath = "Descripcion";
            cbbtipocuenta.SelectedIndex = 0;
        }

        private void cargarAgencias()
        {
            var lista = AgenBll.GetAgencias();
            cbbagenciabenefi.ItemsSource = lista;
            cbbagenciabenefi.SelectedValuePath = "Codigo";
            cbbagenciabenefi.DisplayMemberPath = "Nombre";
            cbbagenciabenefi.SelectedIndex = 0;
        }

        private void cargarBancos()
        {
            var lista = BanBll.GetBancosConductor();
            cbbbanco.ItemsSource = lista;
            cbbbanco.SelectedValuePath = "CodACH";
            cbbbanco.DisplayMemberPath = "Nombre";
            cbbbanco.SelectedIndex = 0;
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

        private void cargarDepartamentos()
        {
            var lista = DeparBll.GetDepartamentos(1);
            cbbdeparconductor.ItemsSource = lista;
            cbbdeparconductor.SelectedValuePath = "Codigo";
            cbbdeparconductor.DisplayMemberPath = "Nombre";
            cbbdeparconductor.SelectedIndex = 0;
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

        private void cargarCiudadesconductor(int departamento)
        {
            var lista = CiuBll.GetCiudades(departamento);
            cbbciudadconductor.ItemsSource = lista;
            cbbciudadconductor.SelectedValuePath = "CodigoReducido";
            cbbciudadconductor.DisplayMemberPath = "Nombre";
            cbbciudadconductor.SelectedIndex = 0;
        }

        private void cargarEstadoPlaca()
        {
            var lista = establl.GetEstadoPlaca();
            cbbestado.ItemsSource = lista;
            cbbestado.SelectedValuePath = "Estado";
            cbbestado.DisplayMemberPath = "Descripcion";
            cbbestado.SelectedValue = 2;
        }

        private void cargarColores()
        {
            var lista = ColBll.Getcolores();
            cbbcolor.ItemsSource = lista;
            cbbcolor.SelectedValuePath = "CODIGO";
            cbbcolor.DisplayMemberPath = "DESCRIPCION";
            cbbcolor.SelectedIndex = 0;
        }

        private void cargarMarcaVehiculo()
        {
            var lista = MarBll.GetMarcasVehiculo();
            cbbmarca.ItemsSource = lista;
            cbbmarca.SelectedValuePath = "IdMarca";
            cbbmarca.DisplayMemberPath = "Descripcion";
            cbbmarca.SelectedIndex = 0;
        }

        private void cargarMarcaRemolque()
        {
            var lista = MarBll.GetMarcasRemolque();
            cbbmarcaremolque.ItemsSource = lista;
            cbbmarcaremolque.SelectedValuePath = "IdMarcaRO";
            cbbmarcaremolque.DisplayMemberPath = "Descripcion";
            cbbmarcaremolque.SelectedIndex = 0;
        }

        private void CargarclaseTanque()
        {
            var lista = TanBll.GetClaseTanque();
            cbbclase.ItemsSource = lista;
            ClaseTanque claset = new ClaseTanque();
            claset.IdClase = 999;
            claset.NombreClase = "No aplica";
            lista.Add(claset);
            cbbclase.SelectedValuePath = "IdClase";
            cbbclase.DisplayMemberPath = "NombreClase";
            cbbclase.SelectedValue = 999;
        }

        private void CargarTipoMotor()
        {
            var lista = MotBll.GetTipoMotorVehiculo();
            cbbtipomotor.ItemsSource = lista;
            cbbtipomotor.SelectedValuePath = "Codigo";
            cbbtipomotor.DisplayMemberPath = "Descripcion";
            cbbtipomotor.SelectedIndex = 0;
        }

        private void CargarTipoVehiculos()
        {
            var lista = vehibll.GetTiposVehiculo();
            cbbtipovehiculo.ItemsSource = lista;
            cbbtipovehiculo.SelectedValuePath = "Codigo";
            cbbtipovehiculo.DisplayMemberPath = "Descripcion";
            cbbtipovehiculo.SelectedIndex = 0;
        }

        private void CargarEjesVehiculo(int idejes)
        {
            var lista = vehibll.GetEjesVehiculo();
            switch (idejes)
            {
                case 61:
                    cbbejes.ItemsSource = lista;
                    cbbejes.SelectedValuePath = "IdEje";
                    cbbejes.DisplayMemberPath = "Nombre";
                    cbbejes.SelectedIndex = 0;
                    break;
                default:
                    var lista1 = lista.Where(a => a.IdEje <= 3).ToList();
                    cbbejes.ItemsSource = lista;
                    cbbejes.SelectedValuePath = "IdEje";
                    cbbejes.DisplayMemberPath = "Nombre";
                    cbbejes.SelectedIndex = 0;
                    break;
            }
        }

        private void CargarTipoAfiliacion()
        {
            var lista = AfilBll.GetTipoAfiliacion();
            cbbtipoafiliacion.ItemsSource = lista;
            cbbtipoafiliacion.SelectedValuePath = "IdCodigo";
            cbbtipoafiliacion.DisplayMemberPath = "Descripcion";
            cbbtipoafiliacion.SelectedValue = 2;
            /////codigo nuevo
            /////26/11/2018
            //if (boolmodificaipoafiliacion != true)
            //{
            //    cbbtipoafiliacion.IsEnabled = false;
            //}
            //else
            //{
            //    cbbtipoafiliacion.IsEnabled = true ;
            //}
            ///// fin codigo nuevo
            /////26/11/2018
        }

        private void CargarTipoCarroceria()
        {
            var lista = CarBLL.GetTipoCarroceria();
            cbbtipocarroceria.ItemsSource = lista;
            cbbtipocarroceria.SelectedValuePath = "Tipo";
            cbbtipocarroceria.DisplayMemberPath = "Nombre";
            cbbtipocarroceria.SelectedIndex = 0;
            cbbtiporemolque.ItemsSource = lista;
            cbbtiporemolque.SelectedValuePath = "Tipo";
            cbbtiporemolque.DisplayMemberPath = "Nombre";
            cbbtiporemolque.SelectedIndex = 0;
        }

        private void CargarTipoCarroceriaRemolque()
        {
            var lista = CarBLL.GetTipoCarroceria();
            cbbtiporemolque.ItemsSource = lista;
            cbbtiporemolque.SelectedValuePath = "Tipo";
            cbbtiporemolque.DisplayMemberPath = "Nombre";
            cbbtiporemolque.SelectedIndex = 0;
        }

        private void CargarAseguradoras()
        {
            var lista = AseBll.GetASeguradora();
            cbbciasoat.ItemsSource = lista;
            cbbciasoat.SelectedValuePath = "ID";
            cbbciasoat.DisplayMemberPath = "Nombre";
            cbbciasoat.SelectedIndex = 0;

            cbbcomRCE.ItemsSource = lista;
            cbbcomRCE.SelectedValuePath = "ID";
            cbbcomRCE.DisplayMemberPath = "Nombre";
            cbbcomRCE.SelectedIndex = 0;
        }

        private void CargarNivelRiesgoCarga()
        {
            cbNivelRiesgo.ItemsSource = null;
            List<Vehit_NivelRiestoConductor> tblNivelRiesgo = contextVehit.Vehit_NivelRiestoConductor.Where(x => x.Activo == true).OrderBy(x => x.IdNivelRiesgoCond).ToList();
            if (tblNivelRiesgo.Count > 0)
            {
                cbNivelRiesgo.ItemsSource = tblNivelRiesgo;
                cbNivelRiesgo.SelectedValuePath = "IdNivelRiesgoCond";
                cbNivelRiesgo.DisplayMemberPath = "NivelRiestoCond";
                cbNivelRiesgo.SelectedIndex = 0;
            }
            cbNivelRiesgoActual.ItemsSource = null;
            List<Vehit_NivelRiestoConductor> tblNivelRiesgoAct = contextVehit.Vehit_NivelRiestoConductor.Where(x => x.Activo == true).OrderBy(x => x.IdNivelRiesgoCond).ToList();
            if (tblNivelRiesgoAct.Count > 0)
            {
                cbNivelRiesgoActual.ItemsSource = tblNivelRiesgoAct;
                cbNivelRiesgoActual.SelectedValuePath = "IdNivelRiesgoCond";
                cbNivelRiesgoActual.DisplayMemberPath = "NivelRiestoCond";
                cbNivelRiesgoActual.SelectedIndex = 0;
            }
        }

        #endregion

        #region metodo cargar informacion desde Base de datos

        private void CargarEstadoplacaVehiculo()
        {
            if (strplaca != "")
            {
                var lista = vehibll.GetEstadoPlaca(strplaca);
                if (lista.Count > 0)
                {
                    lblplaca.Content = strplaca.ToUpper();
                    if (lista[0].EstadoPlaca != 3)
                    {
                        cbbestadoaprobacion.SelectedValue = lista[0].EstadoPlaca;
                    }
                    txtobservacionesaprobacion.Text = lista[0].Observacionbloqueo;
                }
            }
        }

        public void CargarContrato()
        {
            var lista = contratobll.GetContratoX_placa_Conductor(txtplaca.Text, txtNumdocumento.Text);
            if (lista.Count > 0)
            {
                gridlistadocontratos.ItemsSource = null;
                CargarGrilla(lista);
            }
        }

        private void CargarGrilla(List<VehiT_ConsultaContrato_Result> lista)
        {
            gridlistadocontratos.ItemsSource = lista;
            if (habilitarbotoncontrato == true)
            {
                gridlistadocontratos.Columns[0].Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                gridlistadocontratos.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private Boolean validarNumeroCelular(string numcel)
        {
            Boolean respuesta = false;
            if (numcel.Length < 10)
            {
                respuesta = true;
            }
            return respuesta;
        }

        private void CalcularEdad(DateTime dtFechaNacimiento)
        {
            int intEdad = 0;
            DateTime dtFechaSistema = DateTime.Now;
            CRM_FechaSistemaBD_Result tblFecha = contextVehit.CRM_FechaSistemaBD().FirstOrDefault();
            if (tblFecha != null)
            {
                dtFechaSistema = tblFecha.FECHA_ACTUAL;
            }
            TimeSpan age = dtFechaSistema - dtFechaNacimiento;
            DateTime totalTime = new DateTime(age.Ticks);
            intEdad = totalTime.Year;
            Vehit_NivelRiestoConductor tblRiesgo = contextVehit.Vehit_NivelRiestoConductor.Where(x => x.EdadMax == intEdad).FirstOrDefault();
            if (tblRiesgo != null)
            {
                if (tblRiesgo.IdNivelRiesgoCond == 2)
                {
                    rdbmediocondu.IsChecked = true;
                    rdbbajocondu.IsEnabled = false;
                    rdbaltocondu.IsEnabled = false;
                    MessageBox.Show("Se cambia el tipo carga a Riesgo Medio por la edad del conductor (" + intEdad.ToString() + " años).");
                }
                else if (tblRiesgo.IdNivelRiesgoCond == 3)
                {
                    rdbbajocondu.IsChecked = true;
                    rdbmediocondu.IsEnabled = false;
                    rdbaltocondu.IsEnabled = false;
                    MessageBox.Show("Se cambia el tipo carga a Riesgo Bajo por la edad del conductor (" + intEdad.ToString() + " años).");
                }
                else if (rdbbajocondu.IsEnabled == false && rdbaltocondu.IsEnabled == false)
                {
                    rdbbajocondu.IsEnabled = true;
                    rdbaltocondu.IsEnabled = true;
                }
                else if (rdbmediocondu.IsEnabled == false && rdbaltocondu.IsEnabled == false)
                {
                    rdbaltocondu.IsEnabled = true;
                    rdbmediocondu.IsEnabled = true;
                }
            }
            else if (intEdad > 67)
            {
                rdbbajocondu.IsChecked = true;
                rdbmediocondu.IsEnabled = false;
                rdbaltocondu.IsEnabled = false;
                MessageBox.Show("Se cambia el tipo carga a Riesgo Bajo por la edad del conductor (" + intEdad.ToString() + " años).");
            }
            else if (rdbmediocondu.IsEnabled == false && rdbaltocondu.IsEnabled == false)
            {
                rdbaltocondu.IsEnabled = true;
                rdbmediocondu.IsEnabled = true;
            }
        }

        public void cargarinfo()
        {
            if (strplaca != "")
            {
                var lista = vehibll.GetHojaDeVidaVehiculo(strplaca);
                if (lista.Count > 0)
                {
                    if (lista[0].EstadoPlaca == 3)
                    {
                        if (cambiarestado == false)
                        {
                            main.btn_grabar.IsEnabled = false;
                            MessageBox.Show("Puede Ver la información, pero no podrá guardar cambios,por favor comunicarse con el aerea de seguridad de coltanques.");
                        }
                        else
                        {
                            main.btn_grabar.IsEnabled = true;
                        }
                    }
                    HabilitarInfoBancaria(false);
                    if (aprobarPolRC == true)
                    {
                        switch (lista[0].Poliza_RC_Activa)
                        {
                            case true:
                                btn_Aprobar.IsEnabled = false;
                                btn_Noaprobar.IsEnabled = true;
                                lblmensajepolizaRC.Content = "Póliza RC Activa";
                                break;
                            case false:
                                btn_Aprobar.IsEnabled = true;
                                btn_Noaprobar.IsEnabled = false;
                                lblmensajepolizaRC.Content = "Póliza RC Inactiva";
                                break;
                            default:
                                btn_Aprobar.IsEnabled = true;
                                btn_Noaprobar.IsEnabled = false;
                                lblmensajepolizaRC.Content = "Póliza RC Inactiva";
                                break;
                        }
                    }
                    else
                    {
                        btn_Aprobar.IsEnabled = false;
                        btn_Noaprobar.IsEnabled = false;
                        switch (lista[0].Poliza_RC_Activa)
                        {
                            case true:
                                lblmensajepolizaRC.Content = "Póliza RC Activa";
                                break;
                            case false:
                                lblmensajepolizaRC.Content = "Póliza RC Inactiva";
                                break;

                            default:
                                lblmensajepolizaRC.Content = "Póliza RC Inactiva";
                                break;
                        }
                    }
                    if (lista[0].Usuario != "")
                    {
                        txtusuariomodifico.Text = lista[0].Usuario;
                    }
                    else
                    {
                        txtusuariomodifico.Text = "No Registra";
                    }
                    txtplaca.Text = lista[0].IdPlaca;
                    txtplaca.IsEnabled = false;
                    txtplaca.IsReadOnly = true;
                    if (lista[0].Contrato == true)
                    {
                        chk_contraro.IsChecked = true;
                    }
                    else
                    {
                        chk_contraro.IsChecked = false;
                    }
                    if (lista[0].CodigoColor != null)
                    {
                        if (lista[0].CodigoColor != Convert.ToString(2) & lista[0].CodigoColor != Convert.ToString(136) & lista[0].CodigoColor != Convert.ToString(135) & lista[0].CodigoColor != Convert.ToString(127) & lista[0].CodigoColor != Convert.ToString(1))
                        {
                            cbbcolor.SelectedValue = Convert.ToInt32(lista[0].CodigoColor);
                            cbbcolor.IsEnabled = false;
                        }
                        else
                        {
                            cbbcolor.SelectedIndex = 0;
                            cbbcolor.IsEnabled = true;
                        }
                    }
                    else
                    {
                        cbbcolor.SelectedIndex = 0;
                        cbbcolor.IsEnabled = true;
                    }
                    if (udpcolor == false)
                    {
                        cbbcolor.IsEnabled = false;
                    }
                    else
                    {
                        cbbcolor.IsEnabled = true;
                    }
                    txtmodelo.Text = lista[0].Modelo;
                    cbbmarca.SelectedValue = lista[0].Marca;
                    CargarLinea(lista[0].Marca);
                    try
                    {
                        CargarLinea(lista[0].Marca);
                        cbblinea.SelectedValue = lista[0].LineaRNDC;
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                        Mensabll = new MensajeBLL();
                        Mensabll.tipomensajes(4, "");
                        //Venmsj = new VentanaMensaje();
                        //Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                        //Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                        DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdPlaca);
                        //Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                        //Venmsj.ShowDialog();
                    }
                    cbbestado.SelectedValue = lista[0].EstadoPlaca;
                    txttarjetapropiedad.Text = lista[0].TarjetadePropiedad;
                    cbbgrupollantas.SelectedValue = lista[0].GrupoLlantas;
                    txtchasis.Text = lista[0].ChaSerie;
                    cbbtipomotor.SelectedValue = lista[0].TipoMotor;
                    txtnumotor.Text = lista[0].NumeroMotor;
                    cbbtipoafiliacion.SelectedValue = lista[0].TipoAfiliacion;
                    ///codigo nuevo
                    ///23/05/2019
                    ///
                    if (boolmodificaConductorespropios == true)
                    {

                    }
                    else
                    {

                    }
                    if (lista[0].TipoAfiliacion == 1)
                    {
                        pnl_datosconductor.IsEnabled = false;
                        boolmodificaConductorespropios = false;
                    }
                    else
                    {
                        pnl_datosconductor.IsEnabled = true;
                    }
                    /// fin codigo nuevo
                    ///23/05/2019
                    ///
                    /// 
                    ///codigo nuevo
                    ///13/11/2018
                    if (boolmodificaipoafiliacion != true)
                    {
                        cbbtipoafiliacion.IsEnabled = false;
                    }
                    /// fin codigo nuevo
                    ///13/11/2018

                    cbbtipovehiculo.SelectedValue = lista[0].TipoVehiculo;
                    cbbtipovehiculo.IsEnabled = false;
                    cbbtipocarroceria.SelectedValue = lista[0].TipoCarroceria;

                    switch (lista[0].TipoCarroceria)
                    {
                        case 67:
                            Boolean polizaHC = false;
                            var listapoliza = polizabll.ConsultaHistoricopolizas(lista[0].IdPlaca, 1, 10);
                            if (listapoliza.Count > 0)
                            {
                                foreach (var itempol in listapoliza)
                                {
                                    if (FechaActual.SelectedDate < itempol.Fechafinal)
                                    {
                                        polizaHC = true;
                                    }
                                }
                                if (polizaHC == false)
                                {
                                    Mensabll = new MensajeBLL();
                                    Mensabll.tipomensajes(46, "");
                                    DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdPlaca);
                                }
                            }

                            if (chk_remolque.IsChecked == true)
                            {
                                cbbclase.IsEnabled = true;
                                cbbunidadremolque.SelectedValue = 4;
                                dtfeAforotanqueSencillo.SelectedDate = null;
                                dtfeAforotanqueSencillo.IsEnabled = false;
                                btn_aforotanqueSencillo.Visibility = Visibility.Collapsed;
                                dtfeHidrostaticaSencillo.SelectedDate = null;
                                dtfeHidrostaticaSencillo.IsEnabled = false;
                                btn_hidrostaticaSencillo.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                dtfeHidrostaticaSencillo.IsEnabled = true;
                                btn_hidrostaticaSencillo.Visibility = Visibility.Collapsed;
                                dtfeAforotanqueSencillo.IsEnabled = true;
                                btn_aforotanqueSencillo.Visibility = Visibility.Visible;
                            }
                            if (lista[0].FechaAforoSencillo != null)
                            {
                                dtfeAforotanqueSencillo.SelectedDate = lista[0].FechaAforoSencillo;
                            }
                            else
                            {
                                dtfeAforotanqueSencillo.SelectedDate = null;
                            }
                            if (lista[0].FechaHidrostatica != null)
                            {
                                dtfeHidrostaticaSencillo.SelectedDate = lista[0].FechaHidrostatica;
                            }
                            else
                            {
                                dtfeHidrostaticaSencillo.SelectedDate = null;
                            }
                            break;
                        default:

                            dtfeAforotanqueSencillo.SelectedDate = null;
                            dtfeAforotanqueSencillo.IsEnabled = false;
                            btn_aforotanqueSencillo.Visibility = Visibility.Collapsed;
                            dtfeHidrostaticaSencillo.SelectedDate = null;
                            dtfeHidrostaticaSencillo.IsEnabled = false;
                            btn_hidrostaticaSencillo.Visibility = Visibility.Collapsed;
                            cbbunidadremolque.SelectedValue = 8;
                            cbbclase.IsEnabled = false;
                            cbbclase.SelectedValue = 999;

                            break;
                    }
                    dtfechatmecanica.SelectedDate = lista[0].VigenciaRevisionMecan;
                    if (FechaActual.SelectedDate >= dtfechatmecanica.SelectedDate)
                    {
                        Mensabll = new MensajeBLL();
                        Mensabll.tipomensajes(6, "");
                        DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdPlaca);
                        //Venmsj = new VentanaMensaje();
                        //Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                        //Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                        //Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                        //Venmsj.ShowDialog();
                    }
                    dtFechaVenceContrato.SelectedDate = FechaActual.SelectedDate;
                    if (lista[0].VencimientoContrato != null)
                    {
                        dtFechaVenceContrato.SelectedDate = lista[0].VencimientoContrato;
                        //if (dtFechaVenceContrato.SelectedDate < FechaActual.SelectedDate)
                        //{
                        //    Mensabll = new MensajeBLL();
                        //    Mensabll.tipomensajes(61, "");
                        //    DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdPlaca);
                        //}
                    }
                    //else
                    //{
                    //    Mensabll = new MensajeBLL();
                    //    Mensabll.tipomensajes(63);
                    //    DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdPlaca);
                    //}
                    if (lista[0].ModRepot != null)
                    {
                        if (lista[0].ModRepot == 0)
                        {
                            txtmodelorepotenciado.Text = "";
                        }
                        else
                        {
                            txtmodelorepotenciado.Text = lista[0].ModRepot.ToString();
                        }
                    }
                    else
                    {
                        txtmodelorepotenciado.Text = "";
                    }
                    txtpesovacio.Text = lista[0].PesoVacio.ToString();
                    if (lista[0].PesoCabezote != null)
                    {
                        txtpesocabezote.Text = lista[0].PesoCabezote.ToString();
                    }
                    txtciafiliadora.Text = lista[0].Afiliadora1;
                    txttelciafiliadora.Text = lista[0].TelAfiliadora;
                    cbbciasoat.SelectedValue = lista[0].CiaSoat;
                    dtfechasoat.SelectedDate = lista[0].VencimientoSeguroObligatorio;
                    txtnumsoat.Text = lista[0].SeguroObligatorio;
                    if (FechaActual.SelectedDate >= dtfechasoat.SelectedDate)
                    {
                        Mensabll = new MensajeBLL();
                        Mensabll.tipomensajes(7, "");
                        DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdPlaca);
                    }
                    cbbcomRCE.SelectedValue = lista[0].IdAsegRCE;
                    switch (lista[0].TipoCarga)
                    {
                        case 1:
                            rdbalto.IsChecked = true;
                            break;
                        case 2:
                            rdbmedio.IsChecked = true;
                            break;
                        case 3:
                            rdbbajo.IsChecked = true;
                            break;
                    }
                    cbbcomRCE.SelectedValue = lista[0].IdAsegRCE;
                    if (lista[0].PolizaRC != null)
                    {
                        txtnumpolizaRC.Text = lista[0].PolizaRC;
                    }
                    if (lista[0].VencimientoPolizaRC != null)
                    {
                        dtfechaRC.SelectedDate = lista[0].VencimientoPolizaRC;
                    }
                    else
                    {
                        dtfechaRC.SelectedDate = null;
                    }
                    /*
                    if (Convert.ToInt32 ( lista[0].Modelo) < 1995)
                    { 
                        if (FechaActual.SelectedDate >= dtfechaRC.SelectedDate)
                        {
                            Mensabll = new MensajeBLL();
                            Mensabll.tipomensajes(8);
                            DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdPlaca);
                            //Venmsj = new VentanaMensaje();
                            //Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                            //Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                            //Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                            //Venmsj.ShowDialog();
                        }
                    }
                    */
                    dtfechacreacion.SelectedDate = lista[0].FechaCreacion;
                    txtobservaciones.Text = lista[0].Observaciones;
                    if (lista[0].TipoCuenta != "CC" || lista[0].TipoCuenta != "CA")
                    {
                        cbbtipocuenta.SelectedIndex = 0;
                    }
                    else
                    {
                        cbbtipocuenta.SelectedValue = lista[0].TipoCuenta;
                    }
                    cbbempreSatelital.SelectedValue = lista[0].CodEmpresaSat;
                    txtusuariogps.Text = lista[0].UsuarioGPS;
                    txtclavegps.Text = lista[0].PasswordGPS;
                    if (lista[0].Remolque != null)
                    {
                        if (lista[0].Remolque != "")
                        {
                            var listaR = RemolBll.GetRemolquePorPlaca(lista[0].Remolque);
                            if (listaR.Count > 0)
                            {
                                chk_remolque.IsChecked = true;
                                GrBo_inforemolque.Visibility = Visibility.Visible;
                                existeremolque = true;

                                txtTPropiedadremolque.Text = lista[0].TPropiedadRemolque;
                                dtfechaquitanrueda.IsEnabled = true;
                                dtfechaquitanrueda.SelectedDate = lista[0].VenceQuintaRueda;

                                if (FechaActual.SelectedDate >= dtfechaquitanrueda.SelectedDate)
                                {
                                    Mensabll = new MensajeBLL();
                                    Mensabll.tipomensajes(5, "");
                                    DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdPlaca);
                                    //Venmsj = new VentanaMensaje();
                                    //Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                                    //Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                                    //Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                                    //Venmsj.ShowDialog();
                                }
                                txtplacaremolque.Text = listaR[0].IdPlacaRO;
                                txtmodeloremolque.Text = listaR[0].Modelo;

                                switch (listaR[0].NumeroEjes)
                                {
                                    case 1:
                                        cbbejes.SelectedValue = 1;
                                        break;
                                    case 2:
                                        cbbejes.SelectedValue = 2;
                                        break;
                                    case 3:
                                        cbbejes.SelectedValue = 3;
                                        break;
                                    case 4:
                                        cbbejes.SelectedValue = 4;
                                        break;
                                    default:
                                        cbbejes.SelectedValue = listaR[0].NumeroEjes;
                                        break;
                                }

                                cbbtiporemolque.SelectedValue = listaR[0].TipoRemolque;
                                if (Convert.ToInt32(cbbtiporemolque.SelectedValue) == 67)
                                {
                                    cbbclase.IsEnabled = true;
                                    if (listaR[0].IdClase != null)
                                    {
                                        cbbclase.SelectedValue = listaR[0].IdClase;
                                    }
                                    else
                                    {
                                        cbbclase.SelectedValue = 999;
                                    }
                                }
                                else
                                {
                                    cbbclase.SelectedValue = 999;
                                    cbbclase.IsEnabled = false;
                                }
                                if (Convert.ToInt32(cbbtipocarroceria.SelectedValue) == 67)
                                {
                                    dtfeAforotanqueSencillo.IsEnabled = false;
                                    dtfeAforotanqueSencillo.SelectedDate = null;
                                    dtfeHidrostaticaSencillo.SelectedDate = null;
                                    dtfeHidrostaticaSencillo.IsEnabled = false;
                                    btn_aforotanqueSencillo.Visibility = Visibility.Collapsed;
                                    btn_hidrostaticaSencillo.Visibility = Visibility.Collapsed;
                                    cbbclase.IsEnabled = true;
                                    cbbunidadremolque.SelectedValue = 4;
                                    if (listaR[0].IdClase != null)
                                    {
                                        cbbclase.SelectedValue = listaR[0].IdClase;
                                    }
                                    else
                                    {
                                        cbbclase.SelectedValue = 999;
                                    }
                                }
                                else
                                {
                                    cbbunidadremolque.SelectedValue = 8;
                                    cbbclase.SelectedValue = 999;
                                    cbbclase.IsEnabled = false;
                                }
                                txtcapacidadremolque.Text = listaR[0].Capacidad.ToString();
                                if (listaR[0].Unidad == 4 || listaR[0].Unidad == 8)
                                {
                                    cbbunidadremolque.SelectedValue = listaR[0].Unidad;
                                }
                                else
                                {
                                    Mensabll = new MensajeBLL();
                                    Mensabll.tipomensajes(9, "");
                                    DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, listaR[0].IdPlacaRO);
                                    //Venmsj = new VentanaMensaje();
                                    //Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                                    //Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                                    //Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                                    //Venmsj.ShowDialog();
                                }
                                cbbmarcaremolque.SelectedValue = Convert.ToInt32(listaR[0].IdMarca);
                                // cbbclase.SelectedValue = listaR[0].IdClase;
                                txtpesoremolque.Text = listaR[0].PesoVacio.ToString();
                                dtfeHidrostatica.SelectedDate = listaR[0].VencePruebaHidro;
                                if (FechaActual.SelectedDate >= dtfeHidrostatica.SelectedDate)
                                {
                                    Mensabll = new MensajeBLL();
                                    Mensabll.tipomensajes(40, "");
                                    DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdPlaca);
                                    //Venmsj = new VentanaMensaje();
                                    //Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                                    //Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                                    //Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                                    //Venmsj.ShowDialog();
                                }
                                dtfeAforotanque.SelectedDate = listaR[0].FechaAforo;
                                if (listaR[0].FechaVenceKingPin != null)
                                {
                                    dtfekingpin.SelectedDate = listaR[0].FechaVenceKingPin;
                                }
                                else
                                {
                                    dtfekingpin.SelectedDate = null;
                                }
                                if (listaR[0].IdProp != null)
                                {
                                    txtNumdocumentopropi_rem.Text = listaR[0].IdProp.ToString();
                                }
                                else
                                {
                                    dtfekingpin.SelectedDate = null;
                                }
                                //txtNitVerificacionPropi_rem
                                if (listaR[0].dv != null)
                                {
                                    txtNitVerificacionPropi_rem.Text = listaR[0].dv.ToString();
                                }
                                else
                                {
                                    txtNumdocumentopropi_rem.Text = "";
                                }
                                if (listaR[0].Nombre != null)
                                {
                                    txtnombrespropi_rem.Text = listaR[0].Nombre;
                                }
                                else
                                {
                                    txtnombrespropi_rem.Text = "";
                                }
                            }
                            else
                            {
                                chk_remolque.IsChecked = false;
                                GrBo_inforemolque.Visibility = Visibility.Collapsed;
                                existeremolque = false;
                                txtTPropiedadremolque.Text = "";
                                dtfechaquitanrueda.IsEnabled = false;
                                dtfechaquitanrueda.SelectedDate = null;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(cbbtipocarroceria.SelectedValue) == 67)
                            {
                                dtfeAforotanqueSencillo.IsEnabled = true;
                                dtfeHidrostaticaSencillo.IsEnabled = true;
                                btn_aforotanqueSencillo.Visibility = Visibility.Visible;
                                btn_hidrostaticaSencillo.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                dtfeAforotanqueSencillo.IsEnabled = false;
                                dtfeAforotanqueSencillo.SelectedDate = null;
                                dtfeHidrostaticaSencillo.SelectedDate = null;
                                dtfeHidrostaticaSencillo.IsEnabled = false;
                                btn_aforotanqueSencillo.Visibility = Visibility.Collapsed;
                                btn_hidrostaticaSencillo.Visibility = Visibility.Collapsed;
                            }
                            chk_remolque.IsChecked = false;
                            GrBo_inforemolque.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(cbbtipocarroceria.SelectedValue) == 67)
                        {
                            dtfeAforotanqueSencillo.IsEnabled = true;
                            dtfeHidrostaticaSencillo.IsEnabled = true;
                            btn_aforotanqueSencillo.Visibility = Visibility.Visible;
                            btn_hidrostaticaSencillo.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            dtfeAforotanqueSencillo.IsEnabled = false;
                            dtfeAforotanqueSencillo.SelectedDate = null;
                            dtfeHidrostaticaSencillo.SelectedDate = null;
                            dtfeHidrostaticaSencillo.IsEnabled = false;
                            btn_aforotanqueSencillo.Visibility = Visibility.Collapsed;
                            btn_hidrostaticaSencillo.Visibility = Visibility.Collapsed;
                        }
                        chk_remolque.IsChecked = false;
                        GrBo_inforemolque.Visibility = Visibility.Collapsed;
                    }
                    if (lista[0].Conductor != "")
                    {
                        ExisteConductor = true;
                        lblConductor.Content = lista[0].Conductor;
                        txtNumdocumento.Text = lista[0].Conductor;
                        txtnombres.Text = lista[0].Nombres;
                        txtapellido1.Text = lista[0].Apellido1;
                        txtapellido2.Text = lista[0].Apellido2;
                        dtfechanacimieto.SelectedDate = lista[0].FechaNacimiento;
                        if (lista[0].Error_RNDC != null)
                        {
                            lblErrorMinisConductor.Content = lista[0].Error_RNDC;
                        }
                        if (lista[0].DeparConductor != null)
                        {
                            cbbdeparconductor.SelectedValue = lista[0].DeparConductor;
                            cargarCiudadesconductor(Convert.ToInt32(cbbdeparconductor.SelectedValue));
                            cbbciudadconductor.SelectedValue = lista[0].CiudadResidencia;
                        }
                        else
                        {
                            if (lista[0].TipoAfiliacion == 2)
                            {
                                Mensabll = new MensajeBLL();
                                Mensabll.tipomensajes(24, "");
                                DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].Conductor);
                            }

                        }
                        if (lista[0].CelPre != null)
                        {
                            txtcelular.Text = lista[0].CelPre.ToString();
                        }
                        if (lista[0].Telefono != null)
                        {
                            txttelefonocasa.Text = lista[0].Telefono;
                            txtIndicativo.Text = lista[0].IndicativoTelFijo;
                        }
                        if (txtcelular.Text.Trim() == "" && txttelefonocasa.Text.Trim() == "")
                        {
                            if (lista[0].TipoAfiliacion == 2)
                            {
                                Mensabll = new MensajeBLL();
                                Mensabll.tipomensajes(51, "");
                                DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Conductor", lista[0].Conductor);
                            }

                        }
                        else
                        {
                            if (txtcelular.Text.Trim() != "")
                            {
                                if (txtcelular.Text.Trim().Length != 10)
                                {
                                    if (lista[0].TipoAfiliacion == 2)
                                    {
                                        Mensabll = new MensajeBLL();
                                        Mensabll.tipomensajes(48, "");
                                        DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Conductor", lista[0].Conductor);
                                    }

                                }
                                if (txttelefonocasa.Text.Trim() != "")
                                {
                                    if (txttelefonocasa.Text.Trim().Length != 7)
                                    {
                                        if (lista[0].TipoAfiliacion == 2)
                                        {
                                            Mensabll = new MensajeBLL();
                                            Mensabll.tipomensajes(49, "");
                                            DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Conductor", lista[0].Conductor);
                                        }

                                    }
                                }
                            }
                            else
                            {
                                if (txttelefonocasa.Text.Trim().Length != 7)
                                {
                                    if (lista[0].TipoAfiliacion == 2)
                                    {
                                        Mensabll = new MensajeBLL();
                                        Mensabll.tipomensajes(49, "");
                                        DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Conductor", lista[0].Conductor);
                                    }

                                }
                            }
                        }

                        if (lista[0].Direccion != null)
                        {
                            if (lista[0].Direccion.Trim() != "")
                            {
                                txtdireccioncasa.Text = lista[0].Direccion;
                            }
                            else
                            {
                                if (lista[0].TipoAfiliacion == 2)
                                {
                                    Mensabll = new MensajeBLL();
                                    Mensabll.tipomensajes(50, "");
                                    DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Conductor", lista[0].Conductor);
                                }
                            }
                        }
                        else
                        {
                            if (lista[0].TipoAfiliacion == 2)
                            {
                                Mensabll = new MensajeBLL();
                                Mensabll.tipomensajes(50, "");
                                DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Conductor", lista[0].Conductor);
                            }
                        }
                        if (lista[0].CategoriaConductor == 1)
                        {
                            cbbcateglicencia.SelectedValue = 2;
                        }
                        else
                        {
                            cbbcateglicencia.SelectedValue = Convert.ToInt32(lista[0].CategoriaConductor);
                        }
                        txtnumlicencia.Text = lista[0].Licencia;
                        if (lista[0].FechaVencimiento != null)
                        {
                            dtfechalicencia.SelectedDate = lista[0].FechaVencimiento;
                            if (dtfechalicencia.SelectedDate <= FechaActual.SelectedDate)
                            {
                                if (lista[0].TipoAfiliacion == 2)
                                {
                                    Mensabll = new MensajeBLL();
                                    Mensabll.tipomensajes(47, "");
                                    DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdConductor);
                                }
                            }
                            else
                            {
                                int anioactual, aniolicencia, aniodiferencia;
                                DateTime fechalince = (DateTime)dtfechalicencia.SelectedDate;
                                DateTime fechaactu = DateTime.Now;
                                anioactual = DateTime.Now.Year;
                                aniolicencia = fechalince.Year;
                                aniodiferencia = aniolicencia - anioactual;
                                if (aniodiferencia < 0 || aniodiferencia > 3)
                                {
                                    if (lista[0].TipoAfiliacion == 2)
                                    {
                                        Mensabll = new MensajeBLL();
                                        Mensabll.tipomensajes(53, "");
                                        DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdConductor);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (lista[0].TipoAfiliacion == 2)
                            {
                                Mensabll = new MensajeBLL();
                                Mensabll.tipomensajes(53, "");
                                DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdConductor);
                            }
                        }
                        txteps.Text = lista[0].Eps;
                        txtARL.Text = lista[0].Arp;
                        dtfechaPagoSSocial.SelectedDate = lista[0].FechaPagoSegSoc;
                        txtexperiencia.Text = lista[0].Experiencia.ToString();
                        if (lista[0].Tpago_Id != null)
                        {
                            switch (lista[0].Tpago_Id)
                            {
                                case 1:
                                    rdbeletronico.IsChecked = true;
                                    break;
                                case 2:
                                    rdbcheque.IsChecked = true;
                                    break;
                                default:
                                    rdbeletronico.IsChecked = true;
                                    break;
                            }
                        }
                        else
                        {
                            rdbeletronico.IsChecked = true;
                        }
                        if (lista[0].IdBanco != null)
                        {
                            cbbbanco.SelectedValue = lista[0].IdBanco;
                        }
                        else
                        {
                            cbbbanco.SelectedIndex = 0;
                        }
                        if (lista[0].TipoAfiliacion != 1)
                        {
                            if (lista[0].NitCuenta != null)
                            {
                                if (lista[0].NitCuenta.Trim() != "")
                                {
                                    var listabenefi = PropiBeneBll.GetBeneficiarioporplaca(txtplaca.Text);
                                    if (listabenefi.Count > 0)
                                    {
                                        if (listabenefi[0].IdNit.Trim() != null)
                                        {
                                            if (lista[0].NitCuenta.Trim() != listabenefi[0].IdNit.Trim())
                                            {
                                                if (lista[0].NitCuenta.Trim() != lista[0].Conductor.Trim())
                                                {
                                                    if (lista[0].TipoAfiliacion == 2)
                                                    {
                                                        Mensabll = new MensajeBLL();
                                                        Mensabll.tipomensajes(42, "");
                                                        DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].Conductor);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (lista[0].NitCuenta.Trim() != lista[0].Conductor.Trim())
                                            {
                                                if (lista[0].TipoAfiliacion == 2)
                                                {
                                                    Mensabll = new MensajeBLL();
                                                    Mensabll.tipomensajes(42, "");
                                                    DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].Conductor);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (lista[0].NitCuenta.Trim() != lista[0].Conductor.Trim())
                                        {
                                            if (lista[0].TipoAfiliacion == 2)
                                            {
                                                Mensabll = new MensajeBLL();
                                                Mensabll.tipomensajes(42, "");
                                                DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].Conductor);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        txtnumcuenta.Text = lista[0].IdTarRec;
                        txtnumcuenta.IsEnabled = false;
                        txtidtarjeta.Text = lista[0].IdIdenTar;
                        txtidtarjeta.IsEnabled = false;
                        if (lista[0].NitCuenta != null)
                        {
                            txtnitcuenta.Text = lista[0].NitCuenta;
                        }
                        txtnombreferencia.Text = lista[0].RefPersonal;
                        txttelreferencia.Text = lista[0].TelefonoRef;
                        txtobservacionesconductor.Text = lista[0].Observ;
                        txtobservacionesruta.Text = lista[0].ObservaRuta;
                        switch (lista[0].CondTipoCarga)
                        {
                            case 1:
                                rdbaltocondu.IsChecked = true;
                                break;
                            case 2:
                                rdbmediocondu.IsChecked = true;
                                break;
                            case 3:
                                rdbbajocondu.IsChecked = true;
                                break;
                        }
                        cbNivelRiesgoActual.SelectedValue = lista[0].CondTipoCarga;
                        switch (lista[0].IdTipoAfiliacion)
                        {
                            case 3:
                                rdbescolta.IsChecked = true;
                                pnl_tipoafiliacionconductor.IsEnabled = true;
                                break;
                            default:
                                if (lista[0].chkAfiliado == false)
                                {
                                    rdbafiliado.IsChecked = true;
                                    pnl_tipoafiliacionconductor.IsEnabled = true;
                                }
                                else
                                {
                                    rdbpropio.IsChecked = true;
                                    pnl_tipoafiliacionconductor.IsEnabled = false;
                                }
                                break;
                        }
                        txtnombreempresa1.Text = lista[0].Empresa1;
                        txttelempresa1.Text = lista[0].Telefono1;
                        txtantiguedadempresa1.Text = lista[0].TpoServicio1;
                        txttipocargaempresa1.Text = lista[0].TipoCarga1;
                        txtnombreempresa2.Text = lista[0].Empresa2;
                        txttelempresa2.Text = lista[0].Telefono2;
                        txtantiguedadempresa2.Text = lista[0].TpoServicio2;
                        txttipocargaempresa2.Text = lista[0].TipoCarga2;
                        txtobservacionestenedor.Text = lista[0].BenObservacion;
                        if (lista[0].ListaRiesgo == true)
                        {
                            rdblistaaprueba.IsChecked = true;
                        }
                        else
                        {
                            rdblistaNoaprueba.IsChecked = true;
                        }
                        if (lista[0].FechaEstudio != null)
                        {
                            dtfechaEstudioConductor.SelectedDate = lista[0].FechaEstudio;

                        }

                        if (lista[0].FecEstVeh != null)
                        {
                            dtfeEstudioVehiculo.SelectedDate = lista[0].FecEstVeh;

                        }
                        if (dtfechanacimieto.SelectedDate != null)
                        {
                            CalcularEdad((DateTime)dtfechanacimieto.SelectedDate);
                        }
                        //Fecha ultimo pago planilla aportes salud
                        dtFechaPagoSalud.SelectedDate = FechaActual.SelectedDate;
                        if (lista[0].PagoPlanillaSalud != null)
                        {
                            dtFechaPagoSalud.SelectedDate = lista[0].PagoPlanillaSalud;
                            //DateTime dtFecha = Convert.ToDateTime(dtFechaPagoSalud.SelectedDate);
                            //dtFecha = dtFecha.AddMonths(1);
                            //if (dtFecha < FechaActual.SelectedDate)
                            //{
                            //    if (lista[0].IdTipoAfiliacion != 1)
                            //    {
                            //        Mensabll = new MensajeBLL();
                            //        Mensabll.tipomensajes(62, "");
                            //        DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdConductor);
                            //    }
                            //}
                        }
                        //else
                        //{
                        //    Mensabll = new MensajeBLL();
                        //    Mensabll.tipomensajes(63);
                        //    DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdConductor);
                        //}
                        dtFechaExamenes.SelectedDate = FechaActual.SelectedDate;
                        if (lista[0].FechaUltExamenMedico != null)
                        {
                            dtFechaExamenes.SelectedDate = lista[0].FechaUltExamenMedico;
                        }
                        //else
                        //{
                        //    Mensabll = new MensajeBLL();
                        //    Mensabll.tipomensajes(63);
                        //    DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdConductor);
                        //}
                    }
                    else
                    {
                        if (lista[0].TipoAfiliacion == 2)
                        {
                            Mensabll = new MensajeBLL();
                            Mensabll.tipomensajes(55, "");
                            DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Conductor", "");
                        }
                        ExisteConductor = false;
                    }
                    if (lista[0].Propietario != null)
                    {
                        Existepropietario = true;
                        txtnombrespropi.Text = lista[0].ProNombres;
                        txtapellidopropi1.Text = lista[0].ProApellido1;
                        txtapellidopropi2.Text = lista[0].ProApellido2;
                        if (lista[0].ProError_RNDC != null)
                        {
                            lblErrorMinisPropietario.Content = lista[0].ProError_RNDC;
                        }
                        if (lista[0].TipoDoc != "")
                        {
                            if (lista[0].TipoDoc != null)
                            {
                                cbbtipodocumentopropi.SelectedValue = lista[0].TipoDoc;
                                if (lista[0].TipoDoc == "N")
                                {
                                    txtNitVerificacionPropi.Visibility = Visibility.Visible;
                                    if (lista[0].dv != null)
                                    {
                                        txtNitVerificacionPropi.Text = lista[0].dv.ToString();
                                    }
                                    else
                                    {
                                        txtNitVerificacionPropi.Text = "";
                                    }
                                    txtapellidopropi1.Visibility = Visibility.Collapsed;
                                    txtapellidopropi2.Visibility = Visibility.Collapsed;
                                    txtapellidopropi1.Text = "";
                                    txtapellidopropi2.Text = "";
                                }
                                else
                                {
                                    txtNitVerificacionPropi.Visibility = Visibility.Collapsed;
                                    txtNitVerificacionPropi.Text = "";
                                    txtapellidopropi1.Visibility = Visibility.Visible;
                                    txtapellidopropi2.Visibility = Visibility.Visible;
                                }
                            }
                            else
                            {
                                cbbtipodocumentopropi.SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            cbbtipodocumentopropi.SelectedIndex = 0;
                        }
                        txtNumdocumentopropi.Text = lista[0].IdProp.ToString();
                        if (udppropietario == false)
                        {
                            txtNumdocumentopropi.IsEnabled = false;
                        }
                        else
                        {
                            txtNumdocumentopropi.IsEnabled = true;
                        }
                        Habilitarcampospropietario(udppropietario);
                        cbbdeparpropi.SelectedValue = lista[0].DeparPropietario;
                        cargarCiudadespropietario(Convert.ToInt32(lista[0].DeparPropietario));
                        cbbciudadpropi.SelectedValue = lista[0].CiuPro;
                        txttelefonopropi.Text = lista[0].TelPro;
                        if (txttelefonopropi.Text.Trim() != "")
                        {
                            if (txttelefonopropi.Text.Trim().Length == 7 || txttelefonopropi.Text.Trim().Length == 10)
                            {
                                lblerror_telefonopropi.Content = "";
                            }
                            else
                            {
                                Mensabll = new MensajeBLL();
                                Mensabll.tipomensajes(52, "");
                                DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Propietario", lista[0].Propietario.ToString());
                            }
                        }
                        else
                        {
                            Mensabll = new MensajeBLL();
                            Mensabll.tipomensajes(52, "");
                            DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Propietario", lista[0].Propietario.ToString());
                        }
                        if (lista[0].DirPro != null)
                        {
                            if (lista[0].DirPro.Trim() != "")
                            {
                                txtdireccionpropi.Text = lista[0].DirPro;
                            }
                            else
                            {
                                Mensabll = new MensajeBLL();
                                Mensabll.tipomensajes(50, "");
                                DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Propietario", lista[0].Propietario.ToString());
                            }
                        }
                        else
                        {
                            Mensabll = new MensajeBLL();
                            Mensabll.tipomensajes(50, "");
                            DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Propietario", lista[0].Propietario.ToString());
                        }
                        if (lista[0].ProListaRiesgo == true)
                        {
                            rdblistaapruebapropi.IsChecked = true;
                        }
                        else
                        {
                            rdblistaNoapruebapropi.IsChecked = true;
                        }
                        if (lista[0].ProFechaEstudio != null)
                        {
                            dtfechaestudiopropi.SelectedDate = lista[0].ProFechaEstudio;
                        }
                    }
                    else
                    {
                        cbbtipodocumentopropi.SelectedIndex = 1;
                        Existepropietario = false;
                        Mensabll = new MensajeBLL();
                        Mensabll.tipomensajes(56, "");
                        DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " propietario", "");
                    }
                    if (lista[0].NitPropietario != null)
                    {
                        if (lista[0].NitPropietario != "")
                        {
                            Existebeneficiario = true;

                            if (lista[0].BenTipoDoc != null)
                            {
                                if (lista[0].BenTipoDoc.Trim() != "")
                                {
                                    if (lista[0].BenError_RNDC != null)
                                    {
                                        lblErrorMinisbeneficiario.Content = lista[0].BenError_RNDC;
                                    }
                                    //if (lista[0].BenTipoDoc != null)
                                    //{
                                    cbbtipodocumentobenefi.SelectedValue = lista[0].BenTipoDoc;
                                    if (lista[0].BenTipoDoc == "N")
                                    {
                                        txtNitVerificacionBenefi.Visibility = Visibility.Visible;
                                        if (lista[0].Bendv != null)
                                        {
                                            txtNitVerificacionBenefi.Text = lista[0].Bendv.ToString();
                                        }
                                        else
                                        {
                                            txtNitVerificacionBenefi.Text = "";
                                        }
                                        txtapellidobenefi1.Visibility = Visibility.Collapsed;
                                        txtapellidobenefi2.Visibility = Visibility.Collapsed;
                                        txtapellidobenefi1.Text = "";
                                        txtapellidobenefi2.Text = "";
                                    }
                                    else
                                    {
                                        txtapellidobenefi1.Visibility = Visibility.Visible;
                                        txtapellidobenefi2.Visibility = Visibility.Visible;
                                        txtNitVerificacionBenefi.Visibility = Visibility.Visible;
                                        txtNitVerificacionBenefi.Text = "";
                                    }
                                    //}
                                    //else
                                    //{
                                    //    cbbtipodocumentobenefi.SelectedIndex = 0;
                                    //}
                                }
                                else
                                {
                                    cbbtipodocumentobenefi.SelectedIndex = 0;
                                }
                            }
                            else
                            {
                                cbbtipodocumentobenefi.SelectedIndex = 0;
                            }
                            txtNumdocumentobenefi.Text = lista[0].NitPropietario;
                            if (udpbeneficiario == false)
                            {
                                txtNumdocumentobenefi.IsEnabled = false;
                            }
                            else
                            {
                                txtNumdocumentobenefi.IsEnabled = true;
                            }
                            if (boolhabilitarbotonesBene == true)
                            {
                                Habilitarbotonespropietariobeneficiario(true);
                            }
                            else
                            {
                                Habilitarbotonespropietariobeneficiario(true);

                            }
                            Habilitarcamposbeneficiario(udpbeneficiario);
                            //  Habilitarbotonespropietariobeneficiario(udpbeneficiario);
                            txtnombresbenefi.Text = lista[0].BenNombres;
                            txtapellidobenefi1.Text = lista[0].BenApellido1;
                            txtapellidobenefi2.Text = lista[0].BenApellido2;
                            if (lista[0].BenIdDepartamento != null)
                            {
                                cbbdeparbenefi.SelectedValue = lista[0].BenIdDepartamento;
                            }
                            if (lista[0].BenIdDepartamento != null)
                            {
                                cargarCiudadesbeneficiario(Convert.ToInt32(lista[0].BenIdDepartamento));
                            }
                            cbbciudadbenefi.SelectedValue = lista[0].BenCiudad;
                            // txtdireccionbenefi.Text = lista[0].BenDireccion;
                            if (lista[0].BenDireccion != null)
                            {
                                if (lista[0].BenDireccion.Trim() != "")
                                {
                                    txtdireccionbenefi.Text = lista[0].BenDireccion;
                                }
                                else
                                {
                                    Mensabll = new MensajeBLL();
                                    Mensabll.tipomensajes(50, "");
                                    DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Beneficiario", lista[0].NitPropietario);
                                }
                            }
                            else
                            {
                                Mensabll = new MensajeBLL();
                                Mensabll.tipomensajes(50, "");
                                DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Beneficiario", lista[0].NitPropietario);
                            }
                            if (lista[0].BenTelefono != null)
                            {
                                txttelefonobenefi.Text = lista[0].BenTelefono;
                                txtIndicativobenefi.Text = lista[0].IndicativoTelFijoBenefi;
                            }
                            txtcelbenefi.Text = lista[0].Ben_TelCel;
                            if (lista[0].BenEmail != null)
                            {
                                if (lista[0].BenEmail != "")
                                {

                                    txtemail.Text = lista[0].BenEmail.ToString();
                                }
                            }
                            if (txttelefonobenefi.Text.Trim() == "" && txtcelbenefi.Text.Trim() == "")
                            {
                                Mensabll = new MensajeBLL();
                                Mensabll.tipomensajes(51, "");
                                DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Beneficiario", lista[0].NitPropietario);
                            }
                            else
                            {
                                if (txtcelbenefi.Text.Trim() != "")
                                {
                                    if (txtcelbenefi.Text.Trim().Length != 10)
                                    {
                                        Mensabll = new MensajeBLL();
                                        Mensabll.tipomensajes(48, "");
                                        DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Beneficiario", lista[0].NitPropietario);
                                    }
                                    else
                                    {
                                        lblerror_celbenefi.Content = "";
                                    }
                                }
                                else
                                {
                                    if (txttelefonobenefi.Text.Trim() != "")
                                    {
                                        if (txttelefonobenefi.Text.Trim().Length != 7)
                                        {
                                            Mensabll = new MensajeBLL();
                                            Mensabll.tipomensajes(49, "");
                                            DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Beneficiario", lista[0].NitPropietario);
                                        }
                                        else
                                        {
                                            lblerror_celbenefi.Content = "";
                                        }
                                    }
                                }
                            }

                            if (lista[0].PagoCierre != null)
                            {
                                cbbagenciabenefi.SelectedValue = lista[0].PagoCierre;
                            }
                            else
                            {
                                Mensabll = new MensajeBLL();
                                Mensabll.tipomensajes(25, "");
                                DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].NitPropietario);
                            }
                            if (lista[0].BenListaRiesgo == true)
                            {
                                rdblistaapruebabenefi.IsChecked = true;
                            }
                            else
                            {
                                rdblistaNoapruebabenefi.IsChecked = true;
                            }
                            if (lista[0].BenFechaEstudio != null)
                            {
                                dtfechaestudiobenefi.SelectedDate = lista[0].BenFechaEstudio;
                            }
                        }
                        else
                        {
                            cbbtipodocumentobenefi.SelectedIndex = 1;
                            Existebeneficiario = false;
                            Mensabll = new MensajeBLL();
                            Mensabll.tipomensajes(57, "");
                            DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Beneficiario ", "");
                        }
                    }
                    else
                    {
                        cbbtipodocumentobenefi.SelectedIndex = 1;
                        Existebeneficiario = false;
                        Mensabll = new MensajeBLL();
                        Mensabll.tipomensajes(57, "");
                        DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes + " Beneficiario ", "");
                    }
                    CargarContrato();

                    //var rowafiliacio = (TipoAfiliacion)cbbtipoafiliacion.SelectedItem;
                    //if (rowafiliacio != null)
                    //{
                    //    validarmodificacionesportipoAfiliacion(rowafiliacio);
                    //}

                }
                else
                {

                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(2, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    main.pnl_inicio.Children.Clear();
                    main.pnl_inicio.Children.Add(Iniconsul);
                }
            }
        }

        //private void validarmodificacionesportipoAfiliacion(TipoAfiliacion rowafiliacio)
        //{
        //    if (boolmodificatodos == true)
        //    {

        //    }
        //    else
        //    {
        //        switch (rowafiliacio.IdCodigo)
        //        {
        //            case 2:case 3:
        //                if (boolmodificaterceros == true || boolmodificatodos==true )
        //                {

        //                }
        //                else
        //                {
        //                    this.IsEnabled = false ;
        //                }
        //                break;
        //            case 1: case 5:
        //                if (boolmodificapropios == true || boolmodificatodos == true)
        //                {
        //                    this.IsEnabled = true;
        //                }
        //                else
        //                {
        //                    this.IsEnabled = false;
        //                }
        //                break;
        //            default:

        //                break;
        //        }


        //    }
        //}

        private void ConsultaDatosPropietario(string idpropietario)
        {
            var lista = PropiBeneBll.GetDatosPropietarioXID(idpropietario);
            if (lista.Count > 0)
            {
                MessageBoxResult result1 = MessageBox.Show("Este Propietario ya  Existe. Desea cargar sus datos?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result1 == MessageBoxResult.Yes)
                {
                    Existepropietario = true;
                    if (lista[0].TipoDoc != "")
                    {
                        cbbtipodocumentopropi.SelectedValue = lista[0].TipoDoc;

                        if (lista[0].TipoDoc == "N")
                        {
                            txtNitVerificacionPropi.Visibility = Visibility.Visible;
                            if (lista[0].dv != null)
                            {
                                txtNitVerificacionPropi.Text = lista[0].dv.ToString();
                            }
                            else
                            {
                                txtNitVerificacionPropi.Text = "";
                            }
                            txtapellidopropi1.Visibility = Visibility.Collapsed;
                            txtapellidopropi2.Visibility = Visibility.Collapsed;
                            txtapellidopropi1.Text = "";
                            txtapellidopropi2.Text = "";
                        }
                        else
                        {
                            txtNitVerificacionPropi.Visibility = Visibility.Collapsed;
                            txtNitVerificacionPropi.Text = "";
                            txtapellidopropi1.Visibility = Visibility.Visible;
                            txtapellidopropi2.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        cbbtipodocumentopropi.SelectedIndex = 1;
                    }

                    txtNumdocumentopropi.Text = lista[0].IdProp.ToString();
                    //if (udppropietario  == false)
                    //{
                    //    txtNumdocumentopropi.IsEnabled = false;
                    //}
                    //else
                    //{                        
                    //    txtNumdocumentopropi.IsEnabled = true;
                    //}
                    if (lista[0].Nombres != "")
                    {
                        txtnombrespropi.Text = lista[0].Nombres;
                        txtapellidopropi1.Text = lista[0].Apellido1;
                        txtapellidopropi2.Text = lista[0].Apellido2;
                    }
                    else
                    {
                        txtnombrespropi.Text = lista[0].Nombre;
                        //txtapellidopropi1.Text = lista[0].Apellido1;
                        //txtapellidopropi2.Text = lista[0].Apellido2;
                    }
                    txtdireccionpropi.Text = lista[0].DirPro;
                    if (lista[0].TelPro != null)
                    {
                        txttelefonopropi.Text = lista[0].TelPro;
                    }
                    if (lista[0].IdDepartamento != null)
                    {
                        cbbdeparpropi.SelectedValue = lista[0].IdDepartamento;
                        cargarCiudadespropietario(Convert.ToInt32(lista[0].IdDepartamento));
                        cbbciudadpropi.SelectedValue = lista[0].CiuPro;
                    }
                    if (lista[0].ListaRiesgo != null)
                    {
                        if (lista[0].ListaRiesgo == true)
                            rdblistaapruebapropi.IsChecked = true;
                        else
                            rdblistaNoapruebapropi.IsChecked = true;
                    }
                    if (lista[0].FechaEstudio != null)
                    {
                        dtfechaestudiopropi.SelectedDate = lista[0].FechaEstudio;
                    }
                }
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
                        mostrar_mensaje_reportado(39, "");
                        limpiarcampospropietario();
                    }
                    else
                    {
                        Existepropietario = false;
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
                    Existepropietario = false;
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

        private void Habilitarcampospropietario(Boolean Autorizacion)
        {
            cbbtipodocumentopropi.IsEnabled = Autorizacion;
            txtNumdocumentopropi.IsEnabled = Autorizacion;
            txtnombrespropi.IsEnabled = Autorizacion;
            txtapellidopropi1.IsEnabled = Autorizacion;
            txtapellidopropi2.IsEnabled = Autorizacion;
            txtdireccionpropi.IsEnabled = Autorizacion;
            txttelefonopropi.IsEnabled = Autorizacion;
            txttelefonopropi.IsEnabled = Autorizacion;
            cbbdeparpropi.IsEnabled = Autorizacion;
            // cargarCiudadespropietario(Convert.ToInt32(cbbdeparpropi.SelectedValue));
            cbbciudadpropi.IsEnabled = Autorizacion;
            rdblistaNoapruebapropi.IsChecked = false;
            dtfechaestudiopropi.IsEnabled = Autorizacion;
            txtNitVerificacionPropi.IsEnabled = Autorizacion;
        }

        //string mDocumento;
        //  int mDigito = 0;
        //    public int ValidarDigitoVerificacion(string  Numero)
        //    {

        //          Numero = LimpiarSeparadores(Numero);
        //          decimal  valor =Convert.ToDecimal ( Numero);
        //          string format = "000000000000000";
        //          string mDocumento = valor.ToString (format);
        //          //string mDocumento = String.Format(Numero, "000000000000000");
        //          int mDigito =    CalcularDigito(mDocumento);
        //          return  mDigito;
        //    }

        //    private int CalcularDigito(string mDocumento)
        //    {
        //        int mDigito;
        //        long Total;
        //        int Residuo;
        //     Total = Convert.ToInt32  ( mDocumento.Substring(0,1) )* 71;
        //    Total += Convert.ToInt32  ( mDocumento.Substring(1,1) ) * 67;
        //    Total += Convert.ToInt32  ( mDocumento.Substring(2,1) )* 59;
        //    Total += Convert.ToInt32  ( mDocumento.Substring(3,1) ) * 53;
        //    Total += Convert.ToInt32  ( mDocumento.Substring(4,1) ) * 47;
        //    Total += Convert.ToInt32  ( mDocumento.Substring(5,1) ) * 43;
        //    Total += Convert.ToInt32  ( mDocumento.Substring(6,1) ) * 41;
        //    Total += Convert.ToInt32  ( mDocumento.Substring(7,1) ) * 37;
        //    Total += Convert.ToInt32  ( mDocumento.Substring(8,1) ) * 29;
        //    Total += Convert.ToInt32  ( mDocumento.Substring(9,1) )* 23;
        //    Total +=Convert.ToInt32  ( mDocumento.Substring(10,1) )* 19;
        //    Total +=Convert.ToInt32  ( mDocumento.Substring(11,1) ) * 17;
        //    Total += Convert.ToInt32  ( mDocumento.Substring(12,1) ) * 13;
        //    Total +=Convert.ToInt32  ( mDocumento.Substring(13,1) ) * 7;
        //    Total += Convert.ToInt32(mDocumento.Substring(14, 1)) * 3;
        //    Residuo = Convert.ToInt32 (Total % 11);
        //        switch (Residuo)
        //        {
        //            case 0: 
        //                mDigito = 0;
        //                break;
        //            case 1: 
        //                mDigito = 1;
        //                break;
        //            default: 
        //                mDigito = 11 - Residuo;
        //                break;
        //        }

        //        return mDigito;

        //    }

        //    private string LimpiarSeparadores(string Numero) {
        //    Numero = Numero.Replace(",", "");
        //    Numero = Numero.Replace(".", "");
        //    Numero = Numero.Replace("-", "");
        //    Numero = Numero.Replace(" ", "");
        //    return Numero;
        //}

        private void ConsultaDatosBeneficiario(string idbeneficiario)
        {
            Habilitarbotonespropietariobeneficiario(false);
            Boolean reportado = false;
            var listareportado = vetobll.GetDatosPersonaReportada(idbeneficiario);
            var lista = PropiBeneBll.GetDatosBeneficiarioXID(idbeneficiario);
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
                mostrar_mensaje_reportado(39, "");
                limpiarcamposbeneficiario();
            }
            else
            {
                if (lista.Count > 0)
                {
                    MessageBoxResult result1 = MessageBox.Show("Este Beneficiario ya  Existe. Desea cargar sus datos?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result1 == MessageBoxResult.Yes)
                    {
                        Existebeneficiario = true;
                        if (lista[0].TipoDoc != "")
                        {
                            cbbtipodocumentobenefi.SelectedValue = lista[0].TipoDoc;
                            if (lista[0].TipoDoc == "N")
                            {
                                txtNitVerificacionPropi.Visibility = Visibility.Visible;
                                if (lista[0].dv != null)
                                {
                                    txtNitVerificacionBenefi.Text = lista[0].dv.ToString();
                                }
                                else
                                {
                                    txtNitVerificacionBenefi.Text = "";
                                }
                                txtapellidobenefi1.Visibility = Visibility.Collapsed;
                                txtapellidobenefi2.Visibility = Visibility.Collapsed;
                                txtapellidobenefi1.Text = "";
                                txtapellidobenefi2.Text = "";

                            }
                            else
                            {

                                txtNitVerificacionBenefi.Visibility = Visibility.Collapsed;
                                txtNitVerificacionBenefi.Text = "";
                                txtapellidobenefi1.Visibility = Visibility.Visible;
                                txtapellidobenefi2.Visibility = Visibility.Visible;

                            }
                        }
                        else
                        {
                            cbbtipodocumentobenefi.SelectedIndex = 1;
                        }


                        txtNumdocumentobenefi.Text = lista[0].IdNit;
                        if (boolhabilitarbotonesBene == true)
                        {
                            Habilitarbotonespropietariobeneficiario(true);
                        }
                        else
                        {
                            Habilitarbotonespropietariobeneficiario(false);
                        }
                        if (lista[0].Nombres != "")
                        {
                            txtnombresbenefi.Text = lista[0].Nombres;
                            txtapellidobenefi1.Text = lista[0].Apellido1;
                            txtapellidobenefi2.Text = lista[0].Apellido2;
                        }
                        else
                        {
                            txtnombresbenefi.Text = lista[0].Nombre;
                            //txtapellidobenefi1.Text = lista[0].Apellido1;
                            //txtapellidobenefi2.Text = lista[0].Apellido2;
                        }
                        txtdireccionbenefi.Text = lista[0].Direccion;
                        if (lista[0].Telefono != null)
                        {
                            txttelefonobenefi.Text = lista[0].Telefono;
                            txtIndicativobenefi.Text = lista[0].IndicativoTelFijoBenefi;

                        }

                        if (lista[0].PagoCierre != null)
                        {
                            cbbagenciabenefi.SelectedValue = lista[0].PagoCierre.ToString();
                        }
                        txtcelbenefi.Text = lista[0].Ben_TelCel;
                        if (lista[0].IdDepartamento != null)
                        {
                            cbbdeparbenefi.SelectedValue = lista[0].IdDepartamento;
                            cargarCiudadesbeneficiario(Convert.ToInt32(lista[0].IdDepartamento));
                            cbbciudadbenefi.SelectedValue = lista[0].Ciudad;
                        }
                        if (lista[0].ListaRiesgo != null)
                        {
                            if (lista[0].ListaRiesgo == true)
                                rdblistaapruebabenefi.IsChecked = true;
                            else
                                rdblistaNoapruebabenefi.IsChecked = true;
                        }
                        if (lista[0].FechaEstudio != null)
                        {
                            dtfechaestudiobenefi.SelectedDate = lista[0].FechaEstudio;

                        }
                        if (lista[0].Email != null)
                        {
                            if (lista[0].Email != "")
                            {

                                txtemail.Text = lista[0].Email.ToString();
                            }
                        }

                    }

                }
                else
                {


                    //if (listareportado.Count > 0)
                    //{
                    //    foreach (var item in listareportado)
                    //    {
                    //        if (item.Estado == 1)
                    //        {
                    //            reportado = true;
                    //            break;
                    //        }
                    //    }
                    //    if (reportado == true)
                    //    {
                    //        mostrar_mensaje_reportado(39);
                    //        limpiarcamposbeneficiario();
                    //    }
                    //    else
                    //    {
                    //        Existebeneficiario = false;

                    //        MessageBoxResult result1 = MessageBox.Show("Este Beneficiario no existe en la base de datos desea crealo?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    //        if (result1 == MessageBoxResult.Yes)
                    //        {
                    //            string id = txtNumdocumentobenefi.Text;
                    //            limpiarcamposbeneficiario();
                    //            txtNumdocumentobenefi.Text = id;

                    //        }
                    //    }

                    //}
                    //else
                    //{
                    Existebeneficiario = false;

                    MessageBoxResult result1 = MessageBox.Show("Este Beneficiario no existe en la base de datos desea crealo?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result1 == MessageBoxResult.Yes)
                    {
                        string id = txtNumdocumentobenefi.Text;
                        limpiarcamposbeneficiario();
                        txtNumdocumentobenefi.Text = id;

                    }
                    // }



                }
            }




        }

        private void Habilitarcamposbeneficiario(Boolean autorizacion)
        {
            cbbtipodocumentobenefi.IsEnabled = autorizacion;
            txtNitVerificacionBenefi.IsEnabled = autorizacion;
            //txtemail.IsEnabled = autorizacion;
            txtNumdocumentobenefi.IsEnabled = autorizacion;
            txtnombresbenefi.IsEnabled = autorizacion; ;
            txtapellidobenefi1.IsEnabled = autorizacion;
            txtapellidobenefi2.IsEnabled = autorizacion;
            txtdireccionbenefi.IsEnabled = autorizacion;
            txttelefonobenefi.IsEnabled = autorizacion;
            txtIndicativobenefi.IsEnabled = autorizacion;
            cbbagenciabenefi.IsEnabled = autorizacion;
            txtcelbenefi.IsEnabled = autorizacion;
            cbbdeparbenefi.IsEnabled = autorizacion;
            //      cargarCiudadesbeneficiario(Convert.ToInt32(cbbdeparbenefi.SelectedValue));
            cbbciudadbenefi.IsEnabled = autorizacion;
            rdblistaNoapruebabenefi.IsChecked = false;
            //dtfechaestudiobenefi.SelectedDate  = autrotizacion;
            dtfechaestudiobenefi.IsEnabled = autorizacion;
            txtobservacionestenedor.IsEnabled = autorizacion;
        }

        private void ConsultarDatosConductor(string idconductor)
        {
            var listareportadoconduc = vetobll.GetDatosPersonaReportada(idconductor);
            Boolean reportado = false;
            foreach (var item in listareportadoconduc)
            {
                if (item.Estado == 1)
                {
                    reportado = true;
                    break;
                }
            }
            if (reportado == true)
            {
                mostrar_mensaje_reportado(39, "");
                limpiarcamposconductor();
            }
            else
            {
                var lista = ConducBll.GetDAtosconductor(idconductor);
                if (lista.Count > 0)
                {
                    ExisteConductor = true;
                    HabilitarInfoBancaria(false);
                    txtnombres.Text = lista[0].Nombres;
                    txtapellido1.Text = lista[0].Apellido1;
                    txtapellido2.Text = lista[0].Apellido2;
                    dtfechanacimieto.SelectedDate = lista[0].FechaNacimiento;
                    if (lista[0].IdDepartamento != null)
                    {
                        cbbdeparconductor.SelectedValue = lista[0].IdDepartamento;
                        cargarCiudadesconductor(Convert.ToInt32(lista[0].IdDepartamento));
                        cbbciudadconductor.SelectedValue = lista[0].CiudadResidencia;
                    }
                    else
                    {
                        if (lista[0].IdTipoAfiliacion != 1)
                        {
                            Mensabll = new MensajeBLL();
                            Mensabll.tipomensajes(24, "");
                            DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdConductor);
                        }
                    }
                    if (lista[0].Telefono != null)
                    {
                        txttelefonocasa.Text = lista[0].Telefono;
                        txtIndicativo.Text = lista[0].IndicativoTelFijo;
                    }

                    txtcelular.Text = lista[0].CelPre.ToString();
                    txtdireccioncasa.Text = lista[0].Direccion;
                    if (lista[0].Categoria == 1)
                    {
                        cbbcateglicencia.SelectedValue = 2;
                    }
                    else
                    {
                        cbbcateglicencia.SelectedValue = Convert.ToInt32(lista[0].Categoria);
                    }

                    txtnumlicencia.Text = lista[0].Licencia;
                    dtfechalicencia.SelectedDate = lista[0].FechaVencimiento;
                    if (dtfechalicencia.SelectedDate <= FechaActual.SelectedDate)
                    {
                        if (lista[0].IdTipoAfiliacion != 1)
                        {
                            Mensabll = new MensajeBLL();
                            Mensabll.tipomensajes(47, "");
                            DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdConductor);
                        }
                    }
                    txteps.Text = lista[0].Eps;
                    txtARL.Text = lista[0].Arp;
                    dtfechaPagoSSocial.SelectedDate = lista[0].FechaPagoSegSoc;
                    txtexperiencia.Text = lista[0].Experiencia.ToString();

                    switch (Convert.ToInt32(cbbtipoafiliacion.SelectedValue))
                    {
                        case 2:
                            if (lista[0].NitCuenta != null)
                            {
                                string idbeneficiario = "";
                                if (lista[0].NitCuenta != "")
                                {
                                    var listabenefi = PropiBeneBll.GetBeneficiarioporplaca(txtplaca.Text);
                                    if (listabenefi.Count > 0)
                                    {
                                        if (listabenefi[0].IdNit != null)
                                        {
                                            idbeneficiario = listabenefi[0].IdNit;
                                        }
                                        if (lista[0].NitCuenta != listabenefi[0].IdNit)
                                        {
                                            if (lista[0].NitCuenta != idconductor)
                                            {
                                                MessageBox.Show("El nit de cuenta del conductor no coincide con Id del conductor ni con Nit del propietario");
                                                EnviarMailErrorCuentas(lista[0].NitCuenta, idbeneficiario, idconductor, txtplaca.Text);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (lista[0].NitCuenta != idconductor)
                                        {
                                            //if (lista[0].be != null)
                                            //{
                                            //    idbeneficiario = listabenefi[0].IdNit;
                                            //}
                                            MessageBox.Show("El nit de cuenta del conductor no coincide con Id del conductor");
                                            EnviarMailErrorCuentas(lista[0].NitCuenta, idbeneficiario, idconductor, txtplaca.Text);
                                        }
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    if (lista[0].Tpago_Id != null)
                    {
                        switch (lista[0].Tpago_Id)
                        {
                            case 1:
                                rdbeletronico.IsChecked = true;
                                break;
                            case 2:
                                rdbcheque.IsChecked = true;
                                break;
                            default:
                                rdbeletronico.IsChecked = true;
                                break;
                        }
                    }
                    else
                    {
                        rdbeletronico.IsChecked = true;
                    }
                    if (lista[0].NitCuenta != null)
                    {
                        txtnitcuenta.Text = lista[0].NitCuenta;
                    }
                    if (lista[0].IdBanco != null)
                    {
                        cbbbanco.SelectedValue = lista[0].IdBanco;
                    }
                    else
                    {
                        cbbbanco.SelectedIndex = 0;
                    }
                    //if (lista[0].IdBanco != "" )
                    //{
                    //    cbbbanco.SelectedValue = Convert.ToInt32(lista[0].IdBanco);
                    //}
                    //else
                    //{
                    //    cbbbanco.SelectedIndex = 1;
                    //}
                    txtnumcuenta.Text = lista[0].IdTarRec;
                    txtidtarjeta.Text = lista[0].IdIdenTar;

                    txtnombreferencia.Text = lista[0].NombrePariente;
                    txttelreferencia.Text = lista[0].TelefonoPariente;
                    txtobservacionesconductor.Text = lista[0].Observ;
                    txtobservacionesruta.Text = lista[0].ObservaRuta;
                    switch (lista[0].TipoCarga)
                    {
                        case 1:
                            rdbaltocondu.IsChecked = true;
                            break;
                        case 2:
                            rdbmediocondu.IsChecked = true;
                            break;
                        case 3:
                            rdbbajocondu.IsChecked = true;
                            break;
                    }
                    cbNivelRiesgoActual.SelectedValue = lista[0].TipoCarga;
                    switch (lista[0].IdTipoAfiliacion)
                    {
                        case 3:
                            rdbescolta.IsChecked = true;
                            pnl_tipoafiliacionconductor.IsEnabled = true;
                            break;
                        default:
                            if (lista[0].chkAfiliado == false)
                            {
                                rdbafiliado.IsChecked = true;
                                pnl_tipoafiliacionconductor.IsEnabled = true;
                            }
                            else
                            {
                                rdbpropio.IsChecked = true;
                                pnl_tipoafiliacionconductor.IsEnabled = false;
                            }
                            break;
                    }

                    txtnombreempresa1.Text = lista[0].Empresa1;
                    txttelempresa1.Text = lista[0].Telefono1;
                    txtantiguedadempresa1.Text = lista[0].TpoServicio1;
                    txttipocargaempresa1.Text = lista[0].TipoCarga.ToString();
                    txtnombreempresa2.Text = lista[0].Empresa2;
                    txttelempresa2.Text = lista[0].Telefono2;
                    txtantiguedadempresa2.Text = lista[0].TpoServicio2;
                    txttipocargaempresa2.Text = lista[0].TipoCarga1;
                    if (lista[0].ListaRiesgo == true)
                    {
                        rdblistaaprueba.IsChecked = true;
                    }
                    else
                    {
                        rdblistaNoaprueba.IsChecked = true;
                    }
                    if (lista[0].FechaEstudio != null)
                    {
                        dtfechaEstudioConductor.SelectedDate = lista[0].FechaEstudio;

                    }
                    if (dtfechanacimieto.SelectedDate != null)
                    {
                        CalcularEdad((DateTime)dtfechanacimieto.SelectedDate);
                    }
                    //Fecha ultimo pago planilla aportes salud
                    if (lista[0].PagoPlanillaSalud != null)
                    {
                        dtFechaPagoSalud.SelectedDate = lista[0].PagoPlanillaSalud;
                        //DateTime dtFecha = Convert.ToDateTime(dtFechaPagoSalud.SelectedDate);
                        //dtFecha = dtFecha.AddMonths(1);
                        //if (dtFecha < FechaActual.SelectedDate)
                        //{
                        //    if (lista[0].IdTipoAfiliacion != 1)
                        //    {
                        //        Mensabll = new MensajeBLL();
                        //        Mensabll.tipomensajes(62, "");
                        //        DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdConductor);
                        //    }
                        //}
                    }
                    //else
                    //{
                    //    Mensabll = new MensajeBLL();
                    //    Mensabll.tipomensajes(63);
                    //    DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdConductor);
                    //}
                    if (lista[0].FechaUltExamenMedico != null)
                    {
                        dtFechaExamenes.SelectedDate = lista[0].FechaUltExamenMedico;
                        //if (dtFechaExamenes.SelectedDate <= FechaActual.SelectedDate)
                        //{
                        //    if (lista[0].IdTipoAfiliacion != 1)
                        //    {
                        //        Mensabll = new MensajeBLL();
                        //        Mensabll.tipomensajes(63, "");
                        //        DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdConductor);
                        //    }
                        //}
                    }
                    //else
                    //{
                    //    Mensabll = new MensajeBLL();
                    //    Mensabll.tipomensajes(63);
                    //    DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdConductor);
                    //}

                }
                else
                {
                    ExisteConductor = false;
                    MessageBoxResult result1 = MessageBox.Show("Este conductor no existe en la base de datos desea crealo?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result1 == MessageBoxResult.Yes)
                    {
                        HabilitarInfoBancaria(false);
                        string id = txtNumdocumento.Text;
                        limpiarcamposconductor();
                        txtNumdocumento.Text = id;
                    }
                }
            }
        }

        private void EnviarMailErrorCuentas(string _NitCuenta, string _IdBeneficiario, string _IdConductor, string _placa)
        {
            var lista = Coreobll.GetCorreosAgencia();
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
            Email.EnviarEmail("Alerta Información bancaria de un Vehículo", "", lista, _NitCuenta, _IdBeneficiario, _IdConductor, _placa, usuario);
        }

        private void mostrar_mensaje_reportado(int idmensaje, string strComplemento)
        {
            Mensabll = new MensajeBLL();
            Mensabll.tipomensajes(idmensaje, strComplemento);
            Venmsj = new VentanaMensaje();
            Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
            Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
            Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
            Venmsj.ShowDialog();
        }

        private Boolean validarpersonaReportado(string id)
        {
            Boolean respuesta = false;
            var lista = vetobll.GetDatosPersonaReportada(id);
            if (lista.Count > 0)
            {
                respuesta = true;
            }
            else
            {
                respuesta = false;
            }
            return respuesta;
        }

        private void LlenarDatosRemolque(VehiT_ConsultaRemolqueXPlaca_Result rowremolque)
        {
            existeremolque = true;
            txtplacaremolque.Text = rowremolque.IdPlacaRO.Trim();
            txtmodeloremolque.Text = rowremolque.Modelo;
            cbbejes.SelectedValue = rowremolque.NumeroEjes;
            cbbtiporemolque.SelectedValue = rowremolque.TipoRemolque;
            txtcapacidadremolque.Text = rowremolque.Capacidad.ToString();
            validarpermisosoperacioninternacional();
            if (Convert.ToInt32(cbbtipocarroceria.SelectedValue) == 67)
            {
                cbbclase.IsEnabled = true;
                cbbunidadremolque.SelectedValue = 4;
                if (rowremolque.IdClase != null)
                {
                    cbbclase.SelectedValue = rowremolque.IdClase;
                }
                else
                {
                    cbbclase.SelectedValue = 999;
                }
            }
            else
            {
                cbbunidadremolque.SelectedValue = 8;
                cbbclase.SelectedValue = 999;
                cbbclase.IsEnabled = false;
            }
            if (rowremolque.Unidad == 4 || rowremolque.Unidad == 8)
            {
                cbbunidadremolque.SelectedValue = rowremolque.Unidad;
            }
            else
            {
                Mensabll = new MensajeBLL();
                Mensabll.tipomensajes(9, "");
                //DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdPlacaRO);
                Venmsj = new VentanaMensaje();
                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                Venmsj.ShowDialog();
            }
            if (rowremolque.IdMarca != null)
                cbbmarcaremolque.SelectedValue = rowremolque.IdMarca;
            else
            {
                cbbmarcaremolque.SelectedValue = rowremolque.IdMarca;
                Mensabll = new MensajeBLL();
                Mensabll.tipomensajes(27, "");
                //DSMens.AddDSMensajesHvVehiculoRow(Mensabll.titulos, Mensabll.mensajes, lista[0].IdPlacaRO);
                Venmsj = new VentanaMensaje();
                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                Venmsj.ShowDialog();
            }
            if (rowremolque.IdClase != null)
            {
                if (Convert.ToInt32(cbbtipocarroceria.SelectedValue) != rowremolque.IdClase)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(26, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    cbbclase.SelectedValue = rowremolque.IdClase;
                }
                else
                {
                    cbbclase.SelectedValue = rowremolque.IdClase;
                }
            }
            else
            {
                cbbclase.SelectedValue = 999;
            }
            txtpesoremolque.Text = rowremolque.PesoVacio.ToString();
            dtfeHidrostatica.SelectedDate = rowremolque.VencePruebaHidro;
            txtTPropiedadremolque.Text = rowremolque.TarjetadePropiedad;
            dtfeAforotanque.SelectedDate = rowremolque.FechaAforo;
            if (rowremolque.FechaVenceKingPin != null)
            {
                dtfekingpin.SelectedDate = rowremolque.FechaVenceKingPin;
            }
            else
            {
                dtfekingpin.SelectedDate = null;
            }
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            if (rowremolque.FechaVenceKingPin != null)
            {
                dtfekingpin.SelectedDate = rowremolque.FechaVenceKingPin;
            }
            else
            {
                dtfekingpin.SelectedDate = null;
            }
            if (rowremolque.IdProp != null)
            {
                txtNumdocumentopropi_rem.Text = rowremolque.IdProp.ToString();
            }
            else
            {
                dtfekingpin.SelectedDate = null;
            }
            if (rowremolque.dv != null)
            {
                txtNitVerificacionPropi_rem.Text = rowremolque.dv.ToString();
            }
            else
            {
                txtNumdocumentopropi_rem.Text = "";
            }
            if (rowremolque.Nombre != null)
            {
                txtnombrespropi_rem.Text = rowremolque.Nombre;
            }
            else
            {
                txtnombrespropi_rem.Text = "";
            }
        }

        private void ConsultaRemolque(string placaremolque)
        {
            var lista = RemolBll.GetRemolquePorPlaca(placaremolque);
            if (lista.Count > 0)
            {
                var rowafiliacio = (TipoAfiliacion)cbbtipoafiliacion.SelectedItem;
                if (rowafiliacio != null)
                {
                    if (autorizamoftipoAfilremolque == false)
                    {
                        if (rowafiliacio.IdCodigo != lista[0].TipoAfiliacion)
                        {
                            MessageBox.Show("Usted no puede asociar un remoque con un tipo de afiliación diferente al del cabezote.");
                            return;
                        }
                    }
                }
                MessageBoxResult result1 = MessageBox.Show("Este Remolque ya existe en base de datos. ¿Desea Asignar estos datos a éste vehículo?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result1 == MessageBoxResult.Yes)
                {
                    foreach (var row in lista)
                    {
                        LlenarDatosRemolque(row);
                        break;
                    }
                    MoveFocus(new TraversalRequest(
                        FocusNavigationDirection.Next));
                }
                else
                {
                    existeremolque = false;
                    txtplacaremolque.Text = "";
                    MoveFocus(new TraversalRequest(
                        FocusNavigationDirection.Last));
                    limpiarCamposremolque();
                }
            }
            else
            {
                MessageBoxResult result1 = MessageBox.Show("Este remolque no existe en la base de datos desea crealo?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result1 == MessageBoxResult.Yes)
                {
                    validarpermisosoperacioninternacional();
                    string id = txtplacaremolque.Text.Trim();
                    limpiarCamposremolque();
                    txtplacaremolque.Text = id;
                    existeremolque = false;
                }
            }
        }

        private void limpiarCamposremolque()
        {
            txtpesoremolque.Text = "";
            txtTPropiedadremolque.Text = "";
            txtplacaremolque.Text = "";
            txtmodeloremolque.Text = "";
            txtcapacidadremolque.Text = "";
            dtfeAforotanque.SelectedDate = null;
            dtfeHidrostatica.SelectedDate = null;
            dtfekingpin.SelectedDate = null;
            txtNumdocumentopropi_rem.Text = "";
            txtnombrespropi_rem.Text = "";
        }

        private void limpiarcamposconductor()
        {
            txtnombres.Text = "";
            txtapellido1.Text = "";
            txtapellido2.Text = "";
            dtfechanacimieto.SelectedDate = null;
            cbbdeparconductor.SelectedIndex = 1;
            cargarCiudadesconductor(Convert.ToInt32(cbbdeparconductor.SelectedValue));
            cbbciudadconductor.SelectedIndex = 1;
            txttelefonocasa.Text = "";
            txtIndicativo.Text = "";
            txtcelular.Text = "";
            txtdireccioncasa.Text = "";
            cbbcateglicencia.SelectedValue = 2;
            txtnumlicencia.Text = "";
            dtfechalicencia.SelectedDate = null;
            txteps.Text = "";
            txtARL.Text = "";
            dtfechaPagoSSocial.SelectedDate = null;
            txtexperiencia.Text = "";
            txtnumcuenta.Text = "";
            cbbbanco.SelectedIndex = 1;
            txtidtarjeta.Text = "";
            txtnombreferencia.Text = "";
            txttelreferencia.Text = "";
            txtobservacionesconductor.Text = "";
            txtobservacionesruta.Text = "";
            rdbbajocondu.IsChecked = true;
            rdbafiliado.IsChecked = false;
            txtnombreempresa1.Text = "";
            txttelempresa1.Text = "";
            txtantiguedadempresa1.Text = "";
            txttipocargaempresa1.Text = "";
            txtnombreempresa2.Text = "";
            txttelempresa2.Text = "";
            txtantiguedadempresa2.Text = "";
            txttipocargaempresa2.Text = "";
            rdblistaNoaprueba.IsChecked = true;
            dtfechaEstudioConductor.SelectedDate = null;
            txtnitcuenta.Text = "";
            rdbeletronico.IsChecked = true;
            //dtFechaPagoSalud.SelectedDate = null;
            //dtFechaExamenes.SelectedDate = null;
        }

        private void limpiarErroresVehiculo()
        {
            lblerror_apellido1Cond.Content = "";
            lblerror_documenConductor.Content = "";
            lblerror_nombresCond.Content = "";
            lblerror_apellido2Cond.Content = "";
            lblerror_fechanamcond.Content = "";
            lblerror_numlicencia.Content = "";
            lblerror_experiencia.Content = "";
            lblerror_fechalicencia.Content = "";
            lblerror_numcuenta.Content = "";
            lblerror_idtarjeta.Content = "";
            lblerror_VenceContrato.Content = "";
        }

        private void limpiarcamposbeneficiario()
        {
            txtNitVerificacionBenefi.Text = "";
            cbbtipodocumentobenefi.SelectedIndex = 1;
            txtNumdocumentobenefi.Text = "";
            txtnombresbenefi.Text = ""; ;
            txtapellidobenefi1.Text = "";
            txtapellidobenefi2.Text = "";
            txtdireccionbenefi.Text = "";
            txttelefonobenefi.Text = "";
            txtIndicativobenefi.Text = "";
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
            txtNitVerificacionPropi.Text = "";
            rdblistaNoapruebapropi.IsChecked = true;
            dtfechaestudiopropi.SelectedDate = null;
            txtNitVerificacionPropi.Text = "";
        }

        #endregion

        #region Eventos

        #region Eventos Botones

        private void btnaprobar_Click(object sender, RoutedEventArgs e)
        {
            string idcontrato = ((Button)sender).CommandParameter.ToString();
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

            var lista = contratobll.GetExistenciaContrato(Convert.ToInt32(idcontrato));
            if (lista.Count > 0)
            {
                VentanaAprobarContratos.Close();
                VentanaAprobarContratos = new AprobarContratos();
                VentanaAprobarContratos.SetListaContrato(lista, usuario);
                VentanaAprobarContratos.GetPadre(this);
                VentanaAprobarContratos.Show();
            }
        }

        private void btn_AutorizaInfo_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(18);
            Ventanadoc.IdDocumento(txtNumdocumento.Text);
            Ventanadoc.Show();
        }

        private void btn_aceptar_Click(object sender, RoutedEventArgs e)
        {
            if (txtplaca.Text.Trim() == "")
            {
                MessageBox.Show("Ésta Acción no está permitida.");
                return;
            }
            var lista = vehibll.ConsultaExistenciaVehiculo(txtplaca.Text.Trim());
            if (lista.Count > 0)
            {

            }
            else
            {
                MessageBox.Show("El vehículo no Existe, auditoría será informada que está modificando estados de una placa que no existe.");
                return;
            }
            Boolean habilita_viaje = false;
            if (DSMens.Rows.Count > 0 & Convert.ToInt32(cbbestadoaprobacion.SelectedValue) == 1)
            {
                Mensabll = new MensajeBLL();
                Mensabll.tipomensajes(34, "");
                Venmsj = new VentanaMensaje();
                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                Venmsj.ShowDialog();
            }
            else
            {
                MessageBoxResult result1 = MessageBox.Show("Está Seguro que desea Modificar el estado a la placa?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result1 == MessageBoxResult.Yes)
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
                    if (txtobservacionesaprobacion.Text == "")
                    {
                        lblerror_observaciones.Foreground = new SolidColorBrush(Colors.Red);
                        lblerror_observaciones.Content = "Debe digitar el motivo del cambio de estado";
                    }
                    else
                    {
                        var itemesta = (VehiT_ConsultaEstadoPlaca_Result)cbbestadoaprobacion.SelectedItem;
                        if (Convert.ToInt32(itemesta.Estado) == 1)
                        {
                            MessageBoxResult result2 = MessageBox.Show("Desea Habilitar para un sólo viaje?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result2 == MessageBoxResult.Yes)
                            {
                                habilita_viaje = true;
                            }
                            else
                            {
                                habilita_viaje = false;
                            }
                            Boolean respuesta;
                            respuesta = vehibll.ActualizaEstadoPlaca(strplaca, Convert.ToInt16(itemesta.Estado), txtobservacionesaprobacion.Text, habilita_viaje, usuario);
                            mostrarmensaje(respuesta);
                        }
                        else
                        {
                            Boolean respuesta;
                            respuesta = vehibll.ActualizaEstadoPlaca(strplaca, Convert.ToInt16(itemesta.Estado), txtobservacionesaprobacion.Text, habilita_viaje, usuario);
                            mostrarmensaje(respuesta);
                        }
                    }
                }
            }
        }

        private void btn_regresar_Click(object sender, RoutedEventArgs e)
        {
            if (Iniconsul != null)
            {
                main.pnl_inicio.Children.Clear();
                main.pnl_inicio.Children.Add(Iniconsul);
                Iniconsul.RecibirPadre(main);
            }
            else
                btn_regresar.IsEnabled = false;

        }

        private void btn_Alertas_Click(object sender, RoutedEventArgs e)
        {
            if (DSMens.Rows.Count > 0)
            {
                VentanaGrillaAlertas ventGrilla = new VentanaGrillaAlertas();
                ventGrilla.GetDatosGrilla(DSMens);
                ventGrilla.Show();
            }
            else
                btn_Alertas.IsEnabled = false;
        }

        #endregion

        #region eventos grilla

        private void gridlistadocontratos_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            var currentCell = e.ClipboardRowContent[gridlistadocontratos.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
        }

        #endregion

        #region Eventos DropDownlist

        private void cbbtipovehiculo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbtipovehiculo.SelectedValue != null)
            {
                switch (Convert.ToInt32(cbbtipovehiculo.SelectedValue))
                {
                    case 8:
                    case 4:
                    case 1:
                        if (Convert.ToInt32(cbbtipocarroceria.SelectedValue) == 67)
                        {
                            dtfeHidrostaticaSencillo.IsEnabled = true;
                            dtfeAforotanqueSencillo.IsEnabled = true;
                            btn_hidrostaticaSencillo.IsEnabled = true;
                            btn_aforotanqueSencillo.IsEnabled = true;
                        }
                        else
                        {
                            dtfeHidrostaticaSencillo.SelectedDate = null;
                            dtfeAforotanqueSencillo.SelectedDate = null;
                            dtfeHidrostaticaSencillo.IsEnabled = false;
                            dtfeAforotanqueSencillo.IsEnabled = false;
                            btn_hidrostaticaSencillo.IsEnabled = false;
                            btn_aforotanqueSencillo.IsEnabled = false;
                        }
                        break;
                    default:
                        dtfeHidrostaticaSencillo.SelectedDate = null;
                        dtfeAforotanqueSencillo.SelectedDate = null;
                        dtfeHidrostaticaSencillo.IsEnabled = false;
                        dtfeAforotanqueSencillo.IsEnabled = false;
                        btn_hidrostaticaSencillo.IsEnabled = false;
                        btn_aforotanqueSencillo.IsEnabled = false;
                        break;
                }
            }
            else
            {
                dtfeHidrostaticaSencillo.SelectedDate = null;
                dtfeAforotanqueSencillo.SelectedDate = null;
                dtfeHidrostaticaSencillo.IsEnabled = false;
                dtfeAforotanqueSencillo.IsEnabled = false;
                btn_hidrostaticaSencillo.IsEnabled = false;
                btn_aforotanqueSencillo.IsEnabled = false;
            }
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
                    txtNitVerificacionBenefi.Visibility = Visibility.Collapsed;
                    txtapellidobenefi1.Visibility = Visibility.Visible;
                    txtapellidobenefi2.Visibility = Visibility.Visible;
                }
            }
        }

        private void cbbcateglicencia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dtfechalicencia.SelectedDate = null;
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

        private void cbbdeparconductor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = Convert.ToInt32(cbbdeparconductor.SelectedValue);
            cargarCiudadesconductor(id);
        }

        private void cbbtipocarroceria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idcarroceria = Convert.ToInt32(cbbtipocarroceria.SelectedValue);
            cbbtiporemolque.SelectedValue = idcarroceria;
            cbbtiporemolque.IsEnabled = false;
            switch (idcarroceria)
            {
                case 67:
                    //if (cbbtipovehiculo.SelectedValue != null)
                    //{
                    //    switch (Convert.ToInt32(cbbtipovehiculo.SelectedValue))
                    //    {
                    //        case 8:
                    //        case 4:
                    //        case 1:
                    //            if (Convert.ToInt32(cbbtipocarroceria.SelectedValue) == 67)
                    //            {
                    //                dtfeHidrostaticaSencillo.IsEnabled = true;
                    //                dtfeAforotanqueSencillo.IsEnabled = true;
                    //                btn_hidrostaticaSencillo.IsEnabled = true;
                    //                btn_aforotanqueSencillo.IsEnabled = true;
                    //            }
                    //            else
                    //            {
                    //                dtfeHidrostaticaSencillo.SelectedDate = null;
                    //                dtfeAforotanqueSencillo.SelectedDate = null;
                    //                dtfeHidrostaticaSencillo.IsEnabled = false;
                    //                dtfeAforotanqueSencillo.IsEnabled = false;
                    //                btn_hidrostaticaSencillo.IsEnabled = false;
                    //                btn_aforotanqueSencillo.IsEnabled = false;
                    //            }

                    //            break;
                    //        default:
                    //            dtfeHidrostaticaSencillo.SelectedDate = null;
                    //            dtfeAforotanqueSencillo.SelectedDate = null;
                    //            dtfeHidrostaticaSencillo.IsEnabled = false;
                    //            dtfeAforotanqueSencillo.IsEnabled = false;
                    //            btn_hidrostaticaSencillo.IsEnabled = false;
                    //            btn_aforotanqueSencillo.IsEnabled = false;
                    //            break;

                    //    }
                    //}
                    //else
                    //{
                    //    dtfeHidrostaticaSencillo.SelectedDate = null;
                    //    dtfeAforotanqueSencillo.SelectedDate = null;
                    //    dtfeHidrostaticaSencillo.IsEnabled = false;
                    //    dtfeAforotanqueSencillo.IsEnabled = false;
                    //    btn_hidrostaticaSencillo.IsEnabled = false;
                    //    btn_aforotanqueSencillo.IsEnabled = false;
                    //}
                    cbbclase.IsEnabled = true;
                    cbbunidadremolque.SelectedValue = 4;
                    if (chk_remolque.IsChecked == true)
                    {
                        dtfeHidrostaticaSencillo.SelectedDate = null;
                        dtfeAforotanqueSencillo.SelectedDate = null;
                        dtfeHidrostaticaSencillo.IsEnabled = false;
                        dtfeAforotanqueSencillo.IsEnabled = false;
                        btn_hidrostaticaSencillo.IsEnabled = false;
                        btn_aforotanqueSencillo.IsEnabled = false;
                    }
                    else
                    {
                        if (cbbtipovehiculo.SelectedValue != null)
                        {
                            switch (Convert.ToInt32(cbbtipovehiculo.SelectedValue))
                            {
                                case 8:
                                case 4:
                                case 1:
                                    dtfeHidrostaticaSencillo.IsEnabled = true;
                                    dtfeAforotanqueSencillo.IsEnabled = true;
                                    btn_hidrostaticaSencillo.IsEnabled = true;
                                    btn_aforotanqueSencillo.IsEnabled = true;
                                    break;
                                default:
                                    dtfeHidrostaticaSencillo.SelectedDate = null;
                                    dtfeAforotanqueSencillo.SelectedDate = null;
                                    dtfeHidrostaticaSencillo.IsEnabled = false;
                                    dtfeAforotanqueSencillo.IsEnabled = false;
                                    btn_hidrostaticaSencillo.IsEnabled = false;
                                    btn_aforotanqueSencillo.IsEnabled = false;
                                    break;
                            }
                        }
                    }
                    // CargarEjesVehiculo(0);
                    //MessageBox.Show("Seleccionar un cambio en el tipo de carrocería genera un cambio en la cantidad de ejes permitida en la carrocería");
                    break;
                case 61:
                    //   CargarEjesVehiculo(61);
                    // MessageBox.Show ("Seleccionar un cambio en el tipo de carrocería genera un cambio en la cantidad de ejes permitida en la carrocería");
                    break;
                default:
                    // CargarEjesVehiculo(0);
                    dtfeAforotanqueSencillo.SelectedDate = null;
                    dtfeAforotanqueSencillo.IsEnabled = false;
                    dtfeHidrostaticaSencillo.SelectedDate = null;
                    dtfeHidrostaticaSencillo.IsEnabled = false;
                    cbbunidadremolque.SelectedValue = 8;
                    cbbclase.IsEnabled = false;
                    cbbclase.SelectedValue = 999;
                    // MessageBox.Show("Seleccionar un cambio en el tipo de carrocería genera un cambio en la cantidad de ejes permitida en la carrocería");
                    break;
            }
            //if (idcarroceria == 67)
            //{
            //    cbbclase.IsEnabled = true;
            //    cbbunidadremolque.SelectedValue = 4;
            //    if (chk_remolque.IsChecked == true)
            //    {
            //        dtfeAforotanqueSencillo.IsEnabled = false;
            //        dtfeAforotanqueSencillo.SelectedDate = null;
            //    }
            //    else
            //    {
            //        dtfeAforotanqueSencillo.IsEnabled = true;
            //    }
            //}
            //else
            //{
            //    dtfeAforotanqueSencillo.IsEnabled = false;
            //    dtfeAforotanqueSencillo.SelectedDate = null;
            //    //if (chk_remolque.IsChecked == true)
            //    //{
            //    //    dtfeAforotanqueSencillo.IsEnabled = false;
            //    //}
            //    //else
            //    //{
            //    //    dtfeAforotanqueSencillo.IsEnabled = true;
            //    //}

            //    cbbunidadremolque.SelectedValue = 8;
            //    cbbclase.IsEnabled = false ;
            //    cbbclase.SelectedValue = 999;
            //    dtfeAforotanqueSencillo.IsEnabled = true;
            //}
        }

        private void cbbmarca_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idmarca = Convert.ToInt32(cbbmarca.SelectedValue);
            int idmatca2 = GetIDminTransmarcaPorID(idmarca);
            CargarLinea(idmarca);
        }

        private int GetIDminTransmarcaPorID(int idmarca)
        {
            var lista = MarBll.GetMarcaPorID(idmarca);
            int idmarca2 = Convert.ToInt32(lista[0].CodigoMintrans);
            return Convert.ToInt32(idmarca2);
        }

        private void cbbagenciabenefi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbagenciabenefi.SelectedValue != null)
            {
                txtagenciabenefi.Text = cbbagenciabenefi.SelectedValue.ToString();
            }
        }

        #endregion

        #region Eventos CheckBox

        private void chkobservacionestenedor_Checked(object sender, RoutedEventArgs e)
        {
            txtobservacionestenedor.IsEnabled = true;
        }

        private void chkobservacionestenedor_Unchecked(object sender, RoutedEventArgs e)
        {
            txtobservacionestenedor.IsEnabled = false;
        }

        private void chkmodificarobservaciones_Checked(object sender, RoutedEventArgs e)
        {
            txtobservaciones.IsEnabled = true;
        }

        private void chkmodificarobservaciones_Unchecked(object sender, RoutedEventArgs e)
        {
            txtobservaciones.IsEnabled = false;
        }

        private void chkobservacionesconductor_Checked(object sender, RoutedEventArgs e)
        {
            txtobservacionesconductor.IsEnabled = true;
        }

        private void chkobservacionesconductor_Unchecked(object sender, RoutedEventArgs e)
        {
            txtobservacionesconductor.IsEnabled = false;
        }

        private void chkobservacionesruta_Checked(object sender, RoutedEventArgs e)
        {
            txtobservacionesruta.IsEnabled = true;
        }

        private void chkobservacionesruta_Unchecked(object sender, RoutedEventArgs e)
        {
            txtobservacionesruta.IsEnabled = false;
        }

        private void chk_remolque_Checked(object sender, RoutedEventArgs e)
        {
            GrBo_inforemolque.IsEnabled = true;
            GrBo_inforemolque.Visibility = Visibility.Visible;
            dtfechaquitanrueda.DisplayDateStart = DateTime.Now.Date;
            //dtfechaquitanrueda.SelectedDate = null;
            dtfechaquitanrueda.IsEnabled = true;
            Btn_quintaRuta.IsEnabled = true;
            CargarclaseTanque();
            CargarTipoCarroceriaRemolque();
            int tipocarroceria = Convert.ToInt32(cbbtipocarroceria.SelectedValue);
            cbbtiporemolque.SelectedValue = tipocarroceria;
            dtfeAforotanqueSencillo.SelectedDate = null;
            dtfeAforotanqueSencillo.IsEnabled = false;
            btn_aforotanqueSencillo.Visibility = Visibility.Collapsed;
        }

        private void chk_remolque_Unchecked(object sender, RoutedEventArgs e)
        {
            GrBo_inforemolque.IsEnabled = false;
            GrBo_inforemolque.Visibility = Visibility.Collapsed;
            Btn_quintaRuta.IsEnabled = false;
            dtfechaquitanrueda.SelectedDate = null;
            dtfechaquitanrueda.IsEnabled = false;
            if (Convert.ToInt32(cbbtipocarroceria.SelectedValue) == 67)
            {
                dtfeAforotanqueSencillo.IsEnabled = true;
                btn_aforotanqueSencillo.Visibility = Visibility.Visible;
            }
            else
            {
                dtfeAforotanqueSencillo.SelectedDate = null;
                dtfeAforotanqueSencillo.IsEnabled = false;
                btn_aforotanqueSencillo.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region Eventos botones documentos

        private void btn_contratoleasing_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            if (txtplaca.Text.Trim() == "")
            {
                MessageBox.Show("Debe existir un placa para subir este documento.");
                return;
            }
            var lista = vehibll.ConsultaExistenciaVehiculo(txtplaca.Text.Trim());
            if (lista.Count > 0)
            {
                Ventanadoc = new AdmDocumentos(main);
                Ventanadoc.TipoDocumento(46);
                Ventanadoc.IdDocumento(txtplaca.Text.Trim());
                Ventanadoc.Show();
            }
            else
            {
                MessageBox.Show("El vehículo no Existe, auditoría será informada que está subiendo un documento de una placa que no existe.");
                return;
            }
        }

        private void btn_kingpin_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(26);
            Ventanadoc.IdDocumento(txtplacaremolque.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_docbancarioscond_Click(object sender, RoutedEventArgs e)
        {
            Boolean permiso = true;
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(22);
            Ventanadoc.IdDocumento(txtNumdocumento.Text.Trim());
            Ventanadoc.GetIdentificacionconductor(txtNumdocumento.Text.Trim());
            Ventanadoc.PermisoExaminar(permiso);
            Ventanadoc.Show();
        }

        private void btn_docbancariosben_Click(object sender, RoutedEventArgs e)
        {
            Boolean permiso = true;
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(23);
            Ventanadoc.IdDocumento(txtNumdocumentobenefi.Text.Trim());
            Ventanadoc.GetIdentificacionconductor(txtNumdocumento.Text.Trim());
            Ventanadoc.PermisoExaminar(permiso);
            Ventanadoc.Show();
        }

        private void btn_aforotanqueSencillo_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(10);
            Ventanadoc.IdDocumento(txtplaca.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_hidrostaticaSencillo_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(9);
            Ventanadoc.IdDocumento(txtplaca.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_PolizaHc_Click(object sender, RoutedEventArgs e)
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
            if (Convert.ToInt32(cbbtipocarroceria.SelectedValue) == 67)
            {
                if (ventAdmpolizaHc != null)
                {
                    ventAdmpolizaHc.Close();
                }

                ventAdmpolizaHc = new AdmPolizaHC();
                ventAdmpolizaHc.RecibirPadre(main);
                ventAdmpolizaHc.CargarPlaca(txtplaca.Text.Trim(), usuario);
                ventAdmpolizaHc.CargarPlacaRemolque(txtplacaremolque.Text.Trim(), usuario);
                ventAdmpolizaHc.Show();
            }
            else
            {
                Mensabll = new MensajeBLL();
                Mensabll.tipomensajes(32, "");
                Venmsj = new VentanaMensaje();
                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                Venmsj.ShowDialog();
            }
        }

        private void Btn_quintaRuta_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(5);
            Ventanadoc.IdDocumento(txtplaca.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_tpropiedadvehi_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(1);
            Ventanadoc.IdDocumento(txtplaca.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_contratocoltan_Click(object sender, RoutedEventArgs e)
        {

            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(3);
            Ventanadoc.IdDocumento(txtplaca.Text.Trim());
            Ventanadoc.GetIdentificacionconductor(txtNumdocumento.Text.Trim());
            Ventanadoc.PermisoExaminar(cambiarcontrato);
            Ventanadoc.Show();
            CargarContrato();
        }

        private void btn_fotovehiculo_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(2);
            Ventanadoc.IdDocumento(txtplaca.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_tecnicomecanica_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(4);
            Ventanadoc.IdDocumento(txtplaca.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_soat_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(6);
            Ventanadoc.IdDocumento(txtplaca.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_polizaRC_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(11);
            Ventanadoc.IdDocumento(txtplaca.Text.Trim());
            Ventanadoc.Show();

        }

        private void btn_TpropieRemolque_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(7);
            Ventanadoc.IdDocumento(txtplacaremolque.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_hidrostatica_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(9);
            Ventanadoc.IdDocumento(txtplacaremolque.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_aforotanque_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(10);
            Ventanadoc.IdDocumento(txtplacaremolque.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_fotoremolque_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(8);
            Ventanadoc.IdDocumento(txtplacaremolque.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_cedulaconductor_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(12);
            Ventanadoc.IdDocumento(txtNumdocumento.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_licenciaconductor_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(13);
            Ventanadoc.IdDocumento(txtNumdocumento.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_pagoSSocial_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(14);
            Ventanadoc.IdDocumento(txtNumdocumento.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_FotoConductor_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(15);
            Ventanadoc.IdDocumento(txtNumdocumento.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_contratoBeneficiario_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(16);
            Ventanadoc.IdDocumento(txtNumdocumentobenefi.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_certibancaria_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(20);
            Ventanadoc.IdDocumento(txtNumdocumento.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_rut_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(21);
            Ventanadoc.IdDocumento(txtNumdocumento.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_formatCreacion_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(19);
            Ventanadoc.IdDocumento(txtNumdocumento.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_contratoServicio_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(51);
            Ventanadoc.IdDocumento(txtplaca.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_planillaPagoAportes_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(52);
            Ventanadoc.IdDocumento(txtNumdocumento.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_ExamenesMedicos_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(53);
            Ventanadoc.IdDocumento(txtNumdocumento.Text.Trim());
            Ventanadoc.Show();
        }

        #endregion

        #region validaciones campos numéricos

        /// <summary>
        /// Valida la información del Soat permitiendo ingresar una serie inicando por AT y luego solo caracteres numéricos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtnumsoat_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (validarCaracteresespeciales(e, txt) == true)
            {
                if (txt.Text.Length < 2)
                {
                    if (e.Key == Key.A || e.Key == Key.T)
                    {

                        e.Handled = false;
                    }
                    else
                        e.Handled = true;
                }
                else
                {
                    if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                    {
                        e.Handled = false;
                    }
                    else
                        e.Handled = true;
                }
            }
            else
            {
                txt.Text = "";
            }
        }

        private void Texboxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox Text = (TextBox)sender;
            if (numeric.IsNumeric(Text.Text) == false)
            {
                Text.Text = "";
            }
        }

        private void TexboxtCaracteres_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox Text = (TextBox)sender;
            Regex r = new Regex("^[A-Za-z ]+$");
            if (r.IsMatch(Text.Text) == false)
            {
                Text.Text = "";
            }
        }
        /// <summary>
        /// Se validan las diferentes restricciones de fechas del modelo repotenciado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtmodelorepotenciado_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtmodelorepotenciado.Text.Length == 4)
            {
                lblerror_MRepotenciado.Content = "";
                if (txtmodelo.Text != "")
                {
                    if (numeric.IsNumeric(txtmodelo.Text) == true)
                    {
                        int anioactual = DateTime.Now.Year;
                        if (Convert.ToInt32(txtmodelorepotenciado.Text) > anioactual)
                        {
                            Mensabll = new MensajeBLL();
                            Mensabll.tipomensajes(18, "");
                            Venmsj = new VentanaMensaje();
                            Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                            Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                            Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                            Venmsj.ShowDialog();
                            txtmodelorepotenciado.Text = "";
                        }
                        else
                        {
                            if (Convert.ToInt32(txtmodelorepotenciado.Text) <= Convert.ToInt32(txtmodelo.Text))
                            {
                                Mensabll = new MensajeBLL();
                                Mensabll.tipomensajes(19, "");
                                Venmsj = new VentanaMensaje();
                                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                                Venmsj.ShowDialog();
                                txtmodelorepotenciado.Text = "";
                            }
                        }
                    }
                    else
                    {
                        lblerror_MRepotenciado.Content = "***";
                        txtmodelorepotenciado.Text = "";
                    }
                }
                else
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(17, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    txtmodelorepotenciado.Text = "";
                }
            }
            else
                if (txtmodelorepotenciado.Text.Length == 0)
                {
                    lblerror_MRepotenciado.Content = "";
                }
                else
                    lblerror_MRepotenciado.Content = "***";
        }
        /// <summary>
        /// Se validan las diferenctes restricciones de fechas  del modelo del vehículo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtmodelo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtmodelo.Text.Length == 4)
            {
                if (numeric.IsNumeric(txtmodelo.Text) == true)
                {
                    lblerror_Modelo.Content = "";
                    if (txtmodelorepotenciado.Text != "")
                    {
                        txtmodelorepotenciado.Text = "";
                    }
                    else
                    {
                        int anioactual = DateTime.Now.Year;
                        if (Convert.ToInt32(txtmodelo.Text) <= 1900)
                        {
                            Mensabll = new MensajeBLL();
                            Mensabll.tipomensajes(20, "");
                            Venmsj = new VentanaMensaje();
                            Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                            Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                            Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                            Venmsj.ShowDialog();
                            txtmodelo.Text = "";
                        }
                        else
                        {
                            if (Convert.ToInt32(txtmodelo.Text) > (anioactual + 1))
                            {
                                Mensabll = new MensajeBLL();
                                Mensabll.tipomensajes(21, "");
                                Venmsj = new VentanaMensaje();
                                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                                Venmsj.ShowDialog();
                                txtmodelo.Text = "";
                            }
                        }
                    }
                }
                else
                {
                    txtmodelo.Text = "";
                    lblerror_Modelo.Content = "***";
                }
            }
            else
            {
                if (txtmodelo.Text.Length == 0)
                    lblerror_Modelo.Content = "";
                else
                    lblerror_Modelo.Content = "***";
            }
        }
        /// <summary>
        /// valida los modelos del remolque de acuerdo a las restricciones dle negocio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtmodeloremolque_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtmodeloremolque.Text.Length == 4)
            {
                if (numeric.IsNumeric(txtmodeloremolque.Text) == true)
                {
                    lblerror_modeloremolque.Content = "";
                    int anioactual = DateTime.Now.Year;
                    if (Convert.ToInt32(txtmodeloremolque.Text) <= 1900)
                    {
                        Mensabll = new MensajeBLL();
                        Mensabll.tipomensajes(20, "");
                        Venmsj = new VentanaMensaje();
                        Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                        Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                        Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                        Venmsj.ShowDialog();
                        txtmodeloremolque.Text = "";
                    }
                    else
                    {
                        if (Convert.ToInt32(txtmodeloremolque.Text) > (anioactual + 1))
                        {
                            Mensabll = new MensajeBLL();
                            Mensabll.tipomensajes(21, "");
                            Venmsj = new VentanaMensaje();
                            Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                            Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                            Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                            Venmsj.ShowDialog();
                            txtmodeloremolque.Text = "";
                        }
                    }
                }
                else
                {
                    lblerror_modeloremolque.Content = "***";
                    txtmodeloremolque.Text = "";
                }
            }
            else
            {
                if (txtmodeloremolque.Text.Length == 0)
                    lblerror_modeloremolque.Content = "";
                else
                    lblerror_modeloremolque.Content = "***";
            }
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

        #region validaciones fechas

        private void dtfekingpin_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtfekingpin.SelectedDate != null)
            {
                DateTime fechafutura;
                DateTime fechaaux = (DateTime)dtfekingpin.SelectedDate;
                fechafutura = DateTime.Now;
                fechafutura = fechafutura.AddMonths(6);
                if (dtfekingpin.SelectedDate <= FechaActual.SelectedDate)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(14, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfekingpin.SelectedDate = null;
                    dtfekingpin.DisplayDateStart = DateTime.Now;
                }
                else
                {
                    if (dtfekingpin.SelectedDate > fechafutura)
                    {
                        Mensabll = new MensajeBLL();
                        Mensabll.tipomensajes(58, "");
                        Venmsj = new VentanaMensaje();
                        Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                        Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                        Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                        Venmsj.ShowDialog();
                        dtfekingpin.SelectedDate = null;
                        dtfekingpin.DisplayDateEnd = fechafutura;
                    }
                }
            }
        }

        private void dtfeAforotanqueSencillo_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtfeAforotanqueSencillo.SelectedDate != null)
            {
                DateTime fechafutura;
                DateTime fechaaux = (DateTime)dtfeAforotanqueSencillo.SelectedDate;
                fechafutura = DateTime.Now;
                fechafutura = fechafutura.AddYears(1);

                if (dtfeAforotanqueSencillo.SelectedDate > FechaActual.SelectedDate)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(15, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfeAforotanqueSencillo.SelectedDate = null;
                    dtfeAforotanqueSencillo.DisplayDateEnd = DateTime.Now;
                }

            }

        }

        private void dtfeHidrostaticaSencillo_CalendarClosed(object sender, RoutedEventArgs e)
        {

            if (dtfeHidrostaticaSencillo.SelectedDate != null)
            {
                DateTime fechafutura;
                DateTime fechaaux = (DateTime)dtfeHidrostaticaSencillo.SelectedDate;
                fechafutura = DateTime.Now;
                fechafutura = fechafutura.AddYears(1);

                if (dtfeHidrostaticaSencillo.SelectedDate < FechaActual.SelectedDate)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(14, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfeHidrostaticaSencillo.SelectedDate = null;
                    dtfeHidrostaticaSencillo.DisplayDateStart = DateTime.Now;
                }

            }
        }

        private void dtFechaVenceContrato_CalendarClosed(object sender, RoutedEventArgs e)
        {
            //if (dtFechaVenceContrato.SelectedDate != null)
            //{
            //    if (dtFechaVenceContrato.SelectedDate < FechaActual.SelectedDate)
            //    {
            //        Mensabll = new MensajeBLL();
            //        Mensabll.tipomensajes(14, "");
            //        Venmsj = new VentanaMensaje();
            //        Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
            //        Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
            //        Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
            //        Venmsj.ShowDialog();
            //        dtFechaVenceContrato.SelectedDate = null;
            //        dtFechaVenceContrato.DisplayDateStart = DateTime.Now;
            //    }
            //}
        }

        private void dtFechaPagoSalud_CalendarClosed(object sender, RoutedEventArgs e)
        {
            //if (dtFechaPagoSalud.SelectedDate != null)
            //{
            //    DateTime FechaPlanilla = (DateTime)dtFechaPagoSalud.SelectedDate;
            //    FechaPlanilla = FechaPlanilla.AddMonths(1);

            //    if (FechaPlanilla < FechaActual.SelectedDate)
            //    {
            //        Mensabll = new MensajeBLL();
            //        Mensabll.tipomensajes(62, "");
            //        Venmsj = new VentanaMensaje();
            //        Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
            //        Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
            //        Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
            //        Venmsj.ShowDialog();
            //        dtFechaPagoSalud.SelectedDate = null;
            //        dtFechaPagoSalud.DisplayDateStart = DateTime.Now;
            //    }
            //}
        }

        private void dtFechaExamenes_CalendarClosed(object sender, RoutedEventArgs e)
        {

        }

        private void dtfechaEstudioConductor_CalendarOpened(object sender, RoutedEventArgs e)
        {
            dtfechaEstudioConductor.SelectedDate = null;
            dtfechaEstudioConductor.DisplayDateStart = DateTime.Now;
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
        /// <summary>
        /// la fecha no puede ser superior a 1 año ni iferior a la fecha actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtfechatmecanica_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtfechatmecanica.SelectedDate != null)
            {
                DateTime fechafutura;
                DateTime fechaaux = (DateTime)dtfechatmecanica.SelectedDate;
                fechafutura = DateTime.Now;
                int aniovehiculo = 0;
                Boolean anioauxbool = false;
                int anioactual = Convert.ToInt32(fechafutura.Year.ToString());
                if (txtmodelo.Text != "")
                {
                    aniovehiculo = Convert.ToInt32(txtmodelo.Text);
                }
                if (anioactual == aniovehiculo)
                {
                    fechafutura = fechafutura.AddYears(5);
                    anioauxbool = true;
                }
                if (anioactual == (aniovehiculo + 1))
                {
                    fechafutura = fechafutura.AddYears(4);
                    anioauxbool = true;
                }
                if (anioactual == (aniovehiculo + 2))
                {
                    fechafutura = fechafutura.AddYears(3);
                    anioauxbool = true;
                }
                if (anioactual == (aniovehiculo + 3))
                {
                    fechafutura = fechafutura.AddYears(2);
                    anioauxbool = true;
                }
                if (anioauxbool == false)
                {
                    fechafutura = fechafutura.AddYears(1);
                }
                if (fechaaux >= fechafutura)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(13, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfechatmecanica.SelectedDate = null;
                    dtfechatmecanica.DisplayDateStart = DateTime.Now;
                    //dtfechasoat.DisplayDateEnd = DateTime.Now.AddYears(1);
                }
                else
                {
                    if (dtfechatmecanica.SelectedDate < FechaActual.SelectedDate)
                    {
                        Mensabll = new MensajeBLL();
                        Mensabll.tipomensajes(14, "");
                        Venmsj = new VentanaMensaje();
                        Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                        Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                        Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                        Venmsj.ShowDialog();
                        dtfechatmecanica.SelectedDate = null;
                        dtfechatmecanica.DisplayDateStart = DateTime.Now;
                    }
                }
            }
        }
        /// <summary>
        /// la fecha no puede ser superior a 1 año ni iferior a la fecha actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtfechasoat_CalendarClosed(object sender, RoutedEventArgs e)
        {
            //if (dtfechasoat.SelectedDate != null)
            //{
            //    DateTime fechafutura;
            //    DateTime fechaaux = (DateTime)dtfechasoat.SelectedDate;
            //    fechafutura = DateTime.Now;
            //    fechafutura = fechafutura.AddYears(1);
            //    if (fechaaux >= fechafutura)
            //    {
            //        Mensabll = new MensajeBLL();
            //        Mensabll.tipomensajes(13,"");
            //        Venmsj = new VentanaMensaje();
            //        Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
            //        Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
            //        Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
            //        Venmsj.ShowDialog();
            //        dtfechasoat.SelectedDate = null;
            //        dtfechasoat.DisplayDateStart = DateTime.Now;

            //    }
            //    else
            //    {
            //        if (dtfechasoat.SelectedDate < FechaActual.SelectedDate)
            //        {
            //            Mensabll = new MensajeBLL();
            //            Mensabll.tipomensajes(14,"");
            //            Venmsj = new VentanaMensaje();
            //            Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
            //            Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
            //            Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
            //            Venmsj.ShowDialog();
            //            dtfechasoat.SelectedDate = null;
            //            dtfechasoat.DisplayDateStart = DateTime.Now;
            //        }
            //    }
            //}

        }
        /// <summary>
        /// La fecha no puede ser superior a la fecha actual, es la fecha en que se realizó el estudio de aforo del tanque
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtfeAforotanque_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtfeAforotanque.SelectedDate != null)
            {
                DateTime fechafutura;
                DateTime fechaaux = (DateTime)dtfeAforotanque.SelectedDate;
                fechafutura = DateTime.Now;
                fechafutura = fechafutura.AddYears(1);

                if (dtfeAforotanque.SelectedDate > FechaActual.SelectedDate)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(15, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfeAforotanque.SelectedDate = null;
                    dtfeAforotanque.DisplayDateEnd = DateTime.Now;
                }

            }
        }
        /// <summary>
        /// la fecha no puede ser superior  a la fecha actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtfeHidrostatica_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtfeHidrostatica.SelectedDate != null)
            {
                DateTime fechafutura;
                DateTime fechaaux = (DateTime)dtfeHidrostatica.SelectedDate;
                fechafutura = DateTime.Now;
                fechafutura = fechafutura.AddYears(1);
                if (dtfeHidrostatica.SelectedDate < FechaActual.SelectedDate)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(14, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfeHidrostatica.SelectedDate = null;
                    dtfeHidrostatica.DisplayDateStart = DateTime.Now;
                }
            }
        }
        /// <summary>
        /// la fecha no puede ser superior a 1 año ni iferior a la fecha actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtfechaRC_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtfechaRC.SelectedDate != null)
            {
                DateTime fechafutura;
                DateTime fechaaux = (DateTime)dtfechaRC.SelectedDate;
                fechafutura = DateTime.Now;
                fechafutura = fechafutura.AddYears(1);
                if (dtfechaRC.SelectedDate < FechaActual.SelectedDate)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(14, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfechaRC.SelectedDate = null;
                    dtfechaRC.DisplayDateStart = DateTime.Now;
                }
            }
        }
        /// <summary>
        /// la fecha no puede ser superior a la fecha actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtfechanacimieto_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtfechanacimieto.SelectedDate != null)
            {
                DateTime fechafutura;
                DateTime fechaaux = (DateTime)dtfechanacimieto.SelectedDate;
                fechafutura = DateTime.Now;
                fechafutura = fechafutura.AddYears(1);
                if (dtfechanacimieto.SelectedDate > FechaActual.SelectedDate)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(15, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfechanacimieto.SelectedDate = null;
                    dtfechanacimieto.DisplayDateEnd = DateTime.Now;
                }
                else
                {
                    CalcularEdad((DateTime)dtfechanacimieto.SelectedDate);
                }
            }
        }
        /// <summary>
        /// la fecha no puede ser superior a 3 años ni iferior a la fecha actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtfechalicencia_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtfechalicencia.SelectedDate != null)
            {
                DateTime fechafutura;
                DateTime fechaaux = (DateTime)dtfechalicencia.SelectedDate;
                fechafutura = DateTime.Now;
                fechafutura = fechafutura.AddYears(3);
                if (dtfechalicencia.SelectedDate < FechaActual.SelectedDate)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(14, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfechalicencia.SelectedDate = null;
                    dtfechalicencia.DisplayDateStart = DateTime.Now;
                }
                else
                {
                    if (Convert.ToInt32(cbbcateglicencia.SelectedValue) >= 8)
                    {
                        if (fechaaux >= fechafutura)
                        {
                            Mensabll = new MensajeBLL();
                            Mensabll.tipomensajes(16, "");
                            Venmsj = new VentanaMensaje();
                            Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                            Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                            Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                            Venmsj.ShowDialog();
                            dtfechalicencia.SelectedDate = null;
                            dtfechalicencia.DisplayDateStart = DateTime.Now;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// la fecha no puede ser superior a la fecha actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtfechaPagoSSocial_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtfechaPagoSSocial.SelectedDate != null)
            {
                DateTime fechafutura;
                DateTime fechaaux = (DateTime)dtfechaPagoSSocial.SelectedDate;
                fechafutura = DateTime.Now;
                fechafutura = fechafutura.AddYears(1);
                if (dtfechaPagoSSocial.SelectedDate > FechaActual.SelectedDate)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(15, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfechaPagoSSocial.SelectedDate = null;
                    dtfechaPagoSSocial.DisplayDateEnd = DateTime.Now;
                }
            }
        }
        /// <summary>
        /// la fecha no puede ser superior a la fecha actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtfechaquitanrueda_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtfechaquitanrueda.SelectedDate != null)
            {
                DateTime fechafutura;
                DateTime fechaaux = (DateTime)dtfechaquitanrueda.SelectedDate;
                fechafutura = DateTime.Now;
                fechafutura = fechafutura.AddYears(1);
                if (dtfechaquitanrueda.SelectedDate < FechaActual.SelectedDate)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(14, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfechaquitanrueda.SelectedDate = null;
                    dtfechaquitanrueda.DisplayDateStart = DateTime.Now;
                }
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
                    Mensabll.tipomensajes(15, "");
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
                    Mensabll.tipomensajes(15, "");
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

        #endregion

        #region Eventos Consulta Desde TextBoxt

        private void txtplaca_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (validarCaracteresespeciales(e, txt) == true)
            {
                if (e.Key == Key.Enter || e.Key == Key.Tab)
                {
                    if (txtplaca.Text != "")
                    {
                        if (ValidarExisteplaca() == true)
                        {
                            Mensabll = new MensajeBLL();
                            Mensabll.tipomensajes(22, "");
                            Venmsj = new VentanaMensaje();
                            Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                            Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                            Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                            Venmsj.ShowDialog();
                            txtplaca.Text = "";
                            // txt.MoveFocus(new TraversalRequest(
                            //FocusNavigationDirection.First));
                        }
                        else
                        {
                            if (validarVehiculoReportado(txtplaca.Text) == true)
                            {
                                Mensabll = new MensajeBLL();
                                Mensabll.tipomensajes(38, "");
                                Venmsj = new VentanaMensaje();
                                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                                Venmsj.ShowDialog();
                                txtplaca.Text = "";
                                // txt.MoveFocus(new TraversalRequest(
                                //FocusNavigationDirection.First));
                            }
                            else
                            {
                                txt.MoveFocus(new TraversalRequest(
                                                               FocusNavigationDirection.Next));
                            }
                        }
                    }
                }
            }
        }

        private bool validarVehiculoReportado(string placa)
        {
            Boolean respuesta = false;
            var lista = vetobll.GetDatosDatosvehiculoReportado(placa);
            if (lista.Count > 0)
            {
                foreach (var item in lista)
                {
                    if (item.Estado == 1)
                    {
                        respuesta = true;
                    }
                    else
                    {
                        respuesta = false;
                    }
                    break;
                }
            }
            else
            {
                respuesta = false;
            }
            return respuesta;
        }

        private void txtNumdocumento_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (validarCaracteresespeciales(e, txt) == true)
            {
                if (e.Key == Key.Enter || e.Key == Key.Tab)
                {
                    if (txtNumdocumento.Text != "")
                    {
                        ConsultarDatosConductor(txtNumdocumento.Text);
                        //Finalizado la consulta
                        //Movemos el foco al siguiente
                        //TextBox
                        txt.MoveFocus(new TraversalRequest(
                            FocusNavigationDirection.Next));
                    }
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
                    if (txtNumdocumentobenefi.Text != "")
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
        }

        private void txtNumdocumentopropi_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (validarCaracteresespeciales(e, txt) == true)
            {
                if (e.Key == Key.Enter || e.Key == Key.Tab)
                {
                    if (txtNumdocumentopropi.Text != "")
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
        }

        private void txtplacaremolque_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (validarCaracteresespeciales(e, txt) == true)
            {
                if (txt.Text.Length < 1)
                {
                    if (e.Key == Key.S || e.Key == Key.R || e.Key == Key.T)
                    {
                        if (e.Key == Key.Enter || e.Key == Key.Tab)
                        {
                            string remolque = txtplacaremolque.Text.Trim();
                            ConsultaRemolque(txtplacaremolque.Text.Trim());
                            txtplacaremolque.Text = remolque;
                        }
                        e.Handled = false;
                    }
                    else
                        e.Handled = true;
                }
                else
                {
                    if (e.Key == Key.Enter || e.Key == Key.Tab)
                    {
                        string remolque = txtplacaremolque.Text.Trim();
                        ConsultaRemolque(txtplacaremolque.Text.Trim());
                        txtplacaremolque.Text = remolque;
                    }
                    else if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                    {
                        e.Handled = false;
                        //if (numeric.IsNumeric(e.Key.ToString()) == true)
                        //    e.Handled = true;
                        //else
                        //    e.Handled = false;
                    }
                    else
                        e.Handled = true;
                }
            }
        }

        private void TextboxtStringShift_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (validarCaracteresespeciales(e, txt) == true)
            {
                e.Handled = false;
            }
        }

        private void btn_Aprobar_Click(object sender, RoutedEventArgs e)
        {
            if (valdiarDAtospolizaRC() == false)
            {
                Boolean respuesta = true;
                respuesta = vehibll.GuardaAprobacionPolizasRC(txtplaca.Text.Trim(), true, main.txtusuario.Text.Trim(), txtnumpolizaRC.Text.Trim(), (DateTime)dtfechaRC.SelectedDate, cbbcomRCE.SelectedValue.ToString());
                if (respuesta == true)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(12, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    btn_Noaprobar.IsEnabled = true;
                    btn_Aprobar.IsEnabled = false;
                    lblmensajepolizaRC.Content = "Póliza RC Activa";
                    //inicioconsul.CargarContrato();
                }
                else
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(23, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();

                }
            }
        }

        private void btn_Noaprobar_Click(object sender, RoutedEventArgs e)
        {
            if (valdiarDAtospolizaRC() == false)
            {
                Boolean respuesta = true;
                respuesta = vehibll.GuardaAprobacionPolizasRC(txtplaca.Text.Trim(), false, main.txtusuario.Text.Trim(), txtnumpolizaRC.Text.Trim(), (DateTime)dtfechaRC.SelectedDate, cbbcomRCE.SelectedValue.ToString());
                if (respuesta == true)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(12, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    btn_Noaprobar.IsEnabled = false;
                    btn_Aprobar.IsEnabled = true;
                    lblmensajepolizaRC.Content = "Póliza RC Inactiva";
                    //inicioconsul.CargarContrato();
                }
                else
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(23, "");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                }
            }
        }

        private void txtpesocabezote_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (txtpesocabezote.Text.Length == 4)
            //{
            //    lblerror_MRepotenciado.Content = "";
            //    if (txtmodelo.Text != "")
            //    {
            //        if (numeric.IsNumeric(txtmodelo.Text) == true)
            //        {
            //            int anioactual = DateTime.Now.Year;
            //            if (Convert.ToInt32(txtmodelorepotenciado.Text) > anioactual)
            //            {
            //                Mensabll = new MensajeBLL();
            //                Mensabll.tipomensajes(18,"");
            //                Venmsj = new VentanaMensaje();
            //                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
            //                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
            //                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
            //                Venmsj.ShowDialog();
            //                txtmodelorepotenciado.Text = "";
            //            }
            //            else
            //            {
            //                if (Convert.ToInt32(txtmodelorepotenciado.Text) <= Convert.ToInt32(txtmodelo.Text))
            //                {
            //                    Mensabll = new MensajeBLL();
            //                    Mensabll.tipomensajes(19,"");
            //                    Venmsj = new VentanaMensaje();
            //                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
            //                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
            //                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
            //                    Venmsj.ShowDialog();
            //                    txtmodelorepotenciado.Text = "";
            //                }

            //            }

            //        }
            //        else
            //        {
            //            lblerror_MRepotenciado.Content = "***";
            //            txtmodelorepotenciado.Text = "";
            //        }


            //    }
            //    else
            //    {
            //        Mensabll = new MensajeBLL();
            //        Mensabll.tipomensajes(17,"");
            //        Venmsj = new VentanaMensaje();
            //        Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
            //        Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
            //        Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
            //        Venmsj.ShowDialog();
            //        txtmodelorepotenciado.Text = "";
            //    }
            //}
            //else
            //    if (txtmodelorepotenciado.Text.Length == 0)
            //    {
            //        lblerror_MRepotenciado.Content = "";
            //    }
            //    else
            //        lblerror_MRepotenciado.Content = "***";
        }

        private void btn_Eps_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(39);
            Ventanadoc.IdDocumento(txtNumdocumento.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_ARL_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(40);
            Ventanadoc.IdDocumento(txtNumdocumento.Text.Trim());
            Ventanadoc.Show();
        }

        #endregion

        #region Eventos Seguridad

        private void GetListausuario(List<seguridad_consultarMenuPermisos2_Result> list)
        {
            var lista = list;
            Boolean boolcambioestadoobser = false;
            Boolean boolcambioestadotipocarga = false;
            Boolean boolcambioestadolistasri = false;
            Boolean boolcambioreftipocargai = false;
            Boolean boolcambiorefantiguedad = false;
            Boolean boolcambiopolizahc = false;
            Boolean boolBotonEstudioSegT = false;
          
            // Boolean boolhabilitarbotonesBene = false;
            var listaprivi = (from rmenu in lista
                              where rmenu.idMenuPadre == 230 & rmenu.alterno == 1 & rmenu.orden == 3
                              select rmenu).ToList();
            foreach (var item in listaprivi)
            {
                switch (item.nombreMenu)
                {
                    case "Puede Modificar Tipo Afiliación Remolque":
                        if (item.Autorizar == 1)
                        {

                            autorizamoftipoAfilremolque = true;
                        }
                        else
                        {
                            autorizamoftipoAfilremolque = false;
                        }
                        break;

                    case "Habilitar Operación Ecuador":
                        if (item.Autorizar == 1)
                        {
                            Chk_Grabar_operEcuador.IsEnabled = true;
                            autorizaoperacioninternacional = true;
                        }
                        else
                        {
                            autorizaoperacioninternacional = false;
                            Chk_Grabar_operEcuador.IsEnabled = true;
                        }
                        break;
                    case "upd Doc beneficiario":
                        if (item.Autorizar == 1)
                        {
                            boolhabilitarbotonesBene = true;
                        }
                        else
                        {
                            boolhabilitarbotonesBene = false;
                        }
                        break;
                    case "Cambiar Observaciones":
                        if (item.Autorizar == 1)
                        {
                            boolcambioestadoobser = true;
                        }
                        else
                        {
                            boolcambioestadoobser = false;
                        }
                        break;
                    case "Cambiar Tipo de Carga":
                        if (item.Autorizar == 1)
                        {
                            boolcambioestadotipocarga = true;
                        }
                        else
                        {
                            boolcambioestadotipocarga = false;
                        }
                        break;
                    case "Listas de Riesgo":
                        if (item.Autorizar == 1)
                        {
                            boolcambioestadolistasri = true;
                        }
                        else
                        {
                            boolcambioestadolistasri = false;
                        }
                        break;
                    case "Referencias antiguedad":
                        if (item.Autorizar == 1)
                        {
                            boolcambiorefantiguedad = true;
                        }
                        else
                        {
                            boolcambiorefantiguedad = false;
                        }
                        break;
                    case "Referencias Tipo de Carga":
                        if (item.Autorizar == 1)
                        {
                            boolcambioreftipocargai = true;
                        }
                        else
                        {
                            boolcambioreftipocargai = false;
                        }
                        break;
                    case "Póliza HC":
                        if (item.Autorizar == 1)
                        {
                            boolcambiopolizahc = true;
                        }
                        else
                        {
                            boolcambiopolizahc = false;
                        }
                        break;
                    case "Contrato Coltanques":
                        if (item.Autorizar == 1)
                        {
                            habilitarbotoncontrato = true;
                        }
                        else
                            habilitarbotoncontrato = false;
                        break;
                    case "Modificar Contrato Coltanques":
                        if (item.Autorizar == 1)
                        {
                            habilitarcontrato = true;
                        }
                        else
                            habilitarcontrato = false;

                        break;
                    case "Cambiar Contrato Coltanques":
                        if (item.Autorizar == 1)
                        {
                            cambiarcontrato = true;
                        }
                        else
                            cambiarcontrato = false;

                        break;

                    case "Cambiar Estado":
                        if (item.Autorizar == 1)
                        {
                            cambiarestado = true;
                        }
                        else
                        {
                            cambiarestado = false;
                        }
                        break;
                    case "Autoriza udp Beneficiario":
                        if (item.Autorizar == 1)
                        {
                            udpbeneficiario = true;
                        }
                        else
                        {
                            udpbeneficiario = false;
                        }
                        break;
                    case "Autoriza udp Propietario":
                        if (item.Autorizar == 1)
                        {
                            udppropietario = true;
                        }
                        else
                        {
                            udppropietario = false;
                        }
                        break;
                    case "Cambia Color":
                        if (item.Autorizar == 1)
                        {
                            udpcolor = true;
                        }
                        else
                        {
                            udpcolor = false;
                        }
                        break;
                    case "No modifica propietarios_beneficiarios":
                        if (item.Autorizar == 1)
                        {
                            tabpropie.IsEnabled = false;

                            modificabenefi_propie = false;
                            //grboxbeneficiario.IsEnabled = false;
                            //grboxpropietario.IsEnabled = false;
                            //if (nuevo == true)
                            //{
                            //    grboxbeneficiario.IsEnabled = true;
                            //    grboxpropietario.IsEnabled = true;
                            //}
                        }
                        else
                        {
                            tabpropie.IsEnabled = true;
                            modificabenefi_propie = true;
                            //grboxbeneficiario.IsEnabled = true ;
                            //grboxpropietario.IsEnabled = true;
                        }
                        break;
                    case "No modifica Vehiculo_Remoqlue":
                        if (item.Autorizar == 1)
                        {
                            tabvehiremol.IsEnabled = false;
                            modificavehiculo_remolque = false;
                            tabconducto.IsSelected = true;
                        }
                        else
                        {
                            tabvehiremol.IsEnabled = true;
                            modificavehiculo_remolque = true;
                        }
                        break;
                    case "No crea Conductores":
                        if (item.Autorizar == 1)
                        {
                            NoCrearConductores = true;
                        }
                        else
                        {
                            NoCrearConductores = false;
                        }
                        break;
                    case "Aprobaciones Pólizas RC":
                        if (item.Autorizar == 1)
                        {
                            aprobarPolRC = true;
                        }
                        else
                        {
                            aprobarPolRC = false;
                        }
                        break;
                    case "Modificar todos":
                        if (item.Autorizar == 1)
                        {
                            boolmodificatodos = true;
                        }
                        else
                        {
                            boolmodificatodos = false;
                        }
                        break;
                    case "Modificar terceros":
                        if (item.Autorizar == 1)
                        {
                            boolmodificaterceros = true;
                        }
                        else
                        {
                            boolmodificaterceros = false;
                        }
                        break;
                    case "Modificar propios":
                        if (item.Autorizar == 1)
                        {
                            boolmodificapropios = true;
                        }
                        else
                        {
                            boolmodificapropios = false;
                        }
                        break;
                    case "Modificar Tipo Afiliación":
                        if (item.Autorizar == 1)
                        {
                            boolmodificaipoafiliacion = true;
                        }
                        else
                        {
                            boolmodificaipoafiliacion = false;
                        }
                        break;
                    case "Modificar Conductores Todos":
                        if (item.Autorizar == 1)
                        {
                            boolmodificaConductorestodos = true;
                        }
                        else
                        {
                            boolmodificaConductorestodos = false;
                        }
                        break;
                    case "Modificar Conductores Terceros":
                        if (item.Autorizar == 1)
                        {
                            boolmodificaConductoresterceros = true;
                        }
                        else
                        {
                            boolmodificaConductoresterceros = false;
                        }
                        break;
                    case "Modificar Conductores propios":
                        if (item.Autorizar == 1)
                        {
                            boolmodificaConductorespropios = true;
                        }
                        else
                        {
                            boolmodificaConductorespropios = false;
                        }
                        break;
                    case "Autoriza Cambio Riesgo":
                        if (item.Autorizar == 1)
                        {
                            boolAutorizaCambiarRiesgo = true;
                        }
                        else
                        {
                            boolAutorizaCambiarRiesgo = false;
                        }
                        break;

                    case "Botones Est Seguridad ":
                        if (item.Autorizar == 1)
                        {
                            boolBotonEstudioSegT = true;
                            boolBotonEstudioSeg = true;
                        }
                        else
                        {
                            boolBotonEstudioSegT = false;
                            boolBotonEstudioSeg = false;
                        }
                        break;
                }
            }
            Habilitar_Observaciones(boolcambioestadoobser);
            habilita_tipoCarga(boolcambioestadotipocarga);
            Habilita_listasRiesgo(boolcambioestadolistasri);
            Habilitar_RefTipocarga(boolcambioreftipocargai);
            Habilitar_Antiguedad(boolcambiorefantiguedad);
            Habilitar_PolizaHc(boolcambiopolizahc);
            //Habilitar_BotonContratoColtanques();
            Habilitar_tabitemcontrato();
            Habilitar_tabitemaprobar();
            Habilitar_tabAutorizaRiesgo();
            Habilitar_BotonesSeguridad(boolBotonEstudioSegT);
        }

        private void Habilitar_BotonesSeguridad(Boolean habilitarBSeg)
        {
            if (habilitarBSeg==true)
            {
                btn_FechaEstudioVeh.IsEnabled = true;
                btn_EstudioseguridadCond.IsEnabled = true;
                btn_FechaEstudioBeneficiario.IsEnabled = true;
                btn_FechaEstudioPropietario.IsEnabled = true;
            }
            else
            {
                btn_FechaEstudioVeh.IsEnabled = false;
                btn_EstudioseguridadCond.IsEnabled = false;
                btn_FechaEstudioBeneficiario.IsEnabled = false;
                btn_FechaEstudioPropietario.IsEnabled = false;
            }
           

        }


        private void Habilitar_BotonContratoColtanques()
        {
            btn_contratocoltan.IsEnabled = habilitarbotoncontrato;
        }

        private void Habilitar_tabitemaprobar()
        {
            tabAprobacion.IsEnabled = cambiarestado;
        }

        private void Habilitar_tabAutorizaRiesgo()
        {
            //tabAutorizaRiesgo.IsEnabled = boolAutorizaCambiarRiesgo;
        }

        private void Habilitar_tabitemcontrato()
        {
            tabcontrato.IsEnabled = habilitarcontrato;
        }

        private void Habilitar_PolizaHc(Boolean habilitar)
        {
            btn_PolizaHc.IsEnabled = habilitar;
        }

        private void Habilitar_RefTipocarga(Boolean habilitar)
        {
            txttipocargaempresa1.IsEnabled = habilitar;
            txttipocargaempresa2.IsEnabled = habilitar;
        }

        private void Habilitar_Antiguedad(Boolean habilitar)
        {
            txtantiguedadempresa1.IsEnabled = habilitar;
            txtantiguedadempresa2.IsEnabled = habilitar;
        }

        private void Habilitar_Observaciones(Boolean habilitar)
        {
            if (habilitar == false)
            {
                chkmodificarobservaciones.IsEnabled = habilitar;
                chkobservacionesconductor.IsEnabled = habilitar;
                chkobservacionesruta.IsEnabled = habilitar;
                chkobservacionestenedor.IsEnabled = habilitar;

                chkmodificarobservaciones.Visibility = Visibility.Collapsed;
                chkobservacionesconductor.Visibility = Visibility.Collapsed;
                chkobservacionesruta.Visibility = Visibility.Collapsed;
                chkobservacionestenedor.Visibility = Visibility.Collapsed;

            }
            else
            {
                chkmodificarobservaciones.IsEnabled = habilitar;
                chkobservacionesconductor.IsEnabled = habilitar;
                chkobservacionesruta.IsEnabled = habilitar;
                chkobservacionestenedor.IsEnabled = habilitar;

                chkmodificarobservaciones.Visibility = Visibility.Visible;
                chkobservacionesconductor.Visibility = Visibility.Visible;
                chkobservacionesruta.Visibility = Visibility.Visible;
                chkobservacionestenedor.Visibility = Visibility.Visible;
            }


        }

        private void habilita_tipoCarga(Boolean habilitar)
        {

            pnl_tipocargaconductor.IsEnabled = habilitar;
            pnl_tipocargavehiculo.IsEnabled = habilitar;

            /*
            if (habilitar == true)
            {
                pnl_tipocargaconductor.IsEnabled = true;
                pnl_tipocargavehiculo.IsEnabled = true;
            }
            else
            {
                pnl_tipocargaconductor.IsEnabled = false;
                pnl_tipocargavehiculo.IsEnabled = false;
            }
            */
        }

        private void Habilita_listasRiesgo(Boolean habilitar)
        {

            pnl_listasriesgobeneficiario.IsEnabled = habilitar;
            dtfechaestudiobenefi.IsEnabled = habilitar;
            pnl_listasriesgoconductor.IsEnabled = habilitar;
            dtfechaEstudioConductor.IsEnabled = habilitar;
            pnl_listasriesgopropietario.IsEnabled = habilitar;
            dtfechaestudiopropi.IsEnabled = habilitar;
            /* if (habilitar == true)
             {
                 pnl_listasriesgobeneficiario.IsEnabled = true;
                 dtfechaestudiobenefi.IsEnabled = true;
                 pnl_listasriesgoconductor.IsEnabled = true;
                 dtfechaEstudioConductor.IsEnabled = true;
                 pnl_listasriesgopropietario.IsEnabled = true;
                 dtfechaestudiopropi.IsEnabled = true;
             }
             else
             {
                 pnl_listasriesgobeneficiario.IsEnabled = false;
                 dtfechaestudiobenefi.IsEnabled = false ;
                 pnl_listasriesgoconductor.IsEnabled = false;
                 dtfechaEstudioConductor.IsEnabled = false;
                 pnl_listasriesgopropietario.IsEnabled = false;
                 dtfechaestudiopropi.IsEnabled = false;
             }*/
        }

        #endregion

        #endregion

        private void cbbestadoaprobacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btn_aceptarRiesgo_Click(object sender, RoutedEventArgs e)
        {
            if (cbNivelRiesgo.SelectedItem != null && cbNivelRiesgoActual.SelectedItem != null)
            {
                Int16 intOrdenActual = Convert.ToInt16(((Vehit_NivelRiestoConductor)cbNivelRiesgoActual.SelectedItem).Orden);
                Int16 intOrdenNuevo = Convert.ToInt16(((Vehit_NivelRiestoConductor)cbNivelRiesgo.SelectedItem).Orden);
                if (intOrdenActual > intOrdenNuevo)
                {

                    if (lblerror_observacionesRiesgo.Content == "")
                    {
                        string strusuario = "";
                        foreach (FrameworkElement element in main.tolbarinformacion.Items)
                        {
                            if (element is TextBlock)
                            {
                                var textblock = (TextBlock)element;
                                switch (textblock.Name)
                                {
                                    case "txtusuario":
                                        strusuario = textblock.Text;
                                        break;
                                }
                                if (strusuario != "")
                                {
                                    break;
                                }
                            }
                        }
                        VehiT_Autorizar_CambioRiesgo_Result tblAurtorizar = contextVehit.VehiT_Autorizar_CambioRiesgo(lblConductor.Content.ToString(), intOrdenActual, intOrdenNuevo, txtobservacionRiesgo.Text, strusuario, 0, 0, "AUTORIZAR").FirstOrDefault();
                        if (tblAurtorizar != null)
                        {
                            MessageBox.Show(tblAurtorizar.Mensaje);
                        }
                    }
                }
                else
                {
                    lblerror_observacionesRiesgo.Content = "El riesgo nuevo debe ser mayor al actual.";
                }
            }

        }

        private void cbNivelRiesgo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbNivelRiesgo.SelectedItem != null && cbNivelRiesgoActual.SelectedItem != null)
            {
                Int16 intOrdenActual = Convert.ToInt16(((Vehit_NivelRiestoConductor)cbNivelRiesgoActual.SelectedItem).Orden);
                Int16 intOrdenNuevo = Convert.ToInt16(((Vehit_NivelRiestoConductor)cbNivelRiesgo.SelectedItem).Orden);
                lblerror_observacionesRiesgo.Content = "";
                if (intOrdenActual <= intOrdenNuevo)
                {
                    lblerror_observacionesRiesgo.Content = "El riesgo nuevo debe ser mayor al actual.";
                }
            }
        }

        private void btn_FechaEstudioVeh_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(57);
            Ventanadoc.IdDocumento(txtplaca.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_EstudioseguridadCond_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(54);
            Ventanadoc.IdDocumento(txtNumdocumento.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_FechaEstudioPropietario_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(56);
            Ventanadoc.IdDocumento(txtNumdocumentopropi.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_FechaEstudioBeneficiario_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(55);
            Ventanadoc.IdDocumento(txtNumdocumentobenefi.Text.Trim());
            Ventanadoc.Show();
        }

        private void btn_FormatoVinculacion_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(58);
            Ventanadoc.IdDocumento(txtNumdocumentobenefi.Text.Trim());
            Ventanadoc.Show();
        }

    }
}
