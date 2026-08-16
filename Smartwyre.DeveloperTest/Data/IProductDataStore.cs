using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

/// <summary>
/// Abstraction for product retrieval so higher-level business logic is not tightly coupled 
/// to data access implementations.
/// </summary>
public interface IProductDataStore
{
    /// <summary>
    /// Retrieves a product by its unique identifier.
    /// </summary>
    Product GetProduct(string productIdentifier);
}