using System.Windows;
using System.Windows.Controls;

namespace GetDescriptionImageApp.Presentation.Controls.Shared
{
    /// <summary>
    /// Lógica de interacción para GeneralButtonControl.xaml
    /// </summary>
    public partial class GeneralButtonControl : UserControl
    {
        public static readonly DependencyProperty TextValueProperty =
            DependencyProperty.Register("TextValue", typeof(string), typeof(GeneralButtonControl), new FrameworkPropertyMetadata(null));

        public string TextValue
        {
            get
            {
                return this.GetValue(TextValueProperty) as string;
            }
            set
            {
                this.SetValue(TextValueProperty, value);
            }
        }

        public static readonly DependencyProperty ImageNameProperty =
                DependencyProperty.Register("ImageName", typeof(string), typeof(GeneralButtonControl), new FrameworkPropertyMetadata(null));

        public string ImageName
        {
            get
            {
                return this.GetValue(ImageNameProperty) as string;
            }
            set
            {
                this.SetValue(ImageNameProperty, value);
            }
        }

        public GeneralButtonControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public event RoutedEventHandler CustomClick;

        #region private methods

        private void GeneralButtonClick(object sender, RoutedEventArgs e)
        {
            if (CustomClick != null)
            {
                CustomClick(this, new RoutedEventArgs());
            }
        }
        #endregion
    }
}
