using AutoFixture;
using Danske.Cebc.Console.Helpers;
using NUnit.Framework;

namespace Danske.Cebc.Services.Console.Tests.Helpers
{
    namespace Danske.Cebc.Services.Tests.Services.ValidatorServiceTests
    {
        public class ValidatorServiceBase
        {
            protected Fixture Fixture;
            protected ValidatorService Service;

            
            [SetUp]
            public void Setup()
            {
                Fixture = new Fixture();
                Service = new ValidatorService();
            }
        }

        public class IsCultureValidTests : ValidatorServiceBase
        {
            [Test]
            public void ItShouldReturnFalseIfCultureIsInvalid()
            {
                var culture = Fixture.Create<string>();

                var result = Service.IsCultureValid(culture);

                Assert.That(result, Is.False);
            }
            
            [TestCase("en-US")]
            [TestCase("fr-FR")]
            public void ItShouldReturnTrueIfCultureIsValid(string culture)
            {
                var result = Service.IsCultureValid(culture);

                Assert.That(result, Is.True);
            }
        }

        public class IsInputDataValidTests : ValidatorServiceBase
        {
            [Test]
            public void ItShouldAddErrorIfLoanAmountIsDefault()
            {
                var loanAmount = default(decimal);
                var loanDuration = Fixture.Create<int>();

                var result = Service.IsInputDataValid(loanAmount, loanDuration, out var errors);

                Assert.That(result, Is.False);
                Assert.That(errors.Count, Is.EqualTo(1));
            }
            
            [Test]
            public void ItShouldAddErrorIfLoanDurationIsDefault()
            {
                var loanAmount = Fixture.Create<decimal>();
                var loanDuration = default(int);

                var result = Service.IsInputDataValid(loanAmount, loanDuration, out var errors);

                Assert.That(result, Is.False);
                Assert.That(errors.Count, Is.EqualTo(1));
            }
            
            [Test]
            public void ItShouldReturnTrueAndEmptyArrayIfDataIsValid()
            {
                var loanAmount = Fixture.Create<decimal>();
                var loanDuration = Fixture.Create<int>();

                var result = Service.IsInputDataValid(loanAmount, loanDuration, out var errors);

                Assert.That(result, Is.True);
                Assert.That(errors, Is.Empty);
            }
        }
    }
}