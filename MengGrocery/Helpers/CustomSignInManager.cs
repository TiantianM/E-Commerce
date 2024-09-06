using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace MengGrocery.Helpers
{
    public class CustomSignInManager : SignInManager<IdentityUser>
    {
        public CustomSignInManager(UserManager<IdentityUser> userManager, IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<IdentityUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<IdentityUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<IdentityUser> confirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }

        // public override Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        // {
        //     // Custom logic to encrypt/decrypt username and password
        //     // Call base method or completely customize the login process
        //     return base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        // }
    }

}