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

        // --- EXISTING BOOKMARK METHODS ---
        public async Task ToggleSaveRecipeAsync(string userId, int recipeId)
        {
            var existingSave = await _context.SavedRecipes
                .FirstOrDefaultAsync(s => s.UserId == userId && s.RecipeId == recipeId);

            if (existingSave != null)
            {
                _context.SavedRecipes.Remove(existingSave);
            }
            else
            {
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

        public async Task<bool> IsRecipeSavedAsync(string userId, int recipeId)
        {
            return await _context.SavedRecipes
                .AnyAsync(s => s.UserId == userId && s.RecipeId == recipeId);
        }

        public async Task<List<Recipe>> GetSavedRecipesAsync(string userId)
        {
            return await _context.SavedRecipes
                .Where(s => s.UserId == userId)
                .Include(s => s.Recipe)
                    .ThenInclude(r => r.Category) // Include category for display
                .Select(s => s.Recipe)
                .ToListAsync();
        }

        // --- NEW ADMIN METHODS ---

        // 1. Get All Users for Dashboard
        public async Task<List<FlavourFlowUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // 2. Toggle Ban (Lockout)
        public async Task ToggleUserBanAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                // If currently banned (LockoutEnd is in the future), unban them.
                if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.Now)
                {
                    user.LockoutEnd = null; // Unban
                }
                else
                {
                    // Ban for 100 years
                    user.LockoutEnd = DateTimeOffset.Now.AddYears(100);
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}