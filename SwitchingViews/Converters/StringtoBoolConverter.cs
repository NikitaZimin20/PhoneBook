using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PhoneBook.Converters
{
    internal class StringtoBoolConverter:IMultiValueConverter
    {
        Dictionary<string,bool> compare;
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            compare = new Dictionary<string, bool>()
            {
                {
                    "name",IsValid(values[0].ToString())
                },
                {
                    "surname",IsValid(values[1].ToString())
                    
                },
                {
                    "phone",IsNumberValid(values[2].ToString())
                }
            };
            if (compare.ContainsValue(false))
            {
                return false;
            }
            return true;

        }

        private bool IsValid(string value)
    {
        if (value.Length > 2 && value.Length < 50)
        {
            return true;
        }
        return false;
    }
    private bool IsNumberValid(string value)
    {
        var numbers = value.Where(Char.IsDigit).ToArray();
        if (numbers.Length == 11 )
        {
            return true;
        }
        return false;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
}
