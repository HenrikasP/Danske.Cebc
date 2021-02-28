namespace Danske.Cebc.Services.Models
{
    public class LoanResult
    {
        public decimal MonthlyCost { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public decimal TotalAmountPaidInInterest { get; set; }
        public decimal TotalAdministrativeFees { get; set; }
        public decimal AOP { get; set; }
    }
}