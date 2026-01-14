using FlavourFlow.Domains;
using FlavourFlow.Models; // Ensure this matches where your Ingredient/Instruction classes are
using Microsoft.EntityFrameworkCore;

namespace FlavourFlow.Data
{
    public static class DbInitializer
    {
        public static void Initialize(FlavourFlowContext context)
        {
            // 1. Ensure the database exists
            context.Database.EnsureCreated();

            // 2. Check if we already have recipes. If yes, stop.
            if (context.Recipe.Any())
            {
                return;
            }

            // 3. Create Categories
            var asian = new Category { Name = "Asian", Type = "Cuisine" };
            var italian = new Category { Name = "Italian", Type = "Cuisine" };
            var mexican = new Category { Name = "Mexican", Type = "Cuisine" };
            var american = new Category { Name = "American", Type = "Cuisine" };

            // Add categories first to generate IDs
            context.Category.AddRange(asian, italian, mexican, american);
            context.SaveChanges();

            // 4. Create Recipes
            var recipes = new List<Recipe>
            {
                // --- ITALIAN ---
                new Recipe
                {
                    Title = "Spaghetti Carbonara",
                    Description = "Authentic Roman pasta with eggs, cheese, and guanciale.",
                    Difficulty = "Medium",
                    CookTime = 20,
                    ImageURL = "https://images.unsplash.com/photo-1612874742237-98280d207436?auto=format&fit=crop&w=800",
                    CategoryId = italian.CategoryId,
                    UserId = null, // System recipe
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Spaghetti", Quantity = "400g" },
                        new Ingredient { Name = "Eggs", Quantity = "3" },
                        new Ingredient { Name = "Pecorino Cheese", Quantity = "1 cup" },
                        new Ingredient { Name = "Guanciale", Quantity = "150g" }
                    },
                    Instructions = new List<Instruction>
                    {
                        new Instruction { StepNumber = 1, StepDescription = "Boil pasta. Fry guanciale until crisp." },
                        new Instruction { StepNumber = 2, StepDescription = "Mix eggs and cheese in a bowl." },
                        new Instruction { StepNumber = 3, StepDescription = "Toss hot pasta with egg mixture off heat to create sauce." }
                    }
                },

                // --- ASIAN ---
                new Recipe
                {
                    Title = "Spicy Basil Chicken",
                    Description = "Thai street food classic (Pad Krapow Gai).",
                    Difficulty = "Easy",
                    CookTime = 15,
                    ImageURL = "https://images.unsplash.com/photo-1589302168068-964664d93dc0?auto=format&fit=crop&w=800",
                    CategoryId = asian.CategoryId,
                    UserId = null,
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Chicken Mince", Quantity = "300g" },
                        new Ingredient { Name = "Basil Leaves", Quantity = "1 cup" },
                        new Ingredient { Name = "Bird's Eye Chili", Quantity = "4" },
                        new Ingredient { Name = "Soy Sauce", Quantity = "2 tbsp" }
                    },
                    Instructions = new List<Instruction>
                    {
                        new Instruction { StepNumber = 1, StepDescription = "Fry garlic and chilies in hot oil." },
                        new Instruction { StepNumber = 2, StepDescription = "Add chicken and stir-fry until cooked." },
                        new Instruction { StepNumber = 3, StepDescription = "Add sauces and basil, toss for 30 seconds." }
                    }
                }
            };

            context.Recipe.AddRange(recipes);
            context.SaveChanges();
        }
    }
}