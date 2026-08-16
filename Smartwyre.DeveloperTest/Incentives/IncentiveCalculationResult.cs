namespace Smartwyre.DeveloperTest.Incentives;

/// <summary>
/// Encapsulates the result of an incentive calculation strategy
/// Carries both the success status and the calculated rebate amount.
/// </summary>
public class IncentiveCalculationResult
{
    public bool Success { get; set; }
    public decimal Amount { get; set; }

    public static IncentiveCalculationResult CreateSuccessful(decimal amount) => new()
    {
        Success = true,
        Amount = amount
    };

    public static IncentiveCalculationResult CreateFailed() => new()
    {
        Success = false,
        Amount = 0m
    };
}