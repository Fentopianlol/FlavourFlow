using System.ComponentModel.DataAnnotations;

namespace FlavourFlow.Domains
{
    public class Instruction
    {
        [Key]
        public int InstructionId { get; set; }
        public int StepNumber { get; set; }
        public string StepDescription { get; set; } = "";

        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
    }
}