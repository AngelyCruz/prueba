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
    /// Lógica de interacción para documentosRemolque.xaml
    /// </summary>
    public partial class documentosRemolque : UserControl
    {
        #region Declaraciones
        RemolqueBLL remolquebll = new RemolqueBLL();
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj = new VentanaMensaje();
        Numeric numeric = new Numeric();

        private DatePicker FechaActual = new DatePicker();
        MainWindow main;
        TipoDocumentoBLL TiDocBll = new TipoDocumentoBLL();
        CursoBLL Cursosbll = new CursoBLL();
        AdmDocumentos Ventanadoc; 
        #endregion
        #region Load

        public documentosRemolque()
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
        private void llenargrilla()
        {
            if (txtplaca.Text.Trim() == "")
            {
                MessageBox.Show("Debe digitar una placa obligatoriamente");
                return;
            }
            var lista = remolquebll.GetRemolquePorPlaca(txtplaca.Text.Trim());
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

        private void CargarGrilla(List<Modelo.VehiT_ConsultaRemolqueXPlaca_Result> lista)
        {
            gridremolques.ItemsSource = null;
            gridremolques.ItemsSource = lista;
        }
        #endregion
        #region Eventos

        private void txtbusqueda_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if (validarCaracteresespeciales(e, txt) == true)
            {
                if (e.Key == Key.Enter || e.Key == Key.Tab)
                {
                    llenargrilla();
                    //  txt.MoveFocus(new TraversalRequest(
                    //   FocusNavigationDirection.Next));
                }
            }
            else
            {
                txt.Text = "";
            }

        }

        private void btn_Adjuntar_Click(object sender, RoutedEventArgs e)
        {
            main.ValidarVersion();
            //string usuario = "", agencia = "";
            var currentCell = gridremolques.SelectedItem;
            var item = (VehiT_ConsultaRemolqueXPlaca_Result)currentCell;
            if (item != null)
            {
                if (Ventanadoc != null)
                {
                    Ventanadoc.Close();
                }
                Ventanadoc = new AdmDocumentos(main);
                Ventanadoc.TipoDocumento(Convert.ToInt32(7));
                Ventanadoc.IdDocumento(item.IdPlacaRO.Trim());
                //Ventanadoc.recibirAdmcursos(this, item);
                Ventanadoc.Show();
            }
        }

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarPrivilegios();
            DelegarPrivilegios();
            if (IsLoaded)
            {
                //cargarEmpresas();
                //CargarAnalistas();
                //CargarCategorialicencia();
                //CargarTipoDocumentos();
                //cargarDepartamentos();
                //Habilitarbotonesconductor(false);
            }

        }

        private void DelegarPrivilegios()
        {
            
        }

        private void ValidarPrivilegios()
        {
            
        }

        private void btn_buscar_Click(object sender, RoutedEventArgs e)
        {
            llenargrilla();
        }

        

       
    }
}
