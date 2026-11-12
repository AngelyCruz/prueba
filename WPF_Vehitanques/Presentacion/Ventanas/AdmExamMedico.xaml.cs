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
    /// Lógica de interacción para AdmExamMedico.xaml
    /// </summary>
    public partial class AdmExamMedico : Window
    {
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj = new VentanaMensaje();
        private DatePicker FechaActual = new DatePicker();
        AdmDocumentos Ventanadoc;
        MainWindow main;
        ConductorBLL conductorbll = new ConductorBLL();
        VehiT_ConsultaConductor_Result row_conductor = null;


        public AdmExamMedico()
        {
            InitializeComponent();
            FechaActual.SelectedDate = DateTime.Now.Date;
        }
        public void RecibirPadre(MainWindow padre)
        {
            main = padre;
        }
        public void RecibirConductor(Modelo.VehiT_ConsultaConductor_Result _row_conductor)
        {
            row_conductor = _row_conductor;
            txtnombres.Text = _row_conductor.Nombres + " " + _row_conductor.Apellido1 + " " + _row_conductor.Apellido2;           
            txtNumdocumento.Text = row_conductor.IdConductor;
            if (_row_conductor.FechaExamenesPeriodicos != null)
            {
                dtfechaexamen.SelectedDate = _row_conductor.FechaExamenesPeriodicos;
            }
            // txtusername.Text = row_conductor.IdConductor.Trim();
            // cbbtipodocumento.SelectedValue=_row_conductor.
        }
        private void admExamMedico_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarPrivilegios();
            DelegarPrivilegios();
            if (IsLoaded)
            {
                //CargarTipoDocumentos();
                //cargarcursos();
                ////GetDAtosUserAndroid();
                //limpiarCampos();
                //llenargrilla();
            }
        }
        internal void ValidarPrivilegios()
        {
            var lista = SecurityVariables.PPerfilUsuario;
            var listaprivi = (from rmenu in lista
                              where rmenu.idMenuPadre == 230 & rmenu.alterno == 1 & rmenu.orden == 3
                              select rmenu).ToList();
            //foreach (var item in listaprivi)
            //{
            //    switch (item.nombreMenu)
            //    {
            //        //case "UpdCursos Conductor Propio":
            //        //    if (item.Autorizar == 1)
            //        //    {
            //        //        UpdCurso = true;
            //        //    }
            //        //    else
            //        //    {
            //        //        UpdCurso = false;
            //        //    }

            //        //    break;
            //        //case "DelCursos Conductor Propio":
            //        //    if (item.Autorizar == 1)
            //        //    {
            //        //        DeleteCurso = true;
            //        //    }
            //        //    else
            //        //    {
            //        //        DeleteCurso = false;
            //        //    }

            //            break;
                  

            //            break;



            //    }
            //}
        }
        private void DelegarPrivilegios()
        {
            //if (boolmsgandroid == true)
            //{
            //    btn_guardar.Visibility = System.Windows.Visibility.Visible;
            //}
            //else
            //{
            //    btn_guardar.Visibility = System.Windows.Visibility.Collapsed;
            //}
        }
        private void btn_Examenes_Click(object sender, RoutedEventArgs e)
        {
            if (Ventanadoc != null)
            {
                Ventanadoc.Close();
            }
            Ventanadoc = new AdmDocumentos(main);
            Ventanadoc.TipoDocumento(25);
            Ventanadoc.IdDocumento(txtNumdocumento.Text.Trim());
            Ventanadoc.Show();
        }
        private void dtfechaexamen_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtfechaexamen.SelectedDate != null)
            {
                DateTime fechafutura;
                DateTime fechaaux = (DateTime)dtfechaexamen.SelectedDate;
                fechafutura = DateTime.Now;
                fechafutura = fechafutura.AddYears(1);

                if (dtfechaexamen.SelectedDate < FechaActual.SelectedDate)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(14,"");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfechaexamen.SelectedDate = null;
                    dtfechaexamen.DisplayDateStart  = DateTime.Now;
                }

            }
        }
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
        private void btn_guardar_Click(object sender, RoutedEventArgs e)
        {
            if (dtfechaexamen.SelectedDate != null)
            {
                DateTime fechaexamn = (DateTime)dtfechaexamen.SelectedDate;
                Boolean respuesta = conductorbll.UpdateExamenconductor(txtNumdocumento.Text.Trim(), fechaexamn);
                if (respuesta == true)
                {
                    mostrar_mensaje(12);
                    this.Close();
                    //LimpiarCampos();
                }
                else
                {
                    mostrar_mensaje(23);
                    this.Close();
                   // LimpiarCampos();
                }

            }
            else
            {
                MessageBox.Show("debe diligenciar una fecha de examen médico.");
            }
        }

        private void admExamMedico_Closed(object sender, EventArgs e)
        {
            if (Venmsj != null)
            {
                Venmsj.Close();
            }
        }
    }
}
