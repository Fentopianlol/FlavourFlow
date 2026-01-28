using System.Net.Http.Json;
using FlavourFlow.Domains;
using System.Text.Json.Serialization;

namespace FlavourFlow.Services
{
    public class RecipeImportService
    {
        private readonly HttpClient _http;

        public RecipeImportService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://www.themealdb.com/api/json/v1/1/");
        }

        public async Task<Recipe?> GetRandomRecipeAsync()
        {
            try
            {
                var response = await _http.GetFromJsonAsync<MealDbResponse>("random.php");
                var meal = response?.Meals?.FirstOrDefault();
                if (meal == null) return null;

                return MapToDomain(meal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                return null;
            }
        }

        private Recipe MapToDomain(MealApiDto meal)
        {
            var recipe = new Recipe
            {
                Title = meal.StrMeal,
                Description = $"A delicious {meal.StrArea} {meal.StrCategory} dish imported from TheMealDB.",
                ImageURL = meal.StrMealThumb,
                CookTime = 45, // API doesn't provide time, default to 45
                Difficulty = "Medium",
                Tags = $"{meal.StrCategory},{meal.StrArea},Imported",
                Ingredients = new List<Ingredient>(),
                Instructions = new List<Instruction>()
            };

            // Parse Ingredients (API stores them as Ingredient1, Ingredient2...)
            AddIngredient(recipe, meal.StrIngredient1, meal.StrMeasure1);
            AddIngredient(recipe, meal.StrIngredient2, meal.StrMeasure2);
            AddIngredient(recipe, meal.StrIngredient3, meal.StrMeasure3);
            AddIngredient(recipe, meal.StrIngredient4, meal.StrMeasure4);
            AddIngredient(recipe, meal.StrIngredient5, meal.StrMeasure5);
            AddIngredient(recipe, meal.StrIngredient6, meal.StrMeasure6);
            AddIngredient(recipe, meal.StrIngredient7, meal.StrMeasure7);
            AddIngredient(recipe, meal.StrIngredient8, meal.StrMeasure8);
            AddIngredient(recipe, meal.StrIngredient9, meal.StrMeasure9);
            AddIngredient(recipe, meal.StrIngredient10, meal.StrMeasure10);

            // Parse Instructions (Split big text block into steps)
            var steps = meal.StrInstructions.Split(new[] { "\r\n", "\n", "." }, StringSplitOptions.RemoveEmptyEntries);
            int stepNum = 1;
            foreach (var step in steps)
            {
                if (!string.IsNullOrWhiteSpace(step) && step.Length > 10) // Filter out tiny junk strings
                {
                    recipe.Instructions.Add(new Instruction
                    {
                        StepNumber = stepNum++,
                        StepDescription = step.Trim()
                    });
                }
            }

            return recipe;
        }

        private void AddIngredient(Recipe r, string? name, string? measure)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                r.Ingredients.Add(new Ingredient
                {
                    Name = name.Trim(),
                    Quantity = string.IsNullOrWhiteSpace(measure) ? "to taste" : measure.Trim()
                });
            }
        }

        // --- Internal DTOs for JSON Parsing ---
        public class MealDbResponse
        {
            [JsonPropertyName("meals")]
            public List<MealApiDto>? Meals { get; set; }
        }

        public class MealApiDto
        {
            [JsonPropertyName("strMeal")] public string StrMeal { get; set; } = "";
            [JsonPropertyName("strCategory")] public string StrCategory { get; set; } = "";
            [JsonPropertyName("strArea")] public string StrArea { get; set; } = "";
            [JsonPropertyName("strInstructions")] public string StrInstructions { get; set; } = "";
            [JsonPropertyName("strMealThumb")] public string StrMealThumb { get; set; } = "";

            // Ingredients 1-10
            [JsonPropertyName("strIngredient1")] public string? StrIngredient1 { get; set; }
            [JsonPropertyName("strIngredient2")] public string? StrIngredient2 { get; set; }
            [JsonPropertyName("strIngredient3")] public string? StrIngredient3 { get; set; }
            [JsonPropertyName("strIngredient4")] public string? StrIngredient4 { get; set; }
            [JsonPropertyName("strIngredient5")] public string? StrIngredient5 { get; set; }
            [JsonPropertyName("strIngredient6")] public string? StrIngredient6 { get; set; }
            [JsonPropertyName("strIngredient7")] public string? StrIngredient7 { get; set; }
            [JsonPropertyName("strIngredient8")] public string? StrIngredient8 { get; set; }
            [JsonPropertyName("strIngredient9")] public string? StrIngredient9 { get; set; }
            [JsonPropertyName("strIngredient10")] public string? StrIngredient10 { get; set; }

            // Measures 1-10
            [JsonPropertyName("strMeasure1")] public string? StrMeasure1 { get; set; }
            [JsonPropertyName("strMeasure2")] public string? StrMeasure2 { get; set; }
            [JsonPropertyName("strMeasure3")] public string? StrMeasure3 { get; set; }
            [JsonPropertyName("strMeasure4")] public string? StrMeasure4 { get; set; }
            [JsonPropertyName("strMeasure5")] public string? StrMeasure5 { get; set; }
            [JsonPropertyName("strMeasure6")] public string? StrMeasure6 { get; set; }
            [JsonPropertyName("strMeasure7")] public string? StrMeasure7 { get; set; }
            [JsonPropertyName("strMeasure8")] public string? StrMeasure8 { get; set; }
            [JsonPropertyName("strMeasure9")] public string? StrMeasure9 { get; set; }
            [JsonPropertyName("strMeasure10")] public string? StrMeasure10 { get; set; }
        }
    }
}