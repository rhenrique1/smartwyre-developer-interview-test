using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Incentives.Calculators;

/// <summary>
/// Strategy Implementation: FixedRateRebase
/// </summary>
public class FixedRateRebateCalculator : IIncentiveCalculator
{
    public IncentiveType IncentiveType => IncentiveType.FixedRateRebate;

    public IncentiveCalculationResult Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        if (rebate is null || product is null || request is null)
        {
            return IncentiveCalculationResult.CreateFailed();
        }

        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate))
        {
            return IncentiveCalculationResult.CreateFailed();
        }

        if (rebate.Percentage <= 0 || product.Price <= 0 || request.Volume <= 0)
        {
            return IncentiveCalculationResult.CreateFailed();
        }

        decimal amount = product.Price * rebate.Percentage * request.Volume;
        return IncentiveCalculationResult.CreateSuccessful(amount);
    }
}