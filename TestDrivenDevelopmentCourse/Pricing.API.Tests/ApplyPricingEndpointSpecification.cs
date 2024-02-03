using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pricing.API.Tests.TestDoubles;
using Pricing.Core;
using Pricing.Core.Tests.Domain.TestDoubles;

namespace Pricing.API.Tests;

public class ApplyPricingEndpointSpecification
{
    const string _requestUri = "PricingTable";

    [Fact]
    public async Task Should_return_500_when_causes_an_exception()
    {
        using var client = CreateApiWithPricingManager<StubExceptionPricingManager>().CreateClient();

        var response = await client.PutAsJsonAsync(_requestUri, CreateApplyPricingRequest());

        response.Should().HaveStatusCode(HttpStatusCode.InternalServerError);
    }


    [Fact]
    public async Task Should_return_400_when_pricing_manager_return_false()
    {
        using var client = CreateApiWithPricingManager<StubApplyFailPricingManager>().CreateClient();

        var response = await client.PutAsJsonAsync(_requestUri, CreateApplyPricingRequest());

        response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_return_200_when_pricing_manager_return_true()
    {
        var api = CreateApiWithPricingManager<StubApplySucceedPricingManager>();
        using var client = api.CreateClient();

        var response = await client.PutAsJsonAsync(_requestUri, new ApplyPricingRequest(new[]
        {
            new PriceTierRequest(24, 1)
        }));

        response.Should().HaveStatusCode(HttpStatusCode.OK);
    }
    
    
    [Fact]
    public async Task Should_send_request_to_pricing_manager()
    {
        var pricingStore = new InMemoryPricingStoreFake();
        var api = new ApiFactory(services =>
        {
            services.TryAddScoped<IPricingStore>(s =>
                pricingStore);
        });
        var client = api.CreateClient();
        var applyPricingRequest = CreateApplyPricingRequest();

        await client.PutAsJsonAsync(_requestUri,
            applyPricingRequest);

        pricingStore.GetPricingTable()
            .Should()
            .BeEquivalentTo(applyPricingRequest);
    }
    
    private static ApplyPricingRequest CreateApplyPricingRequest()
    {
        return new ApplyPricingRequest(new[]
        {
            new PriceTierRequest(24, 1)
        });
    }

    private static ApiFactory CreateApiWithPricingManager<T>()
        where T : class, IPricingManager
    {
        var api = new ApiFactory(services =>
        {
            services.RemoveAll(typeof(IPricingManager));
            services.TryAddScoped<IPricingManager, T>();
        });
        return api;
    }
}

public class StubApplySucceedPricingManager : IPricingManager
{
    public Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}