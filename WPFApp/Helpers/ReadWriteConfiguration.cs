using NLog;
using System;
using System.Configuration;

namespace GetDescriptionImageApp.Helpers
{
    public class ReadWriteConfiguration
    {
        private readonly Logger Logger;

        public ReadWriteConfiguration()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

        #region Azure Description Configuration
        public string GetAzureLanguage()
        {
            string AzureLanguage = "en"; // valor por defecto, otros idiomas  en, ja, pt, zh, es

            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings != null)
            {
                AzureLanguage = appSettings["AzureLanguage"];
                Logger.Info($"ReadWriteConfiguration Info - GetAzureLanguage Action value recovered: '{AzureLanguage}'");
            }
            else
            {
                Logger.Error($"ReadWriteConfiguration ERROR - GetAzureLanguage Action appSettings is null return default value: '{AzureLanguage}'");
            }

            return AzureLanguage;
        }

        public int GetAzureMaxCandidate()
        {
            int AzureMaxCandidate = 1; // valor por defecto 1, max 3

            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings != null)
            {
                string maxCandidate = appSettings["AzureMaxCandidate"];
                int.TryParse(maxCandidate, out AzureMaxCandidate);
                Logger.Info($"ReadWriteConfiguration Info - GetAzureMaxCandidate Action value recovered: '{maxCandidate}'");
            }
            else
            {
                Logger.Error($"ReadWriteConfiguration ERROR - GetAzureMaxCandidate Action appSettings is null return default value: '{AzureMaxCandidate}'");
            }

