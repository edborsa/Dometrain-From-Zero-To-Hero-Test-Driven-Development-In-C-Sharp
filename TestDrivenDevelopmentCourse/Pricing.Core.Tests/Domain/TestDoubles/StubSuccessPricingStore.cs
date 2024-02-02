using Pricing.Core.Domain;

namespace Pricing.Core.Tests.Domain.TestDoubles;

public class StubSuccessPricingStore : IPricingStore
{
    public Task<bool> SaveAsync(PricingTable pricingTable, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}