using FluentAssertions;
using Pricing.Core.Domain;

namespace Pricing.Core.Tests;

public class ApplyPricingSpecification
{
    private readonly PricingManager _pricingManager;

    public ApplyPricingSpecification()
    {
        _pricingManager = new PricingManager(new DummyPricingStore());
    }

    [Fact]
    public async Task Should_throw_argument_null_exception_if_request_is_null()
    {
        var handleRequest = () => _pricingManager.HandleAsync(null!, default);
        await handleRequest.Should().ThrowAsync<ArgumentNullException>();
    }
    
    [Fact]
    public async Task Should_return_true_if_succeeded()
    {
        var result = await _pricingManager.HandleAsync(new ApplyPricingRequest(), default);
        result.Should().BeTrue();
    }
}

public class DummyPricingStore : IPricingStore
{
    public Task<bool> SaveAsync(PricingTable pricingTable, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}