using PhoneBook.Services;
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
        Dictionary<Fields,bool> compare;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            compare = new Dictionary<Fields, bool>()
            {
                {
                    Fields.Name,IsValid(values[0].ToString())
                },
                {
                    Fields.Surname,IsValid(values[1].ToString())
                    
                },
                {
                    Fields.Phone,IsNumberValid(values[2].ToString())
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
           
            if (!(value.Length > 2 && value.Length < 50))

                return false;

            else if ((value.Any(ch => !Char.IsLetterOrDigit(ch))))

                return false;
          
        return true;
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
