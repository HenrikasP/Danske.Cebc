using System;

namespace Danske.Cebc.Services.Services
{
    public interface IMathService
    {
        decimal Pow(decimal x, int y);
    }
    
    public class MathService : IMathService
    {
        public decimal Pow(decimal x, int y)
        {
            if (y < 0)
                throw new ArgumentException( "Power by negative number is currently not supported", nameof(y));
            
            if (y == 0)
                return 1;
            
            var result = x;

            while (--y > 0)
            {
                result *= x;
            }

            return result;
        }
    }
}