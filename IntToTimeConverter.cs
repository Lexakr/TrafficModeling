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
            var _type = value.GetType();

            // Для целых
            if (_type == typeof(int))
            {
                // секунды
                if ((int)value < 600)
                {
                    return Math.Round(((int)value / 10.0), 2).ToString() + "с";
                }
                // минуты
                else if ((int)value < 36000)
                {
                    return Math.Round(((int)value / 600.0), 2).ToString() + "м";
                }
                // часы
                else
                {
                    return Math.Round(((int)value / 36000.0), 2).ToString() + "ч";
                }
            }
            // Для целых длинных
            if (_type == typeof(long))
            {
                if ((long)value < 600)
                {
                    return Math.Round(((long)value / 10.0), 2).ToString() + "с";
                }
                else if ((long)value < 36000)
                {
                    return Math.Round(((long)value / 600.0), 2).ToString() + "м";
                }
                else
                {
                    return Math.Round(((long)value / 36000.0), 2).ToString() + "ч";
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        // Заглушка для реализации интерфейса, конвертировать обратно нет задачи
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();

    }
}
