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
using WPF_Vehitanques.BLL;
using WPF_Vehitanques.Modelo;
using System.Diagnostics;
using System.Deployment.Application;
using System.Reflection;
using System.Xml;
using System.IO;
using WPF_Vehitanques.Presentacion.Ventanas;
using WPF_Vehitanques.Utilidades;

namespace WPF_Vehitanques
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        InicioVehiculos inivehiculos;
       AdminBeneficiarioPropietario admbenefipropi;
       AdmConductores admconductores;
       admnotificacionesOperacionAndroid amdnotifiOper;
       AdmReportados  admvhireportados;
        EmpresaSatelital empresatelital;
        InicioConsulta consulta;
        SeguridadBLL seguridadbll = new SeguridadBLL();
        AgenciaBLL agenbll = new AgenciaBLL();
        String sUsuario;
        Int16  intAgencia;
        String sAgencia;
        ComercialPEntities contex = new ComercialPEntities();
        #region load_closed
       
        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
            WPF_Vehitanques.App.Current.Shutdown();
        }
        private void Mantenimiento()
        {
            VentanaMantenimiento vent_man = new VentanaMantenimiento();
            vent_man.recibirpadre(this);
            vent_man.Show();
        }
        public void CerrarApp()
        {
            this.Close();
            WPF_Vehitanques.App.Current.Shutdown();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            sUsuario = SecurityVariables.PStrUsuario;
            var lista = seguridadbll.GetListaPermisos(sUsuario);
            if (lista.Count > 0)
            {
                SecurityVariables.PPerfilUsuario = lista;
                dynamic resultUsuario = (from user in lista select user.idAgencia).FirstOrDefault();
                SecurityVariables.PIntAgencia = Convert.ToInt32(resultUsuario);
                intAgencia = Convert.ToInt16( SecurityVariables.PIntAgencia) ;
                var listaagencias = agenbll.GetAgenciaPoxID(intAgencia);
                //var listaagencias = (from a in contex.Agencia
                //                     where a.Codigo == intAgencia
                //                     select a).FirstOrDefault();
                //dynamic rAgencia = (from a in contex.Agencia
                //                    where a.Codigo == intAgencia
                //                    select a.Nombre).FirstOrDefault();
                if (listaagencias != null)
                {
                    sAgencia = listaagencias[0].Nombre;
                    txtagencia.Text = listaagencias[0].Codigo.ToString();
                }
                else
                {
                    MessageBox.Show("La agencia asignada al usuario no existe en la base de datos.");
                    System.Environment.Exit(0);
                }
                CargarMenu(SecurityVariables.PPerfilUsuario);
            }
            else
            {
                MessageBox.Show("Error al verificar los privilegios del usuario en esta aplicación");
                System.Environment.Exit(0);
            }
            txtnombreagencia.Text = sAgencia.ToUpper() + "-";
            txtBaseDatos.Text = "Comercial";
            txtusuario.Text = sUsuario.ToUpper();
            dtfechahoy.SelectedDate = DateTime.Now.Date;   
            dtfechahoy.DisplayDateEnd = DateTime.Now.Date;
            dtfechahoy.DisplayDateStart =DateTime.Now.Date;
              
          // Mantenimiento();
        }
        //public MainWindow()
        //{
        //    ValidarVersion();
        //}
    
        #endregion
        #region Menú_submenú
        private void CargarMenu(List<seguridad_consultarMenuPermisos2_Result> resultPerfilUsuario)
        {
            var lista = (from me in resultPerfilUsuario
                         where me.Menu == 1 & me.orden == 1
                         select me).ToList();
            foreach (var item in lista)
            {
                if (item.Menu == 1)
                {
                    MenuItem menuitem = new MenuItem();
                    var listamenuhijo = (from rmenu in resultPerfilUsuario
                                         where rmenu.idMenuPadre == item.idMenu & rmenu.alterno == 0 & rmenu.Autorizar == 1
                                         orderby rmenu.nombreMenu ascending 
                                         select rmenu).ToList();
                    menuitem = CargarSubmenu(listamenuhijo, menuitem);
                    menuitem.Header = item.nombreMenu;
                    menuitem.Cursor = Cursors.Hand;
                    menuitem.Height = 30;
                    menuitem.VerticalAlignment = VerticalAlignment.Center;
                    MenuPpal.Items.Add(menuitem);
                    MenuPpal.VerticalAlignment = VerticalAlignment.Center;
                }
            }
        }
        private MenuItem CargarSubmenu(List<seguridad_consultarMenuPermisos2_Result> listamenuhijo, MenuItem padre)
        {            
            foreach (var item in listamenuhijo)
            {                
                    MenuItem menuitem = new MenuItem();
                    menuitem.HeaderStringFormat = item.nombreMenu;
                    menuitem.Header = item.nombreMenu;
                    menuitem.Click += new RoutedEventHandler(MenuItem_Click);                   
                //cógido nuevo 11/12/2015.
                //poner un ícono al menú.
                    string strPathIcono = "";
                    //strPathIcono = "/Iconos/IconTruckRed.png";
                    switch (item.idMenuPadre)
                    {
                        case 230:
                            if (item.nombreMenu.ToLower().Contains("reporte") == true)
                            {
                                //strPathIcono = "/Iconos/report2.png";
                                strPathIcono = "/Iconos/reportSEO-icon.png";
                            //E:\Documents\Proyectos Desarrollo\proyectos 2013\WPF_Vehitanques\WPF_Vehitanques\Iconos\report2.png
                            }
                            else
                            {
                                if (item.nombreMenu.ToLower().Contains("rpt") == true)
                                {
                                    //strPathIcono = "/Iconos/report2.png";
                                    strPathIcono = "/Iconos/reportSEO-icon.png";
                                }
                                else
                                {
                                    strPathIcono = "/Iconos/IconTruckRed.png";
                                }

                            }

                            break;
                        default:
                            if (item.nombreMenu.Contains("Reporte") == true)
                            {
                                strPathIcono = "/Iconos/report1.png";
                            }
                            else
                            {
                                if (item.nombreMenu.Contains("rpt") == true)
                                {
                                    strPathIcono = "/Iconos/report1.png";
                                }
                                else
                                {
                                    strPathIcono = "/Iconos/IconTruckRed.png";
                                }
                            }
                            break;
                    }
                    Image icon = new Image();
                    BitmapImage bmImage = new BitmapImage();
                    bmImage.BeginInit();
                    bmImage.UriSource = new Uri(strPathIcono, UriKind.Relative);
                    bmImage.EndInit();
                    icon.Source = bmImage;
                    icon.MaxWidth = 25;
                    menuitem.Icon = icon;
                    //fin código nuevo
                    padre.Items.Add(menuitem);
            }
            return padre;
        }
        #endregion
        #region eventos
        private void btn_grabar_Click(object sender, RoutedEventArgs e)
        {
            ValidarVersion();
            if (pnl_inicio.Children.Count > 0)
            {
                foreach (FrameworkElement element in pnl_inicio.Children)
                {
                    if (element is UserControl)
                    {
                        var usercontr = (UserControl)element;
                        switch (usercontr.Name)
                        {
                            case "Inivehiculos":
                                inivehiculos = (InicioVehiculos)element;
                                inivehiculos.GuardarInformacion();
                                InicioConsulta iniconsul = new InicioConsulta();
                                iniconsul = inivehiculos.retornoRedirect();
                             //   iniconsul.GetListausuario(SecurityVariables.PPerfilUsuario);
                                inivehiculos.RecibeRedirect(iniconsul);
                                pnl_inicio.Children.Clear();
                                pnl_inicio.Children.Add(inivehiculos);
                                break;
                            case "adminprobenefi":
                                admbenefipropi = (AdminBeneficiarioPropietario)element;
                                admbenefipropi.GuardarInformacion();
                                // iniconsul = new InicioConsulta();
                                //iniconsul = inivehiculos.retornoRedirect();
                                //  iniconsul.GetListausuario(SecurityVariables.PPerfilUsuario);
                                // admbenefipropi.RecibeRedirect(iniconsul);
                                pnl_inicio.Children.Clear();
                                pnl_inicio.Children.Add(admbenefipropi);
                                break;
                            case "admreport_Vehi_persona":
                                admvhireportados = (AdmReportados)element;
                                admvhireportados.GuardarDatos();
                                // iniconsul = new InicioConsulta();
                                //iniconsul = inivehiculos.retornoRedirect();
                                //  iniconsul.GetListausuario(SecurityVariables.PPerfilUsuario);
                                // admbenefipropi.RecibeRedirect(iniconsul);
                                pnl_inicio.Children.Clear();
                                pnl_inicio.Children.Add(admvhireportados);
                                break;

                            case "ctrladmconductor":
                                admconductores = (AdmConductores)element;
                                admconductores.GuardarDatos();
                                // iniconsul = new InicioConsulta();
                                //iniconsul = inivehiculos.retornoRedirect();
                                //  iniconsul.GetListausuario(SecurityVariables.PPerfilUsuario);
                                // admbenefipropi.RecibeRedirect(iniconsul);
                                pnl_inicio.Children.Clear();
                                pnl_inicio.Children.Add(admconductores);
                                break;
                            case "ctrlnotifiOper":
                                amdnotifiOper = (admnotificacionesOperacionAndroid)element;
                                amdnotifiOper.GuardarDatos();
                                // iniconsul = new InicioConsulta();
                                //iniconsul = inivehiculos.retornoRedirect();
                                //  iniconsul.GetListausuario(SecurityVariables.PPerfilUsuario);
                                // admbenefipropi.RecibeRedirect(iniconsul);
                                pnl_inicio.Children.Clear();
                                pnl_inicio.Children.Add(amdnotifiOper);
                                break;
                            default:
                                MessageBox.Show("Esta función no se encuentra habilitada para esta ventana.");
                                break;
                        }
                        break;

                    }
                }
            }
        }

        private void btn_nuevo_Click(object sender, RoutedEventArgs e)
        {
            ValidarVersion();
            if (pnl_inicio.Children.Count > 0)
            {
                foreach (FrameworkElement element in pnl_inicio.Children)
                {
                    InicioConsulta iniconsul = new InicioConsulta();
                    if (element is UserControl)
                    {

                        var usercontr = (UserControl)element;
                        //UserControl control = usercontr;
                        switch (usercontr.Name)
                        {
                            case "IniConsulta":
                                inivehiculos = new InicioVehiculos();
                               // iniconsul.GetListausuario(SecurityVariables.PPerfilUsuario);
                                inivehiculos.RecibirPadre(this);
                                //InicioConsulta    = (UserControl)element;
                                inivehiculos.RecibeRedirect(iniconsul);
                                inivehiculos.RecibeOrdenBeneficiario(true);
                                inivehiculos.RecibeOrdenPropietario(true);                                
                                pnl_inicio.Children.Clear();
                                pnl_inicio.Children.Add(inivehiculos);
                                break;
                            case "CtrlEmpresaSatelital":
                                empresatelital =  (EmpresaSatelital )element;
                                // iniconsul.GetListausuario(SecurityVariables.PPerfilUsuario);
                                empresatelital.RecibirPadre(this);
                                //InicioConsulta    = (UserControl)element;
                                empresatelital.Nuevaempresa();
                                //pnl_inicio.Children.Clear();
                                //pnl_inicio.Children.Add(inivehiculos);
                                break;
                            case "admreport_Vehi_persona":
                                //pnl_inicio.Children.Clear();
                                AdmReportados ctrreportados;
                                ctrreportados = (AdmReportados)element;
                                ctrreportados.RecibirPadre(this);
                                //var listareportados = SecurityVariables.PPerfilUsuario;
                                //pnl_inicio.Children.Add(ctrreportados);
                                ctrreportados.limpiarCampos();
                                //var listabotonesreportados = (from rmenu in listareportados
                                //                              where rmenu.idMenu == 346
                                //                              select rmenu).ToList();
                                //VisibilidadBotones(listabotonesreportados.ToList());
                                break;
                            case "ctrladmconductor":
                                //pnl_inicio.Children.Clear();
                                AdmConductores ctrAdmConductores;
                                ctrAdmConductores = (AdmConductores)element;
                                //ctrConductoresPropios.RecibirPadre(this);
                                //var listareportados = SecurityVariables.PPerfilUsuario;
                                //pnl_inicio.Children.Add(ctrreportados);
                                ctrAdmConductores.LimpiarCampos();
                                //var listabotonesreportados = (from rmenu in listareportados
                                //                              where rmenu.idMenu == 346
                                //                              select rmenu).ToList();
                                //VisibilidadBotones(listabotonesreportados.ToList());
                                break;
                            default:
                                MessageBox.Show("Esta función no se encuentra habilitada para esta ventana.");
                                break;

                        }

                    }
                    break;

                }
            }
        }

        private void btn_Salir_Click(object sender, RoutedEventArgs e)
        {
            ValidarVersion();
            Boolean tienecontenido = false;
            if (pnl_inicio.Children.Count > 0)
            {
                foreach (FrameworkElement element in pnl_inicio.Children)
                {
                    if (element is UserControl)
                    {
                        tienecontenido = true;
                        var usercontr = (UserControl)element;
                        pnl_inicio.Children.Clear();
                        var listadefault = SecurityVariables.PPerfilUsuario;
                        var listabotonesdefaul = (from rmenu in listadefault
                                                  where rmenu.idMenu == 229
                                                  select rmenu).ToList();
                        VisibilidadBotones(listabotonesdefaul.ToList());
                        break;
                    }
                }
                if (tienecontenido == false)
                {
                    WPF_Vehitanques.App.Current.Shutdown();
                    Process.GetCurrentProcess().Kill();
                }
            }
            else
            {
                WPF_Vehitanques.App.Current.Shutdown();
                Process.GetCurrentProcess().Kill();
            }


            //if (pnl_inicio.Children != null)
            //{
            //    pnl_inicio.Children.Clear();
            //}
            //else
            //{
               

            //}
           
        }
        private void btn_Consultar_Click(object sender, RoutedEventArgs e)
        {
            ValidarVersion();
            if (pnl_inicio.Children.Count > 0)
            {
                foreach (FrameworkElement element in pnl_inicio.Children)
                {
                    if (element is UserControl)
                    {
                        var usercontr = (UserControl)element;
                        switch (usercontr.Name)
                        {
                           
                            case "ctrlnotifiOper":
                                amdnotifiOper = (admnotificacionesOperacionAndroid)element;
                                amdnotifiOper.Consultar();                                
                                pnl_inicio.Children.Clear();
                                pnl_inicio.Children.Add(amdnotifiOper);
                                break;
                            default:
                                MessageBox.Show("Esta función no se encuentra habilitada para esta ventana.");
                                break;
                        }
                        break;

                    }
                }
            }
        }
        #endregion
        #region Eventos Seguridad
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ValidarVersion();
            var opcion = sender as MenuItem;            
            var listamenu = SecurityVariables.PPerfilUsuario;
            var listabotones = new List<seguridad_consultarMenuPermisos2_Result>();

            switch (opcion.HeaderStringFormat)
            {


                case "Conductores para Envío de mensajes":
                    pnl_inicio.Children.Clear();
                    ctr_msgconductorespropios _ctrmsgconductorespropi = new ctr_msgconductorespropios();
                    _ctrmsgconductorespropi.RecibirPadre(this);
                    pnl_inicio.Children.Add(_ctrmsgconductorespropi);
                    if (listabotones != null)
                    {
                        listabotones = null;
                    }
                    listabotones = (from rmenu in listamenu
                                    where rmenu.idMenu == 2267
                                    select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());
                    break;
                case "Reporte Información Vehículos":
                    pnl_inicio.Children.Clear();
                    rpt_vehiculos _ctrrpt_vehiculos = new rpt_vehiculos();
                    _ctrrpt_vehiculos.RecibirPadre(this);
                    pnl_inicio.Children.Add(_ctrrpt_vehiculos);
                    if (listabotones != null)
                    {
                        listabotones = null;
                    }
                    listabotones = (from rmenu in listamenu
                                    where rmenu.idMenu == 1165
                                    select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());
                    break;
                case "Subir Pólizas Método Masivo":
                    pnl_inicio.Children.Clear();
                    ctrSoatMasivoPropios _ctrSoatMasivoPropios = new ctrSoatMasivoPropios();
                    _ctrSoatMasivoPropios.RecibirPadre(this);
                    pnl_inicio.Children.Add(_ctrSoatMasivoPropios);
                    if (listabotones != null)
                    {
                        listabotones = null;
                    }
                    listabotones = (from rmenu in listamenu
                                    where rmenu.idMenu == 1108
                                    select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());
                    break;

                case "Consulta Vehículo":
                    
                    pnl_inicio.Children.Clear();
                    consulta = new InicioConsulta();
                   // consulta = (InicioConsulta)element;
                    consulta.RecibirPadre(this);
                    if (listabotones != null)
                    {
                        listabotones = null;
                    }
                    listabotones = (from rmenu in listamenu
                                        where rmenu.idMenu == 231
                                        select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());
                   // consulta.RecibePrivilegios(cambiaestado, infobancaria);
                    pnl_inicio.Children.Add(consulta);
                    break;
                case "Administrar Empresas Satelitales":
                    pnl_inicio.Children.Clear();
                    EmpresaSatelital ctremprsatelital = new EmpresaSatelital();
                    ctremprsatelital.RecibirPadre(this);                   
                    pnl_inicio.Children.Add(ctremprsatelital);
                    if (listabotones != null)
                    {
                        listabotones = null;
                    }
                     listabotones = (from rmenu in listamenu
                                        where rmenu.idMenu == 305
                                        select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());

                    break;
                case "Comprobantes Propietario":
                    pnl_inicio.Children.Clear();
                    ComprobantesPropietario ctrComprobantesPropietario = new ComprobantesPropietario();
                    ctrComprobantesPropietario.RecibirPadre(this);                    
                    pnl_inicio.Children.Add(ctrComprobantesPropietario);
                    if (listabotones != null)
                    {
                        listabotones = null;
                    }
                     listabotones = (from rmenu in listamenu
                                        where rmenu.idMenu == 324
                                        select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());
                    break;
                case "Crear Beneficiarios Propietarios":
                    pnl_inicio.Children.Clear();
                    AdminBeneficiarioPropietario ctrbeneficiariopropietario= new AdminBeneficiarioPropietario();
                    ctrbeneficiariopropietario.RecibirPadre(this);                    
                    pnl_inicio.Children.Add(ctrbeneficiariopropietario);
                    if (listabotones != null)
                    {
                        listabotones = null;
                    }
                       listabotones = (from rmenu in listamenu
                                        where rmenu.idMenu == 340
                                        select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());
                    break;
                case "Vehículos Reportados":
                    pnl_inicio.Children.Clear();
                    AdmReportados ctrreportados = new AdmReportados();
                    ctrreportados.RecibirPadre(this);                    
                    pnl_inicio.Children.Add(ctrreportados);
                    if (listabotones != null)
                    {
                        listabotones = null;
                    }
                       listabotones = (from rmenu in listamenu
                                        where rmenu.idMenu == 346
                                        select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());


                  
                    break;
                case "Conductores Propios":
                    pnl_inicio.Children.Clear();
                    AdmConductores  ctrconductores = new AdmConductores ();
                    ctrconductores.RecibirPadre(this);                 
                    pnl_inicio.Children.Add(ctrconductores);                 

                    if (listabotones != null)
                    {
                        listabotones = null;
                    }
                    listabotones = (from rmenu in listamenu
                                        where rmenu.idMenu == 391
                                        select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());
                    break;

                case "Listado Conductores Propios":
                    pnl_inicio.Children.Clear();
                    ConductoresPropios ctrconductorespro = new ConductoresPropios();
                    ctrconductorespro.RecibirPadre(this);                    
                    pnl_inicio.Children.Add(ctrconductorespro);                  

                      if (listabotones != null)
                    {
                        listabotones = null;
                    }
                    listabotones = (from rmenu in listamenu
                                        where rmenu.idMenu == 429
                                        select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());
                    break;
                case "Noficaciones Operación Android":
                    pnl_inicio.Children.Clear();
                    admnotificacionesOperacionAndroid ctrnotifOperAndroid = new admnotificacionesOperacionAndroid();
                    ctrnotifOperAndroid.RecibirPadre(this);                 
                    pnl_inicio.Children.Add(ctrnotifOperAndroid);
                    
                      if (listabotones != null)
                    {
                        listabotones = null;
                    }
                    listabotones = (from rmenu in listamenu
                                        where rmenu.idMenu == 705
                                        select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());
                  
                    break;
                case "Reporte Eventos Android":
                    pnl_inicio.Children.Clear();
                    rptEventosAndroid ctrrptEventosAndroid = new rptEventosAndroid();
                    ctrrptEventosAndroid.RecibirPadre(this);
                    
                    pnl_inicio.Children.Add(ctrrptEventosAndroid);


                    if (listabotones != null)
                    {
                        listabotones = null;
                    }
                    listabotones = (from rmenu in listamenu
                                        where rmenu.idMenu == 711
                                        select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());
                    break;
                case "Mensajes Prog Mantenimiento":
                    pnl_inicio.Children.Clear();
                    ProgramadosMantenimiento ctrProgramadosMantenimientod = new ProgramadosMantenimiento();
                    ctrProgramadosMantenimientod.RecibirPadre(this);
                    
                    pnl_inicio.Children.Add(ctrProgramadosMantenimientod);
                    if (listabotones != null)
                    {
                        listabotones = null;
                    }
                    listabotones = (from rmenu in listamenu
                                        where rmenu.idMenu == 712
                                        select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());
                    
                    break;
                case "Administrar tipo Vehículo":
                    pnl_inicio.Children.Clear();
                    AdmTipoVehiculo ctrAdmTipoVehiculo = new AdmTipoVehiculo();
                    ctrAdmTipoVehiculo.RecibirPadre(this);
                    
                    pnl_inicio.Children.Add(ctrAdmTipoVehiculo);
                    if (listabotones != null)
                    {
                        listabotones = null;
                    }
                    listabotones = (from rmenu in listamenu
                                        where rmenu.idMenu == 717
                                        select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());
                    break;
                case "Reporte Documentos Vencidos":
                    pnl_inicio.Children.Clear();
                    RptDocumentosVencidos ctrRptDocumentosVencidos = new RptDocumentosVencidos();
                    ctrRptDocumentosVencidos.RecibirPadre(this);
                    
                    pnl_inicio.Children.Add(ctrRptDocumentosVencidos);
                    if (listabotones != null)
                    {
                        listabotones = null;
                    }
                    listabotones = (from rmenu in listamenu
                                        where rmenu.idMenu == 723
                                        select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());
                    
                    break;
                case "Reporte Pólizas RC":
                    pnl_inicio.Children.Clear();
                    Ctrl_rptpolizasrcpropios _Ctrl_rptpolizasrcpropios = new Ctrl_rptpolizasrcpropios();
                    _Ctrl_rptpolizasrcpropios.RecibirPadre(this);
                    
                    pnl_inicio.Children.Add(_Ctrl_rptpolizasrcpropios);
                    if (listabotones != null)
                    {
                        listabotones = null;
                    }
                    listabotones = (from rmenu in listamenu
                                        where rmenu.idMenu == 776
                                        select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());
                    
                    break;
                case "Envio Masivo Msg Conductores":
                    pnl_inicio.Children.Clear();
                    MsgConsdutoresPropios ctrMsgConsdutoresPropios = new MsgConsdutoresPropios();
                    ctrMsgConsdutoresPropios.RecibirPadre(this);
                    
                    pnl_inicio.Children.Add(ctrMsgConsdutoresPropios);
                    if (listabotones != null)
                    {
                        listabotones = null;
                    }
                    listabotones = (from rmenu in listamenu
                                        where rmenu.idMenu == 758
                                        select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());
                    
                    break;

                case "Consulta Documentos Remolque":
                    pnl_inicio.Children.Clear();
                    documentosRemolque ctrdocumentosRemolque = new documentosRemolque();
                    ctrdocumentosRemolque.RecibirPadre(this);
                    
                    pnl_inicio.Children.Add(ctrdocumentosRemolque);
                    if (listabotones != null)
                    {
                        listabotones = null;
                    }
                    listabotones = (from rmenu in listamenu
                                        where rmenu.idMenu == 979
                                        select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());
                    
                    break;
                    
                default:
                    if (listabotones != null)
                    {
                        listabotones = null;
                    }
                    listabotones = (from rmenu in listamenu
                                        where rmenu.idMenu == 254
                                        select rmenu).ToList();
                    VisibilidadBotones(listabotones.ToList());

                    break;
                    
            }
        }
        public void VisibilidadBotones(List<seguridad_consultarMenuPermisos2_Result> listabotones)
        {
            if (listabotones.Count > 0)
            {
                if (listabotones[0].Nuevo == 1)
                { btn_nuevo.Visibility = System.Windows.Visibility.Visible; }
                else { btn_nuevo.Visibility = System.Windows.Visibility.Collapsed; }
                if (listabotones[0].Grabar == 1) 
                { btn_grabar.Visibility = System.Windows.Visibility.Visible; }
                else { btn_grabar.Visibility = System.Windows.Visibility.Collapsed; }
                //if (listabotones[0].Actualizar == 1) { btn_Actualizar.Visibility = System.Windows.Visibility.Visible; }
                if (listabotones[0].Eliminar == 1) 
                { btn_Eliminar.Visibility = System.Windows.Visibility.Visible; }
                else { btn_Eliminar.Visibility = System.Windows.Visibility.Collapsed; }
                if (listabotones[0].Consultar == 1) 
                { btn_Consultar.Visibility = System.Windows.Visibility.Visible; }
                else { btn_Consultar.Visibility = System.Windows.Visibility.Collapsed; }
               // if (listabotones[0].Autorizar == 1) { btn_Autorizar.Visibility = System.Windows.Visibility.Visible; }
                if (listabotones[0].Imprimir == 1) 
                { btn_Imprimir.Visibility = System.Windows.Visibility.Visible; }
                else { btn_Imprimir.Visibility = System.Windows.Visibility.Collapsed; }
                if (listabotones[0].Exportar == 1) 
                { btn_Exportar.Visibility = System.Windows.Visibility.Visible; }
                else { btn_Exportar.Visibility = System.Windows.Visibility.Collapsed; }
            }
        }
        public  void ValidarVersion()
        {
            try
            {
                Permisos _permisos = new Permisos();
                Version myVersion;
                myVersion = GetPublishedVersion();
                DirectoryInfo directory = new DirectoryInfo(@"//141.0.0.193/Despliegues/Vehitanques/Application Files");

                FileInfo[] files = directory.GetFiles("*.*");
                //var lastFolder = Directory.GetDirectories(".").OrderBy(folder => folder, new FolderComparer()).Last();
                DirectoryInfo[] directories = directory.GetDirectories();
                var lista = directories.OrderByDescending(d => d.CreationTime).FirstOrDefault();

                //int cantidad = directories.Count();
                //var capeta = directories[cantidad - 1];



                // var capeta = lista[0];
                string[] carpetas = lista.Name.Split('_');
                int cantcarpetas = carpetas.Count();
                int major, minor, build, revision;
                major = Convert.ToInt32(carpetas[cantcarpetas - 4]);
                minor = Convert.ToInt32(carpetas[cantcarpetas - 3]);
                build = Convert.ToInt32(carpetas[cantcarpetas - 2]);
                revision = Convert.ToInt32(carpetas[cantcarpetas - 1]);
                if (myVersion.Build >= build)
                {
                    if (myVersion.Build > build)
                    {
                        return;
                    }
                    if (myVersion.Revision < revision)
                    {
                        MessageBox.Show("SEÑOR USUARIO USTED INTENTA USAR UNA VERSIÓN NO ACTUALIZADA POR FAVOR REINICIAR Y ACTUALIZAR EL APLICATIVO:...." + txtusuario.Text.Trim().ToUpper(), "Alerta de seguridad", MessageBoxButton.OK, MessageBoxImage.Warning);
                        try
                        {
                            var respuesta = seguridadbll.GuardarAuditoriaUsrNoActualizan("WPF_Vehitanques", SecurityVariables.PStrUsuario, myVersion.Major.ToString() + "_" + myVersion.Minor.ToString() + "_" + myVersion.Build.ToString() + "_" + myVersion.Revision.ToString(), major.ToString() + "_" + minor.ToString() + "_" + build.ToString() + "_" + revision.ToString(), _permisos.myIP, _permisos.hostName);
                        }
                        catch
                        {

                        }
                        WPF_Vehitanques.App.Current.Shutdown();
                        Process.GetCurrentProcess().Kill();
                        System.Environment.Exit(0);
                    }
                    //else
                    //{
                    //    int diferencia = myVersion.Revision - revision;
                    //    if (diferencia != 1)
                    //    {
                    //        MessageBox.Show("SEÑOR USUARIO USTED INTENTA USAR UNA VERSIÓN NO ACTUALIZADA POR FAVOR REINICIAR Y ACTUALIZAR EL APLICATIVO:...." + txtusuario.Text.Trim().ToUpper(), "Alerta de seguridad", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //        WPF_Vehitanques.App.Current.Shutdown();
                    //        Process.GetCurrentProcess().Kill();
                    //        System.Environment.Exit(0);
                    //    }
                    //}
                }
                else
                {
                    MessageBox.Show("SEÑOR USUARIO USTED INTENTA USAR UNA VERSIÓN NO ACTUALIZADA POR FAVOR REINICIAR Y ACTUALIZAR EL APLICATIVO:...." + txtusuario.Text.Trim().ToUpper(), "Alerta de seguridad", MessageBoxButton.OK, MessageBoxImage.Warning);
                    try
                    {
                        var respuesta = seguridadbll.GuardarAuditoriaUsrNoActualizan("WPF_Vehitanques", SecurityVariables.PStrUsuario, myVersion.Major.ToString() + "_" + myVersion.Minor.ToString() + "_" + myVersion.Build.ToString() + "_" + myVersion.Revision.ToString(), major.ToString() + "_" + minor.ToString() + "_" + build.ToString() + "_" + revision.ToString(), _permisos.myIP, _permisos.hostName);
                    }
                    catch
                    {

                    }
                    WPF_Vehitanques.App.Current.Shutdown();
                    Process.GetCurrentProcess().Kill();
                    System.Environment.Exit(0);
                }
            }
            catch
            {

            }
        }
        public static Version GetPublishedVersion()
        {
            XmlDocument xmlDoc = new XmlDocument();
            Assembly asmCurrent = System.Reflection.Assembly.GetExecutingAssembly();
            string executePath = new Uri(asmCurrent.GetName().CodeBase).LocalPath;

            xmlDoc.Load(executePath + ".manifest");
            string retval = string.Empty;
            if (xmlDoc.HasChildNodes)
            {
                retval = xmlDoc.ChildNodes[1].ChildNodes[0].Attributes.GetNamedItem("version").Value.ToString();
            }
            return new Version(retval);
        }        

        #endregion

      
      
       
        //private void cargarcontenido()
        //{
        //    consulta = new InicioConsulta();
        //    consulta.RecibirPadre(this);
        //    pnl_inicio.Children.Add(consulta);

        //}
       
    }
}
