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
using WPF_Vehitanques.Utilidades;

namespace WPF_Vehitanques.Presentacion.Ventanas
{
    /// <summary>
    /// Lógica de interacción para AdmCursosConductor.xaml
    /// </summary>
    public partial class AdmCursosConductor : Window
    {
        #region Declaraciones
        private int iStar = 1;
        private int iEnd = 10;
        private int iAdd = 10;
        MensajeBLL Mensabll;
        VentanaMensaje Venmsj = new VentanaMensaje();
        Numeric numeric = new Numeric();
        VehiT_ConsultaConductor_Result row_conductor = null;
        private DatePicker FechaActual = new DatePicker();
        MainWindow main;
        TipoDocumentoBLL TiDocBll = new TipoDocumentoBLL();
        CursoBLL Cursosbll = new CursoBLL();
        private Boolean UpdCurso = false, DeleteCurso = false,DocCurso = false;
        AdmDocumentos Ventanadoc ;
        #endregion
        #region Load
        public AdmCursosConductor()
        {
            InitializeComponent();
            FechaActual.SelectedDate = DateTime.Now.Date;
        }
        private void admcursosconductor_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarPrivilegios();
            DelegarPrivilegios();
            if (IsLoaded)
            {
                CargarTipoDocumentos();
                //cargarcursos();
                CargarModulos();
                //GetDAtosUserAndroid();
                limpiarCampos();
                llenargrilla();
            }
        }

        
        #endregion
        #region Métodos
        private void CargarModulos()
        {
            var lista = Cursosbll.GetModuloCursos();
            cbbgrupo.ItemsSource = lista;
            cbbgrupo.SelectedValuePath = "ModC_Id";
            cbbgrupo.DisplayMemberPath = "ModC_Descripcion";
            cbbgrupo.SelectedIndex = -1;
        }
        internal void UpdateEstadoDocumentoCargado(VehiT_ConsultaCursosPorConductor_Result item)
        {
         Boolean respuesta=   Cursosbll.UpdateEstadoDocumentoCursosConductor(main.txtusuario.Text.Trim(),Convert.ToDecimal( item.CurC_Id), false);
         limpiarCampos();
         //if (respuesta == true)
         //{

         //}
         //else
         //{

         //}
        }
        private void cargarcursos(int idgrupo)
        {
            var lista = Cursosbll.GetCursoporModulo(idgrupo);
            cbbcurso.ItemsSource = lista;
            cbbcurso.SelectedValuePath = "CodigoDoc";
            cbbcurso.DisplayMemberPath = "Descripcion";
            cbbcurso.SelectedIndex = -1;
        }


