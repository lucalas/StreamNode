using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace StreamNode.Validation
{
    class PathExistsValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return !Directory.Exists(value.ToString())
                ? new ValidationResult(false, "Invalid OBS Path, insert a valid Path.")
                : ValidationResult.ValidResult;
        }
    }
}
