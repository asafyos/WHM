using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace warehouse2 {
    class TitleConverter : IValueConverter {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture) {
            if (targetType != typeof(string))
                throw new InvalidOperationException("The target must be a string");
            if (value == null) {
                return "מחסן";
            } else {
                return ("מחסן - " + (string)value);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture) {
            if (targetType != typeof(string))
                throw new InvalidOperationException("The target must be a string");

            return !(bool)value;
        }

        #endregion
    }
}
