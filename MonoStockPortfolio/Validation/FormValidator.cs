using System;
using System.Collections.Generic;
using Android.Widget;

namespace MonoStockPortfolio.Validation
{
    public class FormValidator
    {
        private IList<KeyValuePair<EditText, Func<string>>> _dict;

        public FormValidator()
        {
            _dict = new List<KeyValuePair<EditText, Func<string>>>();
        }

        public void AddValidation(EditText control, Func<string> validationFunction)
        {
            _dict.Add(new KeyValuePair<EditText, Func<string>>(control, validationFunction));
        }
        public void AddRequired(EditText control, string errorMessage)
        {
            AddValidation(control, () => Required(control, errorMessage));
        }
        public void AddValidDecimal(EditText control, string errorMessage)
        {
            AddValidation(control, () => ValidDecimal(control, errorMessage));
        }
        public void AddValidPositiveDecimal(EditText control, string errorMessage)
        {
            AddValidation(control, () => ValidPositiveDecimal(control, errorMessage));
        }

        public string Apply()
        {
            var errorMessage = string.Empty;
            foreach (var keyValuePair in _dict)
            {
                var result = keyValuePair.Value();
                errorMessage += keyValuePair.Value();
                if(result != string.Empty)
                {
                    errorMessage += "\n";
                }
            }
            return errorMessage;
        }

        #region Validation Functions

        private static string Required(EditText control, string errorMessage)
        {
            if (string.IsNullOrEmpty(control.Text.ToString()))
            {
                return errorMessage;
            }
            return string.Empty;
        }
        private static string ValidDecimal(EditText control, string errorMessage)
        {
            var test = control.Text.ToString();
            decimal dummy;
            if(!decimal.TryParse(test, out dummy))
            {
                return errorMessage;
            }
            return string.Empty;
        }
        private static string ValidPositiveDecimal(EditText control, string errorMessage)
        {
            if(ValidDecimal(control, errorMessage) == string.Empty)
            {
                var val = decimal.Parse(control.Text.ToString());
                if (val >= 0) return string.Empty;
            }
            return errorMessage;
        }

        #endregion
    }
}