using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MonoStockPortfolio.Activites.EditPortfolioScreen;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Tests.Framework
{
    public class ValidationTests
    {
        static FormValidator _validator;
        static IEnumerable<string> _errors;

        Establish context = () =>
                                {
                                    _validator = new FormValidator();
                                };

        Because of = () =>
                         {
                             _validator.AddRequired(() => "", "This is required");
                             _errors = _validator.Apply();
                         };

        It should_return_1_error_message = () =>
            _errors.Count().ShouldEqual(1);
    }
}