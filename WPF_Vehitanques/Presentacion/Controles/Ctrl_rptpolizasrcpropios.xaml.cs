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
    /// Lógica de interacción para Ctrl_rptpolizasrcpropios.xaml
    /// </summary>
    public partial class Ctrl_rptpolizasrcpropios : UserControl
    {
        public Ctrl_rptpolizasrcpropios()
        {
            InitializeComponent();
        }
        string agencia;
        DateTime fechaini, fechafin;
        // int tipocarga;
        // DespachoBLL despabll = new DespachoBLL();
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj;
        MainWindow main;
        int idReporte = 1;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private bool _isReportViewerLoaded;

        private void llenarReporteRC()
        {
            string usuario = "";
            string agencia = "";
            if (!_isReportViewerLoaded)
            {
                agencia = main.txtagencia.Text;
                usuario = main.txtusuario.Text;
                Boolean estadoBuscaqueda;
               if(rdbactivas.IsChecked==true)
               {
                   estadoBuscaqueda = true;
               }
               else
               {
                   estadoBuscaqueda = false ;
               }
                Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new
                Microsoft.Reporting.WinForms.ReportDataSource();
                DSReporteDocumentos dataset = new DSReporteDocumentos();
                dataset.BeginInit();
                reportDataSource1.Name = "DataSet1";
                //Name of the report dataset in our .RDLC file
                reportDataSource1.Value = dataset.Vehit_ConsultaReportePolizasRCPropios ;
                this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                //this._reportViewer.LocalReport.ReportPath = "..../Reportes/Report1.rdlc";
                //    this._reportViewer.LocalReport.ReportPath = "C:/Program Files/COLTANQUES/Enturnamiento/Reportes/Report1.rdlc";
                this._reportViewer.LocalReport.ReportPath = "//141.0.0.193/Despliegues/Vehitanques/Reportes/rpt_polizasRCPropios.rdlc";
                dataset.EndInit();
                DS.DSReporteDocumentosTableAdapters.Vehit_ConsultaReportePolizasRCPropiosTableAdapter
                accountsTableAdapter = new
                DS.DSReporteDocumentosTableAdapters.Vehit_ConsultaReportePolizasRCPropiosTableAdapter();
                accountsTableAdapter.ClearBeforeFill = true;
                try
                {
                    accountsTableAdapter.Fill_Vehit_ConsultaReportePolizasRCPropios(dataset.Vehit_ConsultaReportePolizasRCPropios,estadoBuscaqueda );
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

        private void btn_buscar_Click(object sender, RoutedEventArgs e)
        {
            _reportViewer.Reset();
            _reportViewer.Clear();
            // _reportViewer.Refresh();
            _reportViewer.RefreshReport();
            _isReportViewerLoaded = false;
            llenarReporteRC();
        }
    }
}
