using System.Collections.Generic;
using System.Globalization;

namespace Danske.Cebc.Console.Helpers
{
    public interface IValidatorService
    {
        bool IsCultureValid(string culture);
        bool IsInputDataValid(decimal loanAmount, int loanDuration, out List<string> errors);
    }
    
    public class ValidatorService : IValidatorService
    {
        public bool IsCultureValid(string culture)
        {
            try
            {
                var _ = new CultureInfo(culture);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsInputDataValid(decimal loanAmount, int loanDuration, out List<string> errors)
        {
            errors = new List<string>();
            
            if (loanAmount == default)
            {
                errors.Add("Loan amount is required and was not specified or was '0'");
            }

            if (loanDuration == default)
            {
                errors.Add("Loan duration is required and was not specified or was '0'");
            }

            return errors.Count == 0;
        }
    }
}