using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Incentives;

/// <summary>
/// Responsible for resolving the appropriate IIncentiveCalculator strategy for a given IncentiveType.
/// </summary>
public interface IIncentiveCalculatorFactory
{
    /// <summary>
    /// Returns the calculator strategy corresponding to the provided incentive type.
    /// Returns null if no matching calculator is registered.
    /// </summary>
    IIncentiveCalculator GetCalculator(IncentiveType incentiveType);
}