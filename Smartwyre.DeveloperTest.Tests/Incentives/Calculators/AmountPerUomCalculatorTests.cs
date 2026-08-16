using Smartwyre.DeveloperTest.Incentives.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Incentives.Calculators;

public class AmountPerUomCalculatorTests
{
    private readonly AmountPerUomCalculator _calculator = new();

    [Fact]
    public void IncentiveType_IsAmountPerUom()
    {
        Assert.Equal(IncentiveType.AmountPerUom, _calculator.IncentiveType);
    }

    [Fact]
    public void Calculate_WhenRebateIsNull_ReturnsFailure()
    {
        var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
        var request = new CalculateRebateRequest { Volume = 10 };

        var result = _calculator.Calculate(null, product, request);

        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void Calculate_WhenProductIsNull_ReturnsFailure()
    {
        var rebate = new Rebate { Amount = 5m };
        var request = new CalculateRebateRequest { Volume = 10 };

        var result = _calculator.Calculate(rebate, null, request);

        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void Calculate_WhenRequestIsNull_ReturnsFailure()
    {
        var rebate = new Rebate { Amount = 5m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };

        var result = _calculator.Calculate(rebate, product, null);

        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void Calculate_WhenProductDoesNotSupportAmountPerUom_ReturnsFailure()
    {
        var rebate = new Rebate { Amount = 5m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };
        var request = new CalculateRebateRequest { Volume = 10 };

        var result = _calculator.Calculate(rebate, product, request);

        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Theory]
    [InlineData(0, 10)]   // Amount is 0
    [InlineData(-5, 10)]  // Amount is negative
    [InlineData(5, 0)]    // Volume is 0
    [InlineData(5, -10)]  // Volume is negative
    public void Calculate_WhenParametersAreInvalid_ReturnsFailure(decimal amount, decimal volume)
    {
        var rebate = new Rebate { Amount = amount };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
        var request = new CalculateRebateRequest { Volume = volume };

        var result = _calculator.Calculate(rebate, product, request);

        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void Calculate_WhenInputsAreValid_ReturnsSuccessAndCalculatesAmount()
    {
        var rebate = new Rebate { Amount = 7.5m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
        var request = new CalculateRebateRequest { Volume = 4 };

        // Expected: 7.5 * 4 = 30
        var result = _calculator.Calculate(rebate, product, request);

        Assert.True(result.Success);
        Assert.Equal(30m, result.Amount);
    }
}