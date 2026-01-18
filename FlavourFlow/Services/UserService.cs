using Microsoft.EntityFrameworkCore;
using FlavourFlow.Data;
using FlavourFlow.Domains;

namespace FlavourFlow.Services
{
    public class UserService
    {
        private readonly FlavourFlowContext _context;

        public UserService(FlavourFlowContext context)
        {
            _context = context;
        }

        public async Task<bool> IsRecipeSavedAsync(string userId, int recipeId)
        {
            return await _context.SavedRecipes
                .AnyAsync(s => s.UserId == userId && s.RecipeId == recipeId);
        }

        public async Task ToggleSaveRecipeAsync(string userId, int recipeId)
        {
            // 1. Check if the "bookmark" exists in the SavedRecipes table
            var existingSave = await _context.SavedRecipes
                .FirstOrDefaultAsync(s => s.UserId == userId && s.RecipeId == recipeId);

            if (existingSave != null)
            {
                // UNSAVE: Remove the bookmark
                _context.SavedRecipes.Remove(existingSave);
            }
            else
            {
                // SAVE: Create a new bookmark entry
                var newSave = new SavedRecipe
                {
                    UserId = userId,
                    RecipeId = recipeId,
                    SavedAt = DateTime.Now
                };
                _context.SavedRecipes.Add(newSave);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<Recipe>> GetSavedRecipesAsync(string userId)
        {
            // Get recipes by joining with the SavedRecipes table
            return await _context.SavedRecipes
                .Where(s => s.UserId == userId)
                .Include(s => s.Recipe)
                    .ThenInclude(r => r.Category) // Include category info for display
                .Include(s => s.Recipe)
                    .ThenInclude(r => r.User)     // Include author info for display
                .Select(s => s.Recipe!)           // Select the actual Recipe object to return
                .ToListAsync();
        }
    }
}