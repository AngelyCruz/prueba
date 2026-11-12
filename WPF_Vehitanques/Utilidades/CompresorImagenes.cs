using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;


public class CompresorImagenes
{
    public void Compresor(string strFileTmp, string strFile, int intWidth, int intHeigth, int intTamKb)
    {
        Bitmap SourceImage;
        SourceImage = new Bitmap(strFileTmp);
        // Generar un bitmap para el resultado
        Bitmap SourceTam = new Bitmap(SourceImage.Width, SourceImage.Height);
        // Generar un objeto Gráfico para el Bitmap resultante
        bool NeedsHorizontalCrop = true;
        bool NeedsVerticalCrop = false;

        //Determina si la imagen es Landscape o Portrait
        if (SourceTam.Height > SourceTam.Width)
        {
            NeedsHorizontalCrop = false;
            NeedsVerticalCrop = true;
        }

        //Determina si la imagen excede el ancho deseado
        if (SourceTam.Width < intWidth)
        {
            NeedsHorizontalCrop = false;
            if (SourceTam.Height > intHeigth)
            {
                NeedsVerticalCrop = true;
            }
        }

        //Calcula el Factor de Ajuste
        float scale_factor = 1;
        if (SourceTam.Width > 0)
        {
            if (NeedsHorizontalCrop == true)
            {
                // Obtiene el Factor de Ajuste
                scale_factor = intWidth / SourceTam.Width;
            }
        }

        if (SourceTam.Height > 0)
        {
            if (NeedsVerticalCrop == true)
            {
                // Obtiene el Factor de Ajuste
                scale_factor = intHeigth / SourceTam.Height;
            }
        }

        // Generar un bitmap tmp para el resultado. Ajuste Proporcional
        Bitmap DestTmpImage = new Bitmap(Convert.ToInt32(SourceTam.Width * scale_factor), Convert.ToInt32(SourceTam.Height * scale_factor));


        System.Drawing.Image oImage;
        System.Drawing.Image oNewImage;

        oImage = Image.FromFile(strFileTmp);
        Bitmap bmp = new Bitmap(oImage);
        oImage.Dispose();

        oNewImage = bmp.GetThumbnailImage(Convert.ToInt32(SourceTam.Width * scale_factor), Convert.ToInt32(SourceTam.Height * scale_factor), null, IntPtr.Zero);

        try
        {
            SaveJPGWithCompressionSetting(oNewImage, strFile, intTamKb);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            DestTmpImage.Dispose();
            SourceImage.Dispose();
            SourceTam.Dispose();
        }
    }
    private void SaveJPGWithCompressionSetting(System.Drawing.Image img, string strFileName, int intTamKb)
    {
        bool booComprimir = true;
        long lCompression = 100;
        int i = 0;
        while (booComprimir & i < 20)
        {
            EncoderParameters eps = new EncoderParameters(1);
            eps.Param[0] = new EncoderParameter(Encoder.Quality, lCompression);
            ImageCodecInfo ici = GetEncoderInfo("image/jpeg");
            try
            {

                img.Save(strFileName, ici, eps);
                FileInfo myFile = new FileInfo(strFileName);
                if (myFile.Length / 1024 <= intTamKb)
                {
                    booComprimir = false;
                }
            }
            catch (Exception ex)
            {
                string strMen = ex.Message;
            }
            finally
            {
                i += 1;
                lCompression -= 5;
            }
        }
        //if (booComprimir == true)
        //{
        //  int  intVal = 100 / 0;
        //}
    }
    public static ImageCodecInfo GetEncoderInfo(string mimeType)
    {
        int j;
        ImageCodecInfo[] encoders;
        encoders = ImageCodecInfo.GetImageEncoders();
        for (j = 0; j <= encoders.Length; j++)
        {
            if (encoders[j].MimeType == mimeType)
            {
                return encoders[j];
            }
        }
        return null;
    }
}
