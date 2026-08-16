using System;
using System.Globalization;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=================================================");
        Console.WriteLine("        Smartwyre Rebate Calculation Runner      ");
        Console.WriteLine("=================================================");

        string rebateIdentifier = null;
        string productIdentifier = null;
        decimal volume = 0;

        for (int i = 0; i < args.Length; i++)
        {
            if ((args[i].Equals("-r", StringComparison.OrdinalIgnoreCase) ||
                 args[i].Equals("--rebate", StringComparison.OrdinalIgnoreCase)) && i + 1 < args.Length)
            {
                rebateIdentifier = args[++i];
            }
            else if ((args[i].Equals("-p", StringComparison.OrdinalIgnoreCase) ||
                      args[i].Equals("--product", StringComparison.OrdinalIgnoreCase)) && i + 1 < args.Length)
            {
                productIdentifier = args[++i];
            }
            else if ((args[i].Equals("-v", StringComparison.OrdinalIgnoreCase) ||
                      args[i].Equals("--volume", StringComparison.OrdinalIgnoreCase)) && i + 1 < args.Length)
            {
                decimal.TryParse(args[++i], NumberStyles.Any, CultureInfo.InvariantCulture, out volume);
            }
        }

        if (string.IsNullOrWhiteSpace(rebateIdentifier))
        {
            Console.Write("Enter Rebate Identifier: ");
            rebateIdentifier = Console.ReadLine()?.Trim();
        }

        if (string.IsNullOrWhiteSpace(productIdentifier))
        {
            Console.Write("Enter Product Identifier: ");
            productIdentifier = Console.ReadLine()?.Trim();
        }

        if (volume <= 0)
        {
            Console.Write("Enter Calculation Volume: ");
            var volumeInput = Console.ReadLine();
            while (!decimal.TryParse(volumeInput, NumberStyles.Any, CultureInfo.InvariantCulture, out volume) || volume <= 0)
            {
                Console.Write("Invalid volume. Please enter a positive decimal number: ");
                volumeInput = Console.ReadLine();
            }
        }

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume
        };

        Console.WriteLine("\nExecuting calculation with:");
        Console.WriteLine($" - Rebate ID : {request.RebateIdentifier}");
        Console.WriteLine($" - Product ID: {request.ProductIdentifier}");
        Console.WriteLine($" - Volume    : {request.Volume}");
        Console.WriteLine("-------------------------------------------------");

        IRebateService rebateService = new RebateService();
        CalculateRebateResult result = rebateService.Calculate(request);

        Console.WriteLine($"Calculation Result: {(result.Success ? "SUCCESS" : "FAILED")}");
        Console.WriteLine("=================================================");
    }
}
