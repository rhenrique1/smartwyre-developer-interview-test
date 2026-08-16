using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Incentives;

/// <summary>
/// Common strategy interface for incentive calculations.
/// </summary>
public interface IIncentiveCalculator
{
    /// <summary>
    /// The incentive type that this calculator strategy handles.
    /// </summary>
    IncentiveType IncentiveType { get; }

    /// <summary>
    /// Validates inputs and calculates the rebate amount for the specific incentive type.
    /// </summary>
    IncentiveCalculationResult Calculate(Rebate rebate, Product product, CalculateRebateRequest request);
}