            return AzureMaxCandidate;
        }

        public string GetAzureSubscriptionKey()
        {
            string AzureSubscriptionKey = "";

            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings != null)
            {
                AzureSubscriptionKey = appSettings["AzureSubscriptionKey"];
                Logger.Info($"ReadWriteConfiguration Info - GetAzureSubscriptionKey Action value recovered: '{AzureSubscriptionKey}'");
            }
            else
            {
                Logger.Error($"ReadWriteConfiguration ERROR - GetAzureSubscriptionKey Action appSettings is null return empty string");
            }

            return AzureSubscriptionKey;
        }

        public string GetAzureURL()
        {
            string AzureURL = "";

            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings != null)
            {
                AzureURL = appSettings["AzureURL"];
                Logger.Info($"ReadWriteConfiguration Info - GetAzureURL Action value recovered: '{AzureURL}'");
            }
            else
            {
                Logger.Error($"ReadWriteConfiguration ERROR - GetAzureURL Action appSettings is null return empty string");
            }

            return AzureURL;
        }

        public int GetMaxRequestAzure()
        {
            int maxRequestAzure = 20; // valor por defecto 20, número máximo peticiones por minito en la cuenta gratuita

            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings != null)
            {
                string maxRequestAzureString = appSettings["MaxRequestAzure"];
                int.TryParse(maxRequestAzureString, out maxRequestAzure);
                Logger.Info($"ReadWriteConfiguration Info - GetMaxRequestAzure Action value recovered: '{maxRequestAzureString}'");
            }
            else
            {
                Logger.Error($"ReadWriteConfiguration ERROR - GetMaxRequestAzure Action appSettings is null return default value: '{maxRequestAzure}'");
            }

            return maxRequestAzure;
        }


        public bool UpdateAzureLanguage(string valueAzureLanguage)
        {
            bool resultOK = true;

            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var appSettings = configFile.AppSettings.Settings;
                if (appSettings["AzureLanguage"] != null)
                {
                    appSettings["AzureLanguage"].Value = valueAzureLanguage;
                }
                else
                {
                    Logger.Info($"ReadWriteConfiguration Info - UpdateAzureLanguage Action no exists AzureLanguage key");
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (Exception exc)
            {
                resultOK = false;
                Logger.Error(exc, $"ReadWriteConfiguration ERROR - UpdateAzureLanguage Action");
            }

            return resultOK;
        }

        public bool UpdateAzureMaxCandidate(string valueAzureMaxCandidate)
        {
            bool resultOK = true;

            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var appSettings = configFile.AppSettings.Settings;
                if (appSettings["AzureMaxCandidate"] != null)
                {
                    appSettings["AzureMaxCandidate"].Value = valueAzureMaxCandidate;
                }
                else
                {
                    Logger.Info($"ReadWriteConfiguration Info - UpdateAzureMaxCandidate Action no exists AzureMaxCandidate key");
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (Exception exc)
            {
                resultOK = false;
                Logger.Error(exc, $"ReadWriteConfiguration ERROR - UpdateAzureMaxCandidate Action");
            }

            return resultOK;
        }

        public bool UpdateAzureSubscriptionKey(string valueAzureSubscriptionKey)
        {
            bool resultOK = true;

            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var appSettings = configFile.AppSettings.Settings;
                if (appSettings["AzureSubscriptionKey"] != null)
                {
                    appSettings["AzureSubscriptionKey"].Value = valueAzureSubscriptionKey;
                }
                else
                {
                    Logger.Info($"ReadWriteConfiguration Info - UpdateAzureSubscriptionKey Action no exists AzureSubscriptionKey key");
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (Exception exc)
            {
                resultOK = false;
                Logger.Error(exc, $"ReadWriteConfiguration ERROR - UpdateAzureSubscriptionKey Action");
            }

            return resultOK;
        }

        public bool UpdateAzureURL(string valueAzureURL)
        {
            bool resultOK = true;

            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var appSettings = configFile.AppSettings.Settings;
                if (appSettings["AzureURL"] != null)
                {
                    appSettings["AzureURL"].Value = valueAzureURL;
                }
                else
                {
                    Logger.Info($"ReadWriteConfiguration Info - UpdateAzureURL Action no exists AzureURL key");
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (Exception exc)
            {
                resultOK = false;
                Logger.Error(exc, $"ReadWriteConfiguration ERROR - UpdateAzureURL Action");
            }

            return resultOK;
        }

        public bool UpdateMaxRequestAzure(string valueMaxRequestAzure)
        {
            bool resultOK = true;

            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var appSettings = configFile.AppSettings.Settings;
                if (appSettings["MaxRequestAzure"] != null)
                {
                    appSettings["MaxRequestAzure"].Value = valueMaxRequestAzure;
                }
                else
                {
                    Logger.Info($"ReadWriteConfiguration Info - UpdateMaxRequestAzure Action no exists MaxRequestAzure key");
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (Exception exc)
            {
                resultOK = false;
                Logger.Error(exc, $"ReadWriteConfiguration ERROR - MaxRequestAzure Action");
            }

            return resultOK;
        }

        #endregion Azure Configuration


        #region Azure Translate Configurarion
        public string GetAzureTranslateSubscriptionKey()
        {
            string AzureTranslateSubscriptionKey = "";

            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings != null)
            {
                AzureTranslateSubscriptionKey = appSettings["AzureTranslateSubscriptionKey"];
                Logger.Info($"ReadWriteConfiguration Info - GetAzureTranslateSubscriptionKey Action value recovered: '{AzureTranslateSubscriptionKey}'");
            }
            else
            {
                Logger.Error($"ReadWriteConfiguration ERROR - GetAzureTranslateSubscriptionKey Action appSettings is null return empty string");
            }

            return AzureTranslateSubscriptionKey;
        }

        public string GetAzureTranslateRegion()
        {
            string AzureTranslateRegion = "";

            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings != null)
            {
                AzureTranslateRegion = appSettings["AzureTranslateRegion"];
                Logger.Info($"ReadWriteConfiguration Info - GetAzureTranslateRegion Action value recovered: '{AzureTranslateRegion}'");
            }
            else
            {
                Logger.Error($"ReadWriteConfiguration ERROR - GetAzureTranslateRegion Action appSettings is null return empty string");
            }

            return AzureTranslateRegion;
        }

        public string GetAzureGeneralTranslateURL()
        {
            string AzureGeneralTranslateURL = "";

            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings != null)
            {
                AzureGeneralTranslateURL = appSettings["AzureGeneralTranslateURL"];
                Logger.Info($"ReadWriteConfiguration Info - GetAzureGeneralTranslateURL Action value recovered: '{AzureGeneralTranslateURL}'");
            }
            else
            {
                Logger.Error($"ReadWriteConfiguration ERROR - GetAzureGeneralTranslateURL Action appSettings is null return empty string");
            }

            return AzureGeneralTranslateURL;
        }

        public string GetAzureTranslateURL()
        {
            string AzureTranslateURL = "";

            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings != null)
            {
                AzureTranslateURL = appSettings["AzureTranslateURL"];
                Logger.Info($"ReadWriteConfiguration Info - GetAzureTranslateURL Action value recovered: '{AzureTranslateURL}'");
            }
            else
            {
                Logger.Error($"ReadWriteConfiguration ERROR - GetAzureTranslateURL Action appSettings is null return empty string");
            }

            return AzureTranslateURL;
        }

        public bool GetUseAzureTranslate()
        {
            bool UseAzureTranslate = false;

            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings != null)
            {
                bool.TryParse(appSettings["UseAzureTranslate"], out UseAzureTranslate);
                Logger.Info($"ReadWriteConfiguration Info - GetUseAzureTranslate Action value recovered: '{UseAzureTranslate}'");
            }
            else
            {
                Logger.Error($"ReadWriteConfiguration ERROR - GetUseAzureTranslate Action appSettings is null return empty string");
            }

            return UseAzureTranslate;
        }

        public bool UpdateAzureTranslateURL(string azureTranslateURL)
        {
            bool resultOK = true;

            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var appSettings = configFile.AppSettings.Settings;
                if (appSettings["AzureTranslateURL"] != null)
                {
                    appSettings["AzureTranslateURL"].Value = azureTranslateURL;
                }
                else
                {
                    Logger.Info($"ReadWriteConfiguration Info - UpdateAzureTranslateURL Action no exists AzureTranslateURL key");
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (Exception exc)
            {
                resultOK = false;
                Logger.Error(exc, $"ReadWriteConfiguration ERROR - UpdateAzureTranslateURL Action");
            }

            return resultOK;
        }

        public bool UpdateAzureGeneralTranslateURL(string azureGeneralTranslateURL)
        {
            bool resultOK = true;

            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var appSettings = configFile.AppSettings.Settings;
                if (appSettings["AzureGeneralTranslateURL"] != null)
                {
                    appSettings["AzureGeneralTranslateURL"].Value = azureGeneralTranslateURL;
                }
                else
                {
                    Logger.Info($"ReadWriteConfiguration Info - UpdateAzureGeneralTranslateURL Action no exists AzureGeneralTranslateURL key");
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (Exception exc)
            {
                resultOK = false;
                Logger.Error(exc, $"ReadWriteConfiguration ERROR - UpdateAzureGeneralTranslateURL Action");
            }

            return resultOK;
        }

        public bool UpdateAzureTranslateRegion(string azureTranslateRegion)
        {
            bool resultOK = true;

            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var appSettings = configFile.AppSettings.Settings;
                if (appSettings["AzureTranslateRegion"] != null)
                {
                    appSettings["AzureTranslateRegion"].Value = azureTranslateRegion;
                }
                else
                {
                    Logger.Info($"ReadWriteConfiguration Info - UpdateAzureTranslateRegion Action no exists AzureTranslateRegion key");
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (Exception exc)
            {
                resultOK = false;
                Logger.Error(exc, $"ReadWriteConfiguration ERROR - UpdateAzureTranslateRegion Action");
            }

            return resultOK;
        }

        public bool UpdateAzureTranslateSubscriptionKey(string azureTranslateSubscriptionKey)
        {
            bool resultOK = true;

            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var appSettings = configFile.AppSettings.Settings;
                if (appSettings["AzureTranslateSubscriptionKey"] != null)
                {
                    appSettings["AzureTranslateSubscriptionKey"].Value = azureTranslateSubscriptionKey;
                }
                else
                {
                    Logger.Info($"ReadWriteConfiguration Info - UpdateAzureTranslateSubscriptionKey Action no exists AzureTranslateSubscriptionKey key");
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (Exception exc)
            {
                resultOK = false;
                Logger.Error(exc, $"ReadWriteConfiguration ERROR - UpdateAzureTranslateSubscriptionKey Action");
            }

            return resultOK;
        }

        public bool UpdateUseAzureTranslate(string useAzureTranslate)
        {
            bool resultOK = true;

            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var appSettings = configFile.AppSettings.Settings;
                if (appSettings["UseAzureTranslate"] != null)
                {
                    appSettings["UseAzureTranslate"].Value = useAzureTranslate;
                }
                else
                {
                    Logger.Info($"ReadWriteConfiguration Info - UpdateUseAzureTranslate Action no exists UseAzureTranslate key");
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (Exception exc)
            {
                resultOK = false;
                Logger.Error(exc, $"ReadWriteConfiguration ERROR - UpdateUseAzureTranslate Action");
            }

            return resultOK;
        }
        #endregion  Azure Translate Configurarion
    }
}
