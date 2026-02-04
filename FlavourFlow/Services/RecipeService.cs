using Microsoft.EntityFrameworkCore;
using FlavourFlow.Data;
using FlavourFlow.Domains;

namespace FlavourFlow.Services
{
    public class RecipeService
    {
        private readonly FlavourFlowContext _context;

        public RecipeService(FlavourFlowContext context)
        {
            _context = context;
        }

        // --- 1. FETCHING & SEARCH ---
        public async Task<List<Recipe>> GetRecipesAsync()
        {
            return await _context.Recipe
                .Include(r => r.Category)
                .Include(r => r.User)
                .Include(r => r.Reviews)
                .OrderByDescending(r => r.DateCreated)
                .ToListAsync();
        }

        public async Task<Recipe?> GetRecipeByIdAsync(int id)
        {
            return await _context.Recipe
                .Include(r => r.Category)
                .Include(r => r.User)
                .Include(r => r.Ingredients)
                .Include(r => r.Instructions)
                .Include(r => r.Reviews)
                    .ThenInclude(rv => rv.User)
                .FirstOrDefaultAsync(r => r.RecipeId == id);
        }

        public async Task<List<Recipe>> GetRecipesByCreatorAsync(string userId)
        {
            return await _context.Recipe
                .Include(r => r.Category)
                .Include(r => r.Reviews)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.DateCreated)
                .ToListAsync();
        }

        public async Task<List<Recipe>> GetRecipesByFilterAsync(string categoryName)
        {
            if (categoryName == "Everything") return await GetRecipesAsync();

            return await _context.Recipe
                .Include(r => r.Category)
                .Include(r => r.Reviews)
                .Where(r => r.Category != null && r.Category.Name == categoryName)
                .OrderByDescending(r => r.DateCreated)
                .ToListAsync();
        }

        // Alias for CuisinePage/CategoryPage
        public async Task<List<Recipe>> GetRecipesByCategoryAsync(string categoryName)
        {
            return await GetRecipesByFilterAsync(categoryName);
        }

        public async Task<List<Recipe>> SearchRecipesAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) return await GetRecipesAsync();

            var lowerText = searchText.ToLower();
            return await _context.Recipe
                .Include(r => r.Category)
                .Include(r => r.Ingredients)
                .Include(r => r.Reviews)
                .Where(r => r.Title.ToLower().Contains(lowerText) ||
                            (r.Tags != null && r.Tags.ToLower().Contains(lowerText)) ||
                            r.Ingredients.Any(i => i.Name.ToLower().Contains(lowerText)))
                .OrderByDescending(r => r.DateCreated)
                .ToListAsync();
        }

        // --- 2. CREATION & DELETION ---
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<int> CreateRecipeAsync(Recipe recipe)
        {
            if (recipe.Ingredients == null) recipe.Ingredients = new List<Ingredient>();
            if (recipe.Instructions == null) recipe.Instructions = new List<Instruction>();

            _context.Recipe.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe.RecipeId;
        }

        public async Task DeleteRecipeAsync(int recipeId)
        {
            var recipe = await _context.Recipe.FindAsync(recipeId);
            if (recipe != null)
            {
                _context.Recipe.Remove(recipe);
                await _context.SaveChangesAsync();
            }
        }

        // --- 3. REVIEWS ---
        public async Task AddReviewAsync(int recipeId, string userId, int rating, string comment)
        {
            var review = new Review
            {
                RecipeId = recipeId,
                UserId = userId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now
            };
            _context.Review.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasUserReviewedAsync(int recipeId, string userId)
        {
            return await _context.Review.AnyAsync(r => r.RecipeId == recipeId && r.UserId == userId);
        }

        public async Task<List<Review>> GetReviewsByUserIdAsync(string userId)
        {
            return await _context.Review
                .Include(r => r.Recipe)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        // --- NEW: REVIEW MODERATION ---
        public async Task DeleteReviewAsync(int reviewId)
        {
            var review = await _context.Review.FindAsync(reviewId);
            if (review != null)
            {
                _context.Review.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
    }
}