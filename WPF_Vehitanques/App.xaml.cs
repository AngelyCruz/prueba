using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_Vehitanques
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application

    {
        public App()
        {
            try
            {
                // Verifica si ya existe una instancia de la aplicación ejecutándose
                if (AplicacionEjecutandose())
                {
                    MessageBox.Show("USTED YA ESTÁ EJECUTANDO ESTA APLICACIÓN, Auditoría será notificada de este intento adicional: " + SecurityVariables.PStrUsuario.ToString() , "Servicio al Cliente", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Cancel);
                    Application.Current.Shutdown(-1); //Cerrar la aplicación
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        [STAThread]
         //<summary>
         //Application Entry Point.
         //</summary>
       //[System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public static void Main()
        {
          
                ////Mantenimiento.            
                //  WPF_Vehitanques.App.FuncionManteniemiento();
                /////
                string sIdUsuario = "";
                char[] myComma = { ',' };
                //Process.GetCurrentProcess().Kill();
                if (AppDomain.CurrentDomain.SetupInformation.ActivationArguments == null || AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData == null)
                {
                    Assembly assem = typeof(App).Assembly;
                    
                    //SecurityVariables.PStrUsuario = "DHERRERA";
                   // SecurityVariables.PStrUsuario = "LCASTILLO";
                    SecurityVariables.PStrUsuario = "agomezt";

                    //MessageBox.Show("NO tiene permisos para Ejecutar la Aplicacion. ACCESO DENEGADO  !", "Vehitanques", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Cancel);
                    //SecurityVariables.PStrUsuario = "";
                    //Process.GetCurrentProcess().Kill();
                    //System.Environment.Exit(0);
                }
                else
                {
                    string[] activationData = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;
                    foreach (string arg in activationData)
                    {
                        string[] myList = activationData[0].Split(myComma, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string oneItem in myList)
                        {
                            sIdUsuario = oneItem;
                        }
                    }
                    sIdUsuario = sIdUsuario.Replace("'", "");
                    SecurityVariables.PStrUsuario = sIdUsuario.ToUpper();                   
                }
            if(SecurityVariables.PStrUsuario!="" ||SecurityVariables.PStrUsuario!=null  )
            {
                    WPF_Vehitanques.App app;
                    app = new WPF_Vehitanques.App();
                    app.InitializeComponent();
                    app.Run();
            }       
        }
        private static void FuncionManteniemiento()
        {            
            MessageBox.Show("La Aplicación se encuentra en mantenimiento. ACCESO DENEGADO  !", "Vehitanques", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            SecurityVariables.PStrUsuario = "";
            Process.GetCurrentProcess().Kill();
            System.Environment.Exit(0);
        }
        private bool AplicacionEjecutandose()
        {
            Boolean Flag = false;
            //try
            //{
            //    System.Diagnostics.Process[] ProcessObj = null; ;
            //    String ModualName = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
            //    String ProcessName = System.IO.Path.GetFileNameWithoutExtension(ModualName);
            //    // Obtiene todas las intancias del proceso, ejecutándose en el computador local
            //    ProcessObj = System.Diagnostics.Process.GetProcessesByName(ProcessName);
            //    if (ProcessObj.Length > 1) // si ya hay una aplicación, devolvemos true
            //    {

            //        Flag = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            return Flag;
        }
        /*
        private void InstallUpdateSyncWithInfo()
        {
            UpdateCheckInfo info = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();

                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
                    return;
                }
                catch (InvalidDeploymentException ide)
                {
                    MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
                    return;
                }

                if (info.UpdateAvailable)
                {
                    Boolean doUpdate = true;

                    if (!info.IsUpdateRequired)
                    {
                        DialogResult dr = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButtons.OKCancel);
                        if (!(DialogResult.OK == dr))
                        {
                            doUpdate = false;
                        }
                    }
                    else
                    {
                        // Display a message that the app MUST reboot. Display the minimum required version.
                        MessageBox.Show("This application has detected a mandatory update from your current " +
                            "version to version " + info.MinimumRequiredVersion.ToString() +
                            ". The application will now install the update and restart.",
                            "Update Available", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }

                    if (doUpdate)
                    {
                        try
                        {
                            ad.Update();
                            MessageBox.Show("The application has been upgraded, and will now restart.");
                            Application.Restart();
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
                            return;
                        }
                    }
                }
            }
        }

        */
    }
}
