namespace Pricing.Core;

public class PricingManager
{
    public PricingManager(IPricingStore pricingStore)
    {
    }

    public Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken o1)
    {
        ArgumentNullException.ThrowIfNull(request);
        return Task.FromResult(true);
    }
}