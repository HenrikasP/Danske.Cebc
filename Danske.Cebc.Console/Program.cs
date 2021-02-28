using System;
using System.Globalization;
using System.Threading;
using CommandLine;
using Danske.Cebc.Console.Configs;
using Danske.Cebc.Console.Helpers;
using Danske.Cebc.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Danske.Cebc.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Config.Locale);
            
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    var serviceProvider = ConfigureDependencyInjection();
                    
                    var validatorService = serviceProvider.GetRequiredService<IValidatorService>();
                    var loanService = serviceProvider.GetRequiredService<ILoanService>();

                    if (!validatorService.IsCultureValid(Config.Locale))
                    {
                        System.Console.WriteLine($"Locale found in config file: '{Config.Locale}'");
                        return;
                    }
                        
                    System.Console.WriteLine("Welcome to payment overview calculator");
                    System.Console.WriteLine("Calculator is currently configured with these settings:");
                    System.Console.WriteLine($"* Administration fee percent: {Config.AdministrationFeePercent}%");
                    System.Console.WriteLine($"* Minimal administration fee: {Config.AdministrationFeeMin}");
                    System.Console.WriteLine($"* Annual interest rate: {Config.AnnualInterestRate}%");
                    
                    var isInputDataValid = validatorService.IsInputDataValid(o.LoanAmount, o.LoanDuration, out var errors);
                    if (!isInputDataValid)
                    {
                        System.Console.WriteLine("Entered data is invalid. Please check error and try again:");
                        errors.ForEach(System.Console.WriteLine);
                        return;
                    }

                    try
                    {
                        var result = loanService.Calculate(Config.AnnualInterestRate, Config.AdministrationFeeMin, Config.Compound, o.LoanAmount, o.LoanDuration);
                        
                        System.Console.Clear();
                        System.Console.WriteLine($"Request loan amount: {o.LoanAmount} {Config.Currency} for duration of {o.LoanDuration} years");
                        System.Console.WriteLine($"Monthly cost: {result.MonthlyCost:F} {Config.Currency}");
                        System.Console.WriteLine($"Total amount paid in interest cost: {result.TotalAmountPaidInInterest:F} {Config.Currency}");
                        System.Console.WriteLine($"Total administrative fees: {result.TotalAdministrativeFees:F} {Config.Currency}");
                        System.Console.WriteLine($"AOP: {result.AOP:F} %");
                    }
                    catch (Exception)
                    {
                        System.Console.WriteLine("Unexpected exception occurred. Please try to contact developers");
                    }
                });
        }

        private static ServiceProvider ConfigureDependencyInjection()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<ILoanService, LoanService>();
            serviceCollection.AddScoped<ICalculateService, CalculateService>();
            serviceCollection.AddScoped<IMathService, MathService>();
            serviceCollection.AddScoped<IValidatorService, ValidatorService>();

            var build = serviceCollection.BuildServiceProvider();
            return build;
        }
    }
}



































