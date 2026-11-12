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
using WPF_Vehitanques.Presentacion.Controles;
using WPF_Vehitanques.Presentacion.Ventanas;
using WPF_Vehitanques.BLL;
using Microsoft.Win32;
using WPF_Vehitanques.Documentos;
using WPF_Vehitanques.DS;
using System.IO;
using System.Security.Permissions;
using System.Security;
using WPF_Vehitanques.Utilidades;
using System.Net;
namespace WPF_Vehitanques.Presentacion.Ventanas
{
    /// <summary>
    /// Lógica de interacción para AdmDocumentos.xaml
    /// </summary>
    public partial class AdmDocumentos : Window
    {

        #region Declaraciones
        MensajeBLL mensaje = new MensajeBLL();
        VentanaMensaje ventamensaje = new VentanaMensaje();
        StackPanel stPnl;
        TextBlock lbl_Nombre;
        Button boton;
        private string RutaDocu;
        // Dim boton As ButtonBase
        WrapPanel wrapboton;
        Image imagenboton;
        BitmapImage imagen;
        FileInfo File;
        String[] Name, peso;
        Documentos.Documentos Doc;
        int idcaso, docId;
        private string nombredirectorio, nombredirBD;
        DateTime fecha = new DateTime();
        private int TipoDoc;
        DocumentosBLL DocuBll = new DocumentosBLL();
        DSDocumentos.SubidaArchivosDataTable DSSubAr = new DSDocumentos.SubidaArchivosDataTable();
        double pesolimite;
        string identificador;
        private String Usuario;
        private string identificacionconductor="";
        private string identificacionbeneficiario= "";
        private Boolean boolautorizacambios = false;
        AdmCursosConductor admnCursosConduc;
        string nombreDoc = "";
        Modelo.VehiT_ConsultaCursosPorConductor_Result item;
        MainWindow main;
        #endregion
        #region load
        public AdmDocumentos(MainWindow _main)
        {
            InitializeComponent();
            main=_main;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarPrivilegios();
            GetRutaDocumentos();
            CargarDocumento();
        
        }
        #endregion
        #region Métodos

        public void recibirAdmcursos(AdmCursosConductor adm, Modelo.VehiT_ConsultaCursosPorConductor_Result _item)
        {
            admnCursosConduc=adm;
            item = _item;
        }
        private void ValidaBotones()
        {
            if (boolautorizacambios == false)
            {
                btn_Examinar2.IsEnabled = false;
            }
            else
            {
                btn_Examinar2.IsEnabled = true ;
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
                    case "Agregar Documentación":
                        if (item.Autorizar == 1)
                        {
                            boolautorizacambios = true;
                        }
                        else
                        {
                            boolautorizacambios = false;
                        }
                       
                        break;
                    //case "Información Bancaria":
                    //    if (item.Actualizar == 1)
                    //    {
                    //        boolinfobancaria = true;
                    //    }
                    //    else
                    //    {
                    //        boolinfobancaria = false;
                    //    }
                    //    break;
                    //case "Ver Info Vehiculo":
                    //    if (item.Actualizar == 1)
                    //    {
                    //        boolinfovehiculo = true;
                    //    }
                    //    else
                    //    {
                    //        boolinfovehiculo = false;
                    //    }
                    //    break;

                }
            }
            PermisoExaminar(boolautorizacambios);
        }
        public void crearNombreArchivo()
        {
            var lista = DocuBll.GetTipoDocumentos(TipoDoc);
            if (lista.Count > 0)
            {
                nombredirectorio = lista[0].Descripcion;

            }
        }
        public void GetIdentificacionconductor(string _id)
        {
            identificacionconductor = _id;
        }
        public void GetIdentificacionBENEFICIARIO(string _id)
        {
            identificacionbeneficiario = _id;
        }

