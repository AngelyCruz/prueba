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
    /// Lógica de interacción para AdmConductores.xaml
    /// </summary>
    public partial class AdmConductores : UserControl
    {
        #region Declaraciones
        AnalistaBLL AnalistBll = new AnalistaBLL();
        EmpresaBLL emprbll = new EmpresaBLL();
        MainWindow main;
        TipoDocumentoBLL TiDocBll = new TipoDocumentoBLL();
        DepartamentoBLL DeparBll = new DepartamentoBLL();
        LicenciaBLL licenBll = new LicenciaBLL();
        Numeric numeric = new Numeric();
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj = new VentanaMensaje();
        private DatePicker FechaActual = new DatePicker();
        CiudadBLL CiuBll = new CiudadBLL();
        ConductorBLL ConducBll = new ConductorBLL();
        AdmDocumentos Ventanadoc;
        Boolean booluserandroid = false,boolmsgandroid=false,boolcursoscon=false  ,boolnoverdocumentos=false ,boolexamen=false  ;
        VehiT_ConsultaConductor_Result row_conductor = null;
        AdmUserAndroid VentUserAndroid;
        admnotificacionesusuarioAndroid  VentnotificAndroid;
        AdmCursosConductor VentCursoscond;
        AdmExamMedico ventExamenes;
        //MainWindow main;
        #endregion
        #region Load
        public AdmConductores()
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
                cargarEmpresas();
                CargarAnalistas();
                CargarCategorialicencia();
                CargarTipoDocumentos();
                cargarDepartamentos();
                Habilitarbotonesconductor(false );
            }
            funcionbloqueopantalla(false );
           
        }

        private void funcionbloqueopantalla(Boolean orden)
        {
            this.IsEnabled = orden;
            main.btn_grabar.IsEnabled = orden;
            if (orden == false)
            {
                MessageBox.Show("Ésta función se ha deshabilitado en este sistema.", "Vehitanques", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes);
            }
        }
        #endregion
        #region Métodos
        private void mostrar_mensaje(int idmensaje)
        {
            Mensabll = new MensajeBLL();
            Mensabll.tipomensajes(idmensaje,"");
            Venmsj = new VentanaMensaje();
            Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
            Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
            Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
            Venmsj.ShowDialog();
        }
        public void RecibirPadre(MainWindow padre)
        {
            main = padre;
        }
        private void cargarCiudadesconductor(int departamento)
        {
            var lista = CiuBll.GetCiudades(departamento);

            cbbciudadconductor.ItemsSource = lista;
            cbbciudadconductor.SelectedValuePath = "CodigoReducido";
            cbbciudadconductor.DisplayMemberPath = "Nombre";
            cbbciudadconductor.SelectedIndex = 0;

        }
        
        private void CargarCategorialicencia()
        {
            var lista = licenBll.GetCategoriaLicencia();
            cbbcateglicencia.ItemsSource = lista;
            cbbcateglicencia.SelectedValuePath = "Cat_Id";
            cbbcateglicencia.DisplayMemberPath = "Cat_Descripcion";
            cbbcateglicencia.SelectedIndex = 0;
        }
        private void cargarDepartamentos()
        {
            var lista = DeparBll.GetDepartamentos(1);

            cbbdeparconductor.ItemsSource = lista;
            cbbdeparconductor.SelectedValuePath = "Codigo";
            cbbdeparconductor.DisplayMemberPath = "Nombre";
            cbbdeparconductor.SelectedIndex = 0;


        }
        private void CargarTipoDocumentos()
        {
            var lista = TiDocBll.GetTipoDocumentoIdentificacion();
            cbbtipodocumento.ItemsSource = lista;
            cbbtipodocumento.SelectedValuePath = "Codigo";
            cbbtipodocumento.DisplayMemberPath = "TipoDocumento";
            cbbtipodocumento.SelectedIndex = 0;
        }

        private void cargarEmpresas()
        {
            var lista = emprbll.GetEmpresasConductor();
            cbbempresas.ItemsSource = null;
            cbbempresas.ItemsSource = lista;
            cbbempresas.DisplayMemberPath = "RazonSocial";
            cbbempresas.SelectedValuePath = "IndCond";
            cbbempresas.SelectedIndex = 0;

        }
        private void CargarAnalistas()
        {
            var lista = AnalistBll.GetAnalistasConductor();
            cbbanalista.ItemsSource = null;
            cbbanalista.ItemsSource = lista;
            cbbanalista.DisplayMemberPath = "Analista";
            cbbanalista.SelectedValuePath = "Cod";
            cbbanalista.SelectedIndex = 0;
        }


        private void DelegarPrivilegios()
        {
            if (booluserandroid == true)
            {
                pnl_userAndroid.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                pnl_userAndroid.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (boolmsgandroid == true)
            {
                pnl_notificaciones .Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                pnl_notificaciones.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (boolcursoscon == true)
            {
                pnl_cursos.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                pnl_cursos.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (boolexamen  == true)
            {
                pnl_examen.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                pnl_examen.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (boolnoverdocumentos == true)
            {
                Habilitarbotonesconductor(false);
            }
            //else
            //{
            //    Habilitarbotonesconductor(false);
            //}
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
            foreach (var item in listaprivi)
            {
                switch (item.nombreMenu)
                {
                    case "Usuario Android":
                        if (item.Autorizar == 1)
                        {
                            booluserandroid  = true;
                        }
                        else
                        {
                            booluserandroid = false;
                        }

                        break;
                    case "Noficaciones usuario Android":
                        if (item.Autorizar == 1)
                        {
                            boolmsgandroid = true;
                        }
                        else
                        {
                            boolmsgandroid = false;
                        }
                        break;
                    case "Cursos Conductor Propio":
                        if (item.Autorizar == 1)
                        {
                            boolcursoscon = true;
                        }
                        else
                        {
                            boolcursoscon = false;
                        }
                        break;

                    case "No Upd Documentación cond Propios":
                        if (item.Autorizar == 1)
                        {
                            boolnoverdocumentos  = true;
                        }
                        else
                        {
                            boolnoverdocumentos = false;
                        }
                        break;
                    case "Modificar Examen Médico":
                        if (item.Autorizar == 1)
                        {
                            boolexamen   = true;
                        }
                        else
                        {
                            boolexamen = false;
                        }
                        break;
                        
                        
                    
                }
            }
        }
        private void ConsultarDatosConductor(string idconductor)
        {
            row_conductor = null;
            var lista = ConducBll.GetDAtosconductor(idconductor);
            if (lista.Count > 0)
            {

                foreach (var iteml in lista)
                {
                    if (iteml.chkAfiliado == true)
                    {
                        row_conductor = iteml;
                        CargarDatosConductor(iteml );
                        btn_Usuario_Android.IsEnabled = true;
                        btn_MailAndroid.IsEnabled = true;
                        btn_Cursos.IsEnabled = true;
                        Habilitarbotonesconductor(true );
                        if (boolcursoscon == true)
                        {
                            btn_Cursos.IsEnabled = true;
                        }
                        else
                        {
                            btn_Cursos.IsEnabled = false ;
                        }
                        if (boolexamen == true)
                        {
                            btn_examMedico.IsEnabled = true;
                        }
                        else
                        {
                            btn_examMedico.IsEnabled = false;
                        }
                        break;

                    }
                    else
                    {
                            MessageBoxResult result1 = MessageBox.Show("Este conductor ya existe y está creado como tercero. Desea cargar sus datos para afiliarlo como propio?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result1 == MessageBoxResult.Yes)
                        {
                            row_conductor = iteml;
                         CargarDatosConductor(iteml );
                         btn_Usuario_Android.IsEnabled = true;
                         btn_MailAndroid.IsEnabled = true;
                         if (boolcursoscon == true)
                         {
                             btn_Cursos.IsEnabled = true;
                         }
                         else
                         {
                             btn_Cursos.IsEnabled = false;
                         }
                         Habilitarbotonesconductor(true);
                         if (boolcursoscon == true)
                         {
                             btn_Cursos.IsEnabled = true;
                         }
                         else
                         {
                             btn_Cursos.IsEnabled = false;
                         }
                         if (boolexamen  == true)
                         {
                             btn_examMedico.IsEnabled = true;
                         }
                         else
                         {
                             btn_examMedico.IsEnabled = false;
                         }
                         break;

                        }
                        else
                        {
                            row_conductor = null ;
                            LimpiarCampos();
                            btn_Usuario_Android.IsEnabled = false ;
                            btn_MailAndroid.IsEnabled = false;
                            btn_Cursos.IsEnabled = false;
                            btn_examMedico.IsEnabled = false;
                            Habilitarbotonesconductor(false);
                            break;
                        }
                    }
                }
            }
            else
            {
                btn_Usuario_Android.IsEnabled = false;
                btn_MailAndroid.IsEnabled = false ;
                btn_Cursos.IsEnabled = false;
                btn_examMedico.IsEnabled = false;
                Habilitarbotonesconductor(false );
            }

        }
        public  void LimpiarCampos()
        {
                        txtNumdocumento.Text = "" ;
                        txtnombres.Text = "" ;
                        txtapellido1.Text = "" ;
                        txtapellido2.Text = "" ;
                        txtRH.Text = "";
                        dtfechanacimieto.SelectedDate =null ;
                        cbbdeparconductor.SelectedIndex = 0;
                        txtcelular.Text = "" ;
                            txttelefonocasa.Text = "" ;
                        txtdireccioncasa.Text = "" ;
                        cbbcateglicencia.SelectedIndex = 0;
                        txtnumlicencia.Text = "" ;
                        dtfechalicencia.SelectedDate = null;
                        txteps.Text = "" ;
                        txtARL.Text = "" ;
                        txtafp.Text="" ;
                            dtfechaPagoSSocial.SelectedDate = null;
                            cbbanalista.SelectedIndex = 0;
                        txtexperiencia.Text = "" ;
                        cbbempresas.SelectedIndex = 0;
                        txtIndicativo.Text = "";
        }
        private void CargarDatosConductor(Modelo.VehiT_ConsultaConductor_Result iteml)
        {
                        rdbpropio.IsChecked = true;
                        txtNumdocumento.Text = iteml.IdConductor ;
                        txtnombres.Text = iteml.Nombres;
                        txtapellido1.Text = iteml.Apellido1;
                        txtapellido2.Text = iteml.Apellido2;
                        dtfechanacimieto.SelectedDate = iteml.FechaNacimiento;
                        if (iteml.IdDepartamento != null)
                        {
                            cbbdeparconductor.SelectedValue = iteml.IdDepartamento;
                            cargarCiudadesconductor(Convert.ToInt32(cbbdeparconductor.SelectedValue));
                            cbbciudadconductor.SelectedValue = iteml.CiudadResidencia;
                        }
                        if (iteml.CelPre != null)
                        {
                            txtcelular.Text = iteml.CelPre.ToString();
                        }
                        if (iteml.Telefono != null)
                        {
                            txttelefonocasa.Text = iteml.Telefono;
                            txtIndicativo.Text = iteml.IndicativoTelFijo;
                        }

                        txtdireccioncasa.Text = iteml.Direccion;
                        if (iteml.Categoria == 1)
                        {
                            cbbcateglicencia.SelectedValue = 2;
                        }
                        else
                        {
                            cbbcateglicencia.SelectedValue = Convert.ToInt32(iteml.Categoria);
                        }
                        if (iteml.chkPatio != null)
                        {
                            if (iteml.chkPatio == true)
                            {
                                chkPatio.IsChecked = true;
                            }
                            else
                            {
                                chkPatio.IsChecked = false ;
                            }
                        }
                        else
                        {
                            chkPatio.IsChecked = false;
                        }
                        txtRH.Text = iteml.FactorRh;
                        txtnumlicencia.Text = iteml.Licencia;
                        dtfechalicencia.SelectedDate = iteml.FechaVencimiento;
                        txteps.Text = iteml.Eps;
                        txtARL.Text = iteml.Arp;
                        txtafp.Text=iteml.Afp ;
                        if(iteml.FechaPagoSegSoc!=null ){
                            dtfechaPagoSSocial.SelectedDate = iteml.FechaPagoSegSoc;
                        }
                        if(iteml.Analista!=null )
                        {
                            if (iteml.Analista == 0)
                            {
                                cbbanalista.SelectedIndex = 0;
                            }
                            else
                            {
                                cbbanalista.SelectedValue = iteml.Analista;
                            }

                        }
                        else
                        {
                            cbbanalista.SelectedIndex = 0;
                        }
                        txtexperiencia.Text = iteml.Experiencia.ToString();
                        if(iteml.Afinidad !=null)
                        {
                            if (iteml.Afinidad == 0)
                            {
                                cbbempresas.SelectedIndex = 0;
                            }
                            else
                            {
                                cbbempresas.SelectedValue = iteml.Afinidad;
                            }

                        }
                        else
                        {
                            cbbempresas.SelectedIndex = 0;
                        }
                        if (iteml.Estado == 0)
                        {
                            rdbactivo.IsChecked = true;
                        }
                        else
                        {
                            rdbinactivo.IsChecked = true;
                        }

        }
        public void GuardarDatos()
        {
            if (ValidarDatos() == false)
            {
                int estado;
                   int analista;
                bool _chkpatio;
                bool _chkafiliado;
                if (chkPatio.IsChecked == true)
                {
                    _chkpatio = true;
                }
                else
                {
                    _chkpatio = false ;
                }
                if (rdbactivo.IsChecked == true)
                {
                    estado = 0;
                    analista = Convert.ToInt32 ( cbbanalista.SelectedValue);
                    _chkafiliado = true ;
                }
                else
                {
                    _chkafiliado = false ;
                    estado = 1;
                    analista = 0;
                }
                Nullable<DateTime> FechapagoSSocial;
                DateTime fechalicencia = (DateTime)dtfechalicencia.SelectedDate;
                DateTime fechanacimiento = (DateTime)dtfechanacimieto.SelectedDate;
                if (dtfechaPagoSSocial.SelectedDate != null)
                {
                    FechapagoSSocial = (DateTime)dtfechaPagoSSocial.SelectedDate;
                }
                else
                {
                    FechapagoSSocial = null;

                }
                var lista = ConducBll.GetDAtosconductor(txtNumdocumento.Text.Trim());
                Boolean respuesta;
              

                if (lista.Count > 0)
                {
                   
                    MessageBoxResult result1 = MessageBox.Show("Este conductor ya existe. Desea Actualizar Sus Datos?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result1 == MessageBoxResult.Yes)
                    {                  
                         
                     respuesta=   ConducBll.UpdateConductoresPropios(txtNumdocumento.Text.Trim(), (short)Convert.ToInt16(main.txtagencia.Text), 
                         txtnombres.Text, txtnumlicencia.Text, Convert.ToInt32 ( cbbcateglicencia.SelectedValue), fechalicencia,txtdireccioncasa.Text,
                            txttelefonocasa.Text, txtcelular.Text, cbbciudadconductor.SelectedValue.ToString(), fechanacimiento, _chkafiliado, 
                            main.txtusuario.Text, txtARL.Text, txteps.Text, txtafp.Text, Convert.ToInt32(txtexperiencia.Text), FechapagoSSocial, txtapellido1.Text,
                            txtapellido2.Text, Convert.ToInt32(cbbempresas.SelectedValue), analista , Convert.ToInt16(estado), txtRH.Text, _chkpatio,txtIndicativo.Text.Trim());


   //                     (string IdConductor, short IdAgencia, string Nombres,
   //string Licencia, int Categoria, DateTime FechaVencimiento, string Direccion,
   //string Telefono, string CelPre, string CiudadResidencia, DateTime FechaNacimiento, Boolean chkAfiliado,
   //string Usuario, string Arp, string Eps, string Afp, int Experiencia,  Nullable <DateTime>  FechaPagoSegSoc,
   //        string Apellido1, string Apellido2, int Afinidad, int Analista, short Estado)


                     if (respuesta == true)
                     {
                         mostrar_mensaje(12);
                         LimpiarCampos();
                     }
                     else
                     {
                         mostrar_mensaje(23);
                         LimpiarCampos();
                     }

                    }
                    
                }
                else
                {

                    respuesta = ConducBll.GuardarConductoresPropios(txtNumdocumento.Text, (short)Convert.ToInt16(main.txtagencia.Text),
                         txtnombres.Text, txtnumlicencia.Text, Convert.ToInt32(cbbcateglicencia.SelectedValue), fechalicencia, txtdireccioncasa.Text,
                            txttelefonocasa.Text, txtcelular.Text, cbbciudadconductor.SelectedValue.ToString(), fechanacimiento, true,
                            main.txtusuario.Text, txtARL.Text, txteps.Text, txtafp.Text, Convert.ToInt32(txtexperiencia.Text), FechapagoSSocial, txtapellido1.Text,
                            txtapellido2.Text, Convert.ToInt32(cbbempresas.SelectedValue), Convert.ToInt32(cbbanalista.SelectedValue), Convert.ToInt16(estado), txtRH.Text, _chkpatio, txtIndicativo.Text.Trim());
                    
                    if (respuesta == true)
                    {
                        mostrar_mensaje(12);
                        btn_Usuario_Android.IsEnabled = true ;
                    }
                    else
                    {
                        mostrar_mensaje(23);
                        btn_Usuario_Android.IsEnabled = false;
                    }
                   // ConducBll.GuardarConductoresPropios();
                }
                
            }
        }
        private Boolean ValidarDatos()
        {
            Boolean respuesta = false;
            if (txtNumdocumento.Text.Trim() == "")
            {
                lblerror_documenConductor.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_documenConductor.Content = "";
            }
            if (txtnombres .Text.Trim() == "")
            {
                lblerror_nombresCond.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_nombresCond.Content = "";
            }
            if (txtapellido1.Text.Trim() == "")
            {
                lblerror_apellido1Cond .Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_apellido1Cond.Content = "";
            }

            if (txtapellido2.Text == "")
            {
                lblerror_apellido2Cond.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_apellido2Cond.Content = "";
            }
            if (dtfechanacimieto.SelectedDate ==null )
            {
                lblerror_fechanamcond .Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_fechanamcond.Content = "";
            }


            if (txtdireccioncasa.Text.Trim() == "")
            {
                lblerror_direccioncasa.Content = "***";
                respuesta = true;
            }
            else { lblerror_direccioncasa.Content = ""; }
            //if (txtcelular.Text.Trim() != "")
            //{
            //    if (validarNumeroCelular(txtcelular.Text.Trim()) == true)
            //    {
            //        MessageBox.Show("El número de celular del conductor no tiene la longitud adecuada.");
            //        respuesta = true;
            //    }

            //}
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
                          


            if (dtfechanacimieto.SelectedDate != null) 
            {
                int anioactualnac, anionaci, aniodiferencianaci;
                DateTime fechanaci = (DateTime)dtfechanacimieto.SelectedDate;
                DateTime fechaactual = DateTime.Now;

                anioactualnac = DateTime.Now.Year;
                anionaci = fechanaci.Year;

                aniodiferencianaci =  anioactualnac - anionaci ;
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
                default:
                    MessageBox.Show("la categoría de licencia seleccionada no es permitida para conductores propios o terceros");
                    respuesta = true;
                    break;
            }
            //if (dtfechalicencia.SelectedDate == null)
            //{
            //    lblerror_fechalicencia.Content = "***";
            //    respuesta = true;
            //}
            //else
            //{

            //    int anioactual, aniolicencia, aniodiferencia;
            //    DateTime fechalince = (DateTime)dtfechalicencia.SelectedDate;
            //    DateTime fechaactu = DateTime.Now;
            //    if (dtfechalicencia.SelectedDate <= FechaActual.SelectedDate)
            //    {
            //        lblerror_fechalicencia.Content = "***";
            //        MessageBox.Show("la fecha de vencimiento de la licencia no corresponde a los parámetros del MINISTERIO DE TRANSPORTE");
            //        respuesta = true;
            //    }
            //    else
            //    {
            //        anioactual = DateTime.Now.Year;
            //        aniolicencia = fechalince.Year;
            //        aniodiferencia = aniolicencia - anioactual;
            //        if (aniodiferencia < 0 || aniodiferencia > 3)
            //        {
            //            lblerror_fechalicencia.Content = "***";
            //            MessageBox.Show("la fecha de vencimiento de la licencia no corresponde a los parámetros del MINISTERIO DE TRANSPORTE");
            //            respuesta = true;

            //        }
            //        else
            //        {
            //            lblerror_fechalicencia.Content = "";
            //        }
            //    }



            //    //respuesta = false;
            //}

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

            if (txtexperiencia.Text.Trim() == "")
            {
                lblerror_experiencia .Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_experiencia.Content = "";
            }

            if (txteps .Text.Trim() == "")
            {
                lblerror_eps.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_eps.Content = "";
            }
            if (txtafp .Text.Trim() == "")
            {
                lblerror_afp.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_afp.Content = "";
            }
            if (txtARL.Text.Trim() == "")
            {
                lblerror_arl.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_arl.Content = "";
            }
            if (cbbciudadconductor.SelectedIndex < 0)
            {
                lblerror_ciudadconductor.Content = "***";
                respuesta = true;
            }
            else
            {
                lblerror_ciudadconductor.Content = "";
            }
            return respuesta;
        }
        public void Habilitarbotonesconductor(Boolean? habilitar)
        {
            if (boolnoverdocumentos == true)
            {
                habilitar = false;
            }
            if (habilitar != true)
            {
                btn_cedulaconductor.IsEnabled = false;
                btn_licenciaconductor.IsEnabled = false;
                btn_pagoSSocial.IsEnabled = false;
                btn_FotoConductor.IsEnabled = false;
               // btn_Examenes.IsEnabled = false;
                btn_HojaDeVida.IsEnabled = false;
                
              //  btn_AutorizaInfo.IsEnabled = false;
            }
            else
            {
                if (habilitar == true)
                {
                    btn_cedulaconductor.IsEnabled = true;
                    btn_licenciaconductor.IsEnabled = true;
                    btn_pagoSSocial.IsEnabled = true;
                    btn_FotoConductor.IsEnabled = true;
                    //btn_Examenes.IsEnabled = true;
                    btn_HojaDeVida.IsEnabled = true;
                   // btn_AutorizaInfo.IsEnabled = true;
                }
            }
            //ValidarPrivilegios();
        }
        #endregion

        #region Eventos
        private void btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
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

        private void btn_HojaDeVida_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(24);
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

        private void cbbdeparconductor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = Convert.ToInt32(cbbdeparconductor.SelectedValue);
            cargarCiudadesconductor(id);
        }
        private void cbbcateglicencia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dtfechalicencia.SelectedDate = null;
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
                    Mensabll.tipomensajes(15,"");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfechanacimieto.SelectedDate = null;
                    dtfechanacimieto.DisplayDateEnd = DateTime.Now;
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
                    Mensabll.tipomensajes(14,"");
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
                            Mensabll.tipomensajes(16,"");
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

                //else
                //{

                //}
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
                    Mensabll.tipomensajes(15,"");
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
        private void txtNumdocumento_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (validarCaracteresespeciales(e, txt) == true)
            {

                if (e.Key == Key.Enter || e.Key == Key.Tab)
                {
                    if (txtNumdocumento.Text.Trim () != "")
                    {
                        string cond = txtNumdocumento.Text;
                        LimpiarCampos();
                        txtNumdocumento.Text = cond;
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

        
        private void btn_Usuario_Android_Click(object sender, RoutedEventArgs e)
        {
            if (VentUserAndroid != null)
            {
                VentUserAndroid.Close();
                VentUserAndroid = new AdmUserAndroid();
            }
            else
            {
                VentUserAndroid = new AdmUserAndroid();
            }
            VentUserAndroid.RecibirConductor(row_conductor);
            VentUserAndroid.RecibirPadre(main);
            VentUserAndroid.Show();
        }

        private void btn_MailAndroid_Click(object sender, RoutedEventArgs e)
        {

            if (VentnotificAndroid != null)
            {
                VentnotificAndroid.Close();
                VentnotificAndroid = new admnotificacionesusuarioAndroid();
            }
            else
            {
                VentnotificAndroid = new admnotificacionesusuarioAndroid();
            }
            VentnotificAndroid.RecibirConductor(row_conductor);
            VentnotificAndroid.RecibirPadre(main);
            VentnotificAndroid.Show();

        }

        private void btn_Cursos_Click(object sender, RoutedEventArgs e)
        {
            if (VentCursoscond != null)
            {
                VentCursoscond.Close();
                VentCursoscond = new  AdmCursosConductor ();
            }
            else
            {
                VentCursoscond = new AdmCursosConductor();
            }
            VentCursoscond.RecibirConductor(row_conductor);
            VentCursoscond.RecibirPadre(main);
            VentCursoscond.Show();

        }

        private void btn_examMedico_Click(object sender, RoutedEventArgs e)
        {
            if (ventExamenes != null)
            {
                ventExamenes.Close();
                ventExamenes = new AdmExamMedico ();
            }
            else
            {
                ventExamenes = new AdmExamMedico ();
            }
            ventExamenes.RecibirConductor(row_conductor);
            ventExamenes.RecibirPadre(main);
            ventExamenes.Show();
        }

       
        

      






    }
}
