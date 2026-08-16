using System;
using Smartwyre.DeveloperTest.Incentives;
using Smartwyre.DeveloperTest.Incentives.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Incentives;

public class IncentiveCalculatorFactoryTests
{
    private readonly IncentiveCalculatorFactory _factory = new();

    [Theory]
    [InlineData(IncentiveType.FixedCashAmount, typeof(FixedCashAmountCalculator))]
    [InlineData(IncentiveType.FixedRateRebate, typeof(FixedRateRebateCalculator))]
    [InlineData(IncentiveType.AmountPerUom, typeof(AmountPerUomCalculator))]
    public void GetCalculator_ForKnownIncentiveType_ReturnsExpectedCalculatorType(
        IncentiveType incentiveType, Type expectedCalculatorType)
    {
        var calculator = _factory.GetCalculator(incentiveType);

        Assert.NotNull(calculator);
        Assert.IsType(expectedCalculatorType, calculator);
        Assert.Equal(incentiveType, calculator.IncentiveType);
    }

    [Fact]
    public void GetCalculator_ForUnknownIncentiveType_ReturnsNull()
    {
        var unknownType = (IncentiveType)999;

        var calculator = _factory.GetCalculator(unknownType);

        Assert.Null(calculator);
    }

}