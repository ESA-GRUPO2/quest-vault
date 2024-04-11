using questvault.Data;

namespace questvaultTest
{
  public class BackofficeControllerTests(
      ApplicationDbContext context
    ) : IClassFixture<ApplicationDbContextFixture>
  {
    private FakeSignInManager signInManager;

  }
}
