using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoStockPortfolio.Framework
{
    public class NewFormValidator
    {
        private readonly IList<Func<string>> _list;

        public NewFormValidator()
        {
            _list = new List<Func<string>>();
        }

        public void AddValidation(Func<string> getValue, Func<string> validationFunction)
        {
            _list.Add(validationFunction);
        }

        public void AddRequired(Func<string> getValue, string errorMessage)
        {
            AddValidation(getValue, () => Required(getValue(), errorMessage));
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

        #endregion
    }
}