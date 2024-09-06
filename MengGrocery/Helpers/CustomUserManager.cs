using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace MengGrocery.Helpers
{
    public class CustomUserManager : UserManager<IdentityUser>
    {
        public CustomUserManager(IUserStore<IdentityUser> store, IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<IdentityUser> passwordHasher, IEnumerable<IUserValidator<IdentityUser>> userValidators,
            IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<IdentityUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        // Override necessary methods here
    }

}