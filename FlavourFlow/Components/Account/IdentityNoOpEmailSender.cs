using FlavourFlow.Data;
using FlavourFlow.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace FlavourFlow.Components.Account
{
    // A dummy email sender that does nothing (so the app doesn't crash)
    public sealed class IdentityNoOpEmailSender : IEmailSender<FlavourFlowUser>
    {
        private readonly IEmailSender _emailSender = new NoOpEmailSender();

        public Task SendConfirmationLinkAsync(FlavourFlowUser user, string email, string confirmationLink) =>
            _emailSender.SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

        public Task SendPasswordResetLinkAsync(FlavourFlowUser user, string email, string resetLink) =>
            _emailSender.SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");

        public Task SendPasswordResetCodeAsync(FlavourFlowUser user, string email, string resetCode) =>
            _emailSender.SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}");
    }

    // Basic internal helper
    internal class NoOpEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Just pretend to send email
            return Task.CompletedTask;
        }
    }
}