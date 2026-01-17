using Microsoft.AspNetCore.Identity;

namespace FlavourFlow.Domains
{
    public class FlavourFlowUser : IdentityUser
    {
        // FIX: Initialize these with empty strings to satisfy the compiler
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public List<Recipe> SavedRecipes { get; set; } = new();
    }
}