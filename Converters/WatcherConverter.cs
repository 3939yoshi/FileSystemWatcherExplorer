using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FileSystemWatcherExplorer.Converters 
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class WatcherConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool watch = System.Convert.ToBoolean(value);/// value as bool;

            return watch ? "監視中" : "停止中";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
