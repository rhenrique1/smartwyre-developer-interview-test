using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Incentives;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

public class RebateServiceTests
{
    private readonly Mock<IRebateDataStore> _mockRebateDataStore;
    private readonly Mock<IProductDataStore> _mockProductDataStore;
    private readonly Mock<IIncentiveCalculatorFactory> _mockCalculatorFactory;
    private readonly Mock<IIncentiveCalculator> _mockCalculator;
    private readonly RebateService _sut;

    public RebateServiceTests()
    {
        _mockRebateDataStore = new Mock<IRebateDataStore>();
        _mockProductDataStore = new Mock<IProductDataStore>();
        _mockCalculatorFactory = new Mock<IIncentiveCalculatorFactory>();
        _mockCalculator = new Mock<IIncentiveCalculator>();

        _sut = new RebateService(
            _mockRebateDataStore.Object,
            _mockProductDataStore.Object,
            _mockCalculatorFactory.Object);
    }

    [Fact]
    public void Calculate_WhenRequestIsNull_ReturnsFailure()
    {
        var result = _sut.Calculate(null);

        Assert.False(result.Success);
        _mockRebateDataStore.Verify(
            x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
    }

    [Fact]
    public void Calculate_WhenRebateNotFound_ReturnsFailure()
    {
        var request = new CalculateRebateRequest { RebateIdentifier = "rebate-123", ProductIdentifier = "prod-123", Volume = 5 };
        _mockRebateDataStore.Setup(
            x => x.GetRebate("rebate-123")).Returns((Rebate)null);

        var result = _sut.Calculate(request);

        Assert.False(result.Success);
        _mockRebateDataStore.Verify(
            x => x.StoreCalculationResult(
                It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
    }

    [Fact]
    public void Calculate_WhenNoCalculatorRegisteredForIncentiveType_ReturnsFailure()
    {
        var request = new CalculateRebateRequest { RebateIdentifier = "rebate-123", ProductIdentifier = "prod-123", Volume = 5 };
        var rebate = new Rebate { Identifier = "rebate-123", Incentive = IncentiveType.FixedCashAmount };
        var product = new Product { Identifier = "prod-123" };

        _mockRebateDataStore.Setup(x => x.GetRebate("rebate-123")).Returns(rebate);
        _mockProductDataStore.Setup(x => x.GetProduct("prod-123")).Returns(product);
        _mockCalculatorFactory.Setup(x => x.GetCalculator(rebate.Incentive)).Returns((IIncentiveCalculator)null);

        var result = _sut.Calculate(request);

        Assert.False(result.Success);
        _mockRebateDataStore.Verify(x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
    }

    [Fact]
    public void Calculate_WhenCalculatorFails_ReturnsFailureAndDoesNotStoreResult()
    {
        var request = new CalculateRebateRequest { RebateIdentifier = "rebate-123", ProductIdentifier = "prod-123", Volume = 5 };
        var rebate = new Rebate { Identifier = "rebate-123", Incentive = IncentiveType.FixedCashAmount };
        var product = new Product { Identifier = "prod-123" };

        _mockRebateDataStore.Setup(x => x.GetRebate("rebate-123")).Returns(rebate);
        _mockProductDataStore.Setup(x => x.GetProduct("prod-123")).Returns(product);
        _mockCalculatorFactory.Setup(
            x => x.GetCalculator(rebate.Incentive)).Returns(_mockCalculator.Object);
        _mockCalculator.Setup(x => x.Calculate(rebate, product, request))
            .Returns(IncentiveCalculationResult.CreateFailed());

        var result = _sut.Calculate(request);

        Assert.False(result.Success);
        _mockRebateDataStore.Verify(x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
    }

    [Fact]
    public void Calculate_WhenCalculatorSucceeds_ReturnsSuccessAndStoresResult()
    {
        var request = new CalculateRebateRequest { RebateIdentifier = "rebate-123", ProductIdentifier = "prod-123", Volume = 5 };
        var rebate = new Rebate { Identifier = "rebate-123", Incentive = IncentiveType.FixedCashAmount, Amount = 100m };
        var product = new Product { Identifier = "prod-123" };
        var expectedAmount = 100m;

        _mockRebateDataStore.Setup(x => x.GetRebate("rebate-123")).Returns(rebate);
        _mockProductDataStore.Setup(x => x.GetProduct("prod-123")).Returns(product);
        _mockCalculatorFactory.Setup(
            x => x.GetCalculator(rebate.Incentive)).Returns(_mockCalculator.Object);
        _mockCalculator.Setup(x => x.Calculate(rebate, product, request))
            .Returns(IncentiveCalculationResult.CreateSuccessful(expectedAmount));

        var result = _sut.Calculate(request);

        Assert.True(result.Success);
        _mockRebateDataStore.Verify(
            x => x.StoreCalculationResult(rebate, expectedAmount), Times.Once);
    }
}