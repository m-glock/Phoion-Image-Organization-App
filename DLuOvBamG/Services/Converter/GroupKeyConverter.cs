using System;
using System.Globalization;
using Xamarin.Forms;

namespace DLuOvBamG.Services.Converter
{
    class GroupKeyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.GetType().Name != "String")
            {
                return parameter;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
