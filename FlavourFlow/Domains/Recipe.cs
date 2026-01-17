using FlavourFlow.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlavourFlow.Domains
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Difficulty { get; set; } = "Medium";
        public int CookTime { get; set; }
        public string ImageURL { get; set; } = "";

        // Relationships
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public string UserId { get; set; } = "";
        public FlavourFlowUser? User { get; set; }

        public List<Ingredient> Ingredients { get; set; } = new();
        public List<Instruction> Instructions { get; set; } = new();

        // --- NEW: Reviews Relationship ---
        public List<Review> Reviews { get; set; } = new();

        // Helper to calculate Average Rating automatically
        [NotMapped]
        public double AverageRating => Reviews.Any() ? Math.Round(Reviews.Average(r => r.Rating), 1) : 0;

        [NotMapped]
        public int ReviewCount => Reviews.Count;
    }
}