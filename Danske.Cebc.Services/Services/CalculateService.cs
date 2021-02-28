using System;
using Danske.Cebc.Services.Models;

namespace Danske.Cebc.Services.Services
{
    public interface ICalculateService
    {
        int GetDuration(int LoanDuration, Compound compound);
        decimal GetDiscountFactor(decimal annualInterestRate, int loanDuration, Compound compound);
        decimal GetMonthlyCost(decimal loanAmount, decimal discountFactor);
        decimal GetTotalAmountPaid(decimal monthlyCost, int loanDuration);
        decimal GetAOP(decimal loanAmount, Compound compound, decimal monthlyCost);
        decimal GetTotalAdministrativeFees(decimal loanAmount, decimal annualInterestRate, decimal administrationFeeMin);
        decimal GetTotalAmountPaidInInterest(decimal totalAmountPaid, decimal loanAmount);
    }
    
    public class CalculateService : ICalculateService
    {
        private readonly IMathService _mathService;

        public CalculateService(IMathService mathService)
        {
            _mathService = mathService;
        }
        
        public int GetDuration(int LoanDuration, Compound compound)
        {
            switch (compound)
            {
                case Compound.Monthly:
                    return LoanDuration * (int)Compound.Monthly;
                case Compound.Daily:
                    return LoanDuration * (int)Compound.Daily;
                case Compound.Weekly:
                    return LoanDuration * (int)Compound.Weekly;
                default:
                    throw new ArgumentException("Provided compound is invalid");
            }
        }

        public decimal GetDiscountFactor(decimal annualInterestRate, int loanDuration, Compound compound)
        {
            var r = annualInterestRate / 100 / (int) compound;
            var power = _mathService.Pow(1 + r, loanDuration);
            var result = (power - 1) / (r * power);
            return result;
        }
        
        public decimal GetMonthlyCost(decimal loanAmount, decimal discountFactor)
        {
            return loanAmount / discountFactor;
        }
        
        public decimal GetTotalAmountPaid(decimal monthlyCost, int loanDuration)
        {
            return monthlyCost * loanDuration;
        }
        
        public decimal GetAOP(decimal loanAmount, Compound compound, decimal monthlyCost)
        {
            return monthlyCost * (int)compound * 100 / loanAmount;
        }

        public decimal GetTotalAdministrativeFees(decimal loanAmount, decimal annualInterestRate, decimal administrationFeeMin)
        {
            return Math.Min(loanAmount * annualInterestRate, administrationFeeMin);
        }

        public decimal GetTotalAmountPaidInInterest(decimal totalAmountPaid, decimal loanAmount)
        {
            return totalAmountPaid - loanAmount;
        }
    }
}