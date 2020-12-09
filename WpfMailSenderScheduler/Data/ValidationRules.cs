using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfMailSenderScheduler.Data.ValidationRules
{
    public static class UtilValidation
    {
        private static string regPattern = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        public static bool ValidateAddress(string address, out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(address) || Regex.IsMatch(address, regPattern, RegexOptions.IgnoreCase))
                return true;
            error = "Введите адрес электронной почты!";
            return false;
        }
    }
    public class IntValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value?.ToString();
            var result = str == null || string.IsNullOrWhiteSpace(str) || !Int32.TryParse(str, out _) ? 0 : 1;

            if (result != 0) return new ValidationResult(true, null);
          
             return new ValidationResult(false, "Введите число!"); 
        }         
    }

    public class MailValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var address  = value?.ToString();
            if (UtilValidation.ValidateAddress(address, out var err))
               return new ValidationResult(true, null);
            
            return new ValidationResult(false, err); 
        }
    }

    public class IntPositiveValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int intValue = 0;
            var str = value?.ToString();
            var result = str == null || string.IsNullOrWhiteSpace(str) || !Int32.TryParse(str, out intValue) ? 0 : 1;

            if (result == 0) return  new ValidationResult(false, "Введите число!");          

            if (intValue < 0) return new ValidationResult(false, "Введите число больше 0!");

            return new ValidationResult(true, null);
        }
    }
}
