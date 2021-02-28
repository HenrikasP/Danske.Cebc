using System;
using AutoFixture;
using Danske.Cebc.Services.Models;
using Danske.Cebc.Services.Services;
using Moq;
using NUnit.Framework;

namespace Danske.Cebc.Services.Tests.Services
{
    namespace Danske.Cebc.Services.Tests.Services.CalculateServiceTests
    {
        public class CalculateServiceTestBase
        {
            protected Fixture Fixture;
            
            protected Mock<IMathService> MathServiceMock;
            protected CalculateService Service;

            
            [SetUp]
            public void Setup()
            {
                Fixture = new Fixture();

                MathServiceMock = new Mock<IMathService>();
                Service = new CalculateService(MathServiceMock.Object);
            }
        }

        public class GetDurationInMonthsTests : CalculateServiceTestBase
        {
            [Test]
            public void ItShouldGetDurationInMonths()
            {
                var loanDuration = Fixture.Create<int>();
                var compound = Fixture.Create<Compound>();

                var result = Service.GetDuration(loanDuration, compound);

                Assert.That(result, Is.EqualTo(loanDuration * 12));
            }
        }

        public class GetDiscountFactorTests : CalculateServiceTestBase
        {
            [Test]
            public void ItShouldGetDiscountFactor()
            {
                var annualInterestRate = Fixture.Create<decimal>();
                var loanDuration = Fixture.Create<int>();
                var compound = Fixture.Create<Compound>();

                var power = Fixture.Create<decimal>();
                MathServiceMock.Setup(m => m.Pow(It.IsAny<decimal>(), It.IsAny<int>()))
                    .Returns(power);

                var result = Service.GetDiscountFactor(annualInterestRate, loanDuration, compound);
                
                var r = annualInterestRate / 100 / (int) compound;
                Assert.That(result, Is.EqualTo((power - 1) / (r * power)));
            }
        }
        
        public class GetMonthlyCostTests : CalculateServiceTestBase
        {
            [Test]
            public void ItShouldGetMonthlyCost()
            {
                var loanAmount = Fixture.Create<decimal>();
                var discountFactor = Fixture.Create<decimal>();

                var result = Service.GetMonthlyCost(loanAmount, discountFactor);

                Assert.That(result, Is.EqualTo(loanAmount / discountFactor));
            }
        }
        
        public class GetTotalAmountPaidTests : CalculateServiceTestBase
        {
            [Test]
            public void ItShouldGetTotalAmountPaid()
            {
                var monthlyCost = Fixture.Create<decimal>();
                var loanDuration = Fixture.Create<int>();

                var result = Service.GetTotalAmountPaid(monthlyCost, loanDuration);

                Assert.That(result, Is.EqualTo(monthlyCost * loanDuration));
            }
        }
        
        public class GetAOPTests : CalculateServiceTestBase
        {
            [Test]
            public void ItShouldGetAOP()
            {
                var loanAmount = Fixture.Create<decimal>();
                var compound = Fixture.Create<Compound>();
                var monthlyCost = Fixture.Create<int>();

                var result = Service.GetAOP(loanAmount, compound, monthlyCost);

                Assert.That(result, Is.EqualTo(monthlyCost * 12 * 100 / loanAmount));
            }
        }
        
        public class GetTotalAdministrativeFeesTests : CalculateServiceTestBase
        {
            [Test]
            public void ItShouldGetTotalAdministrativeFees()
            {
                var loanAmount = Fixture.Create<decimal>();
                var annualInterestRate = Fixture.Create<decimal>();
                var administrationFeeMin = Fixture.Create<decimal>();

                var result = Service.GetTotalAdministrativeFees(loanAmount, annualInterestRate, administrationFeeMin);

                Assert.That(result, Is.EqualTo(Math.Min(loanAmount * annualInterestRate, administrationFeeMin)));
            }
        }
        
        public class GetTotalAmountPaidInInterestTests : CalculateServiceTestBase
        {
            [Test]
            public void ItShouldGetTotalAdministrativeFees()
            {
                var totalAmountPaid = Fixture.Create<decimal>();
                var loanAmount = Fixture.Create<decimal>();

                var result = Service.GetTotalAmountPaidInInterest(totalAmountPaid, loanAmount);

                Assert.That(result, Is.EqualTo(totalAmountPaid - loanAmount));
            }
        }
    }
}