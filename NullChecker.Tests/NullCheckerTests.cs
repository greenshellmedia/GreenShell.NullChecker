using System;
using Xunit;

namespace LiamHT.NullChecker.Core.Tests
{
    public class NullCheckerTests
    {
        [Fact]
        public void For_CreatesGenericObject()
        {
            var subject = NullChecker.For(new TestableObject());

            Assert.IsType<NullChecker<TestableObject>>(subject);
        }

        [Fact]
        public void Validate_WhenNoPropertiesAreNull_ReturnsTrue()
        {
            var toValidate = new TestableObject()
            {
                FullName = "liamht"
            };

            var result = NullChecker.For(toValidate).Validate();

            Assert.True(result);
        }

        [Fact]
        public void Validate_WhenAnyPropertyIsNull_ReturnsFalse()
        {
            var toValidate = new TestableObject();

            var result = NullChecker.For(toValidate).Validate();

            Assert.Null(toValidate.FullName);
            Assert.False(result);
        }

        [Fact]
        public void Ignore_WhenValidated_IfPropertyIsNull_DoesNotValidateIt()
        {
            var toValidate = new TestableObject();

            var resultWithoutIgnore = NullChecker.For(toValidate).Validate();

            var resultWithIgnore = NullChecker.For(toValidate)
                .Ignore(nameof(toValidate.FullName))
                .Validate();

            Assert.False(resultWithoutIgnore);
            Assert.True(resultWithIgnore);
        }

        [Fact]
        public void AllowValueTypeValidation_WhenValidated_IfValueObjectIsDefault_ReturnsFalse()
        {
            var withDefault = new TestableObject()
            {
                 //DateOfBirth = Default,
                 Age = 99,
                 FullName = "Liamht"
            };

            var withoutDefault = new TestableObject()
            {
                DateOfBirth = DateTime.Now,
                Age = 99,
                FullName = "Liamht"
            };

            var resultWithDefaults = NullChecker.For(withDefault).AllowValueTypeValidation().Validate();
            var resultWithoutDefaults = NullChecker.For(withoutDefault).AllowValueTypeValidation().Validate();

            Assert.False(resultWithDefaults);
            Assert.True(resultWithoutDefaults);
        }
    }
}
