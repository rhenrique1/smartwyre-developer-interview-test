using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Incentives.Calculators;

/// <summary>
/// Strategy Implementation: FixedCashAmount
/// </summary>
public class FixedCashAmountCalculator : IIncentiveCalculator
{
    public IncentiveType IncentiveType => IncentiveType.FixedCashAmount;
    public IncentiveCalculationResult Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        if (rebate is null || product is null || request is null)
        {
            return IncentiveCalculationResult.CreateFailed();
        }

        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount))
        {
            return IncentiveCalculationResult.CreateFailed();
        }

        // Assuming that rebates cannot be negative
        if (rebate.Amount <= 0)
        {
            return IncentiveCalculationResult.CreateFailed();
        }

        return IncentiveCalculationResult.CreateSuccessful(rebate.Amount);
    }
}