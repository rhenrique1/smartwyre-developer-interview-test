using System;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Incentives;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    private readonly IIncentiveCalculatorFactory _calculatorFactory;
    public RebateService()
        : this(new RebateDataStore(), new ProductDataStore(), new IncentiveCalculatorFactory())
    {
    }

    public RebateService(
        IRebateDataStore rebateDataStore,
        IProductDataStore productDataStore,
        IIncentiveCalculatorFactory calculatorFactory
    )
    {
        _rebateDataStore = rebateDataStore ?? throw new ArgumentNullException(nameof(rebateDataStore));
        _productDataStore = productDataStore ?? throw new ArgumentNullException(nameof(productDataStore));
        _calculatorFactory = calculatorFactory ?? throw new ArgumentNullException(nameof(calculatorFactory));
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var result = new CalculateRebateResult();

        if (request is null)
        {
            result.Success = false;
            return result;
        }

        Rebate rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);

        if (rebate is null)
        {
            result.Success = false;
            return result;
        }

        Product product = _productDataStore.GetProduct(request.ProductIdentifier);

        IIncentiveCalculator calculator = _calculatorFactory.GetCalculator(rebate.Incentive);
        if (calculator is null)
        {
            result.Success = false;
            return result;
        }

        IncentiveCalculationResult calculationResult = calculator.Calculate(rebate, product, request);
        result.Success = calculationResult.Success;

        if (result.Success)
        {
            _rebateDataStore.StoreCalculationResult(rebate, calculationResult.Amount);
        }

        return result;
    }
}
