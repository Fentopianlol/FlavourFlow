using FlavourFlow.Data;
using FlavourFlow.Domains;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Text.Json;

namespace Microsoft.AspNetCore.Routing
{
    internal static class IdentityComponentsEndpointRouteBuilderExtensions
    {
        public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            var accountGroup = endpoints.MapGroup("/Account");

            accountGroup.MapPost("/Logout", async (
                ClaimsPrincipal user,
                SignInManager<FlavourFlowUser> signInManager,
                [FromForm] string returnUrl) =>
            {
                await signInManager.SignOutAsync();

                // --- THE FIX ---
                // We hardcode "/" (Home Page) here. 
                // This prevents the "URL is not local" crash entirely.
                return TypedResults.LocalRedirect("/");
            });

            return accountGroup;
        }
    }
}