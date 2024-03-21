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
using questvault.Models;
namespace questvaultTest
{
    public class FakeUserManager : UserManager<User>
    {
        public FakeUserManager()
            : base(new Mock<IUserStore<User>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<IPasswordHasher<User>>().Object,
                  new IUserValidator<User>[0],
                  new IPasswordValidator<User>[0],
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<IServiceProvider>().Object,
                  new Mock<ILogger<UserManager<User>>>().Object)
                  //new Mock<IHttpContextAccessor>().Object)
        { }

        public override Task<IdentityResult> CreateAsync(User user, string password)
        {
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
