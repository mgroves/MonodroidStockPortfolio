using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Tests.Framework
{
    public class When_validating_forms_with_validation_errors
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
                             _validator.AddValidDecimal(() => "not a decimal", "Decimal required");
                             _validator.AddValidPositiveDecimal(() => "-18.9", "Positive decimal required");
                             _validator.AddValidation(() => "arbitrary error!");
                             _errors = _validator.Apply();
                         };

        It should_return_1_error_message = () =>
            _errors.Count().ShouldEqual(4);
        It should_have_a_required_message = () =>
            _errors.Any(e => e == "This is required").ShouldBeTrue();
        It should_have_a_valid_decimal_message = () =>
            _errors.Any(e => e == "Decimal required").ShouldBeTrue();
        It should_have_a_valid_positive_decimal_message = () =>
            _errors.Any(e => e == "Positive decimal required").ShouldBeTrue();
        It should_have_an_arbitrary_message = () =>
            _errors.Any(e => e == "arbitrary error!").ShouldBeTrue();
    }
}