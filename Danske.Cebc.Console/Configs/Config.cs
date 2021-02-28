using System;
using System.Configuration;
using Danske.Cebc.Services.Models;

namespace Danske.Cebc.Console.Configs
{
    internal static class Config
    {
        internal static string Locale => ConfigurationManager.AppSettings["Locale"] ?? "en-US";
        internal static decimal AnnualInterestRate => decimal.Parse(ConfigurationManager.AppSettings["AnnualInterestRate"] ?? string.Empty);
        internal static int AdministrationFeePercent => int.Parse(ConfigurationManager.AppSettings["AdministrationFeePercent"] ?? string.Empty);
        internal static decimal AdministrationFeeMin => decimal.Parse(ConfigurationManager.AppSettings["AdministrationFeeMin"] ?? string.Empty);
        internal static Compound Compound => (Compound) Enum.Parse(typeof(Compound), ConfigurationManager.AppSettings["Compound"] ?? string.Empty);
        internal static string Currency => ConfigurationManager.AppSettings["Currency"] ?? string.Empty;
    }
}