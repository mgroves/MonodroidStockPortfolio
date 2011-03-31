using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoStockPortfolio.Framework
{
    public class FormValidator
    {
        private readonly IList<Func<string>> _list;

        public FormValidator()
        {
            _list = new List<Func<string>>();
        }

        public void AddValidation(Func<string> validationFunction)
        {
            _list.Add(validationFunction);
        }

        public void AddRequired(Func<string> getValue, string errorMessage)
        {
            AddValidation(() => Required(getValue(), errorMessage));
        }

        public void AddValidDecimal(Func<string> getValue, string errorMessage)
        {
            AddValidation(() => ValidDecimal(getValue(), errorMessage));
        }
        public void AddValidPositiveDecimal(Func<string> getValue, string errorMessage)
        {
            AddValidation(() => ValidPositiveDecimal(getValue(), errorMessage));
        }

        public IEnumerable<string> Apply()
        {
            return _list.Select(validation => validation())
                .Where(result => !string.IsNullOrEmpty(result));
        }

        #region validation functions

        private static string Required(string getValue, string errorMessage)
        {
            if (string.IsNullOrEmpty(getValue))
            {
                return errorMessage;
            }
            return string.Empty;
        }

        private static string ValidDecimal(string getValue, string errorMessage)
        {
            decimal dummy;
            if (!decimal.TryParse(getValue, out dummy))
            {
                return errorMessage;
            }
            return string.Empty;
        }

        private static string ValidPositiveDecimal(string getValue, string errorMessage)
        {
            if (ValidDecimal(getValue, errorMessage) == string.Empty)
            {
                var val = decimal.Parse(getValue);
                if (val >= 0) return string.Empty;
            }
            return errorMessage;
        }
        #endregion
    }
}