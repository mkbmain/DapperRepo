using System;
using Mkb.DapperRepo.Reflection;
using Xunit;

namespace Mkb.DapperRepo.Tests.Tests.ReflectionUtilsTests
{
    public class GetPropertyInfoOfTypeTests
    {
        private class ReflectionTest
        {
            public string Name { get; set; }
        }

        [Fact]
        public void Ensure_we_can_get_back_correct_info()
        {
            var item = ReflectionUtils.GetPropertyInfoOfType<ReflectionTest>(typeof(string),
                nameof(ReflectionTest.Name));
            
            Assert.NotNull(item);
        }

        [Fact]
        public void Ensure_we_throw_if_property_not_found()
        {
            var exp = Assert.Throws<Exception>(
                () => ReflectionUtils.GetPropertyInfoOfType<ReflectionTest>(typeof(string),
                    nameof(ReflectionTest.Name) + "tes")
            );

            Assert.Contains($"Property:{nameof(ReflectionTest.Name)}tes not found in Type:{nameof(ReflectionTest)}", exp.Message);
        }

        [Fact]
        public void Ensure_we_throw_if_property_not_type_we_specified()
        {
            var exp = Assert.Throws<Exception>(
                () => ReflectionUtils.GetPropertyInfoOfType<ReflectionTest>(typeof(int),
                    nameof(ReflectionTest.Name))
            );

            Assert.Contains($"Type Must Be {nameof(Int32)}", exp.Message);
        }

        [Fact]
        public void Ensure_we_do_not_throw_if_specified_not_to_with_property_not_found()
        {
            var exp = ReflectionUtils.GetPropertyInfoOfType<ReflectionTest>(typeof(string),
                nameof(ReflectionTest.Name) + "tes", false);

            Assert.Null(exp);
        }

        [Fact]
        public void Ensure_we_do_not_throw_if_specified_not_to_with_property_of_wrong_type()
        {
            var exp = ReflectionUtils.GetPropertyInfoOfType<ReflectionTest>(typeof(int),
                nameof(ReflectionTest.Name), false);

            Assert.Null(exp);
        }

        [Fact]
        public void Ensure_we_do_not_throw_if_specified_not_to_with_property_of_wrong_type_and_wrong_name()
        {
            var exp = ReflectionUtils.GetPropertyInfoOfType<ReflectionTest>(typeof(int),
                nameof(ReflectionTest.Name) + "brow", false);

            Assert.Null(exp);
        }
    }
}