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

namespace WPF_Vehitanques.Presentacion.Controles
{
    /// <summary>
    /// Lógica de interacción para RptDocumentosVencidos.xaml
    /// </summary>
    public partial class RptDocumentosVencidos : UserControl
    {
       
        string  agencia;
        DateTime fechaini, fechafin;
       // int tipocarga;
       // DespachoBLL despabll = new DespachoBLL();
        MensajeBLL Mensabll;
        AfiliacionBLL AfilBll = new AfiliacionBLL();
        CarroceriaBLL CarBLL = new CarroceriaBLL();
        VentanaMensaje Venmsj;
        MainWindow main;
        int idReporte = 1;
        private bool _isReportViewerLoaded;
        public RptDocumentosVencidos()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
       

        private void llenarReporteconductores()
        {
            string usuario = "";
            string agencia = "";
            if (!_isReportViewerLoaded)
            {
                agencia = main.txtagencia.Text;
                usuario = main.txtusuario.Text;
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
                //            case "txtagencia":
                //                agencia = textblock.Text;
                //                break;
                //        }
                //        if (usuario != "" & agencia != "")
                //        {
                //            break;
                //        }
                //    }
                //}
                Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new
                Microsoft.Reporting.WinForms.ReportDataSource();
                DSReporteDocumentos dataset = new DSReporteDocumentos();
                dataset.BeginInit();
                reportDataSource1.Name = "DataSet1";
                //Name of the report dataset in our .RDLC file
                reportDataSource1.Value = dataset.VehiT_ConsultaConductoresVencimientosDocumentos;
                this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                //this._reportViewer.LocalReport.ReportPath = "..../Reportes/Report1.rdlc";
                //    this._reportViewer.LocalReport.ReportPath = "C:/Program Files/COLTANQUES/Enturnamiento/Reportes/Report1.rdlc";
                this._reportViewer.LocalReport.ReportPath = "//141.0.0.193/Despliegues/Vehitanques/Reportes/rptDocumentosVencidosConductor.rdlc";
                dataset.EndInit();
                DS.DSReporteDocumentosTableAdapters.VehiT_ConsultaConductoresVencimientosDocumentosTableAdapter
                accountsTableAdapter = new
                DS.DSReporteDocumentosTableAdapters.VehiT_ConsultaConductoresVencimientosDocumentosTableAdapter();
                accountsTableAdapter.ClearBeforeFill = true;
                try
                {
                    accountsTableAdapter.Fill_VehiT_ConsultaConductoresVencimientosDocumentos(dataset.VehiT_ConsultaConductoresVencimientosDocumentos );
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
               // _reportViewer.Refresh();
                _reportViewer.RefreshReport();
                _isReportViewerLoaded = true;
            }
        }
        private void llenarReporteVehiculos()
        {
            string usuario = "";
            string agencia = "";
            if (!_isReportViewerLoaded)
            {
                agencia = main.txtagencia.Text;
                usuario = main.txtusuario.Text;
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
                //            case "txtagencia":
                //                agencia = textblock.Text;
                //                break;
                //        }
                //        if (usuario != "" & agencia != "")
                //        {
                //            break;
                //        }
                //    }
                //}
                Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new
                Microsoft.Reporting.WinForms.ReportDataSource();
                DSReporteDocumentos dataset = new DSReporteDocumentos();
                dataset.BeginInit();
                reportDataSource1.Name = "DataSet1";
                //Name of the report dataset in our .RDLC file
                reportDataSource1.Value = dataset.VehiT_ConsultavehiculosVencimientosDocumentos;
                this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                //this._reportViewer.LocalReport.ReportPath = "..../Reportes/Report1.rdlc";
                //    this._reportViewer.LocalReport.ReportPath = "C:/Program Files/COLTANQUES/Enturnamiento/Reportes/Report1.rdlc";
                this._reportViewer.LocalReport.ReportPath = "//141.0.0.193/Despliegues/Vehitanques/Reportes/rptDocumentosVencidosVehiculos.rdlc";
                dataset.EndInit();
                DS.DSReporteDocumentosTableAdapters.VehiT_ConsultavehiculosVencimientosDocumentosTableAdapter
                accountsTableAdapter = new
                DS.DSReporteDocumentosTableAdapters.VehiT_ConsultavehiculosVencimientosDocumentosTableAdapter();
                accountsTableAdapter.ClearBeforeFill = true;
                try
                {
                    accountsTableAdapter.Fill_VehiT_ConsultavehiculosVencimientosDocumentos(dataset.VehiT_ConsultavehiculosVencimientosDocumentos);
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
                // _reportViewer.Refresh();
                _reportViewer.RefreshReport();
                _isReportViewerLoaded = true;
            }
        }
        private void llenarReporteRemolques()
        {
            string usuario = "";
            string agencia = "";
            if (!_isReportViewerLoaded)
            {
                agencia = main.txtagencia.Text;
                usuario = main.txtusuario.Text;
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
                //            case "txtagencia":
                //                agencia = textblock.Text;
                //                break;
                //        }
                //        if (usuario != "" & agencia != "")
                //        {
                //            break;
                //        }
                //    }
                //}
                Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new
                Microsoft.Reporting.WinForms.ReportDataSource();
                DSReporteDocumentos dataset = new DSReporteDocumentos();
                dataset.BeginInit();
                reportDataSource1.Name = "DataSet1";
                //Name of the report dataset in our .RDLC file
                reportDataSource1.Value = dataset.VehiT_ConsultaremolquesVencimientosDocumentos ;
                this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                //this._reportViewer.LocalReport.ReportPath = "..../Reportes/Report1.rdlc";
                //    this._reportViewer.LocalReport.ReportPath = "C:/Program Files/COLTANQUES/Enturnamiento/Reportes/Report1.rdlc";
                this._reportViewer.LocalReport.ReportPath = "//141.0.0.193/Despliegues/Vehitanques/Reportes/rptDocumentosVencidosRemolque.rdlc";
                dataset.EndInit();
                DS.DSReporteDocumentosTableAdapters.VehiT_ConsultaremolquesVencimientosDocumentosTableAdapter 
                accountsTableAdapter = new
                DS.DSReporteDocumentosTableAdapters.VehiT_ConsultaremolquesVencimientosDocumentosTableAdapter();

                accountsTableAdapter.ClearBeforeFill = true;
                try
                {
                    accountsTableAdapter.Fill_VehiT_ConsultaremolquesVencimientosDocumentos(dataset.VehiT_ConsultaremolquesVencimientosDocumentos );
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
                // _reportViewer.Refresh();
                _reportViewer.RefreshReport();
                _isReportViewerLoaded = true;
            }
        }
     
   
        public void RecibirPadre(MainWindow padre)
        {
            if (main == null)
                main = padre;
        }
        //private Boolean ValidarDatos()
        //{
        //    Boolean respuesta = false;
        //    if (dtfechainicial.SelectedDate != null)
        //    {
        //        lblerror_fechaInicial.Content = "";
        //    }
        //    else
        //    {
        //        lblerror_fechaInicial.Content = "***";
        //        respuesta = true;
        //    }

        //    if (dtfechafinal.SelectedDate != null)
        //    {

        //        lblerror_fechafinal.Content = "";
        //    }
        //    else
        //    {
        //        lblerror_fechafinal.Content = "***";
        //        respuesta = true;
        //    }


        //    return respuesta;
        //}
      
        private void btn_buscar_Click(object sender, RoutedEventArgs e)
        {

            if (rdbconductores.IsChecked == true)
            {
                idReporte = 1;
            }
            if (rdbvehiculos.IsChecked == true)
            {
                idReporte = 2;
            }
            if (rdbremolques.IsChecked == true)
            {
                idReporte = 3;
            }
               
                _reportViewer.Reset();
                _reportViewer.Clear();
               // _reportViewer.Refresh();
                _reportViewer.RefreshReport();
                _isReportViewerLoaded = false;
                // _reportViewer.Load += ReportViewer_Load;
                switch (idReporte)
                {
                    case 1:
                        llenarReporteconductores();
                        break;
                    case 2:
                        llenarReporteVehiculos();
                        break;
                    case 3:
                        llenarReporteRemolques();
                        break;
                    default:
                        llenarReporteconductores();
                        break;
                }
        }

        //private void dtfechafinal_CalendarClosed(object sender, RoutedEventArgs e)
        //{
        //    if (dtfechafinal.SelectedDate != null)
        //    {
        //        DateTime fechaactual;
        //        DateTime fechaaux = (DateTime)dtfechafinal.SelectedDate;
        //        fechaactual = DateTime.Now;
        //        // fechafutura = fechafutura.AddYears(1);

        //        if (fechaaux > fechaactual)
        //        {
        //            Mensabll = new MensajeBLL();
        //            Mensabll.tipomensajes(1,"");
        //            Venmsj = new VentanaMensaje();
        //            Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
        //            Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
        //            Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
        //            Venmsj.ShowDialog();
        //            dtfechafinal.SelectedDate = null;
        //            dtfechafinal.DisplayDateEnd = DateTime.Now;
        //        }
        //    }
        //}

        //private void dtfechainicial_CalendarClosed(object sender, RoutedEventArgs e)
        //{
        //    if (dtfechainicial.SelectedDate != null)
        //    {
        //        DateTime fechaactual;
        //        DateTime fechaaux = (DateTime)dtfechainicial.SelectedDate;
        //        fechaactual = DateTime.Now;
        //        // fechafutura = fechafutura.AddYears(1);

        //        if (fechaaux > fechaactual)
        //        {
        //            Mensabll = new MensajeBLL();
        //            Mensabll.tipomensajes(1,"");
        //            Venmsj = new VentanaMensaje();
        //            Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
        //            Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
        //            Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
        //            Venmsj.ShowDialog();
        //            dtfechainicial.SelectedDate = null;
        //            dtfechainicial.DisplayDateEnd = DateTime.Now;
        //        }
        //    }
        //}
    }
}
