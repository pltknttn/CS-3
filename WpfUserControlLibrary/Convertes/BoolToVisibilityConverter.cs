using System; 
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfUserControlLibrary.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class BoolToVisibilityConverter : IValueConverter
    {
        public BoolToVisibilityConverter() : this(false, false) { }
        public BoolToVisibilityConverter(bool isReversed, bool useHidden)
        {
            IsReversed = isReversed;
            UseHidden = useHidden;
        }

        public bool IsReversed { get; set; }

        public bool UseHidden { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = System.Convert.ToBoolean(value, CultureInfo.InvariantCulture);
            if (IsReversed) val = !val; 
            if (val) return Visibility.Visible;
            return UseHidden ? Visibility.Hidden : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility)) return DependencyProperty.UnsetValue; 

            var result = (Visibility)value == Visibility.Visible; 
            return IsReversed ? !result : result;
        }
    }
}
