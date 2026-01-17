using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FlavourFlow.Domains;

namespace FlavourFlow.Data
{
    public class FlavourFlowContext(DbContextOptions<FlavourFlowContext> options) : IdentityDbContext<FlavourFlowUser>(options)
    {
        public DbSet<Recipe> Recipe { get; set; } = default!;
        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Review> Review { get; set; } = default!;

        // --- NEW: Fix the Multiple Cascade Path Error ---
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // This tells SQL Server: "If a Recipe is deleted, prevent it if there are reviews" 
            // OR "Don't try to auto-delete reviews via this path during a User delete."
            // This solves the "Cycles or Multiple Cascade Paths" error.
            builder.Entity<Review>()
                .HasOne(r => r.Recipe)
                .WithMany(rec => rec.Reviews)
                .HasForeignKey(r => r.RecipeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}