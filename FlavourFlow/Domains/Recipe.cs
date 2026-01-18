using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlavourFlow.Domains
{
    public class Recipe
    {
        [Key]
        public int RecipeId { get; set; }

        [Required]
        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public int CookTime { get; set; } // In minutes

        public string Difficulty { get; set; } = "Medium";

        public string ImageURL { get; set; } = "";

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public string Tags { get; set; } = ""; // Stores "Vegan,Spicy,Quick"

        // Foreign Keys
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public string? UserId { get; set; }
        public FlavourFlowUser? User { get; set; }

        // Relationships
        public List<Review> Reviews { get; set; } = new();
        public List<Ingredient> Ingredients { get; set; } = new();
        public List<Instruction> Instructions { get; set; } = new();

        // Computed Properties (Not stored in DB)
        [NotMapped]
        public double AverageRating
        {
            get
            {
                if (Reviews != null && Reviews.Any())
                {
                    return Reviews.Average(r => r.Rating);
                }
                return 0;
            }
        }

        [NotMapped]
        public int ReviewCount => (Reviews != null) ? Reviews.Count : 0;
    }
}