        public void GetRutaDocumentos()
        {
            var lista = DocuBll.GetRutaDocumentos(1);
            if (lista != null)
            {
                RutaDocu = lista[0].Ruta_Direccion;
            }
        }
        public  void PermisoExaminar(Boolean examinar)
        {
            btn_Examinar2.IsEnabled = examinar;
           // btn_CargarDoc.IsEnabled = examinar;
        }
        
        public   void TipoDocumento(int idtipodoc)
        {

            TipoDoc = idtipodoc;
            crearNombreArchivo();
        }
        internal void IdDocumento(string identificacion)
        {

            identificador = identificacion;

        }
        // Encrypt a file.
        public static void AddEncryption(string FileName)
        {

            System.IO.File.Encrypt(FileName);

        }

        // Decrypt a file.
        public static void RemoveEncryption(string FileName)
        {
            System.IO.File.Decrypt(FileName);
        }

        private void subirarchivo(DSDocumentos.SubidaArchivosRow row)
        {
            try
            {

                String RutaDoc = row.Id + "\\" + nombredirectorio.ToString();
                string uploadPath = RutaDocu + "\\" + RutaDoc;
                DirectoryInfo dir = new DirectoryInfo(uploadPath);
                nombredirBD = uploadPath;
                if (!dir.Exists)
                {
                    dir.Create();
                }
                try
                {
                    if (nombreDoc != "")
                    {
                        if (row.Nombre == nombreDoc)
                        {
                             GeneradorPassword Generator = new GeneradorPassword(4, 50, 0, 50);
            //GeneradorPassword Generator = new GeneradorPassword(10);
                    string nuevaclave = Generator.GetNewPassword();
                 
                            string[] aux=nombreDoc.Split('.');
                            nombreDoc = aux[0] + nuevaclave+"."+aux[1];

                        }
                        else
                        {
                            nombreDoc = row.Nombre;
                        }
                    }
                    else
                    {
                        nombreDoc = row.Nombre;
                    }
                    
                    try
                    {
                        System.IO.File.Copy(row.Ubicacion, uploadPath + @"\" + nombreDoc, true);
                        DocuBll.GuardarDocumento(nombreDoc, row.Tipo, row.Peso, row.TipoDocumento, uploadPath, row.Id, identificacionconductor, 1, main.txtusuario.Text.Trim());
                    }
                    catch { MessageBox.Show("No tiene privilegios para salvar documentos"); }
                    

                    //FileIOPermission f2 = new FileIOPermission(FileIOPermissionAccess.Read, uploadPath);
                    //try
                    //{
                    //    f2.Demand();
                    //}
                    //catch (SecurityException s)
                    //{
                    //    ///Console.WriteLine(s.Message);
                    //}
                    //System.IO.File.Encrypt(uploadPath);
                    //   AddEncryption(uploadPath );
                    // RemoveEncryption(uploadPath + @"\" + Nombre);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                finally
                {
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                //errorbll = new ErroresBLL();
                //errorbll.guardaerrores(ex.Message.ToString(), ex.StackTrace.ToUpper().ToString(), conten.strNombre);
            }
        }

        #endregion
        #region Eventos
        private void eliminar_click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            // var commandParameter = ((Button)sender).CommandParameter;
            // if (commandParameter != null)
            // {
            //     if (commandParameter is StackPanel)
            //     {
            //         StackPanel vari;

            //     }
            // }
            // var variable =   ((Button)sender).CommandParameter;
            //// StackPanel var = ((Button)sender).CommandParameter;
            // //DSDocumentos.CR_SubidaArchivosRow AdaprowAux;
            // for (int i = 0; i < vari.Children.Count; i++)
            // {
            // //    if (var.Children(i) is TextBlock)
            // //   {
            // //        TextBlock txt = ((TextBlock)var.Children(i));
            // ////        DataRow[] dr = DSSubAr.Select("Nombre='" + txt.Text + "'");
            // ////        if (dr.Length > 0)
            // ////        {
            // ////            AdaprowAux = ((DSDocumentos.CR_SubidaArchivosRow)dr[0]);
            // ////        }
            // ////        DSSubAr.RemoveCR_SubidaArchivosRow(AdaprowAux);
            // ////        DSSubAr.AcceptChanges();
            // ////        var.Visibility = Windows.Visibility.Collapsed;
            // ////        i = var.Children.Count + 1;
            // ////        //Exit For
            // //   }
            // }
        }
        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
        private void btn_Examinar2_Click(object sender, RoutedEventArgs e)
        {
            if (identificador != "")
            {
                MessageBoxResult result1 = MessageBox.Show("Está Seguro que desea cargar un nuevo documento?, el nuevo documento reemplazará el anterior", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result1 == MessageBoxResult.Yes)
                {
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.Multiselect = true;
                    dlg.DefaultExt = ".jpg"; // Extensión de archivos por defaul
                    dlg.Filter = "Todos(*.*)|*.*|Imagenes|*.jpg;*.gif;*.png;*.jpeg|Word|*.Doc;*.Docx|PDF|*.pdf;*.bmp|Excel|*.xlsx;*.xls"; // Filtros permitidos
                    Nullable<bool> result = dlg.ShowDialog();
                    if (result == true)
                    {
                        DSSubAr.Clear();
                        //WrapPanel1.Children.Clear();
                        //WrapPanel1 = new StackPanel();
                        wp_Imagenes.Children.Clear();
                        wp_PDF.Children.Clear();
                        wp_Doc.Children.Clear();
                        wp_Excel.Children.Clear();
                        foreach (string stringarchivo in dlg.FileNames)
                        {
                            FileInfo pFile = new FileInfo(stringarchivo);
                            Doc = new Documentos.Documentos(pFile);
                            Doc.Detalles = detalles;
                            stPnl = new StackPanel();
                            lbl_Nombre = new TextBlock();
                            lbl_Nombre.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            if (Doc.Nombre.Length >= 80)
                            {
                                ventamensaje = new VentanaMensaje();
                                mensaje = new MensajeBLL();
                                mensaje.tipomensajes(10,"");
                                ventamensaje.cargarmensaje(mensaje.titulos, Doc.Nombre);
                                ventamensaje.Show();
                            }
                            else
                            {
                                if (Doc.Tipo == Documentos.TipoDoc.RECHAZADO)
                                {
                                    ventamensaje = new VentanaMensaje();
                                    mensaje = new MensajeBLL();
                                    mensaje.tipomensajes(27,"");
                                    ventamensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                                    ventamensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                                    ventamensaje.cargarmensaje(mensaje.titulos, mensaje.mensajes);
                                    ventamensaje.Show();
                                }
                                else
                                {
                                    var lista = DocuBll.GetTamaniodocumentos();
                                    if (Doc.Peso.Contains("M"))
                                    {
                                        pesolimite = Convert.ToDouble(lista[1].Peso);
                                    }
                                    else
                                    {
                                        if (Doc.Peso.Contains("K"))
                                        {
                                            // var lista = DocuBll.GetTamaniodocumentos();
                                            pesolimite = Convert.ToDouble(lista[0].Peso);
                                        }
                                    }
                                    String[] peso = Doc.Peso.Split('K', 'b', 'M');
                                    if (Convert.ToDouble(peso[0]) > pesolimite)
                                    {
                                        if (Doc.Peso.Contains("K"))
                                        {
                                            ventamensaje = new VentanaMensaje();
                                            mensaje = new MensajeBLL();
                                            mensaje.tipomensajes(11,"");
                                            ventamensaje.cargarmensaje((mensaje.titulos + pesolimite.ToString() + " KiloBytes"), Doc.Nombre);
                                            ventamensaje.Show();
                                        }
                                        if (Doc.Peso.Contains("M"))
                                        {
                                            ventamensaje = new VentanaMensaje();
                                            mensaje = new MensajeBLL();
                                            mensaje.tipomensajes(11,"");
                                            ventamensaje.cargarmensaje((mensaje.titulos + pesolimite.ToString() + mensaje.mensajes), Doc.Nombre);
                                            ventamensaje.Show();
                                        }
                                    }
                                    else
                                    {
                                        btn_CargarDoc.IsEnabled = true;
                                        lbl_Nombre.Text = Doc.Nombre;
                                        lbl_Nombre.Width = 100;
                                        lbl_Nombre.TextWrapping = TextWrapping.Wrap;
                                        labelNombre.Content = Doc.Nombre;
                                        LabelTamanio.Content = Doc.Peso;
                                        Labeltipo.Content = Doc.Tipo.ToString();
                                        //AddHandler boton.Click, AddressOf eliminar_click;
                                        //boton.Click += new RoutedEventHandler(eliminar_click);
                                        if (Doc.Tipo == Documentos.TipoDoc.GIF | Doc.Tipo == Documentos.TipoDoc.JPEG |
                                             Doc.Tipo == Documentos.TipoDoc.JPG | Doc.Tipo == Documentos.TipoDoc.PNG)
                                        {
                                            // stPnl.Children.Add(boton);                               
                                            stPnl.Children.Add(Doc.ImagenTipo);
                                            stPnl.Children.Add(lbl_Nombre);
                                            //wp_Imagenes.Children.Clear();
                                            //wp_PDF.Children.Clear();
                                            //wp_Doc.Children.Clear();
                                            //wp_Excel.Children.Clear();
                                            wp_Imagenes.Children.Add(stPnl);
                                            DSSubAr.AddSubidaArchivosRow(Doc.Nombre, Doc.Tipo.ToString(), Doc.Peso, Doc.Path, TipoDoc, identificador);
                                            // DSSubAr.AddCR_SubidaArchivosRow(Doc.Nombre, Doc.Tipo.ToString(), Doc.Peso, Doc.Path, cbbTipodoc.SelectedValue, idcaso);
                                        }
                                        if (Doc.Tipo == Documentos.TipoDoc.DOC | Doc.Tipo == Documentos.TipoDoc.DOCX)
                                        {
                                            // stPnl.Children.Add(boton);
                                            stPnl.Children.Add(Doc.ImagenTipo);
                                            stPnl.Children.Add(lbl_Nombre);
                                            //wp_Imagenes.Children.Clear();
                                            //wp_PDF.Children.Clear();
                                            //wp_Doc.Children.Clear();
                                            //wp_Excel.Children.Clear();
                                            wp_Doc.Children.Add(stPnl);
                                            DSSubAr.AddSubidaArchivosRow(Doc.Nombre, Doc.Tipo.ToString(), Doc.Peso, Doc.Path, TipoDoc, identificador);
                                            //DSSubAr.AddCR_SubidaArchivosRow(Doc.Nombre, Doc.Tipo.ToString(), Doc.Peso, Doc.Path, cbbTipodoc.SelectedValue, idcaso);
                                        }
                                        if (Doc.Tipo == Documentos.TipoDoc.XLS | Doc.Tipo == Documentos.TipoDoc.XLSX)
                                        {
                                            //stPnl.Children.Add(boton);
                                            stPnl.Children.Add(Doc.ImagenTipo);
                                            stPnl.Children.Add(lbl_Nombre);
                                            //wp_Imagenes.Children.Clear();
                                            //wp_PDF.Children.Clear();
                                            //wp_Doc.Children.Clear();
                                            //wp_Excel.Children.Clear();
                                            wp_Excel.Children.Add(stPnl);
                                            DSSubAr.AddSubidaArchivosRow(Doc.Nombre, Doc.Tipo.ToString(), Doc.Peso, Doc.Path, TipoDoc, identificador);
                                            // DSSubAr.AddCR_SubidaArchivosRow(Doc.Nombre, Doc.Tipo.ToString(), Doc.Peso, Doc.Path, cbbTipodoc.SelectedValue, idcaso);
                                        }
                                        if (Doc.Tipo == Documentos.TipoDoc.PDF)
                                        {
                                            // stPnl.Children.Add(boton);
                                            stPnl.Children.Add(Doc.ImagenTipo);
                                            stPnl.Children.Add(lbl_Nombre);
                                            //wp_Imagenes.Children.Clear();
                                            //wp_PDF.Children.Clear();
                                            //wp_Doc.Children.Clear();
                                            //wp_Excel.Children.Clear();
                                            wp_PDF.Children.Add(stPnl);
                                            DSSubAr.AddSubidaArchivosRow(Doc.Nombre, Doc.Tipo.ToString(), Doc.Peso, Doc.Path, TipoDoc, identificador);
                                            //DSSubAr.AddCR_SubidaArchivosRow(Doc.Nombre, Doc.Tipo.ToString(), Doc.Peso, Doc.Path, cbbTipodoc.SelectedValue, idcaso);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                ventamensaje = new VentanaMensaje();
                mensaje = new MensajeBLL();
                mensaje.tipomensajes(54,"");
                ventamensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                ventamensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                ventamensaje.cargarmensaje(mensaje.titulos, mensaje.mensajes);
                ventamensaje.Show();
                this.Close();
            }
            
        }
        #endregion

        private void btn_CargarDoc_Click(object sender, RoutedEventArgs e)
        {
            foreach (DSDocumentos.SubidaArchivosRow row in DSSubAr)
            {
                DocuBll.UpdateEStadoDocumentos(row.TipoDocumento, row.Id);
             
                break;
            }
            foreach (DSDocumentos.SubidaArchivosRow row in DSSubAr)
            {
                subirarchivo(row);
               // break;
            }
            
            wp_Doc.Children.Clear();
            wp_Excel.Children.Clear();
            wp_Imagenes.Children.Clear();
            wp_PDF.Children.Clear();
            DSSubAr.Clear();
            mensaje = new MensajeBLL();
            mensaje.tipomensajes(12,"");
            ventamensaje = new VentanaMensaje();
            ventamensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
            ventamensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
            ventamensaje.cargarmensaje(mensaje.titulos, mensaje.mensajes);
            ventamensaje.ShowDialog();
            if (admnCursosConduc != null & item!=null )
            {
                switch (item.CodigoDoc )
                {
                    case 27:case 28:case 29:case 30:case 31:case 32:case 33:
                    case 34:case 35:case 36:case 37:case 38 :
                        admnCursosConduc.UpdateEstadoDocumentoCargado(item );
                        admnCursosConduc.llenargrilla();
                      //  admnCursosConduc = null;
                        break;
                    //default:
                    //    admnCursosConduc = null;
                    //    break;
                }
            }
            this.Close();
        }

        private void CargarDocumento()
        {
            if (identificador != "") 
            {
                var listadoc = DocuBll.GetDocumentoXPlaca_identificador(TipoDoc, identificador);
                if (listadoc.Count > 0)
                {
                    DSSubAr.Clear();
                    foreach (var item in listadoc)
                    {
                        //WrapPanel1.Children.Clear();
                        //WrapPanel1 = new StackPanel();
                        //  wp_Imagenes.Children.Clear();
                        //    string stringarchivo = listadoc[0].Doc_Ubicacion + '\\' + listadoc[0].HisDoc_Nombre;
                        string stringarchivo = item.Ruta_Direccion + '\\' + item.Placa_Identificador + '\\' + item.Descripcion + '\\' + item.HisDoc_Nombre;
                        //string stringarchivo = @"d:\Documents\Angelica.Gomez\Documentacion\Vehitanques\Prueba" + item.HisDoc_Nombre;
                        FileInfo pFile = new FileInfo(stringarchivo);
                      //  FtpWebRequest dirFtp = ((FtpWebRequest)FtpWebRequest.Create(stringarchivo));
                        // Los datos del usuario (credenciales)
                        //NetworkCredential cr = new NetworkCredential(user, pass);
                       // dirFtp.Credentials = cr;

                        if (pFile.Exists)
                        {
                            Doc = new Documentos.Documentos(pFile);
                            nombreDoc = item.HisDoc_Nombre;
                            Doc.Detalles = detalles;
                            stPnl = new StackPanel();
                            lbl_Nombre = new TextBlock();
                            lbl_Nombre.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            if (Doc.Nombre.Length >= 80)
                            {
                                ventamensaje = new VentanaMensaje();
                                mensaje = new MensajeBLL();
                                mensaje.tipomensajes(10,"");
                                ventamensaje.cargarmensaje(mensaje.titulos, Doc.Nombre);
                                ventamensaje.Show();
                            }
                            else
                            {
                                if (Doc.Tipo == Documentos.TipoDoc.RECHAZADO)
                                {
                                    ventamensaje = new VentanaMensaje();
                                    mensaje = new MensajeBLL();
                                    mensaje.tipomensajes(27,"");
                                    ventamensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                                    ventamensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                                    ventamensaje.cargarmensaje(mensaje.titulos, mensaje.mensajes);
                                    ventamensaje.Show();
                                }
                                else
                                {
                                    var lista = DocuBll.GetTamaniodocumentos();
                                    if (Doc.Peso.Contains("M"))
                                    {
                                        pesolimite = Convert.ToDouble(lista[1].Peso);
                                    }
                                    else
                                    {
                                        if (Doc.Peso.Contains("K"))
                                        {
                                            pesolimite = Convert.ToDouble(lista[0].Peso);
                                        }
                                    }
                                    String[] peso = Doc.Peso.Split('K', 'b', 'M');
                                    if (Convert.ToDouble(peso[0]) > pesolimite)
                                    {
                                        if (Doc.Peso.Contains("K"))
                                        {
                                            ventamensaje = new VentanaMensaje();
                                            mensaje = new MensajeBLL();
                                            mensaje.tipomensajes(11,"");
                                            ventamensaje.cargarmensaje((mensaje.titulos + pesolimite.ToString() + " KiloBytes"), Doc.Nombre);
                                            ventamensaje.Show();
                                        }
                                        if (Doc.Peso.Contains("M"))
                                        {
                                            ventamensaje = new VentanaMensaje();
                                            mensaje = new MensajeBLL();
                                            mensaje.tipomensajes(11,"");
                                            ventamensaje.cargarmensaje((mensaje.titulos + pesolimite.ToString() + mensaje.mensajes), Doc.Nombre);
                                            ventamensaje.Show();
                                        }
                                    }
                                    else
                                    {
                                        lbl_Nombre.Text = Doc.Nombre;
                                        lbl_Nombre.Width = 100;
                                        lbl_Nombre.TextWrapping = TextWrapping.Wrap;
                                        labelNombre.Content = Doc.Nombre;
                                        LabelTamanio.Content = Doc.Peso;
                                        Labeltipo.Content = Doc.Tipo.ToString();
                                        //AddHandler boton.Click, AddressOf eliminar_click;
                                        //boton.Click += new RoutedEventHandler(eliminar_click);
                                        if (Doc.Tipo == Documentos.TipoDoc.GIF | Doc.Tipo == Documentos.TipoDoc.JPEG |
                                             Doc.Tipo == Documentos.TipoDoc.JPG | Doc.Tipo == Documentos.TipoDoc.PNG)
                                        {
                                            // stPnl.Children.Add(boton);                               
                                            stPnl.Children.Add(Doc.ImagenTipo);
                                            stPnl.Children.Add(lbl_Nombre);
                                            //wp_Imagenes.Children.Clear();
                                            //wp_PDF.Children.Clear();
                                            //wp_Doc.Children.Clear();
                                            //wp_Excel.Children.Clear();
                                            wp_Imagenes.Children.Add(stPnl);
                                            DSSubAr.AddSubidaArchivosRow(Doc.Nombre, Doc.Tipo.ToString(), Doc.Peso, Doc.Path, TipoDoc, identificador);
                                            // DSSubAr.AddCR_SubidaArchivosRow(Doc.Nombre, Doc.Tipo.ToString(), Doc.Peso, Doc.Path, cbbTipodoc.SelectedValue, idcaso);
                                        }
                                        if (Doc.Tipo == Documentos.TipoDoc.DOC | Doc.Tipo == Documentos.TipoDoc.DOCX)
                                        {
                                            // stPnl.Children.Add(boton);
                                            stPnl.Children.Add(Doc.ImagenTipo);
                                            stPnl.Children.Add(lbl_Nombre);
                                            //wp_Imagenes.Children.Clear();
                                            //wp_PDF.Children.Clear();
                                            //wp_Doc.Children.Clear();
                                            //wp_Excel.Children.Clear();
                                            wp_Doc.Children.Add(stPnl);
                                            DSSubAr.AddSubidaArchivosRow(Doc.Nombre, Doc.Tipo.ToString(), Doc.Peso, Doc.Path, TipoDoc, identificador);
                                            //DSSubAr.AddCR_SubidaArchivosRow(Doc.Nombre, Doc.Tipo.ToString(), Doc.Peso, Doc.Path, cbbTipodoc.SelectedValue, idcaso);
                                        }
                                        if (Doc.Tipo == Documentos.TipoDoc.XLS | Doc.Tipo == Documentos.TipoDoc.XLSX)
                                        {
                                            //stPnl.Children.Add(boton);
                                            stPnl.Children.Add(Doc.ImagenTipo);
                                            stPnl.Children.Add(lbl_Nombre);
                                            //wp_Imagenes.Children.Clear();
                                            //wp_PDF.Children.Clear();
                                            //wp_Doc.Children.Clear();
                                            //wp_Excel.Children.Clear();
                                            wp_Excel.Children.Add(stPnl);
                                            DSSubAr.AddSubidaArchivosRow(Doc.Nombre, Doc.Tipo.ToString(), Doc.Peso, Doc.Path, TipoDoc, identificador);
                                            // DSSubAr.AddCR_SubidaArchivosRow(Doc.Nombre, Doc.Tipo.ToString(), Doc.Peso, Doc.Path, cbbTipodoc.SelectedValue, idcaso);
                                        }
                                        if (Doc.Tipo == Documentos.TipoDoc.PDF)
                                        {
                                            // stPnl.Children.Add(boton);
                                            stPnl.Children.Add(Doc.ImagenTipo);
                                            stPnl.Children.Add(lbl_Nombre);
                                            //wp_Imagenes.Children.Clear();
                                            //wp_PDF.Children.Clear();
                                            //wp_Doc.Children.Clear();
                                            //wp_Excel.Children.Clear();
                                            wp_PDF.Children.Add(stPnl);
                                            DSSubAr.AddSubidaArchivosRow(Doc.Nombre, Doc.Tipo.ToString(), Doc.Peso, Doc.Path, TipoDoc, identificador);
                                            //DSSubAr.AddCR_SubidaArchivosRow(Doc.Nombre, Doc.Tipo.ToString(), Doc.Peso, Doc.Path, cbbTipodoc.SelectedValue, idcaso);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            ventamensaje = new VentanaMensaje();
                            mensaje = new MensajeBLL();
                            mensaje.tipomensajes(28,"");
                            ventamensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                            ventamensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                            ventamensaje.cargarmensaje(mensaje.titulos, mensaje.mensajes);
                            ventamensaje.Show();
                            DocuBll.UpdateEStadoDocumentos(TipoDoc, identificador);
                            this.Close();
                        }
                    }
                }
            }
            else
            {
                ventamensaje = new VentanaMensaje();
                mensaje = new MensajeBLL();
                mensaje.tipomensajes(54,"");
                ventamensaje.lblTitulo.Foreground = new SolidColorBrush(Colors.Red);
                ventamensaje.lblmensaje.Foreground = new SolidColorBrush(Colors.Navy);
                ventamensaje.cargarmensaje(mensaje.titulos, mensaje.mensajes);
                ventamensaje.Show();
                this.Close();
            }
            
        }

       
    }
}
