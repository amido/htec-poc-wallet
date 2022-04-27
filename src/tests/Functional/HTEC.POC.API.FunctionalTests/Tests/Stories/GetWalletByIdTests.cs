using TestStack.BDDfy;
using Xunit;
using HTEC.POC.API.FunctionalTests.Tests.Fixtures;
using HTEC.POC.API.FunctionalTests.Tests.Steps;

namespace HTEC.POC.API.FunctionalTests.Tests.Functional;

//Define the story/feature being tested
[Story(
    AsA = "user of the Yumido website",
    IWant = "to be able to view specific wallets",
    SoThat = "I can choose what to eat")]
public class GetWalletByIdTests : IClassFixture<AuthFixture>
{
    private readonly AuthFixture fixture;
    private readonly WalletSteps steps;

    public GetWalletByIdTests(AuthFixture fixture)
    {
        //Get instances of the fixture and steps required for the test
        this.fixture = fixture;
        steps = new WalletSteps();
    }

    //Add all tests that make up the story to this class.
    [Fact]
    public void Users_Can_View_Existing_Wallets()
    {
        this.Given(s => fixture.GivenAUser())
            .And(s => steps.GivenAWalletAlreadyExists())
            .When(s => steps.WhenIGetAWallet())
            .Then(s => steps.ThenICanReadTheWalletReturned())
            .BDDfy();
    }

    [Fact]
    [Trait("Category", "SmokeTest")]
    public void Admins_Can_View_Existing_Wallets()
    {
        this.Given(s => fixture.GivenAnAdmin())
            .And(s => steps.GivenAWalletAlreadyExists())
            .When(s => steps.WhenIGetAWallet())
            .Then(s => steps.ThenICanReadTheWalletReturned())
            .BDDfy();
    }
}