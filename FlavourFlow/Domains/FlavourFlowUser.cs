using Microsoft.AspNetCore.Identity;

namespace FlavourFlow.Domains
{
    public class FlavourFlowUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}