using Pricing.Core.Domain;

namespace Pricing.Core.Tests.Domain.TestDoubles;

public class DummyPricingStore : IPricingStore
{
    public Task<bool> SaveAsync(PricingTable pricingTable, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}