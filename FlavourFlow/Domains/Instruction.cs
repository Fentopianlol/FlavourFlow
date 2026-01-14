namespace FlavourFlow.Models
{
    public class Instruction
    {
        public int InstructionId { get; set; }
        public int StepNumber { get; set; }
        public string StepDescription { get; set; } = "";
        public int RecipeId { get; set; }

    }
}