        private void cargarcursos()
        {
            var lista = Cursosbll.GetListaCursos();
            cbbcurso.ItemsSource = lista;
            cbbcurso.SelectedValuePath = "CodigoDoc";
            cbbcurso.DisplayMemberPath = "Descripcion";
            cbbcurso.SelectedIndex = -1;
        }
        private void limpiarCampos()
        {
            lblidnotificacion.Content = "";
            dtfechacurso.SelectedDate = null;
            dtfechavigenciacurso.SelectedDate = null;
            txtvigenciameses.Text = "";
           // txtobservaciones.Text = "";
        }
        public  void llenargrilla()
        {
            try
            {
                pnl_Paginacion.Visibility = Visibility.Visible;
                gridlistanotificaciones.ItemsSource = null;
                gridlistanotificaciones.Visibility = Visibility.Visible;
                lbl_Inicio.Text = iStar.ToString();

                lbl_Total.Text = Convert.ToString(Cursosbll.ContadorCursosConductor(row_conductor.IdConductor));
                if (Convert.ToInt32(lbl_Total.Text) <= iEnd)
                {
                    lbl_Fin.Text = lbl_Total.Text;
                    btn_sig.IsEnabled = false;
                    pnl_Paginacion.Visibility = Visibility.Collapsed;
                }
                else
                {
                    pnl_Paginacion.Visibility = Visibility.Visible;
                    lbl_Fin.Text = iEnd.ToString();
                    //'linea nueva de código
                    btn_sig.IsEnabled = true;
                }
                var lista = Cursosbll.GetCursosConductor(row_conductor.IdConductor, iStar, iEnd);
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
            catch (Exception ex)
            {
                ex.Message.ToString();
                Venmsj = new VentanaMensaje();
                Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                Venmsj.cargarmensaje(ex.ToString(), ex.Message.ToString());
                Venmsj.ShowDialog();
            }
        }
        private void CargarGrilla(List<VehiT_ConsultaCursosPorConductor_Result> lista)
        {
            gridlistanotificaciones.ItemsSource = null;
            gridlistanotificaciones.ItemsSource = lista;

            if (DocCurso == true)
            {
                gridlistanotificaciones.Columns[0].Visibility = System.Windows.Visibility.Visible;

            }
            else
            {
                gridlistanotificaciones.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
            }
            if (UpdCurso == true)
            {
                gridlistanotificaciones.Columns[1].Visibility = System.Windows.Visibility.Visible;

            }
            else
            {
                gridlistanotificaciones.Columns[1].Visibility = System.Windows.Visibility.Collapsed;
            }
            if (DeleteCurso == true)
            {
                gridlistanotificaciones.Columns[2].Visibility = System.Windows.Visibility.Visible;

            }
            else
            {
                gridlistanotificaciones.Columns[2].Visibility = System.Windows.Visibility.Collapsed;
            }
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
                limpiarCampos();
                llenargrilla();
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
        internal void ValidarPrivilegios()
        {
            var lista = SecurityVariables.PPerfilUsuario;
            var listaprivi = (from rmenu in lista
                              where rmenu.idMenuPadre == 230 & rmenu.alterno == 1 & rmenu.orden == 3
                              select rmenu).ToList();
            foreach (var item in listaprivi)
            {
                switch (item.nombreMenu)
                {
                    case "UpdCursos Conductor Propio":
                        if (item.Autorizar == 1)
                        {
                             UpdCurso = true;
                        }
                        else
                        {
                            UpdCurso = false;
                        }

                        break;
                    case "DelCursos Conductor Propio":
                        if (item.Autorizar == 1)
                        {
                            DeleteCurso  = true;
                        }
                        else
                        {
                            DeleteCurso = false;
                        }

                        break;
                    case "DocCursos Conductor Propio":
                        if (item.Autorizar == 1)
                        {
                            DocCurso  = true;
                        }
                        else
                        {
                            DocCurso = false;
                        }

                        break;



                }
            }
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
        public void RecibirPadre(MainWindow padre)
        {
            if (main == null)
                main = padre; 
        }
        private void CargarTipoDocumentos()
        {
            var lista = TiDocBll.GetTipoDocumentoIdentificacion();
            cbbtipodocumento.ItemsSource = lista;
            cbbtipodocumento.SelectedValuePath = "Codigo";
            cbbtipodocumento.DisplayMemberPath = "TipoDocumento";
            cbbtipodocumento.SelectedIndex = 0;
        }
       
        public void RecibirConductor(Modelo.VehiT_ConsultaConductor_Result _row_conductor)
        {
            row_conductor = _row_conductor;
            txtnombres.Text = _row_conductor.Nombres;
            txtapellido1.Text = _row_conductor.Apellido1;
            txtapellido2.Text = _row_conductor.Apellido2;
            txtNumdocumento.Text = row_conductor.IdConductor;
            // txtusername.Text = row_conductor.IdConductor.Trim();
            // cbbtipodocumento.SelectedValue=_row_conductor.
        }
        private Boolean validarcamposCurso()
        {
            if (cbbcurso.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar un curso");
                return true;
            }
            if (dtfechacurso.SelectedDate == null)
            {
                MessageBox.Show("Debe seleccionar una fecha de elaboración del curso");
                return true;
            }
            if (dtfechavigenciacurso.SelectedDate == null)
            {
                MessageBox.Show("Debe seleccionar una fecha de elaboración del curso,para recargar la fecha de vigencia");
                return true;
            }
            return false;
        }
       
        #endregion
        #region Eventos
        private void btn_guardar_Click(object sender, RoutedEventArgs e)
        {
            main.ValidarVersion();
            if (validarcamposCurso() == false)
            {
                if (lblidnotificacion.Content == "")
                {
                    if (cbbcurso.SelectedItem != null)
                    {
                        Nullable<DateTime> fechavigencia = null;
                        DateTime fechacurso = (DateTime)dtfechacurso.SelectedDate;
                        int vigenciameses = 0;

                        if (dtfechavigenciacurso.SelectedDate != null)
                        {
                            fechavigencia = (DateTime)dtfechavigenciacurso.SelectedDate;
                        }
                        if (txtvigenciameses.Text.Trim() != "")
                        {
                            vigenciameses = Convert.ToInt32(txtvigenciameses.Text.Trim());
                        }

                        Vehit_ConsultaCursosPorGrupo_Result item = (Vehit_ConsultaCursosPorGrupo_Result)cbbcurso.SelectedItem;
                        Boolean respuesta = Cursosbll.GuardarCursoConductor(item.CodigoDoc, txtNumdocumento.Text.Trim(), fechavigencia, vigenciameses, fechacurso, main.txtusuario.Text.Trim());

                        mostrarmensaje(respuesta);
                        if (respuesta == true)
                        {
                            llenargrilla();
                            limpiarCampos();
                        }

                    }
                }
                else
                {
                    if (cbbcurso.SelectedItem != null)
                    {
                        MessageBoxResult result1 = MessageBox.Show("Este curso ya  Existe para éste conductor. Está seguro que desea actualizar sus datos?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result1 == MessageBoxResult.Yes)
                        {
                            Nullable<DateTime> fechavigencia = null;
                            DateTime fechacurso = (DateTime)dtfechacurso.SelectedDate;
                            int vigenciameses = 0;

                            if (dtfechavigenciacurso.SelectedDate != null)
                            {
                                fechavigencia = (DateTime)dtfechavigenciacurso.SelectedDate;
                            }
                            if (txtvigenciameses.Text.Trim() != "")
                            {
                                vigenciameses = Convert.ToInt32(txtvigenciameses.Text.Trim());
                            }
                            Vehit_ConsultaCursosPorGrupo_Result item = (Vehit_ConsultaCursosPorGrupo_Result)cbbcurso.SelectedItem;
                            Boolean respuesta = Cursosbll.UpdateCursoConductor(fechavigencia, vigenciameses, fechacurso, main.txtusuario.Text.Trim(), Convert.ToDecimal(lblidnotificacion.Content));
                            mostrarmensaje(respuesta);
                            if (respuesta == true)
                            {
                                llenargrilla();
                                limpiarCampos();
                            }
                        }
                        else
                        {
                            limpiarCampos();

                        }
                    }
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
        private void btn_ant_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int iRes = 0;
                if (Convert.ToInt32(lbl_Fin.Text) == Convert.ToInt32(lbl_Total.Text))
                {
                    iRes = Convert.ToInt32(lbl_Fin.Text) - Convert.ToInt32(lbl_Inicio.Text);
                }
                else
                {
                    iRes = iAdd;
                }
                lbl_Inicio.Text = Convert.ToString(Convert.ToInt32(lbl_Inicio.Text) - iAdd);
                lbl_Fin.Text = Convert.ToString(Convert.ToInt32(lbl_Fin.Text) - iRes);
                lbl_Total.Text = Convert.ToString(Cursosbll.ContadorCursosConductor(row_conductor.IdConductor));
                if (Convert.ToInt32(lbl_Inicio.Text) > 1)
                {
                    btn_sig.IsEnabled = true;
                }
                else
                {
                    lbl_Inicio.Text = Convert.ToString(iStar);
                    lbl_Fin.Text = Convert.ToString(iEnd);
                    btn_ant.IsEnabled = false;
                    btn_sig.IsEnabled = true;
                }
                gridlistanotificaciones.ItemsSource = null;
                var lista = Cursosbll.GetCursosConductor (row_conductor.IdConductor, Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
                if (lista.Count > 0)
                {
                    CargarGrilla(lista);
                }
                else
                {

                }
                gridlistanotificaciones.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
        private void btn_sig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lbl_Inicio.Text = Convert.ToString(Convert.ToInt32(lbl_Inicio.Text) + iAdd);
                lbl_Fin.Text = Convert.ToString(Convert.ToInt32(lbl_Fin.Text) + iAdd);
                lbl_Total.Text = Convert.ToString(Cursosbll.ContadorCursosConductor(row_conductor.IdConductor));
                if (Convert.ToInt32(lbl_Fin.Text) < Convert.ToInt32(lbl_Total.Text))
                {
                    btn_ant.IsEnabled = true;
                }
                else
                {
                    lbl_Fin.Text = lbl_Total.Text;
                    btn_sig.IsEnabled = false;
                    btn_ant.IsEnabled = true;
                }
                gridlistanotificaciones.ItemsSource = null;
                var lista = Cursosbll.GetCursosConductor (row_conductor.IdConductor, Convert.ToInt32(lbl_Inicio.Text), Convert.ToInt32(lbl_Fin.Text));
                if (lista.Count > 0)
                {
                    CargarGrilla(lista);
                }
                gridlistanotificaciones.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
        private void btn_Editar_Click(object sender, RoutedEventArgs e)
        {
            limpiarCampos();
            var currentCell = gridlistanotificaciones.SelectedItem;
            var item = (VehiT_ConsultaCursosPorConductor_Result)currentCell;
            if (item != null)
            {
                cbbcurso.SelectedValue = item.CodigoDoc;
                if (item.VigenciaEnMeses != null)
                {
                    txtvigenciameses.Text = item.VigenciaEnMeses.ToString();
                }
                else
                {
                    txtvigenciameses.Text = "";
                }
                
                dtfechacurso.SelectedDate = item.FechaCurso;
                if (item.FechaVigencia != null)
                {
                    dtfechavigenciacurso.SelectedDate = item.FechaVigencia;
                }
                else
                {

                    dtfechavigenciacurso.SelectedDate = null;
                }
               // txtobservaciones.Text = item.DescripcionNotificacion;
                lblidnotificacion.Content = item.CurC_Id.ToString();
            }

        }

        private void btn_Eliminar_Click(object sender, RoutedEventArgs e)
        {
            string usuario="", agencia="";
            var currentCell = gridlistanotificaciones.SelectedItem;
            var item = (VehiT_ConsultaCursosPorConductor_Result )currentCell;
            if (item != null)
            {
                usuario = main.txtusuario.Text.Trim();
                agencia  = main.txtagencia.Text.Trim();
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
                Boolean respuesta = Cursosbll.updateEstadoCursoConductor( Convert.ToDecimal( item.CurC_Id), usuario );
                mostrarmensaje(respuesta);
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
                MessageBox.Show(" la combinación de ésta tecla, no está pérmitida " + e.Key.ToString());
                txt.Text = "";
                respuesta = false;
            }
            else
            {
                respuesta = true;
            }
            return respuesta;
        }
        private void dtfechacurso_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (cbbcurso.SelectedItem != null)
            {
                Vehit_ConsultaCursosPorGrupo_Result item = (Vehit_ConsultaCursosPorGrupo_Result)cbbcurso.SelectedItem;
                //  int idcurso = Convert.ToInt32(cbbcurso.SelectedItem.ToString());
                if (item != null)
                {
                    int idcu = item.CodigoDoc;
                    var lista = Cursosbll.GetParametrosCurso(item.CodigoDoc);
                    if (lista.Count > 0)
                    {
                        txtvigenciameses.Text = lista[0].PCurC_VigenciaEnMeses.ToString();
                    }
                    else
                    {
                        txtvigenciameses.Text = "0";
                    }
                }
                else
                {
                    txtvigenciameses.Text = "0";
                }
            }
            if (dtfechacurso.SelectedDate != null)
            {
                DateTime fechafutura;
                DateTime fechaaux = (DateTime)dtfechacurso.SelectedDate;
                fechafutura = DateTime.Now;
                fechafutura = fechafutura.AddYears(1);
                if (dtfechacurso.SelectedDate > FechaActual.SelectedDate)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(15,"");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfechacurso.SelectedDate = null;
                    dtfechacurso.DisplayDateEnd = DateTime.Now;
                }
                else
                {
                    if (txtvigenciameses.Text.Trim() != "")
                    {
                        int vigencia = Convert.ToInt32(txtvigenciameses.Text.Trim());
                        fechafutura = (DateTime )dtfechacurso.SelectedDate;
                        fechafutura = fechafutura.AddMonths(vigencia);
                        dtfechavigenciacurso.SelectedDate = fechafutura;
                    }
                    else
                    {
                        dtfechavigenciacurso.SelectedDate = null;
                    }
                }
            }
        }

        private void dtfechavigenciacurso_CalendarClosed(object sender, RoutedEventArgs e)
        {

            if (dtfechavigenciacurso.SelectedDate != null)
            {
                DateTime fechafutura;
                DateTime fechaaux = (DateTime)dtfechavigenciacurso.SelectedDate;
                fechafutura = DateTime.Now;
                fechafutura = fechafutura.AddYears(1);
                if (dtfechavigenciacurso.SelectedDate < FechaActual.SelectedDate)
                {
                    Mensabll = new MensajeBLL();
                    Mensabll.tipomensajes(14,"");
                    Venmsj = new VentanaMensaje();
                    Venmsj.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                    Venmsj.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                    Venmsj.cargarmensaje(Mensabll.titulos, Mensabll.mensajes);
                    Venmsj.ShowDialog();
                    dtfechavigenciacurso.SelectedDate = null;
                    dtfechavigenciacurso.DisplayDateStart = DateTime.Now;
                }
            }
        }


        private void cbbcurso_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbcurso.SelectedItem != null)
            {
                Vehit_ConsultaCursosPorGrupo_Result item = (Vehit_ConsultaCursosPorGrupo_Result)cbbcurso.SelectedItem;
                //  int idcurso = Convert.ToInt32(cbbcurso.SelectedItem.ToString());
                if (item != null)
                {
                    int idcu = item.CodigoDoc;
                    var lista = Cursosbll.GetParametrosCurso(item.CodigoDoc);
                    if (lista.Count > 0)
                    {
                        txtvigenciameses.Text = lista[0].PCurC_VigenciaEnMeses.ToString();
                    }
                    else
                    {
                        txtvigenciameses.Text = "0";
                    }
                }
                else
                {
                    txtvigenciameses.Text = "0";
                }
            }
        }

        private void btn_Adjuntar_Click(object sender, RoutedEventArgs e)
        {
            main.ValidarVersion();
            string usuario = "", agencia = "";
            var currentCell = gridlistanotificaciones.SelectedItem;
            var item = (VehiT_ConsultaCursosPorConductor_Result)currentCell;
            if (item != null)
            {
                if (Ventanadoc != null)
                {
                    Ventanadoc.Close();
                }                
                Ventanadoc = new AdmDocumentos(main );
                Ventanadoc.TipoDocumento(Convert.ToInt32(item.CodigoDoc));
                Ventanadoc.IdDocumento(txtNumdocumento.Text.Trim());
                Ventanadoc.recibirAdmcursos(this, item);
                Ventanadoc.Show();
            }
        }
        private void cbbgrupo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Vehit_ConsultaGrupoCursosConductor_Result item = (Vehit_ConsultaGrupoCursosConductor_Result)cbbgrupo.SelectedItem ;
            if (item != null)
            {
                cargarcursos(item.ModC_Id);
            }

        }
      
        #endregion

        private void btn_guardarmodulo_Click(object sender, RoutedEventArgs e)
        {
            main.ValidarVersion();
            if (cbbgrupo.SelectedIndex >= 0)
            {
                Vehit_ConsultaGrupoCursosConductor_Result item = (Vehit_ConsultaGrupoCursosConductor_Result)cbbgrupo.SelectedItem;
                if (item != null)
                {
                    if (item.PermiteGuardarTodos == true)
                    {
                        Nullable<DateTime> fechavigencia = null;
                        
                        int vigenciameses = 0;
                        
                        if (dtfechacurso.SelectedDate == null)
                        {
                            MessageBox.Show("Debe seleccionar una fecha de elaboración del curso");
                            return ;
                        }
                        if (dtfechavigenciacurso.SelectedDate == null)
                        {
                            MessageBox.Show("Debe seleccionar una fecha de elaboración del curso,para recargar la fecha de vigencia");
                            return ;
                        }
                        DateTime fechacurso = (DateTime)dtfechacurso.SelectedDate;

                        if (dtfechavigenciacurso.SelectedDate != null)
                        {
                            fechavigencia = (DateTime)dtfechavigenciacurso.SelectedDate;
                        }
                        if (txtvigenciameses.Text.Trim() != "")
                        {
                            vigenciameses = Convert.ToInt32(txtvigenciameses.Text.Trim());
                        }

                        var lista = Cursosbll.GetCursoporModulo(item.ModC_Id);
                        Boolean respuesta = false;
                        foreach (var rowcurso in lista)
                        {

                             respuesta = Cursosbll.GuardarCursoConductor(rowcurso.CodigoDoc, txtNumdocumento.Text.Trim(), fechavigencia, vigenciameses, fechacurso, main.txtusuario.Text.Trim());

                           

                        }
                        mostrarmensaje(respuesta);
                        if (respuesta == true)
                        {
                            llenargrilla();
                            limpiarCampos();
                        }
                       
                    }
                    else
                    {

                        MessageBox.Show("Este módulo no está permitido para registrar todos sus cursos");
                    }
                }
            }
            
        }

       

       

       
    }
}
