namespace Pricing.Core;

// FROM my understanting here, we receive something that is a PricingRequest
// This request has a PriceTierRequest that is not a PriceTier
// and the priceStore is right now an in memory representation of the database
// that's why we are creating it and then passing
public record ApplyPricingRequest(IReadOnlyCollection<PriceTierRequest> Tiers);

public record PriceTierRequest(int HourLimit, decimal Price );