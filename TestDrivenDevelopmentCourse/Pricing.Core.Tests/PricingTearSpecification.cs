using FluentAssertions;
using Pricing.Core.Domain;
using Pricing.Core.Domain.Exceptions;

namespace Pricing.Core.Tests;

public class PricingTearSpecification
{
    [Theory]
    [InlineData(25, 1)]
    [InlineData(23, -1)]
    public void Should_throw_invalid_pricing_tier_when_hour_limit_is_invalid(int hourLimit, decimal price)
    {
        var create = () => new PriceTier(hourLimit, price);

        create.Should().ThrowExactly<InvalidPricingTierException>();
    }
    
    [Fact]
    public void Should_throw_invalid_pricing_tier_when_price_is_negative()
    {
        var create = () => new PriceTier(25, -1);

        create.Should().ThrowExactly<InvalidPricingTierException>();
    }
}