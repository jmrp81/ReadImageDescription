using GetDescriptionImageApp.Helpers;
using NLog;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GetDescriptionImageApp.Presentation.Controls
{
    public partial class ConfigurationControl : UserControl, INotifyPropertyChanged
    {
        private readonly Logger Logger;
        private readonly ReadWriteConfiguration readWriteConfiguration;

        private string language = "";

        #region azure description properties
        public static readonly DependencyProperty URLAzureProperty =
            DependencyProperty.Register("URLAzure", typeof(string), typeof(ConfigurationControl), new FrameworkPropertyMetadata(null));

        public string URLAzure
        {
            get
            {
                return this.GetValue(URLAzureProperty) as string;
            }
            set
            {
                this.SetValue(URLAzureProperty, value);
                OnPropertyChanged("URLAzure");
            }
        }

        public static readonly DependencyProperty AzurekeyProperty =
            DependencyProperty.Register("Azurekey", typeof(string), typeof(ConfigurationControl), new FrameworkPropertyMetadata(null));

        public string Azurekey
        {
            get
            {
                return this.GetValue(AzurekeyProperty) as string;
            }
            set
            {
                this.SetValue(AzurekeyProperty, value);
                OnPropertyChanged("Azurekey");
            }
        }

        public static readonly DependencyProperty maxCandidateNumProperty =
            DependencyProperty.Register("MaxCandidateNum", typeof(string), typeof(ConfigurationControl), new FrameworkPropertyMetadata(null));

        public string MaxCandidateNum
        {
            get
            {
                return this.GetValue(maxCandidateNumProperty) as string;
            }
            set
            {
                this.SetValue(maxCandidateNumProperty, value);
                OnPropertyChanged("MaxCandidateNum");
            }
        }

        public static readonly DependencyProperty isEnglishProperty =
            DependencyProperty.Register("IsEnglish", typeof(bool), typeof(ConfigurationControl), new FrameworkPropertyMetadata(null));

        public bool IsEnglish
        {
            get
            {
                return (bool)this.GetValue(isEnglishProperty);
            }
            set
            {
                this.SetValue(isEnglishProperty, value);
                OnPropertyChanged("IsEnglish");
            }
        }

        public static readonly DependencyProperty isSpanishProperty =
          DependencyProperty.Register("IsSpanish", typeof(bool), typeof(ConfigurationControl), new FrameworkPropertyMetadata(null));

        public bool IsSpanish
        {
            get
            {
                return (bool)this.GetValue(isSpanishProperty);
            }
            set
            {
                this.SetValue(isSpanishProperty, value);
                OnPropertyChanged("IsSpanish");
                this.radioTranslateSpanish.IsEnabled = value;
            }
        }

        public static readonly DependencyProperty requestMaxAzureProperty =
           DependencyProperty.Register("RequestMaxAzure", typeof(string), typeof(ConfigurationControl), new FrameworkPropertyMetadata(null));

        public string RequestMaxAzure
        {
            get
            {
                return this.GetValue(requestMaxAzureProperty) as string;
            }
            set
            {
                this.SetValue(requestMaxAzureProperty, value);
                OnPropertyChanged("RequestMaxAzure");
            }
        }
        #endregion azure description properties

        #region azure translate properties
        public static readonly DependencyProperty IsSpanishWithTransalationProperty =
            DependencyProperty.Register("IsSpanishWithTransalation", typeof(bool), typeof(ConfigurationControl), new FrameworkPropertyMetadata(null));

        public bool IsSpanishWithTransalation
        {
            get
            {
                return (bool)this.GetValue(IsSpanishWithTransalationProperty);
            }
            set
            {
                this.SetValue(IsSpanishWithTransalationProperty, value);
                OnPropertyChanged("IsSpanishWithTransalation");
            }
        }

        public static readonly DependencyProperty RequestTranslateSubscriptionKeyProperty =
            DependencyProperty.Register("RequestTranslateSubscriptionKey", typeof(string), typeof(ConfigurationControl), new FrameworkPropertyMetadata(null));

        public string RequestTranslateSubscriptionKey
        {
            get
            {
                return this.GetValue(RequestTranslateSubscriptionKeyProperty) as string;
            }
            set
            {
                this.SetValue(RequestTranslateSubscriptionKeyProperty, value);
                OnPropertyChanged("RequestTranslateSubscriptionKey");
            }
        }

        public static readonly DependencyProperty RequestGeneralTranslateURLProperty =
            DependencyProperty.Register("RequestGeneralTranslateURL", typeof(string), typeof(ConfigurationControl), new FrameworkPropertyMetadata(null));

        public string RequestGeneralTranslateURL
        {
            get
            {
                return this.GetValue(RequestGeneralTranslateURLProperty) as string;
            }
            set
            {
                this.SetValue(RequestGeneralTranslateURLProperty, value);
                OnPropertyChanged("RequestGeneralTranslateURL");
            }
        }

        public static readonly DependencyProperty RequestTranslateRegionProperty =
            DependencyProperty.Register("RequestTranslateRegion", typeof(string), typeof(ConfigurationControl), new FrameworkPropertyMetadata(null));

        public string RequestTranslateRegion
        {
            get
            {
                return this.GetValue(RequestTranslateRegionProperty) as string;
            }
            set
            {
                this.SetValue(RequestTranslateRegionProperty, value);
                OnPropertyChanged("RequestTranslateRegion");
            }
        }
        #endregion  azure translate properties

        public ConfigurationControl()
        {
            IsEnglish = true;
            Logger = LogManager.GetCurrentClassLogger();
            readWriteConfiguration = new ReadWriteConfiguration();

            InitializeComponent();
            this.DataContext = this;
            ReadConfiguration();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ReadConfiguration()
        {
            Logger.Info($"ConfigurationControl Start - ReadConfiguration Action");
            if (readWriteConfiguration == null)
            {
                Logger.Error($"ConfigurationControl - ReadConfiguration Action readWriteConfiguration object is null");
            }
            else
            {
                Logger.Info($"ConfigurationControl Start - ReadConfiguration Action old information: language: '{language}', maxCandidate: '{MaxCandidateNum}', subscriptionKey: '{Azurekey}', urlAzure: '{URLAzure}', requestAzure: '{RequestMaxAzure}'");

                language = readWriteConfiguration.GetAzureLanguage();
                MaxCandidateNum = readWriteConfiguration.GetAzureMaxCandidate().ToString();
                Azurekey = readWriteConfiguration.GetAzureSubscriptionKey();
                URLAzure = readWriteConfiguration.GetAzureURL();
                RequestMaxAzure = readWriteConfiguration.GetMaxRequestAzure().ToString();

                switch (language)
                {
                    case "en":
                        IsEnglish = true;
                        IsSpanish = false;
                        break;
                    case "es":
                        IsEnglish = false;
                        IsSpanish = true;
                        break;
                }

                Logger.Info($"ConfigurationControl Start - ReadConfiguration Action NEW information: language: '{language}', maxCandidate: '{MaxCandidateNum}', subscriptionKey: '{Azurekey}', urlAzure: '{URLAzure}', requestAzure: '{RequestMaxAzure}'");


                Logger.Info($"ConfigurationControl Start - ReadConfiguration Action old translate information: TranslateAzureKey: '{RequestTranslateSubscriptionKey}', translateAzureKey: '{RequestGeneralTranslateURL}', azureRegion: '{RequestTranslateRegion}', useTranslate: '{IsSpanishWithTransalation}'");

                RequestTranslateSubscriptionKey = readWriteConfiguration.GetAzureTranslateSubscriptionKey();
                RequestGeneralTranslateURL = readWriteConfiguration.GetAzureGeneralTranslateURL();
                RequestTranslateRegion = readWriteConfiguration.GetAzureTranslateRegion();
                IsSpanishWithTransalation = readWriteConfiguration.GetUseAzureTranslate();

                if (IsSpanishWithTransalation)
                {
                    IsEnglish = false;
                    IsSpanish = true;
                }

                Logger.Info($"ConfigurationControl Start - ReadConfiguration Action NEW translate information: TranslateAzureKey: '{RequestTranslateSubscriptionKey}', translateAzureKey: '{RequestGeneralTranslateURL}', azureRegion: '{RequestTranslateRegion}', useTranslate: '{IsSpanishWithTransalation}'");
            }

            Logger.Info("ConfigurationControl FINISH - ReadConfiguration Action");
        }

        private void SaveConfiguration(object sender, RoutedEventArgs e)
        {
            SpinnerConfiguration.Visibility = Visibility.Visible;

            try
            {

                Logger.Info($"ConfigurationControl Start - SaveConfiguration Action old information: language: '{language}', maxCandidate: '{MaxCandidateNum}', subscriptionKey: '{Azurekey}', urlAzure: '{URLAzure}', requestAzure: '{RequestMaxAzure}'");

                if (IsSpanishWithTransalation)
                {
                    readWriteConfiguration.UpdateAzureLanguage("en");
                    readWriteConfiguration.UpdateUseAzureTranslate("true");
                }
                else if (IsEnglish)
                {
                    readWriteConfiguration.UpdateAzureLanguage("en");
                    readWriteConfiguration.UpdateUseAzureTranslate("false");
                }
                else
                {
                    readWriteConfiguration.UpdateAzureLanguage("es");
                    readWriteConfiguration.UpdateUseAzureTranslate("false");
                }

                readWriteConfiguration.UpdateAzureMaxCandidate(MaxCandidateNum);
                readWriteConfiguration.UpdateAzureSubscriptionKey(Azurekey);
                readWriteConfiguration.UpdateAzureURL(URLAzure);
                readWriteConfiguration.UpdateMaxRequestAzure(RequestMaxAzure);

                Logger.Info($"ConfigurationControl Start - SaveConfiguration Action NEW information: language: '{language}', maxCandidate: '{MaxCandidateNum}', subscriptionKey: '{Azurekey}', urlAzure: '{URLAzure}', requestAzure: '{RequestMaxAzure}'");


                Logger.Info($"ConfigurationControl Start - SaveConfiguration Action old translate information: TranslateAzureKey: '{RequestTranslateSubscriptionKey}', translateAzureKey: '{RequestGeneralTranslateURL}', azureRegion: '{RequestTranslateRegion}'");

                readWriteConfiguration.UpdateAzureTranslateSubscriptionKey(RequestTranslateSubscriptionKey);
                readWriteConfiguration.UpdateAzureGeneralTranslateURL(RequestGeneralTranslateURL);
                readWriteConfiguration.UpdateAzureTranslateRegion(RequestTranslateRegion);

                Logger.Info($"ConfigurationControl Start - SaveConfiguration Action NEW translate information: TranslateAzureKey: '{RequestTranslateSubscriptionKey}', translateAzureKey: '{RequestGeneralTranslateURL}', azureRegion: '{RequestTranslateRegion}'");

                MessageBox.Show("Configuración Guardada Correctamente", "Operación Correcta", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exc)
            {
                Logger.Error(exc, "ConfigurationControl ERROR - SaveConfiguration Action");
                MessageBox.Show(exc.Message, "Error guardando la configuración.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Logger.Info("ConfigurationControl FINISH - SaveConfiguration Action");
                SpinnerConfiguration.Visibility = Visibility.Hidden;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void radioSpanishClick(object sender, RoutedEventArgs e)
        {
            this.radioTranslateSpanish.IsEnabled = IsSpanish;
        }

        private void radioEnglishClick(object sender, RoutedEventArgs e)
        {
            this.radioTranslateSpanish.IsEnabled = false;
            IsSpanishWithTransalation = false;
        }
    }
}
