using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;
using questvault.Controllers;
using questvault.Data;
using questvault.Models;
using questvault.Services;

namespace questvaultTest
{
    public class TestSignInManager : SignInManager<User>
    {
        public TestSignInManager(IHttpContextAccessor contextAccessor, IUserConfirmation<User> userConfirmation)
    : base(new FakeSignInManager(),
          contextAccessor,
          new Mock<IUserClaimsPrincipalFactory<User>>().Object,
          new Mock<IOptions<IdentityOptions>>().Object,
          new Mock<ILogger<SignInManager<User>>>().Object,
        new Mock<IAuthenticationSchemeProvider>().Object,
        userConfirmation
        
        )
        {
        }

        public override Task SignInAsync(User user, bool isPersistent, string authenticationMethod = null)
        {
            return Task.FromResult(0);
        }

        public override Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return Task.FromResult(SignInResult.Success);
        }

        public override Task SignOutAsync()
        {
            return Task.FromResult(0);
        }
    }
}
