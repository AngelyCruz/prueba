using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace WPF_Vehitanques.Presentacion.Controles
{
    /// <summary>
    /// Lógica de interacción para ctrSoatMasivoPropios.xaml
    /// </summary>
    public partial class ctrSoatMasivoPropios : UserControl
    {
        #region Clases Formulario

        public class SoatPlaca
        {
            public string NumPoliza { get; set; }
            public DateTime Fechainicio { get; set; }
            public DateTime FechaFin { get; set; }
            public string Placa { get; set; }
            public string IdSoatAseguradora { get; set; }
            public int IdTipoAfiliacion { get; set; }
            public int IdCodigoDoc { get; set; }
        } 
        #endregion


        #region Declaraciones

        private string RutaDocu;
        OleDbConnection Conn;
        OleDbCommand Cmd;
        Documentos.Documentos Doc;

        MensajeBLL Mensabll = new MensajeBLL();
        AgenciaBLL agenciabll = new AgenciaBLL();
        VehiculoBLL vehiculobll = new VehiculoBLL();
        AseguradoraBLL AseBll = new AseguradoraBLL();
        DocumentosBLL DocuBll = new DocumentosBLL();
        PolizaRCBLL polizarcbll = new PolizaRCBLL();
        PolizaBLL polizabll = new PolizaBLL();


        DSDocumentos.SubidaArchivosDataTable DSSubAr = new DSDocumentos.SubidaArchivosDataTable();        

        Numeric numeric = new Numeric();

        MainWindow main;
        VentanaMensaje Venmsj = new VentanaMensaje();

        private int TipoDoc = 6;
        private string nombredirectorio, nombredirBD; 

        #endregion
        #region Load

        public ctrSoatMasivoPropios()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarPrivilegios();
            if (IsLoaded)
            {
                validarVersion();
                CargarAseguradoras();
                GetRutaDocumentos();
                cargartipoDocumentos();
            }
        }

       
        #endregion

        #region Métodos Seguridad Formulario
        public void RecibirPadre(MainWindow mainWindow)
        {
            main = mainWindow;
        }
        private void validarVersion()
        {
            main.ValidarVersion();
        }
        private void ValidarPrivilegios()
        {
            var lista = SecurityVariables.PPerfilUsuario;
            var listaprivi = (from rmenu in lista
                              where rmenu.idMenuPadre == 443 & rmenu.alterno == 1
                              & rmenu.Autorizar == 1
                              select rmenu).ToList();
            foreach (var item in listaprivi)
            {
                switch (item.nombreMenu)
                {


                    default:
                        break;

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
            Venmsj.Show();
        }
        #endregion

        #region Métodos
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
        private void cargartipoDocumentos()
        {
            var lista = DocuBll.GetDocumentosTipoPoliza();
            cbbtipodoc.ItemsSource = lista;
            cbbtipodoc.SelectedValuePath = "CodigoDoc";
            cbbtipodoc.DisplayMemberPath = "Descripcion";
            cbbtipodoc.SelectedIndex = 0;
        }
        private void CargarArchivosExcel()
        {
            //if (txtprefijo.Text.Trim()=="")
            //{
            //    MessageBox.Show("debe digitar el prefijo del soat, un ejemplo es AT1318");
            //    return;
            //}
            if (cbbciasoat.SelectedIndex >= 0)
            {
                var asegurado = (Aseguradoras)cbbciasoat.SelectedItem;
                SetProgressBar(0);
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Multiselect = true;
                dlg.DefaultExt = ".xlsx"; // Extensión de archivos por defaul
                dlg.Filter = "Excel Files|*.xlsx";
                ///dlg.Filter = "Todos(*.*)|*.*|Imagenes|*.jpg;*.gif;*.png;*.jpeg|Word|*.Doc;*.Docx|PDF|*.pdf;*.bmp|Excel|*.xlsx;*.xls"; // Filtros permitidos
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {                   
                    string numaseguradora = asegurado.ID.Trim();
                    foreach (string stringarchivo in dlg.FileNames)
                    {
                        FileInfo pFile = new FileInfo(stringarchivo);
                        string ExcelFilePath = pFile.FullName;
                        DataView tabla = LeerExcel(ExcelFilePath, pFile.Name);
                        int cantidad = tabla.Count;
                        int  fraccion = 0;
                        var rowpoliza = (Vehit_ConsultaDocumentosTipoPoliza_Result)cbbtipodoc.SelectedItem;
                        if (cantidad > 100)
                        {
                            fraccion = cantidad / 100;
                        }
                        else
                        {
                            fraccion = ( 100 / cantidad);
                            if (fraccion == 0)
                            {
                                fraccion = 1;
                            }
                        }
                        int  cantidadregistros = 0;
                        for (int i = 0; i < tabla.Count; i++)
                        {
                            var iD = tabla[i][0];
                            if (iD.ToString().Trim() != "")
                            {
                                var numpoliza = tabla[i][1];
                                var fechaini = tabla[i][2];
                                var fechafin = tabla[i][3];
                                var placa = tabla[i][8];
                                                             
                                switch (rowpoliza.CodigoDoc)
                                {
                                    case 6:
                                        ///soat
                                        var numSoat = txtprefijo.Text.Trim() + numpoliza;
                                        var resultado = vehiculobll.UpdateSoatXplaca(numSoat.ToString(), numaseguradora, placa.ToString(), Convert.ToDateTime(fechafin), main.txtusuario.Text.Trim());
                                        break;
                                    case 11:
                                        ///Póliza RC
                                        polizarcbll.GuardarDAtosPolizaRC(Convert.ToString(numpoliza),numaseguradora,  Convert.ToDateTime(fechafin), Convert.ToString(placa),  main.txtusuario.Text.Trim());
                                        break;
                                    case 17:
                                        ///Póliza de Hidrocarburos
                                        /// 
                                        string  Remolque=""  ;
                                        try
                                        {
                                            Remolque = Convert.ToString(tabla[i][9]);
                                        }
                                        catch
                                        {

                                        }
                                        polizabll.GuardarDAtosPolizapropios(Convert.ToString(numpoliza), Convert.ToDateTime(fechaini), Convert.ToDateTime(fechafin), Convert.ToString(placa), numaseguradora, main.txtusuario.Text.Trim());
                                        if (Remolque == null || Remolque == "")
                                        {
                                            
                                        }
                                        else
                                        {
                                            polizabll.GuardarDAtosPolizapropios(Convert.ToString(numpoliza), Convert.ToDateTime(fechaini), Convert.ToDateTime(fechafin), Convert.ToString(Remolque), numaseguradora, main.txtusuario.Text.Trim());
                                        }  
                                        break;
                                }                                
                            }
                            cantidadregistros = cantidadregistros + fraccion;
                            SetProgressBar(cantidadregistros);
                        }                      
                        SetProgressBar(100);
                        MessageBox.Show(tabla.Count.ToString() + " --placas  actualizadas en su póliza.");
                        break;
                    }

                }
            }
        }
        public void crearNombreArchivo()
        {
            var lista = DocuBll.GetTipoDocumentos(TipoDoc);
            if (lista.Count > 0)
            {
                nombredirectorio = lista[0].Descripcion;
            }
        }

        protected void SetProgressBar(int   valor)
        {
            try
            {
                if (valor == 0)
                {
                    this.Dispatcher.BeginInvoke(new Action(delegate { this._SetProgressBar.Value = valor; }));
                }
                else
                {
                    this.Dispatcher.BeginInvoke(new Action(delegate { this._SetProgressBar.Value += valor; }));
                }
            }
            catch
            {
            }
        }
        private void CargarArchivos()
        {
            SetProgressBar(0);  
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.FileName = "@";
            dlg.ShowReadOnly = true;
            //dlg.CheckPathExists = true;
            //dlg.ShowReadOnly = false;
            //dlg.ReadOnlyChecked = true;
            dlg.CheckFileExists = false;
            //dlg.ValidateNames = false;

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                var rowtipodoc = (Vehit_ConsultaDocumentosTipoPoliza_Result)cbbtipodoc.SelectedItem;
                TipoDoc = rowtipodoc.CodigoDoc;
                crearNombreArchivo();
                string ruta = dlg.FileName;
                char delimiter = '@';
                String[] substrings = ruta.Split(delimiter);
                ruta = substrings[0];
                DirectoryInfo directory = new DirectoryInfo(ruta);
                try
                {
                    ///dlg.Filter = "Todos(*.*)|*.*|Imagenes|*.jpg;*.gif;*.png;*.jpeg|Word|*.Doc;*.Docx|PDF|*.pdf;*.bmp|Excel|*.xlsx;*.xls"; // Filtros permitidos

                    //foreach (var fi in directory.GetFiles())
                    //{
                    //    DirectoryInfo[] directories = directory.GetDirectories();
                    //}
                    DirectoryInfo[] directories1 = directory.GetDirectories();
                    FileInfo[] files = directory.GetFiles("*.*");
                    int cantidad = files.Count();
                    int fraccion = 0;
                    //if (cantidad > 100)
                    //{
                    //    fraccion = cantidad / 100;
                    //}else 
                    //{
                        fraccion = 100/cantidad ;
                    //}
                        int cantidadregistros = 0;
                      
                    foreach (var _file in files)
                    {
                        var nme = _file.Name;
                        FileInfo pFile = new FileInfo(ruta + "\\" + nme);
                        char delimiter2 = '.';
                        String[] placas = nme.Split(delimiter2);
                        if (placas[0].Length == 6)
                        {
                            Doc = new Documentos.Documentos(pFile);
                            DSSubAr.AddSubidaArchivosRow(Doc.Nombre, Doc.Tipo.ToString(), Doc.Peso, Doc.Path, rowtipodoc.CodigoDoc , placas[0]);
                        }
                        cantidadregistros = cantidadregistros + fraccion;
                        SetProgressBar(cantidadregistros);
                    }
                    SetProgressBar(25);
                    //var lastFolder = Directory.GetDirectories(".").OrderBy(folder => folder, new FolderComparer()).Last();
                    //DirectoryInfo[] directories = directory.GetDirectories();
                    foreach (DSDocumentos.SubidaArchivosRow row in DSSubAr)
                    {
                        DocuBll.UpdateEStadoDocumentos(row.TipoDocumento, row.Id);

                    }
                    SetProgressBar(50);
                    cantidadregistros = 0;
                    foreach (DSDocumentos.SubidaArchivosRow row in DSSubAr)
                    {
                        subirarchivo(row);
                       // cantidadregistros = cantidadregistros + fraccion;
                        //SetProgressBar(cantidadregistros);
                        // break;
                    }
                    SetProgressBar(100);
                }
                catch (Exception ex)
                {

                }
            }
        }
        public void GetRutaDocumentos()
        {
            var lista = DocuBll.GetRutaDocumentos(1);
            if (lista != null)
            {
                RutaDocu = lista[0].Ruta_Direccion;
            }
        }
        private void CargarAseguradoras()
        {
            var lista = AseBll.GetASeguradora();
            cbbciasoat.ItemsSource = lista;
            cbbciasoat.SelectedValuePath = "ID";
            cbbciasoat.DisplayMemberPath = "Nombre";
            cbbciasoat.SelectedIndex = 0;


        }
        private DataView LeerExcel(string ExcelFilePath, string name)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            //    m_Excel.ActiveWorkbook.Close();
            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName("Excel");
            foreach (System.Diagnostics.Process p in process)
            {
                if (!string.IsNullOrEmpty(p.ProcessName))
                {
                    try
                    {
                        if (p.MainWindowTitle == name || p.MainWindowTitle == "")
                        {
                            p.Kill();
                        }
                    }
                    catch { }
                }
            }
            excelApp = new Microsoft.Office.Interop.Excel.Application();
            //Static File From Base Path...........
            //Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "TestExcel.xlsx", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            //Dynamic File Using Uploader...........
            Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(ExcelFilePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel.Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets.get_Item(1); ;
            Microsoft.Office.Interop.Excel.Range excelRange = excelSheet.UsedRange;

            string strCellData = "";

            decimal douCellData;
            int rowCnt = 0;
            int colCnt = 0;

            DataTable dt = new DataTable();
            for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
            {
                string strColumn = "";
                strColumn = (string)(excelRange.Cells[1, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                dt.Columns.Add(strColumn, typeof(string));
            }

            int cantidad = excelRange.Rows.Count;
            //double  fraccion = 0;
            //if (cantidad > 100)
            //{
            //    fraccion = cantidad / 100;
            //}else 
            //{
            float porcent = 100;
          var  fraccion =( porcent / cantidad);
            //}
            float   cantidadregistros = 0;
            for (rowCnt = 2; rowCnt <= excelRange.Rows.Count; rowCnt++)
            {
                string strData = "";
                for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                {
                    try
                    {
                        switch (colCnt)
                        {
                            case 4:
                            case 3:
                                string sDate = Convert.ToString((excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2.ToString());
                                double date = double.Parse(sDate);
                                //var   dateTime = DateTime.FromOADate(date).ToString("MMMM dd, yyyy");
                                var dateTime = DateTime.FromOADate(date).ToString("yyyy/MM/dd");
                                strCellData = dateTime.ToString();
                                strData += strCellData + "|";
                                break;
                            case 5:
                            case 6:
                            case 7:
                                douCellData = Convert.ToDecimal((excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2);
                                strData += douCellData.ToString() + "|";
                                break;

                            default:
                                strCellData = Convert.ToString((excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2);
                                strData += strCellData + "|";
                                break;
                        }
                        cantidadregistros = cantidadregistros + fraccion;
                        //SetProgressBar(cantidadregistros);
                    }
                    catch (Exception ex)
                    {
                        douCellData = Convert.ToDecimal((excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2);
                        strData += douCellData.ToString() + "|";
                    }
                }
                strData = strData.Remove(strData.Length - 1, 1);
                dt.Rows.Add(strData.Split('|'));
            }
            excelBook.Close(true, null, null);
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            //    m_Excel.ActiveWorkbook.Close();
            process = System.Diagnostics.Process.GetProcessesByName("Excel");
            foreach (System.Diagnostics.Process p in process)
            {
                if (!string.IsNullOrEmpty(p.ProcessName))
                {
                    try
                    {
                        if (p.MainWindowTitle == name || p.MainWindowTitle == "")
                        {
                            p.Kill();
                        }
                    }
                    catch { }
                }
            }
            return dt.DefaultView;

        }
        private void subirarchivo(DSDocumentos.SubidaArchivosRow row)
        {
            try
            {
             
                String RutaDoc = row.Id + "\\" + nombredirectorio.ToString();
                string uploadPath = RutaDocu + "\\" + RutaDoc;
                DirectoryInfo dir = new DirectoryInfo(uploadPath);
                nombredirBD = uploadPath;
                string nombreDoc = "";
                if (!dir.Exists)
                {
                    dir.Create();
                }
                try
                {

                    nombreDoc = row.Nombre;
                    GeneradorPassword Generator = new GeneradorPassword(4, 50, 0, 50);
                    //GeneradorPassword Generator = new GeneradorPassword(10);
                    string nuevaclave = Generator.GetNewPassword();
                    string[] aux = nombreDoc.Split('.');
                    nombreDoc = aux[0] + nuevaclave + "." + aux[1];
                    try
                    {
                        System.IO.File.Copy(row.Ubicacion, uploadPath + @"\" + nombreDoc, true);
                        DocuBll.GuardarDocumento(nombreDoc, row.Tipo, row.Peso, row.TipoDocumento, uploadPath, row.Id, "", 1, main.txtusuario.Text.Trim());
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
        private void btn_Examinar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result1 = MessageBox.Show("La Aseguradora, Y el Tipo de Póliza  que selecciono son los correctos?.. Desea Actualizar sus datos?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result1 == MessageBoxResult.Yes)
            {
                MessageBox.Show("El proceso puede tardar unos minutos, por favor tenga paciencia.");
                CargarArchivosExcel();
           }
        }
        private void btn_archivossoat_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result1 = MessageBox.Show("La Aseguradora, Y el Tipo de Póliza  que selecciono son los correctos?.. Desea Actualizar sus datos?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result1 == MessageBoxResult.Yes)
            {
                MessageBox.Show("El proceso puede tardar unos minutos, por favor tenga paciencia.");
                CargarArchivos();
            }
            MessageBox.Show("Documentos Subidos de manera Exitosa.");   
        }
        #endregion
        
    }
}
