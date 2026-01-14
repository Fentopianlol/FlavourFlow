using FlavourFlow.Domains;
using FlavourFlow.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlavourFlow.Data
{
    // Renamed to 'FlavourFlowContext' and using 'FlavourFlowUser'
    public class FlavourFlowContext(DbContextOptions<FlavourFlowContext> options) : IdentityDbContext<FlavourFlowUser>(options)
    {
        // Your Database Tables
        public DbSet<Recipe> Recipe { get; set; } = default!;
        public DbSet<Ingredient> Ingredient { get; set; } = default!;
        public DbSet<Instruction> Instruction { get; set; } = default!;
        public DbSet<Category> Category { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);



            // Seeding Data (Optional)
            builder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Asian", Type = "Cuisine" },
                new Category { CategoryId = 2, Name = "Italian", Type = "Cuisine" },
                new Category { CategoryId = 3, Name = "Vegan", Type = "Dietary" }
            );
        }
    }
}
