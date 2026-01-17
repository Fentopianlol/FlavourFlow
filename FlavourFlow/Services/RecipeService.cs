using FlavourFlow.Data;
using FlavourFlow.Domains;
using Microsoft.EntityFrameworkCore;

namespace FlavourFlow.Services
{
    public class RecipeService(FlavourFlowContext context)
    {
        public async Task<Recipe?> GetRecipeByIdAsync(int id)
        {
            return await context.Recipe
                .Include(r => r.Ingredients)
                .Include(r => r.Instructions)
                .Include(r => r.Category)
                .Include(r => r.User)
                // --- NEW: Include Reviews and the User who wrote them ---
                .Include(r => r.Reviews).ThenInclude(rv => rv.User)
                .FirstOrDefaultAsync(r => r.RecipeId == id);
        }

        public async Task<List<Recipe>> GetRecipesByCategoryAsync(string cuisineName)
        {
            return await context.Recipe
                .Include(r => r.Category)
                .Where(r => r.Category.Name == cuisineName)
                .ToListAsync();
        }

        public async Task<List<Recipe>> GetRecipesByCreatorAsync(string userId)
        {
            return await context.Recipe
                .Include(r => r.Category)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Recipe>> GetRecipesByFilterAsync(string filter)
        {
            var query = context.Recipe
                .Include(r => r.Category)
                .Include(r => r.User)
                .AsQueryable();

            if (filter == "Under 30 Min")
            {
                query = query.Where(r => r.CookTime <= 30);
            }
            else if (filter == "Leisurely Cook")
            {
                query = query.Where(r => r.CookTime > 30);
            }

            return await query.ToListAsync();
        }

        // --- NEW METHOD: Add Review ---
        public async Task AddReviewAsync(int recipeId, string userId, int rating, string comment)
        {
            var review = new Review
            {
                RecipeId = recipeId,
                UserId = userId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };
            context.Review.Add(review);
            await context.SaveChangesAsync();
        }
    }
}