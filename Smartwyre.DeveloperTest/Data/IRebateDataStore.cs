using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

/// <summary>
/// Abstraction for rebate retrieval and store calculation result. 
/// This makes the service easily testable using mock implementations.
/// </summary>
public interface IRebateDataStore
{

    /// <summary>
    /// Retrieves a rebate by its unique identifier.
    /// </summary>
    Rebate GetRebate(string rebateIdentifier);

    void StoreCalculationResult(Rebate account, decimal rebateAmount);
}