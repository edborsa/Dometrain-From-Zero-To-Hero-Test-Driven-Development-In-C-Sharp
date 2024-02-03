using Pricing.Core.Domain;

namespace Pricing.Core.Tests.Domain.TestDoubles;

public class StubFailPricingStore : IPricingStore
{
    public Task<bool> SaveAsync(PricingTable pricingTable, CancellationToken cancellationToken)
    {
        return Task.FromResult(false);
    }
}