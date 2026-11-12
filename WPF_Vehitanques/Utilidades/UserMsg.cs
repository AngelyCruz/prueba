using System;
using System.Configuration;

namespace WPF_Vehitanques.Utilidades
{
    public class UserMsg
    {
        public String GetUsuario()
        {
            return GetRequiredAppSetting("ElibomUsername");
        }

        public string GetPassword()
        {
            return GetRequiredAppSetting("ElibomPassword");
        }

        private static string GetRequiredAppSetting(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ConfigurationErrorsException(
                    "The appSetting '" + key + "' is missing or empty.");
            }

            return value;
        }
    }
}
