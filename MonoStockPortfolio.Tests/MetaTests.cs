using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Machine.Specifications;

namespace MonoStockPortfolio.Tests
{
    [Tags("MetaTest")]
    public class MetaTests
    {
        static Type[] _allTypes;
        static IList<Type> _allTestTypes;

        Establish context = () =>
            {
                _allTypes = Assembly.GetAssembly(typeof (MetaTests)).GetTypes();
            };

        Because of = () =>
            {
                _allTestTypes = _allTypes.Where(IsTestFixture).ToList();
            };

        It should_have_found_all_the_test_fixtures = () =>
            _allTestTypes.ShouldNotBeEmpty();

        It should_have_tags_on_every_test_fixture = () =>
            _allTestTypes.All(HasProperTags).ShouldBeTrue();

        It should_output_all_test_classes_that_arent_tagged_properly = () =>
            {
                var sb = new StringBuilder();
                var missingtags = _allTestTypes.Where(t => !HasProperTags(t));
                if (missingtags.Any())
                {
                    foreach (var missingtag in missingtags)
                    {
                        sb.AppendFormat("Test class missing tag: {0}", missingtag.Name)
                            .AppendLine();
                    }
                    throw new CustomAttributeFormatException(sb.ToString());
                }
            };

        static bool HasProperTags(Type type)
        {
            var attrs = Attribute.GetCustomAttributes(type);
            var tagAttrs = attrs.Where(a => a.GetType() == typeof (TagsAttribute));
            if(!tagAttrs.Any())
            {
                return false;
            }
            return tagAttrs.Select(a => (TagsAttribute) a)
                .All(ta => ta.Tags
                    .All(t => t.Name == "UnitTest"
                        || t.Name == "IntegrationTest"
                        || t.Name == "MetaTest"));
        }

        static bool IsTestFixture(Type type)
        {
            var privateFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
            foreach (var privateField in privateFields)
            {
                Console.WriteLine(privateField.GetType());
            }
            return privateFields.Any(f => f.FieldType == typeof (It));
        }

    }
}