using Pricing.Core;
using Pricing.Core.Domain.Exceptions;

namespace Pricing.API.Tests.TestDoubles;

public class StubExceptionPricingManager : IPricingManager
{
    public Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken cancellationToken)
    {
        throw new InvalidPricingTierException();
    }
}