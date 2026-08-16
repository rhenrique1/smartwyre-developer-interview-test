using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Incentives.Calculators;

/// <summary>
/// Strategy Implementation: AmountPerUom (Amount Per Unit of Measure)
/// </summary>
public class AmountPerUomCalculator : IIncentiveCalculator
{
    public IncentiveType IncentiveType => IncentiveType.AmountPerUom;
    public IncentiveCalculationResult Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        if (rebate is null || product is null || request is null)
        {
            return IncentiveCalculationResult.CreateFailed();
        }

        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom))
        {
            return IncentiveCalculationResult.CreateFailed();
        }

        if (rebate.Amount <= 0 || request.Volume <= 0)
        {
            return IncentiveCalculationResult.CreateFailed();
        }

        decimal amount = rebate.Amount * request.Volume;
        return IncentiveCalculationResult.CreateSuccessful(amount);
    }
}