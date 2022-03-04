using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;


namespace Accounting.Web.Client.Models
{
    public class CustomUserModelFactory : AccountClaimsPrincipalFactory<CustomUserModel>
    {
        public CustomUserModelFactory(NavigationManager navigationManager,
            IAccessTokenProviderAccessor accessor)
            : base(accessor)
        {
        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(
            CustomUserModel account,
            RemoteAuthenticationUserOptions options)
        {
            var initialUser = await base.CreateUserAsync(account, options);

            if (initialUser.Identity.IsAuthenticated)
            {
                var userIdentity = (ClaimsIdentity)initialUser.Identity;


                if (account.Roles != null)
                {
                    foreach (var role in account?.Roles)
                    {
                        userIdentity.AddClaim(new Claim("role", role));
                    }
                }


                if (account != null)
                {
                    if (account.Groups != null)
                    {
                        foreach (var group in account.Groups)
                        {
                            userIdentity.AddClaim(new Claim("group", group));
                        }
                    }
                }

                if (account != null)
                {
                    if (account.Wids != null)
                    {
                        foreach (var w in account.Wids)
                        {
                            userIdentity.AddClaim(new Claim("wid", w));
                        }
                    }
                }

            }

            return initialUser;
        }
    }
}
