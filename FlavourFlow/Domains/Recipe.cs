using FlavourFlow.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlavourFlow.Domains
{
    public class Recipe
    {
        [Key]
        public int RecipeId { get; set; } // <--- CHANGED FROM 'Id' TO 'RecipeId'

        [Required]
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Difficulty { get; set; } = "Medium";
        public string ImageURL { get; set; } = "/images/default-recipe.jpg";
        public int CookTime { get; set; }

        // NAVIGATION PROPERTIES
        public List<Ingredient> Ingredients { get; set; } = new();
        public List<Instruction> Instructions { get; set; } = new();

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public FlavourFlowUser? User { get; set; }
    }
}