using Smartwyre.DeveloperTest.Incentives.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Incentives.Calculators;

public class FixedCashAmountCalculatorTests
{
    private readonly FixedCashAmountCalculator _calculator = new();

    [Fact]
    public void IncentiveType_IsFixedCashAmount()
    {
        Assert.Equal(IncentiveType.FixedCashAmount, _calculator.IncentiveType);
    }

    [Fact]
    public void Calculate_WhenRebateIsNull_ReturnsFailure()
    {
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };
        var request = new CalculateRebateRequest { Volume = 10 };

        var result = _calculator.Calculate(null, product, request);

        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void Calculate_WhenProductIsNull_ReturnsFailure()
    {
        var rebate = new Rebate { Amount = 100m };
        var request = new CalculateRebateRequest { Volume = 10 };

        var result = _calculator.Calculate(rebate, null, request);

        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void Calculate_WhenRequestIsNull_ReturnsFailure()
    {
        var rebate = new Rebate { Amount = 100m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };

        var result = _calculator.Calculate(rebate, product, null);

        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void Calculate_WhenProductDoesNotSupportFixedCashAmount_ReturnsFailure()
    {
        var rebate = new Rebate { Amount = 100m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate };
        var request = new CalculateRebateRequest { Volume = 10 };

        var result = _calculator.Calculate(rebate, product, request);

        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Calculate_WhenRebateAmountIsZeroOrNegative_ReturnsFailure(decimal amount)
    {
        var rebate = new Rebate { Amount = amount };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };
        var request = new CalculateRebateRequest { Volume = 10 };

        var result = _calculator.Calculate(rebate, product, request);

        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void Calculate_WhenInputsAreValid_ReturnsSuccessAndRebateAmount()
    {
        var rebate = new Rebate { Amount = 150m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };
        var request = new CalculateRebateRequest { Volume = 10 };

        var result = _calculator.Calculate(rebate, product, request);

        Assert.True(result.Success);
        Assert.Equal(150m, result.Amount);
    }
}