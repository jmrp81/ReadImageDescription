using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GetDescriptionImageApp.Presentation.Controls.Shared
{
    public partial class SpinnerControl : UserControl
    {
        public static readonly DependencyProperty DiameterProperty = DependencyProperty.Register("Diameter", typeof(int), typeof(SpinnerControl), new PropertyMetadata(20, OnDiameterPropertyChanged));

        public int Diameter
        {
            get { return (int)GetValue(DiameterProperty); }
            set
            {
                if (value < 10)
                    value = 10;
                SetValue(DiameterProperty, value);
            }
        }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(int), typeof(SpinnerControl), new PropertyMetadata(15, null, OnCoerceRadius));

        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }


        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register("InnerRadius", typeof(int), typeof(SpinnerControl), new PropertyMetadata(2, null, OnCoerceInnerRadius));

        public int InnerRadius
        {
            get { return (int)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(SpinnerControl), new PropertyMetadata(new Point(15, 15), null, OnCoerceCenter));

        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        public static readonly DependencyProperty ColorAProperty = DependencyProperty.Register("ColorA", typeof(Color), typeof(SpinnerControl), new PropertyMetadata(Colors.Green));

        public Color ColorA
        {
            get { return (Color)GetValue(ColorAProperty); }
            set { SetValue(ColorAProperty, value); }
        }

        public static readonly DependencyProperty ColorBProperty = DependencyProperty.Register("ColorB", typeof(Color), typeof(SpinnerControl), new PropertyMetadata(Colors.Transparent));

        public Color ColorB
        {
            get { return (Color)GetValue(ColorBProperty); }
            set { SetValue(ColorBProperty, value); }
        }

        public SpinnerControl()
        {
            InitializeComponent();
        }

        private static void OnDiameterPropertyChanged(DependencyObject depObject, DependencyPropertyChangedEventArgs e)
        {
            depObject.CoerceValue(CenterProperty);
            depObject.CoerceValue(RadiusProperty);
            depObject.CoerceValue(InnerRadiusProperty);
        }

        private static object OnCoerceRadius(DependencyObject depObject, object baseValue)
        {
            var control = (SpinnerControl)depObject;
            int newRadius = (int)(control.GetValue(DiameterProperty)) / 2;
            return newRadius;
        }

        private static object OnCoerceInnerRadius(DependencyObject depObject, object baseValue)
        {
            var control = (SpinnerControl)depObject;
            int newInnerRadius = (int)(control.GetValue(DiameterProperty)) / 4;
            return newInnerRadius;
        }

        private static object OnCoerceCenter(DependencyObject depObject, object baseValue)
        {
            var control = (SpinnerControl)depObject;
            int newCenter = (int)(control.GetValue(DiameterProperty)) / 2;
            return new Point(newCenter, newCenter);
        }
    }
}
