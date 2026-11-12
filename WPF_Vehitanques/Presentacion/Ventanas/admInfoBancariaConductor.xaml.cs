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
using WPF_Vehitanques.Presentacion.Controles;
using WPF_Vehitanques.Utilidades;


namespace WPF_Vehitanques.Presentacion.Ventanas
{
    /// <summary>
    /// Lógica de interacción para admInfoBancariaConductor.xaml
    /// </summary>
    public partial class admInfoBancariaConductor : Window
    {
        
        #region Declaraciones
        InicioConsulta inicioconsul;
        BancoBLL BanBll = new BancoBLL();
        CuentaBLL CuenBll = new CuentaBLL();
        ConductorBLL ConducBll = new ConductorBLL();
        MensajeBLL Mensabll = new MensajeBLL();
        VentanaMensaje Venmsj = new VentanaMensaje();
        Numeric numeric = new Numeric();
        private string idconductor,idbeneficiario, usuario, placa;
        AdmDocumentos Ventanadoc;
        MainWindow main;
        VehiT_ConsultarVehiculoXplaca_condu_propi_Result currentcell;
        #endregion
        #region Load
        public admInfoBancariaConductor()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                ValidarPrivilegios();

            }

        }
        #endregion
        
        #region Métodos
        public void GetPadre(InicioConsulta inicio,MainWindow _main)
        {
            inicioconsul = inicio;
            main = _main;
        }

        internal void ValidarPrivilegios()
        {
            var lista = SecurityVariables.PPerfilUsuario;
            var listaprivi = (from rmenu in lista
                              where rmenu.idMenu == 233 & rmenu.Autorizar == 1
                              select rmenu).ToList();
            if (listaprivi.Count <= 0)
            {
                MessageBox.Show("Su usuario no tiene privilegios de abrir esta ventana" + usuario.ToUpper(), "Alerta de seguridad", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Close();
            }
        }

        public void GetConductor(string stridconductor, string strusaurio, string idplaca,string idben)
        {
            cargarBancos();
            cargarTipoCuenta();
            idconductor = stridconductor;
            usuario = strusaurio;
            placa = idplaca;           
            idbeneficiario = idben;
            lblidconductor.Content = stridconductor;
            lblidpropietario.Content = idben;
            CargarDatosConductor();
        }
        private void cargarTipoCuenta()
        {
            var lista = CuenBll.GetTiposcuenta();
            cbbtipocuenta.ItemsSource = lista;
            cbbtipocuenta.SelectedValuePath = "IdTipoCuenta";
            cbbtipocuenta.DisplayMemberPath = "Descripcion";
            cbbtipocuenta.SelectedIndex = 0;
        }
        private void cargarBancos()
        {
            var lista = BanBll.GetBancosConductor();
            cbbbanco.ItemsSource = lista;
            cbbbanco.SelectedValuePath = "CodACH";
            cbbbanco.DisplayMemberPath = "Nombre";
            cbbbanco.SelectedIndex = 0;
        }

        private void CargarDatosConductor()
        {
            var lista = ConducBll.ConsultaConductorAsociadoPlacaXPlaca_Idcondutor(placa, idconductor);
            if (lista.Count > 0)
            {
                if (lista[0].IdBanco != null)
                {
                    cbbbanco.SelectedValue = lista[0].IdBanco;
                }
                else
                {
                    cbbbanco.SelectedIndex = 0;
                }


                if (lista[0].TipoCuenta != null)
                {
                    cbbtipocuenta.SelectedValue = lista[0].TipoCuenta;
                }
                else
                {
                    cbbtipocuenta.SelectedIndex = 1;
                }


                txtnumcuenta.Text = lista[0].IdTarRec;
                txtidtarjeta.Text = lista[0].IdIdenTar;
                txtobservaciones.Text = lista[0].ObservacionCuenta;

                if (lista[0].Nombres != "")
                {
                    txtnombres.Text = lista[0].Nombres + " " + lista[0].Apellido1 + "  " + lista[0].Apellido2;
                }
                else
                {
                    txtnombres.Text = lista[0].Nombre;
                }
                if (lista[0].chkAfiliado == false)
                {
                    radioTercero.IsChecked = true;
                    // txtnitcuenta.IsEnabled = true;
                }
                else
                {
                    radiopropio.IsChecked = true;
                    // txtnitcuenta.IsEnabled = false;
                }
                txtnitcuenta.Text = lista[0].NitCuenta;
                //lblidpropietario.Content = lista[0].NitPropietario;
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
            }
            else
            {
                Mensabll = new MensajeBLL();
                Mensabll.tipomensajes(28,"");
                Venmsj = new VentanaMensaje();
                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                Venmsj.ShowDialog();
                inicioconsul.recibirordenestadoplaca(true);
               // this.Close();
            }
        }
        private Boolean ValidarDatos()
        {
            Boolean respuesta = false;
            if (cbbbanco.SelectedIndex < 0)
            {
                cbbbanco.SelectedIndex = 1;
            }
            if (cbbtipocuenta.SelectedIndex < 0)
            {
                cbbtipocuenta.SelectedIndex = 1;
            }

            //if (txtidtarjeta.Text == "")
            //{
            //    lblerror_idtarjeta.Content = "***";
            //    respuesta = true;
            //}
            //else
            //{
            //    lblerror_idtarjeta.Content = "";
            //}

            //if (txtnumcuenta.Text != "")
            //{

            //    //lblerror_numcuenta.Content = "***";
            //    //respuesta = true;
            //}
            //else
            //{
            //    lblerror_numcuenta.Content = "";
            //}
            if (txtobservaciones.Text == "")
            {
                lblerror_observaciones.Foreground = new SolidColorBrush(Colors.Red);
                lblerror_observaciones.Content = "Debe digitar el motivo de la modificación en la información bancaria";
                respuesta = true;
            }
            else
            {
                lblerror_observaciones.Content = "";
                lblerror_observaciones2.Content = "";
            }
            return respuesta;
        }
        private void GuardarDatosInfoBancariaConductor()
        {
            if (ValidarDatos() == false)
            {
                Boolean respuesta;
                int tipopago;
                if (rdbeletronico.IsChecked == true)
                {
                    tipopago = 1;

                }
                else
                {
                    tipopago = 2;
                }
                respuesta = ConducBll.UpdateInfoBancariaConductor(txtidtarjeta.Text.Trim(), cbbbanco.SelectedValue.ToString(), txtnitcuenta.Text.Trim(), cbbtipocuenta.SelectedValue.ToString(), txtobservaciones.Text.Trim(), idconductor, txtnumcuenta.Text.Trim(), usuario, tipopago);
                if (respuesta == true)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(12,"");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    inicioconsul.recibirordenestadoplaca(true);
                    this.Close();
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
                    inicioconsul.recibirordenestadoplaca(false);
                    this.Close();
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
        private Boolean Validaridconductorypropietario()
        {
            Boolean respuesta = false;
            if (txtnitcuenta.Text.Trim() != "")
            {
                if (txtnitcuenta.Text.Trim() != idconductor.Trim())
                {
                    string nitcuenta = txtnitcuenta.Text.Trim();
                    string idpropietario = lblidpropietario.Content.ToString();
                    string dv = "";
                    if (currentcell.dv != null)
                    {
                        dv = currentcell.dv.ToString().Trim();

                    }
                    
                    if (currentcell.TipoDoc != null)
                    {
                        if (currentcell.TipoDoc =="N")
                        {
                            if(dv=="")
                            {
                                MessageBox.Show("El Beneficiario no posee Dígito de verificación, debe solicitar que actualicen los datos antes de registrar la cuenta bancaria.");
                                return true;                                                
                            }
                            if (nitcuenta.Trim() != (currentcell.IdNit.Trim() + dv))
                            {
                                if (nitcuenta.Trim() != (currentcell.IdNit.Trim()))
                                {
                                    respuesta = true;
                                }
                            }
                            
                        }
                        else
                        {
                            if (nitcuenta.Trim() != (currentcell.IdNit.Trim()))
                            {
                                respuesta = true;
                            }
                        }
                    }
                    else
                    {
                        if (nitcuenta.Trim() != (currentcell.IdNit.Trim()))
                        {
                            respuesta = true;
                        }
                    }
                   
                }
            }
            return respuesta;
        }
        #endregion


        #region Eventos
        private void textboxt_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (validarCaracteresespeciales(e, txt) == true)
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                {
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

        private void TextboxtStringShift_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (validarCaracteresespeciales(e, txt) == true)
            {
                e.Handled = false;
            }
        }

        private void btn_aceptar_Click(object sender, RoutedEventArgs e)
        {
            if (Validaridconductorypropietario() == false)
            {
                GuardarDatosInfoBancariaConductor();
            }
            else
            {
                lblerror_observaciones.Content = "El NIT Digitado no corresponde ni al Id del conductor ni al Id del propietario";
                lblerror_observaciones2.Content = " con el dígito de verificación";
            }

        }

        private void btn_certibancaria_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            
            Ventanadoc = new AdmDocumentos(main );
            Ventanadoc.TipoDocumento(20);
            Ventanadoc.IdDocumento(idconductor);
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
            Ventanadoc.IdDocumento(idconductor);
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
            Ventanadoc.IdDocumento(idconductor);
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
            Ventanadoc.IdDocumento(idconductor);
            Ventanadoc.Show();
        }
        #endregion

        private void btn_docbancarioscond_Click(object sender, RoutedEventArgs e)
        {
            Boolean permiso = true;
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(22);
            Ventanadoc.IdDocumento(idconductor);
            Ventanadoc.GetIdentificacionconductor(idconductor );
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
            Ventanadoc.IdDocumento(idbeneficiario);
            Ventanadoc.GetIdentificacionBENEFICIARIO(idbeneficiario);
            Ventanadoc.PermisoExaminar(permiso);
            Ventanadoc.Show();
        }




        public  void cargarDatos(VehiT_ConsultarVehiculoXplaca_condu_propi_Result itemcell)
        {

            currentcell = itemcell;
            cargarBancos();
            cargarTipoCuenta();
            idconductor = itemcell.Conductor;
            usuario = main.txtusuario.Text.Trim();
            placa = itemcell.IdPlaca ;
            idbeneficiario = itemcell.NitPropietario ;
            lblidconductor.Content = itemcell.Conductor ;
            lblidpropietario.Content = itemcell.NitPropietario;
            if (itemcell.dv != null)
            {
                lbldigVer.Content = itemcell.dv;
            }            
            CargarDatosConductor();
        }
    }
}
