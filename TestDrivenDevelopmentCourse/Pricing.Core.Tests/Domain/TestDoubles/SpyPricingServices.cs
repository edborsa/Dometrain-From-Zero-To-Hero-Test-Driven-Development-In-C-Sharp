using Pricing.Core.Domain;

namespace Pricing.Core.Tests.Domain.TestDoubles;

public class SpyPricingServices : IPricingStore
{
    public Task<bool> SaveAsync(PricingTable pricingTable, CancellationToken cancellationToken)
    {
        NumberOfSaves++;
        return Task.FromResult(true);
    }

    public int NumberOfSaves { get; set; }
}