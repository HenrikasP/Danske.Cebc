using Danske.Cebc.Services.Models;

namespace Danske.Cebc.Services.Services
{
    public interface ILoanService
    {
        LoanResult Calculate(decimal annualInterestRate, decimal administrationFeeMin, Compound compound, decimal loanAmount, int LoanDuration);
    }
    
    public class LoanService : ILoanService
    {
        private readonly ICalculateService _calculateService;

        public LoanService(ICalculateService calculateService)
        {
            _calculateService = calculateService;
        }
        
        public LoanResult Calculate(decimal annualInterestRate, decimal administrationFeeMin, Compound compound, decimal loanAmount, int LoanDuration)
        {
            var duration = _calculateService.GetDuration(LoanDuration, compound);
            var discountFactor = _calculateService.GetDiscountFactor(annualInterestRate, duration, compound);
            var monthlyCost = _calculateService.GetMonthlyCost(loanAmount, discountFactor);
            
            var totalAmountPaid = _calculateService.GetTotalAmountPaid(monthlyCost, duration);
            var totalAmountPaidInInterest = _calculateService.GetTotalAmountPaidInInterest(totalAmountPaid, loanAmount);
            var totalAdministrativeFees = _calculateService.GetTotalAdministrativeFees(loanAmount, annualInterestRate, administrationFeeMin);
            var aop = _calculateService.GetAOP(loanAmount, compound, monthlyCost);
            
            return new LoanResult
            {
                MonthlyCost = monthlyCost,
                TotalAdministrativeFees = totalAdministrativeFees,
                TotalAmountPaid = totalAmountPaid,
                TotalAmountPaidInInterest = totalAmountPaidInInterest,
                AOP = aop
            };
        }
    }
}