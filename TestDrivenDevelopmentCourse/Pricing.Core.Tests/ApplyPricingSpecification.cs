using FluentAssertions;
using NSubstitute;
using Pricing.Core.Domain;
using Pricing.Core.Tests.Domain.TestDoubles;

namespace Pricing.Core.Tests;

public class ApplyPricingSpecification
{
    [Fact]
    public async Task Should_throw_argument_null_exception_if_request_is_null()
    {
        var pricingManager = new PricingManager(new DummyPricingStore());
        var handleRequest = () => pricingManager.HandleAsync(null!, default);
        await handleRequest.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task Should_return_true_if_succeeded()
    {
        var pricingManager = new PricingManager(new StubSuccessPricingStore());
        var result = await pricingManager.HandleAsync(CreateRequest(), default);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Should_return_fail_if_fail_to_save()
    {
        var pricingManager = new PricingManager(new StubFailPricingStore());
        var result = await pricingManager.HandleAsync(CreateRequest(), default);
        result.Should().BeFalse();
    }


    [Fact]
    public async Task Should_save_only_once()
    {
        var spyPricingServices = new SpyPricingServices();
        var pricingManager = new PricingManager(spyPricingServices);
        var _ = await pricingManager.HandleAsync(CreateRequest(), default);
        spyPricingServices.NumberOfSaves.Should().Be(1);
    }

    [Fact]
    public async Task Should_save_expected_data()
    {
        var expectedPricingTable = new PricingTable(new[] { new PriceTier(24, 1) });
        var mockPricingStore = new MockPricingStore();
        mockPricingStore.ExpectedToSave(expectedPricingTable);
        var pricingManager = new PricingManager(mockPricingStore);
        var _ = await pricingManager.HandleAsync(CreateRequest(), default);

        mockPricingStore.Verify();
    }
    
    [Fact]
    public async Task Should_save_expected_data_nsubstitute()
    {
        var expectedPricingTable = new PricingTable(new[] { new PriceTier(24, 1) });
        var mockPricingStore = Substitute.For<IPricingStore>();
        var pricingManager = new PricingManager(mockPricingStore);
        
        var _ = await pricingManager.HandleAsync(CreateRequest(), default);

        await mockPricingStore.Received().SaveAsync(Arg.Is<PricingTable>(
            table => table.Tiers.Count == expectedPricingTable.Tiers.Count), default);
    }
    
    
    [Fact]
    public async Task Should_save_equivalent_data_to_the_storage()
    {
        var pricingStore = new InMemoryPricingStoreFake();
        var pricingManager = new PricingManager(pricingStore);

        var applyPricingRequest = CreateRequest();
        var _ = await pricingManager.HandleAsync(applyPricingRequest, default);

        pricingStore.GetPricingTable()
            .Should()
            .BeEquivalentTo(applyPricingRequest);

    }
    

    private static ApplyPricingRequest CreateRequest()
    {
        return new ApplyPricingRequest(
            new[]
            {
                new PriceTierRequest(24, 1)
            }
        );
    }
}