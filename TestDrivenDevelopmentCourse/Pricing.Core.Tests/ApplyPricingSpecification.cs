using FluentAssertions;
using Pricing.Core.Tests.Domain.TestDoubles;

namespace Pricing.Core.Tests;

public class ApplyPricingSpecification
{
    [Fact]
    public async Task Should_throw_argument_null_exception_if_request_is_null()
    {
        var pricingManager = new PricingManager(new DummyPricingStore());
        var handleRequest = () => pricingManager.HandleAsync(null!, default);
        await handleRequest.Should().ThrowAsync<ArgumentNullException>();
    }
    
    [Fact]
    public async Task Should_return_true_if_succeeded()
    {
        var pricingManager = new PricingManager(new StubSuccessPricingStore());
        var result = await pricingManager.HandleAsync(new ApplyPricingRequest(), default);
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task Should_return_fail_if_fail_to_save()
    {
        var pricingManager = new PricingManager(new StubFailPricingStore());
        var result = await pricingManager.HandleAsync(new ApplyPricingRequest(), default);
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task Should_save_only_once()
    {
        var spyPricingServices = new SpyPricingServices();
        var pricingManager = new PricingManager(spyPricingServices);
        var _ = await pricingManager.HandleAsync(new ApplyPricingRequest(), default);
        spyPricingServices.NumberOfSaves.Should().Be(1);
    }
}