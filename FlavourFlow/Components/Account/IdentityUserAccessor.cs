using FlavourFlow.Data;
using FlavourFlow.Domains;
using Microsoft.AspNetCore.Identity;

namespace FlavourFlow.Components.Account
{
    public sealed class IdentityUserAccessor(UserManager<FlavourFlowUser> userManager, IdentityRedirectManager redirectManager)
    {
        public async Task<FlavourFlowUser> GetRequiredUserAsync(HttpContext context)
        {
            var user = await userManager.GetUserAsync(context.User);

            if (user is null)
            {
                redirectManager.RedirectToWithStatus("Account/Login", "Error: Unable to load user with ID '" + userManager.GetUserId(context.User) + "'.", context);
            }

            return user;
        }
    }
}