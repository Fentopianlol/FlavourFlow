using FlavourFlow.Data;
using FlavourFlow.Domains;
using Microsoft.EntityFrameworkCore;

namespace FlavourFlow.Services // <--- FIXED: PLURAL
{
    public class UserService(FlavourFlowContext context)
    {
        public async Task<List<Recipe>> GetSavedRecipesAsync(string userId)
        {
            var user = await context.Users
                .Include(u => u.SavedRecipes)
                    .ThenInclude(r => r.Category)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.SavedRecipes ?? new List<Recipe>();
        }

        public async Task<int> GetSavedCountAsync(string userId)
        {
            var user = await context.Users
                .Include(u => u.SavedRecipes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.SavedRecipes.Count ?? 0;
        }

        public async Task ToggleSaveRecipeAsync(string userId, int recipeId)
        {
            var user = await context.Users
                .Include(u => u.SavedRecipes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var recipe = await context.Recipe.FindAsync(recipeId);

            if (user != null && recipe != null)
            {
                if (user.SavedRecipes.Contains(recipe))
                {
                    user.SavedRecipes.Remove(recipe);
                }
                else
                {
                    user.SavedRecipes.Add(recipe);
                }
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsRecipeSavedAsync(string userId, int recipeId)
        {
            var user = await context.Users
               .Include(u => u.SavedRecipes)
               .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.SavedRecipes.Any(r => r.RecipeId == recipeId) ?? false;
        }
    }
}