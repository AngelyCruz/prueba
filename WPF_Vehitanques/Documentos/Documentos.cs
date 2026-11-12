using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF_Vehitanques.Documentos
{
    public enum TipoDoc
    {
        DOC,
        DOCX,
        XLS,
        XLSX,
        PDF,
        JPG,
        JPEG,
        PNG,
        GIF,
        RECHAZADO
    }


    public class Documentos
    {

        public string Nombre;
        public string Clase;
        public string Peso;
        public string Path;
        public TipoDoc Tipo;
        public string Ext;

        public System.Windows.Controls.Grid Detalles;
        private FileInfo at_Archivo;
        private Image at_ImagenTipo;

        public FileInfo File
        {
            get { return at_Archivo; }

        }
        public Image ImagenTipo
        {
            get { return at_ImagenTipo; }
            set
            {
                var b = new BitmapImage();
                string strPathIcono;
                strPathIcono = "";
                b.BeginInit();

                switch (Tipo)
                {
                    case TipoDoc.DOC:
                        //'New Uri("/imgaCargar.jpg", UriKind.Relative)
                        //strPathIcono = "/Iconos/word.png";
                        strPathIcono = "/Iconos/word.png";
                        
                        break;
                    case TipoDoc.DOCX:
                        strPathIcono = "/Iconos/word.png";
                        
                        break;
                    case TipoDoc.GIF:
                        strPathIcono = "/Iconos/GIF.png";
                        break;
                    case TipoDoc.JPEG:
                        strPathIcono = "/Iconos/jpeg.png";
                        break;
                    case TipoDoc.JPG:
                        strPathIcono = "/Iconos/jpg.png";
                        break;
                    case TipoDoc.PDF:
                        strPathIcono = "/Iconos/PDF.png";
                        break;
                    
                    case TipoDoc.PNG:
                        strPathIcono = "/Iconos/PNG.png";
                        break;
                    case TipoDoc.XLS:
                        strPathIcono = "/Iconos/Excel.png";
                        break;
                    case TipoDoc.XLSX:
                        strPathIcono = "/Iconos/Excel.png";
                        break;
                    default:
                        strPathIcono = "/Iconos/Alerta_Roja.png";
                        break;
                }
                b.UriSource = new Uri(strPathIcono, UriKind.Relative);
                b.EndInit();
                value.Source = b;
                at_ImagenTipo = value;

                // Create the image element.
                //Image simpleImage = new Image();
                //simpleImage.Width = 200;
                //simpleImage.Margin = new Thickness(5);

                //// Create source.
                //BitmapImage bi = new BitmapImage();
                //// BitmapImage.UriSource must be in a BeginInit/EndInit block.
                //bi.BeginInit();
                //bi.UriSource = new Uri(@"/sampleImages/cherries_larger.jpg", UriKind.RelativeOrAbsolute);
                //bi.EndInit();
                //// Set the image source.
                //simpleImage.Source = bi;
            }
        }

        private void getTamFile()
        {
            FileInfo fi = new FileInfo(Path);
            if (fi.Exists)
            {
                if ((fi.Length / 1024) > 1024)
                {
                    double aux = (fi.Length / 1024);
                    aux = aux / 1024;
                    aux = Math.Round(aux, 2);
                    Peso = aux.ToString() + "Mb";
                    //Peso = Math.Round(((fi.Length / 1024) / 1024), 2).ToString() + " Mb";
                }
                else
                {
                    double aux = (fi.Length / 1024);
                    aux = Math.Round(aux, 2);
                    Peso = aux.ToString() + "Kb";
                    //Peso = Math.Round((fi.Length / 1024), 2).ToString() + " Kb";
                }
            }
            else
            {
                Peso = String.Empty;
            }
        }
        private void GetTipo()
        {
            String[] sNombre = Nombre.Split('.');
            int i = sNombre.Length;
            Ext = sNombre[i - 1].ToUpper();
            switch (sNombre[i - 1].ToLower())
            {
                case "doc":
                    Tipo = TipoDoc.DOC;
                    break;
                case "docx":
                    Tipo = TipoDoc.DOCX;
                    break;
                case "xls":
                    Tipo = TipoDoc.XLS;
                    break;
                case "xlsx":
                    Tipo = TipoDoc.XLSX;
                    break;
                case "pdf":
                    Tipo = TipoDoc.PDF;
                    break;
                case "png":
                    Tipo = TipoDoc.PNG;
                    break;
                case "jpg":
                    Tipo = TipoDoc.JPG;
                    break;
                case "jpeg":
                    Tipo = TipoDoc.JPEG;
                    break;
                case "gif":
                    Tipo = TipoDoc.GIF;
                    break;
                default:
                    Tipo = TipoDoc.RECHAZADO;
                    break;
            }
        }
        private void GetImage()
        {
            ImagenTipo = new Image();
            ImagenTipo.Stretch = Stretch.Fill;
            ImagenTipo.Width = 100;
            ImagenTipo.Height = 100;
            ImagenTipo.Margin = new Thickness(15, 15, 15, 15);

            ImagenTipo.MouseLeftButtonDown += ImagenTipo_MouseLeftButtonDown;
            ImagenTipo.MouseEnter += ImagenTipo_MouseEnter;
            ImagenTipo.MouseLeave += ImagenTipo_MouseLeave;
            //AddHandler ImagenTipo.MouseRightButtonDown, AddressOf ImagenTipoMouseRightButtonDown

        }

        //private void ImagenTipo_MouseLeave(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void ImagenTipo_MouseEnter(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void ImagenTipo_MouseLeftButtonDown(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
        public Documentos(FileInfo Archivo)
        {
            at_Archivo = Archivo;
            Nombre = Archivo.Name;
            Path = Archivo.FullName;
            GetTipo();
            getTamFile();
            GetImage();
        }
        private void ImagenTipo_MouseLeftButtonDown(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = Path;
            Process.Start(psi);
        }
        private void ImagenTipo_MouseEnter(object sender, EventArgs e)
        {

            Detalles.DataContext = this;
        }
        private void ImagenTipo_MouseLeave(object sender, EventArgs e)
        {

            Detalles.DataContext = null;
        }
    }
}
