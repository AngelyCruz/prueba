using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using WPF_Vehitanques.Modelo;


namespace WPF_Vehitanques.Utilidades
{
   public  class Email
    {
       

        private static String AgregarTitulo(string titulo)
        {
            string strRow = "<th align='center' style='border:1px solid black;'>";
            strRow = strRow + "<b>" + titulo + "</b>";

            return strRow + "</th>";
        }
        private static String AgregarRow(string rowstr)
        {
            string strRow = "<td style='border:1px solid black;'>";
            strRow = strRow + rowstr;

            return strRow + "</td>";

        }
        private static String AgregarTabla(string _NitCuenta, string _IdBeneficiario, string _IdConductor, string _placa,string usuario)
       {
           string strRow = "<table> <tr>";
           string strcolum = "";
           //for (int i = 0; i < 3; i++)
           //{
           //    switch (i)
           //    {
           //        case 0:
           strcolum = strcolum + AgregarTitulo("Nit Cuenta");
           //    break;
           //case 1:
           strcolum = strcolum + AgregarTitulo("Id Beneficiario");
           //    break;
           //case 2:
           strcolum = strcolum + AgregarTitulo("CC Conductor");
           strcolum = strcolum + AgregarTitulo("Placa");
           strcolum = strcolum + AgregarTitulo("Usuario");
           // break;
           //    }
           //}
           strRow = strRow + strcolum + "</tr>" + "<tr>";
           //  strcolum = "";
           string row1 = "";
           row1 = row1 + AgregarRow(_NitCuenta);
           row1 = row1 + AgregarRow(_IdBeneficiario );
           row1 = row1 + AgregarRow(_IdConductor );
           row1 = row1 + AgregarRow(_placa );
           row1 = row1 + AgregarRow(usuario );
           
           strRow = strRow + row1 + "</tr> </table>";
           return strRow;

       }

       public static void EnviarEmail( string asunto, string detalleMsg, IList<string> listaDestinatarios,string _NitCuenta,string _IdBeneficiario,string _IdConductor ,string _placa ,string _usuario )
       {
           Modelo.ComercialPEntities contextVehit = new Modelo.ComercialPEntities();
           CRM_FechaSistemaBD_Result tblFechaSistema = contextVehit.CRM_FechaSistemaBD().FirstOrDefault();

           string TablaMsg = AgregarTabla(_NitCuenta, _IdBeneficiario, _IdConductor, _placa,_usuario );
           TablaMsg = TablaMsg + "<h2>  Se realizó cambio de conductor y el nit de cuenta no coincide, de acuerdo a la información de la tabla incluida en el mensaje</h2>";
          
           System.Net.Mail.MailMessage miCorreo = new System.Net.Mail.MailMessage();
           System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
           string emailPara;
           string emailDe=tblFechaSistema.CTA_CORREO_AUTOMATICO;
           //string token = "";
           //  string[] lista = null;
           //string emailCC = Session("EmailConCopia");
           //string emailDefault = Session("EmailDefault");
           string emailCredUser = tblFechaSistema.CTA_CORREO_AUTOMATICO;
           string emailCredPsw = tblFechaSistema.CCORREO_AUTOMATICO;
           //if (!string.IsNullOrEmpty(ConfigurationSettings.AppSettings["Password"]))
           //{
           //    emailCredPsw = ConfigurationSettings.AppSettings["Password"];
           //} 
            
            
           //string emailCredPsw = "coltanques";         
           if (listaDestinatarios.Count == 0)
           {
               //listaDestinatarios.Clear();
               listaDestinatarios.Add("informatica@coltanques.com.co");
           }
           
     //      AlternateView plainView =
     //AlternateView.CreateAlternateViewFromString(TablaMsg,
     //                        Encoding.UTF8,
     //                        MediaTypeNames.Text.Plain);

           string MensajeHTML = "<span style='color: Black'>" + TablaMsg + "</span><br /><img src='cid:imagen' />";

           AlternateView htmlView =
    AlternateView.CreateAlternateViewFromString(MensajeHTML,
                            Encoding.UTF8,
                            MediaTypeNames.Text.Html);

           // Creamos el recurso a incrustar. Observad
           // que el ID que le asignamos (arbitrario) está
           // referenciado desde el código HTML como origen
           // de la imagen (resaltado en amarillo)...

           LinkedResource img =
               new LinkedResource(@"\\141.0.0.193\Despliegues\Vehitanques\color.png",
                                   MediaTypeNames.Image.Jpeg);
           img.ContentId = "imagen";

           // Lo incrustamos en la vista HTML...

           htmlView.LinkedResources.Add(img);

           // Por último, vinculamos ambas vistas al mensaje...

          // miCorreo.AlternateViews.Add(plainView);
           miCorreo.AlternateViews.Add(htmlView);
           //miCorreo.Bcc.Add("edwin.gomez@coltanques.com.co,esperanza.becerra@coltanques.com.co");
           //miCorreo.Bcc.Add("edwin.gomez@coltanques.com.co");
           miCorreo.IsBodyHtml = true;
           miCorreo.Priority = System.Net.Mail.MailPriority.High;
           //Prioridad 
           miCorreo.From = new MailAddress(emailDe);
           //Mail desde donde se envía 
           miCorreo.Subject = asunto.Trim();          

           miCorreo.Body = MensajeHTML;
        
           foreach (var correo in listaDestinatarios)
           {
               miCorreo.To.Add(correo);
           }
           try
           {
               smtp.Credentials = new System.Net.NetworkCredential(emailCredUser, emailCredPsw);
               smtp.Host = tblFechaSistema.HOST_AUTOMATICO;
               smtp.Port = Convert.ToInt32(tblFechaSistema.PORT_AUTOMATICO);
               smtp.EnableSsl = Convert.ToBoolean(tblFechaSistema.ENABLESSL_AUTOMATICO);
           }
           catch
           {
               throw;
           }
          
           //smtp.EnableSsl = True;
           try
           {
               smtp.Send(miCorreo);
               // WriteErrorLog("Email Enviado ha:" + emailPara + "a las: " + DateTime.Now.ToString());
           }
           catch (Exception ex)
           {
             //  WriteErrorLog(ex.InnerException.Message);
               throw;
           }
       }

    }
}
