using Smartwyre.DeveloperTest.Incentives.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Incentives.Calculators;

public class FixedRateRebateCalculatorTests
{
    private readonly FixedRateRebateCalculator _calculator = new();

    [Fact]
    public void IncentiveType_IsFixedRateRebate()
    {
        Assert.Equal(IncentiveType.FixedRateRebate, _calculator.IncentiveType);
    }

    [Fact]
    public void Calculate_WhenRebateIsNull_ReturnsFailure()
    {
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 10m };
        var request = new CalculateRebateRequest { Volume = 5 };

        var result = _calculator.Calculate(null, product, request);

        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void Calculate_WhenProductIsNull_ReturnsFailure()
    {
        var rebate = new Rebate { Percentage = 0.1m };
        var request = new CalculateRebateRequest { Volume = 5 };

        var result = _calculator.Calculate(rebate, null, request);

        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void Calculate_WhenRequestIsNull_ReturnsFailure()
    {
        var rebate = new Rebate { Percentage = 0.1m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 10m };

        var result = _calculator.Calculate(rebate, product, null);

        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void Calculate_WhenProductDoesNotSupportFixedRateRebate_ReturnsFailure()
    {
        var rebate = new Rebate { Percentage = 0.1m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount, Price = 10m };
        var request = new CalculateRebateRequest { Volume = 5 };

        var result = _calculator.Calculate(rebate, product, request);

        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Theory]
    [InlineData(0, 10, 5)]     // Percentage is 0
    [InlineData(-0.1, 10, 5)]  // Percentage is negative
    [InlineData(0.1, 0, 5)]    // Price is 0
    [InlineData(0.1, -10, 5)]  // Price is negative
    [InlineData(0.1, 10, 0)]   // Volume is 0
    [InlineData(0.1, 10, -5)]  // Volume is negative
    public void Calculate_WhenParametersAreInvalid_ReturnsFailure(decimal percentage, decimal price, decimal volume)
    {
        var rebate = new Rebate { Percentage = percentage };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = price };
        var request = new CalculateRebateRequest { Volume = volume };

        var result = _calculator.Calculate(rebate, product, request);

        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void Calculate_WhenInputsAreValid_ReturnsSuccessAndCalculatesAmount()
    {
        var rebate = new Rebate { Percentage = 0.2m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 50m };
        var request = new CalculateRebateRequest { Volume = 10 };

        // Expected: 50 * 0.2 * 10 = 100
        var result = _calculator.Calculate(rebate, product, request);

        Assert.True(result.Success);
        Assert.Equal(100m, result.Amount);
    }
}