using System.ComponentModel.DataAnnotations;

namespace FlavourFlow.Domains
{
    public class Review
    {
        public int ReviewId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; } // 1 to 5 stars

        public string Comment { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Keys
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        public string UserId { get; set; } = "";
        public FlavourFlowUser? User { get; set; }
    }
}