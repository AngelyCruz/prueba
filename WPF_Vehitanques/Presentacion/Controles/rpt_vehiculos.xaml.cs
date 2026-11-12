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
using WPF_Vehitanques.Modelo;
using WPF_Vehitanques.Presentacion.Ventanas;
using WPF_Vehitanques.Utilidades;

namespace WPF_Vehitanques.Presentacion.Controles
{
    /// <summary>
    /// Lógica de interacción para rpt_vehiculos.xaml
    /// </summary>
    public partial class rpt_vehiculos : UserControl
    {
        #region Declaraciones
        Numeric numeric = new Numeric();
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj;
        MainWindow main;
        int idReporte = 1;
        private bool _isReportViewerLoaded;
        Nullable<int> tipocarroceria = null, tipoafiliacion = null, tipovehiculo = null, modelo = null;
        AfiliacionBLL AfilBll = new AfiliacionBLL();
        CarroceriaBLL CarBLL = new CarroceriaBLL();
        VehiculoBLL vehibll = new VehiculoBLL(); 
        #endregion

        #region load
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                CargarTipoAfiliacion();
                CargarTipoCarroceria();
                CargarTipoVehiculos();
            }
        }
        public rpt_vehiculos()
        {
            InitializeComponent();
        } 
        #endregion

        #region Métodos
        public void RecibirPadre(MainWindow padre)
        {
            if (main == null)
                main = padre;
        }
        private void CargarTipoAfiliacion()
        {
            var lista = AfilBll.GetTipoAfiliacion();
            var item = new TipoAfiliacion() { 
            IdCodigo=0,
            Descripcion="Seleccione una opción",
            };
            lista.Add(item);
            lista = lista.OrderBy(p => p.IdCodigo).ToList();
            cbbtipoafiliacion.ItemsSource = lista;
            cbbtipoafiliacion.SelectedValuePath = "IdCodigo";
            cbbtipoafiliacion.DisplayMemberPath = "Descripcion";
            cbbtipoafiliacion.SelectedIndex = 0;
        }
        private void CargarTipoCarroceria()
        {
            var lista = CarBLL.GetTipoCarroceria();
            var item = new VehiT_ConsultaTipoCarroceria_Result()
            {
                Tipo = 0,
                Nombre = "Seleccione una opción",
            };
            lista.Add(item);
            lista = lista.OrderBy(p => p.Tipo).ToList();
            cbbtipocarroceria.ItemsSource = lista;
            cbbtipocarroceria.SelectedValuePath = "Tipo";
            cbbtipocarroceria.DisplayMemberPath = "Nombre";
            cbbtipocarroceria.SelectedIndex = 0;

        }

        private void CargarTipoVehiculos()
        {
            var lista = vehibll.GetTiposVehiculo();
            var item = new VehiT_ConsultaTiposVehiculo_Result()
            {
                Codigo  = 0,
                Descripcion = "Seleccione una opción",
            };
            lista.Add(item);
            lista = lista.OrderBy(p => p.Codigo).ToList();
            cbbtipovehiculo.ItemsSource = lista;
            cbbtipovehiculo.SelectedValuePath = "Codigo";
            cbbtipovehiculo.DisplayMemberPath = "Descripcion";
            cbbtipovehiculo.SelectedIndex = 0;
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
        private void llenarReporte()
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

                DSVehiculo dataset = new DSVehiculo();
                dataset.BeginInit();
                reportDataSource1.Name = "DataSet1";
                //Name of the report dataset in our .RDLC file
                reportDataSource1.Value = dataset.Vehit_ConsultaReporteVehiculos_tipoCar_tipoAfil_TipoVeh_Modelo;
                this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                //this._reportViewer.LocalReport.ReportPath = "..../Reportes/Report1.rdlc";
                //    this._reportViewer.LocalReport.ReportPath = "C:/Program Files/COLTANQUES/Enturnamiento/Reportes/Report1.rdlc";
                this._reportViewer.LocalReport.ReportPath = "//141.0.0.193/Despliegues/Vehitanques/Reportes/rpt_infovehiculos.rdlc";
                dataset.EndInit();
                DS.DSVehiculoTableAdapters.Vehit_ConsultaReporteVehiculos_tipoCar_tipoAfil_TipoVeh_ModeloTableAdapter
                accountsTableAdapter = new
                DS.DSVehiculoTableAdapters.Vehit_ConsultaReporteVehiculos_tipoCar_tipoAfil_TipoVeh_ModeloTableAdapter();
                accountsTableAdapter.ClearBeforeFill = true;
                try
                {
                    accountsTableAdapter.Fill_Vehit_ConsultaReporteVehiculos_tipoCar_tipoAfil_TipoVeh_Modelo(dataset.Vehit_ConsultaReporteVehiculos_tipoCar_tipoAfil_TipoVeh_Modelo, tipocarroceria, tipoafiliacion, tipovehiculo, modelo);
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
        private void validarcamposbusuqeda()
        {
            if (cbbtipoafiliacion.SelectedIndex > 0)
            {
                var itemtipoafi = (TipoAfiliacion)cbbtipoafiliacion.SelectedItem;
                tipoafiliacion = itemtipoafi.IdCodigo;
            }
            else
            {
                tipoafiliacion = null;
            }


            if (cbbtipocarroceria.SelectedIndex > 0)
            {
                var itemtipocar = (VehiT_ConsultaTipoCarroceria_Result)cbbtipocarroceria.SelectedItem;
                tipocarroceria = itemtipocar.Tipo;
            }
            else
            {
                tipocarroceria = null;
            }
            if (cbbtipovehiculo.SelectedIndex > 0)
            {
                var itemtipovehi = (VehiT_ConsultaTiposVehiculo_Result)cbbtipovehiculo.SelectedItem;
                tipovehiculo = itemtipovehi.Codigo;
            }
            else
            {
                tipovehiculo = null;
            }
            if (txtmodelo.Text.Trim() != "")
            {
                modelo = Convert.ToInt32(txtmodelo.Text.Trim());
            }
            else
            {
                modelo = null ;
            }
        } 
        #endregion

        #region Eventos
        private void btn_buscar_Click(object sender, RoutedEventArgs e)
        {
            _reportViewer.Reset();
            _reportViewer.Clear();
            // _reportViewer.Refresh();
            _reportViewer.RefreshReport();
            _isReportViewerLoaded = false;
            validarcamposbusuqeda();


            llenarReporte();

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
