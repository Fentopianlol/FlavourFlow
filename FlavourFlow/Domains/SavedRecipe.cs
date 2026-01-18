using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlavourFlow.Domains
{
    public class SavedRecipe
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public FlavourFlowUser? User { get; set; }

        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        public DateTime SavedAt { get; set; } = DateTime.Now;
    }
}