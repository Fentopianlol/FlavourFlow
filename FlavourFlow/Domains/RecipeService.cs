using FlavourFlow.Data;
using FlavourFlow.Domains;
using Microsoft.EntityFrameworkCore;

namespace FlavourFlow.Services
{
    public class RecipeService
    {
        private readonly FlavourFlowContext _context;

        public RecipeService(FlavourFlowContext context)
        {
            _context = context;
        }

        // Method 1: Get recipes for a specific user
        public async Task<List<Recipe>> GetRecipesForUserAsync(string userId)
        {
            return await _context.Recipe
                .Where(r => r.UserId == userId)
                .Include(r => r.Category)
                .ToListAsync();
        }

        // Method 2: Add a new recipe
        public async Task AddRecipeAsync(Recipe recipe)
        {
            _context.Recipe.Add(recipe);
            await _context.SaveChangesAsync();
        }

        // Method 3: Get a single recipe by ID
        public async Task<Recipe?> GetRecipeByIdAsync(int id)
        {
            return await _context.Recipe
                .Include(r => r.Ingredients)
                .Include(r => r.Instructions)
                .Include(r => r.Category)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.RecipeId == id); // <--- FIXED: uses RecipeId
        }

        // Method 4: Get recipes by Category
        public async Task<List<Recipe>> GetRecipesByCategoryAsync(string categoryName)
        {
            return await _context.Recipe
                .Include(r => r.Category)
                .Where(r => r.Category.Name == categoryName)
                .ToListAsync();
        }
    }
}