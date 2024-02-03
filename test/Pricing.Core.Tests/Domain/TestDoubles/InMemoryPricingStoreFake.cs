using Pricing.Core.Domain;

namespace Pricing.Core.Tests.Domain.TestDoubles;

public class InMemoryPricingStoreFake : IPricingStore
{
    private PricingTable _pricingTable;

    public Task<bool> SaveAsync(PricingTable pricingTable, CancellationToken cancellationToken)
    {
        _pricingTable = pricingTable;
        return Task.FromResult(true);
    }

    public PricingTable GetPricingTable()
    {
        return _pricingTable;
    }
}