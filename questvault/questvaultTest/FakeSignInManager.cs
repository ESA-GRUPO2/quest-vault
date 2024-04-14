using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using questvault.Models;
using System.Security.Claims;
namespace questvaultTest
{
  public class FakeSignInManager : SignInManager<User>
  {
    public FakeSignInManager()
        : base(new FakeUserManager(),
             new Mock<IHttpContextAccessor>().Object,
             new Mock<IUserClaimsPrincipalFactory<User>>().Object,
             new Mock<IOptions<IdentityOptions>>().Object,
             new Mock<ILogger<SignInManager<User>>>().Object,
             new Mock<IAuthenticationSchemeProvider>().Object,
             new Mock<IUserConfirmation<User>>().Object)
    { }
    public override Task SignInAsync(User user, AuthenticationProperties authenticationProperties, string? authenticationMethod = null)
    {
      ( (FakeUserManager) UserManager ).CurrentUser = user;
      ( (FakeUserManager) UserManager ).ClaimsPrincipal = ClaimsFactory.CreateAsync(user).Result;
      return Task.CompletedTask;
    }
    public override Task SignInAsync(User user, bool isPersistent, string? authenticationMethod = null)
    {
      ( (FakeUserManager) UserManager ).CurrentUser = user;
      ( (FakeUserManager) UserManager ).ClaimsPrincipal = ClaimsFactory.CreateAsync(user).Result;
      return Task.CompletedTask;
    }
  }

  public class FakeUserManager : UserManager<User>
  {
    public FakeUserManager()
        : base(new Mock<IUserStore<User>>().Object,
              new Mock<IOptions<IdentityOptions>>().Object,
              new Mock<IPasswordHasher<User>>().Object,
              [],
              [],
              new Mock<ILookupNormalizer>().Object,
              new Mock<IdentityErrorDescriber>().Object,
              new Mock<IServiceProvider>().Object,
              new Mock<ILogger<UserManager<User>>>().Object)
    { }
    public User CurrentUser { get; set; }
    public ClaimsPrincipal ClaimsPrincipal { get; set; }

    public override Task<User?> GetUserAsync(ClaimsPrincipal principal) => Task.FromResult(principal == ClaimsPrincipal ? CurrentUser : null);

    public override string? GetUserId(ClaimsPrincipal principal) => principal == ClaimsPrincipal ? CurrentUser.Id : null;

    public override

    public override Task<IdentityResult> CreateAsync(User user, string password)
    {
      return Task.FromResult(IdentityResult.Success);
    }

    public override Task<IdentityResult> AddToRoleAsync(User user, string role)
    {
      return Task.FromResult(IdentityResult.Success);
    }

    public override Task<string> GenerateEmailConfirmationTokenAsync(User user)
    {
      return Task.FromResult(Guid.NewGuid().ToString());
    }
  }
}
