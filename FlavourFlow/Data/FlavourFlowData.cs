using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FlavourFlow.Domains;

namespace FlavourFlow.Data
{
    public class FlavourFlowContext : IdentityDbContext<FlavourFlowUser>
    {
        public FlavourFlowContext(DbContextOptions<FlavourFlowContext> options)
            : base(options)
        {
        }

        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Review> Review { get; set; }

        // --- NEW TABLE REGISTRATION ---
        public DbSet<SavedRecipe> SavedRecipes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Additional configuration if needed
        }
    }
}