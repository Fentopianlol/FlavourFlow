using FlavourFlow.Domains;
using FlavourFlow.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlavourFlow.Data
{
    public static class DbInitializer
    {
        public static void Initialize(FlavourFlowContext context)
        {

            // 1. Ensure the database exists
            context.Database.EnsureCreated();

            // 2. Avoid duplicates - if we have more than 5 recipes, assume we are already seeded.
            if (context.Recipe.Count() > 5)
            {
                return;
            }

            // 3. Seed Users (The "Chefs")
            var hasher = new PasswordHasher<FlavourFlowUser>();

            var users = new List<FlavourFlowUser>
            {
                new FlavourFlowUser
                {
                    Id = "user-1", // Hardcoded IDs for linking
                    UserName = "grandma_gina@example.com",
                    Email = "grandma_gina@example.com",
                    NormalizedUserName = "GRANDMA_GINA@EXAMPLE.COM",
                    NormalizedEmail = "GRANDMA_GINA@EXAMPLE.COM",
                    FirstName = "Gina",
                    LastName = "Rossi",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    PasswordHash = hasher.HashPassword(null!, "Password123!")
                },
                new FlavourFlowUser
                {
                    Id = "user-2",
                    UserName = "chef_kenji@example.com",
                    Email = "chef_kenji@example.com",
                    NormalizedUserName = "CHEF_KENJI@EXAMPLE.COM",
                    NormalizedEmail = "CHEF_KENJI@EXAMPLE.COM",
                    FirstName = "Kenji",
                    LastName = "Tanaka",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    PasswordHash = hasher.HashPassword(null!, "Password123!")
                },
                new FlavourFlowUser
                {
                    Id = "user-3",
                    UserName = "bbq_steve@example.com",
                    Email = "bbq_steve@example.com",
                    NormalizedUserName = "BBQ_STEVE@EXAMPLE.COM",
                    NormalizedEmail = "BBQ_STEVE@EXAMPLE.COM",
                    FirstName = "Steve",
                    LastName = "Miller",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    PasswordHash = hasher.HashPassword(null!, "Password123!")
                },
                new FlavourFlowUser
                {
                    Id = "user-4",
                    UserName = "rosa_eats@example.com",
                    Email = "rosa_eats@example.com",
                    NormalizedUserName = "ROSA_EATS@EXAMPLE.COM",
                    NormalizedEmail = "ROSA_EATS@EXAMPLE.COM",
                    FirstName = "Rosa",
                    LastName = "Diaz",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    PasswordHash = hasher.HashPassword(null!, "Password123!")
                }
            };

            // Only add users if they don't exist
            foreach (var user in users)
            {
                if (!context.Users.Any(u => u.Id == user.Id))
                {
                    context.Users.Add(user);
                }
            }
            context.SaveChanges();

            // 4. Seed Categories
            var categories = new List<Category>
            {
                new Category { Name = "Asian", Type = "Cuisine" },
                new Category { Name = "Italian", Type = "Cuisine" },
                new Category { Name = "Mexican", Type = "Cuisine" },
                new Category { Name = "American", Type = "Cuisine" },
                new Category { Name = "Breakfast", Type = "MealType" },
                new Category { Name = "Healthy", Type = "Dietary" }
            };

            foreach (var c in categories)
            {
                // Check by name to avoid duplicates
                if (!context.Category.Any(cat => cat.Name == c.Name))
                {
                    context.Category.Add(c);
                }
            }
            context.SaveChanges();

            // Refetch categories to get their generated IDs
            var asian = context.Category.First(c => c.Name == "Asian");
            var italian = context.Category.First(c => c.Name == "Italian");
            var mexican = context.Category.First(c => c.Name == "Mexican");
            var american = context.Category.First(c => c.Name == "American");
            var breakfast = context.Category.First(c => c.Name == "Breakfast");

            // 5. Seed Recipes
            var recipes = new List<Recipe>
            {
                // --- ITALIAN (Gina) ---
                new Recipe
                {
                    Title = "Classic Carbonara",
                    Description = "Authentic Roman pasta. No cream, just eggs and cheese.",
                    Difficulty = "Medium",
                    CookTime = 20, // Quick
                    ImageURL = "https://images.unsplash.com/photo-1612874742237-98280d207436?auto=format&fit=crop&w=800",
                    CategoryId = italian.CategoryId,
                    UserId = "user-1", // Gina
                    Ingredients = new List<Ingredient> { new() { Name = "Spaghetti", Quantity = "400g" }, new() { Name = "Eggs", Quantity = "3" }, new() { Name = "Guanciale", Quantity = "150g" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Boil pasta." }, new() { StepNumber = 2, StepDescription = "Fry guanciale." }, new() { StepNumber = 3, StepDescription = "Mix eggs and cheese, combine off heat." } }
                },
                new Recipe
                {
                    Title = "Homemade Lasagna",
                    Description = "Layers of rich meat sauce, creamy bechamel, and pasta sheets.",
                    Difficulty = "Hard",
                    CookTime = 120, // Leisurely
                    ImageURL = "https://images.unsplash.com/photo-1574868461052-6593098d6b1a?auto=format&fit=crop&w=800",
                    CategoryId = italian.CategoryId,
                    UserId = "user-1",
                    Ingredients = new List<Ingredient> { new() { Name = "Ground Beef", Quantity = "500g" }, new() { Name = "Lasagna Sheets", Quantity = "1 box" }, new() { Name = "Mozzarella", Quantity = "2 cups" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Make ragu sauce." }, new() { StepNumber = 2, StepDescription = "Make bechamel." }, new() { StepNumber = 3, StepDescription = "Layer and bake for 45 mins." } }
                },

                // --- ASIAN (Kenji) ---
                new Recipe
                {
                    Title = "Spicy Basil Chicken",
                    Description = "Thai street food classic (Pad Krapow Gai). Fast and flavorful.",
                    Difficulty = "Easy",
                    CookTime = 15, // Quick
                    ImageURL = "https://images.unsplash.com/photo-1589302168068-964664d93dc0?auto=format&fit=crop&w=800",
                    CategoryId = asian.CategoryId,
                    UserId = "user-2", // Kenji
                    Ingredients = new List<Ingredient> { new() { Name = "Chicken Mince", Quantity = "300g" }, new() { Name = "Basil", Quantity = "1 cup" }, new() { Name = "Chili", Quantity = "3" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Stir fry garlic and chili." }, new() { StepNumber = 2, StepDescription = "Add chicken." }, new() { StepNumber = 3, StepDescription = "Add sauce and basil." } }
                },
                new Recipe
                {
                    Title = "Tonkotsu Ramen",
                    Description = "Rich pork bone broth simmered for 12 hours.",
                    Difficulty = "Hard",
                    CookTime = 360, // Leisurely
                    ImageURL = "https://images.unsplash.com/photo-1569718212165-3a8278d5f624?auto=format&fit=crop&w=800",
                    CategoryId = asian.CategoryId,
                    UserId = "user-2",
                    Ingredients = new List<Ingredient> { new() { Name = "Pork Bones", Quantity = "1kg" }, new() { Name = "Ramen Noodles", Quantity = "4 packs" }, new() { Name = "Chashu Pork", Quantity = "8 slices" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Boil bones for 12 hours." }, new() { StepNumber = 2, StepDescription = "Strain broth." }, new() { StepNumber = 3, StepDescription = "Serve with noodles and toppings." } }
                },

                // --- MEXICAN (Rosa) ---
                new Recipe
                {
                    Title = "Baja Fish Tacos",
                    Description = "Crispy battered fish with tangy slaw and lime crema.",
                    Difficulty = "Medium",
                    CookTime = 25, // Quick
                    ImageURL = "https://images.unsplash.com/photo-1512838243147-8480a29cf7fc?auto=format&fit=crop&w=800",
                    CategoryId = mexican.CategoryId,
                    UserId = "user-4", // Rosa
                    Ingredients = new List<Ingredient> { new() { Name = "White Fish", Quantity = "400g" }, new() { Name = "Corn Tortillas", Quantity = "8" }, new() { Name = "Cabbage", Quantity = "1 cup" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Batter and fry fish." }, new() { StepNumber = 2, StepDescription = "Warm tortillas." }, new() { StepNumber = 3, StepDescription = "Assemble with slaw." } }
                },
                new Recipe
                {
                    Title = "Beef Enchiladas",
                    Description = "Rolled tortillas stuffed with beef and covered in red sauce.",
                    Difficulty = "Medium",
                    CookTime = 50, // Leisurely
                    ImageURL = "https://images.unsplash.com/photo-1534352956036-cd81e27dd615?auto=format&fit=crop&w=800",
                    CategoryId = mexican.CategoryId,
                    UserId = "user-4",
                    Ingredients = new List<Ingredient> { new() { Name = "Ground Beef", Quantity = "500g" }, new() { Name = "Tortillas", Quantity = "8" }, new() { Name = "Enchilada Sauce", Quantity = "1 can" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Cook beef." }, new() { StepNumber = 2, StepDescription = "Roll into tortillas." }, new() { StepNumber = 3, StepDescription = "Cover in sauce and cheese, bake." } }
                },

                // --- AMERICAN (Steve) ---
                new Recipe
                {
                    Title = "Smash Burger",
                    Description = "Crispy edges, juicy center, melted cheese. Perfection.",
                    Difficulty = "Easy",
                    CookTime = 15, // Quick
                    ImageURL = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?auto=format&fit=crop&w=800",
                    CategoryId = american.CategoryId,
                    UserId = "user-3", // Steve
                    Ingredients = new List<Ingredient> { new() { Name = "Ground Beef", Quantity = "2 patties" }, new() { Name = "American Cheese", Quantity = "2 slices" }, new() { Name = "Brioche Bun", Quantity = "1" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Smash beef onto hot griddle." }, new() { StepNumber = 2, StepDescription = "Flip and add cheese." }, new() { StepNumber = 3, StepDescription = "Toast bun and assemble." } }
                },
                new Recipe
                {
                    Title = "BBQ Ribs",
                    Description = "Fall-off-the-bone ribs slow cooked with smoky BBQ sauce.",
                    Difficulty = "Hard",
                    CookTime = 240, // Leisurely (4 hours)
                    ImageURL = "https://images.unsplash.com/photo-1544025162-d76690b67f14?auto=format&fit=crop&w=800",
                    CategoryId = american.CategoryId,
                    UserId = "user-3",
                    Ingredients = new List<Ingredient> { new() { Name = "Pork Ribs", Quantity = "1 rack" }, new() { Name = "BBQ Sauce", Quantity = "1 cup" }, new() { Name = "Dry Rub", Quantity = "2 tbsp" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Apply dry rub." }, new() { StepNumber = 2, StepDescription = "Slow roast at 150C for 3 hours." }, new() { StepNumber = 3, StepDescription = "Glaze with sauce and grill for 10 mins." } }
                },
                 new Recipe
                {
                    Title = "Fluffy Pancakes",
                    Description = "Tall, souffle-style pancakes with maple syrup.",
                    Difficulty = "Medium",
                    CookTime = 25, // Quick
                    ImageURL = "https://images.unsplash.com/photo-1506084868230-bb9d95c24759?auto=format&fit=crop&w=800",
                    CategoryId = breakfast.CategoryId,
                    UserId = "user-1",
                    Ingredients = new List<Ingredient> { new() { Name = "Flour", Quantity = "2 cups" }, new() { Name = "Milk", Quantity = "1.5 cups" }, new() { Name = "Eggs", Quantity = "2" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Whip egg whites." }, new() { StepNumber = 2, StepDescription = "Fold into batter." }, new() { StepNumber = 3, StepDescription = "Cook on low heat." } }
                }
            };

            context.Recipe.AddRange(recipes);
            context.SaveChanges();
        }
    }
}