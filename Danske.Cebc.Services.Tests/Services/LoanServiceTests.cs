using AutoFixture;
using Danske.Cebc.Services.Models;
using Danske.Cebc.Services.Services;
using Moq;
using NUnit.Framework;

namespace Danske.Cebc.Services.Tests.Services
{
    namespace Danske.Cebc.Services.Tests.Services.LoanServiceTests
    {
        public class LoanServiceBase
        {
            protected Fixture Fixture;
            
            protected Mock<ICalculateService> CalculateServiceMock;
            protected LoanService Service;

            
            [SetUp]
            public void Setup()
            {
                Fixture = new Fixture();

                CalculateServiceMock = new Mock<ICalculateService>();
                
                Service = new LoanService(CalculateServiceMock.Object);
            }
        }

        public class CalculateTests : LoanServiceBase
        {
            [Test]
            public void ItShouldThrowExceptionIfPowIsNegative()
            {
                var annualInterestRate = Fixture.Create<decimal>();
                var administrationFeeMin = Fixture.Create<decimal>();
                var loanAmount = Fixture.Create<decimal>();
                var loanDuration = Fixture.Create<int>();
                var compound = Fixture.Create<Compound>();

                var duration = Fixture.Create<int>();
                CalculateServiceMock.Setup(m => m.GetDuration(loanDuration, compound))
                    .Returns(duration);

                var discountFactor = Fixture.Create<decimal>();
                CalculateServiceMock.Setup(m => m.GetDiscountFactor(annualInterestRate, duration, compound))
                    .Returns(discountFactor);

                var monthlyCost = Fixture.Create<decimal>();
                CalculateServiceMock.Setup(m => m.GetMonthlyCost(loanAmount, discountFactor))
                    .Returns(monthlyCost);

                var totalAmountPaid = Fixture.Create<decimal>();
                CalculateServiceMock.Setup(m => m.GetTotalAmountPaid(monthlyCost, duration))
                    .Returns(totalAmountPaid);

                var totalAmountPaidInInterest = Fixture.Create<decimal>();
                CalculateServiceMock.Setup(m => m.GetTotalAmountPaidInInterest(totalAmountPaid, loanAmount))
                    .Returns(totalAmountPaidInInterest);

                var totalAdministrativeFees = Fixture.Create<decimal>();
                CalculateServiceMock.Setup(m => m.GetTotalAdministrativeFees(loanAmount, annualInterestRate, administrationFeeMin))
                    .Returns(totalAdministrativeFees);

                var aop = Fixture.Create<decimal>();
                CalculateServiceMock.Setup(m => m.GetAOP(loanAmount, compound, monthlyCost))
                    .Returns(aop);

                var result = Service.Calculate(annualInterestRate, administrationFeeMin, compound, loanAmount, loanDuration);
                
                CalculateServiceMock.Verify(m => m.GetDuration(It.IsAny<int>(), It.IsAny<Compound>()), Times.Once);
                CalculateServiceMock.Verify(m => m.GetDuration(loanDuration, compound), Times.Once);
                
                CalculateServiceMock.Verify(m => m.GetDiscountFactor(It.IsAny<decimal>(), It.IsAny<int>(), It.IsAny<Compound>()), Times.Once);
                CalculateServiceMock.Verify(m => m.GetDiscountFactor(annualInterestRate, duration, compound), Times.Once);
                
                CalculateServiceMock.Verify(m => m.GetMonthlyCost(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once);
                CalculateServiceMock.Verify(m => m.GetMonthlyCost(loanAmount, discountFactor), Times.Once);
                
                CalculateServiceMock.Verify(m => m.GetTotalAmountPaid(It.IsAny<decimal>(), It.IsAny<int>()), Times.Once);
                CalculateServiceMock.Verify(m => m.GetTotalAmountPaid(monthlyCost, duration), Times.Once);
                
                CalculateServiceMock.Verify(m => m.GetTotalAmountPaidInInterest(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once);
                CalculateServiceMock.Verify(m => m.GetTotalAmountPaidInInterest(totalAmountPaid, loanAmount), Times.Once);
                
                CalculateServiceMock.Verify(m => m.GetTotalAdministrativeFees(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once);
                CalculateServiceMock.Verify(m => m.GetTotalAdministrativeFees(loanAmount, annualInterestRate, administrationFeeMin), Times.Once);
                
                CalculateServiceMock.Verify(m => m.GetAOP(It.IsAny<decimal>(), It.IsAny<Compound>(), It.IsAny<decimal>()), Times.Once);
                CalculateServiceMock.Verify(m => m.GetAOP(loanAmount, compound, monthlyCost), Times.Once);

                Assert.That(result, Is.Not.Null);
                Assert.That(result.MonthlyCost, Is.EqualTo(monthlyCost));
                Assert.That(result.TotalAdministrativeFees, Is.EqualTo(totalAdministrativeFees));
                Assert.That(result.TotalAmountPaid, Is.EqualTo(totalAmountPaid));
                Assert.That(result.TotalAmountPaidInInterest, Is.EqualTo(totalAmountPaidInInterest));
                Assert.That(result.AOP, Is.EqualTo(aop));
            }
        }
    }
}