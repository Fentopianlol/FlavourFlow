using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace FlavourFlow.Components.Account
{
    public sealed class IdentityRedirectManager(NavigationManager navigationManager)
    {
        public const string StatusCookieName = "Identity.StatusMessage";

        private static readonly CookieBuilder StatusCookieBuilder = new()
        {
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
            IsEssential = true,
            MaxAge = TimeSpan.FromSeconds(5),
        };

        [DoesNotReturn]
        public void RedirectTo(string? uri)
        {
            uri ??= "";
            // Prevent open redirect attacks.
            if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
            {
                uri = navigationManager.ToBaseRelativePath(uri);
            }
            // This method stops execution effectively
            navigationManager.NavigateTo(uri);
            throw new InvalidOperationException("This code should never be reached.");
        }

        [DoesNotReturn]
        public void RedirectTo(string uri, Dictionary<string, object?> queryParameters)
        {
            var uriWithoutQuery = navigationManager.ToAbsoluteUri(uri).GetLeftPart(UriPartial.Path);
            var newUri = navigationManager.GetUriWithQueryParameters(uriWithoutQuery, queryParameters);
            RedirectTo(newUri);
        }

        [DoesNotReturn]
        public void RedirectToCurrentPageWithStatus(string message, HttpContext context)
        {
            RedirectToWithStatus(navigationManager.Uri, message, context);
        }

        [DoesNotReturn]
        public void RedirectToWithStatus(string uri, string message, HttpContext context)
        {
            context.Response.Cookies.Append(StatusCookieName, message, StatusCookieBuilder.Build(context));
            RedirectTo(uri);
        }
    }
}
