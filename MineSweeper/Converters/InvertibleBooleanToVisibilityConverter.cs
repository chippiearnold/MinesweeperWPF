using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MineSweeper.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class InvertableBooleanToVisibilityConverter : IValueConverter
    {
        private enum Parameters
        {
            Inverted,
            Normal
        }

        public object Convert(object value, Type targetType,object parameter, CultureInfo culture)
        {
            var boolValue = (bool)value;
            var direction = (Parameters)Enum.Parse(typeof(Parameters), (string)parameter);

            switch (direction)
            {
                case Parameters.Inverted:
                    return !boolValue ? Visibility.Visible : Visibility.Collapsed;
                case Parameters.Normal:
                    return boolValue ? Visibility.Visible : Visibility.Collapsed;
                default:
                    return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}