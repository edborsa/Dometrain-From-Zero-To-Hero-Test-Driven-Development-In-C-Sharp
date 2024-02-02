using FluentAssertions;
using Pricing.Core.Domain;

namespace Pricing.Core.Tests;

public class PricingTableSpecification
{
    [Fact]
    public void Should_fail_if_price_tiers_is_null()
    {
        var create = () => new PricingTable(null);

        create.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Should_fail_if_has_no_price_tiers()
    {
        var create = () => new PricingTable(Array.Empty<PriceTier>());

        create.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void Should_have_one_tier_when_created_with_one()
    {
        var pricingTable = new PricingTable(new[] { createPriceTier() });

        pricingTable.Tiers.Should().HaveCount(1);
    }

    [Fact]
    public void Price_tiers_should_be_ordered_by_hour_limit()
    {
        var pricingTable = new PricingTable(new[] { createPriceTier(24), createPriceTier(4) });

        pricingTable.Tiers.Should().BeInAscendingOrder(tier => tier.HourLimit);
    }

    [Theory]
    [InlineData(2,1,25)]
    [InlineData(3,2,49)]
    public void Maximum_daily_price_should_be_calculated_using_tiers_if_not_provided(decimal price1, decimal price2, decimal maxPrice)
    {
        var pricingTable = new PricingTable(new[]
            {
                createPriceTier(1, price1),
                createPriceTier(24, price2)
            },
            maxDailyPrice: null);

        pricingTable.GetMaxDailyPrice().Should().Be(maxPrice);
    }
    
    
    [Fact]
    public void Should_be_able_to_set_max_daily_price()
    {
        const int maxDailyPrice = 15;
        var pricingTable = new PricingTable(new[] { createPriceTier() }, maxDailyPrice: maxDailyPrice);

        pricingTable.GetMaxDailyPrice().Should().Be(maxDailyPrice);
    }
    
    [Fact]
    public void Should_fail_if_tiers_to_no_cover_24_hours()
    {
        var create = () => new PricingTable(new[] { createPriceTier(20) });

        create.Should().ThrowExactly<ArgumentException>();
    }
    
    [Fact]
    public void Should_fail_if_max_daily_price_gt_tiers_price()
    {
        var create = () => new PricingTable(new[] { createPriceTier(20) }, maxDailyPrice: 26);

        create.Should().ThrowExactly<ArgumentException>();
    }


    private static PriceTier createPriceTier(int hourLimit = 24, decimal price = 1) => new(hourLimit, price);
}