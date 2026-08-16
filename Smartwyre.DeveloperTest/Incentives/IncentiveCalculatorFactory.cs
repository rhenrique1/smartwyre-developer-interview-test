using System;
using System.Collections.Generic;
using System.Linq;
using Smartwyre.DeveloperTest.Incentives.Calculators;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Incentives;

/// <summary>
/// Factory Implmentation: Resolves an IIncentiveCalculator strategy based on the IncentiveType.
/// </summary>
public class IncentiveCalculatorFactory : IIncentiveCalculatorFactory
{
    private readonly Dictionary<IncentiveType, IIncentiveCalculator> _calculators;

    /// <summary>
    /// Default constructor initializing all standard built-in calculation strategies.
    /// </summary>
    public IncentiveCalculatorFactory()
        : this([
            new FixedCashAmountCalculator(),
            new FixedRateRebateCalculator(),
            new AmountPerUomCalculator()
        ])
    {
    }

    /// <summary>
    /// Constructor supporting Dependency Injection (DI) by receiving all registered calculators.
    /// </summary>
    public IncentiveCalculatorFactory(IEnumerable<IIncentiveCalculator> calculators)
    {
        if (calculators == null)
        {
            throw new ArgumentNullException(nameof(calculators));
        }

        _calculators = calculators.ToDictionary(c => c.IncentiveType, c => c);
    }

    public IIncentiveCalculator GetCalculator(IncentiveType incentiveType)
    {
        _calculators.TryGetValue(incentiveType, out var calculator);
        return calculator;
    }
}