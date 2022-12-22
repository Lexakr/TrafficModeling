using System;
using System.Globalization;
using System.Windows.Data;

namespace TrafficModeling
{
    public class IntToTimeConverter : IValueConverter
    {
        /// <summary>
        /// Форматирование децисекунд в секунды, минуты и часы для корректного вывода на экран.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value < 600)
            {
                return Math.Round(((int)value / 10.0), 2).ToString() + "s";
            }
            else if ((int)value < 36000)
            {
                return Math.Round(((int)value / 600.0), 2).ToString() + "m";
            }
            else
            {
                return Math.Round(((int)value / 36000.0), 2).ToString() + "h";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();

    }
}
