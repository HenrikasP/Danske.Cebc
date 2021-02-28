using CommandLine;

namespace Danske.Cebc.Console.Configs
{
    internal class Options
    {
        [Option('l', "loanAmount", Required = true, HelpText = "Loan amount.")]
        public decimal LoanAmount { get; set; }
        
        [Option('y', "loanDuration", Required = true, HelpText = "Loan duration in years.")]
        public int LoanDuration { get; set; }
    }
}