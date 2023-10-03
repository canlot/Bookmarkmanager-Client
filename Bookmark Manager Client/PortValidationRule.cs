using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bookmark_Manager_Client
{
    public class PortValidationRule : ValidationRule
    {
        public PortValidationRule() { }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                Convert.ToInt32(value, cultureInfo);
            }
            catch 
            {
                return new ValidationResult(false, "not integer");
            }
            return new ValidationResult(true, "");
        }
    }
}
