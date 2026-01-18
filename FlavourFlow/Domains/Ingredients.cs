using System.ComponentModel.DataAnnotations;

namespace FlavourFlow.Domains
{
    public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }
        public string Name { get; set; } = "";
        public string Quantity { get; set; } = "";

        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
    }
}