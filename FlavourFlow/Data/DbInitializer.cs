using FlavourFlow.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlavourFlow.Data
{
    public static class DbInitializer
    {
        public static void Initialize(FlavourFlowContext context)
        {
            // 1. Ensure database is created
            try
            {
                context.Database.Migrate();
            }
            catch (Exception) { }

            // 2. Check if already seeded
            if (context.Recipe.Any())
            {
                return; // DB has been seeded
            }

            // ============================================================
            // 3. SEED USERS
            // ============================================================
            var hasher = new PasswordHasher<FlavourFlowUser>();
            var users = new List<FlavourFlowUser>
            {
                new() { Id = "user-1", UserName = "grandma_gina@example.com", Email = "grandma_gina@example.com", NormalizedUserName = "GRANDMA_GINA@EXAMPLE.COM", NormalizedEmail = "GRANDMA_GINA@EXAMPLE.COM", FirstName = "Gina", LastName = "Rossi", EmailConfirmed = true, SecurityStamp = Guid.NewGuid().ToString("D"), PasswordHash = hasher.HashPassword(null!, "Password123!") },
                new() { Id = "user-2", UserName = "chef_kenji@example.com", Email = "chef_kenji@example.com", NormalizedUserName = "CHEF_KENJI@EXAMPLE.COM", NormalizedEmail = "CHEF_KENJI@EXAMPLE.COM", FirstName = "Kenji", LastName = "Tanaka", EmailConfirmed = true, SecurityStamp = Guid.NewGuid().ToString("D"), PasswordHash = hasher.HashPassword(null!, "Password123!") },
                new() { Id = "user-3", UserName = "bbq_steve@example.com", Email = "bbq_steve@example.com", NormalizedUserName = "BBQ_STEVE@EXAMPLE.COM", NormalizedEmail = "BBQ_STEVE@EXAMPLE.COM", FirstName = "Steve", LastName = "Miller", EmailConfirmed = true, SecurityStamp = Guid.NewGuid().ToString("D"), PasswordHash = hasher.HashPassword(null!, "Password123!") },
                new() { Id = "user-4", UserName = "rosa_eats@example.com", Email = "rosa_eats@example.com", NormalizedUserName = "ROSA_EATS@EXAMPLE.COM", NormalizedEmail = "ROSA_EATS@EXAMPLE.COM", FirstName = "Rosa", LastName = "Diaz", EmailConfirmed = true, SecurityStamp = Guid.NewGuid().ToString("D"), PasswordHash = hasher.HashPassword(null!, "Password123!") },
                new() { Id = "user-5", UserName = "baker_sophie@example.com", Email = "baker_sophie@example.com", NormalizedUserName = "BAKER_SOPHIE@EXAMPLE.COM", NormalizedEmail = "BAKER_SOPHIE@EXAMPLE.COM", FirstName = "Sophie", LastName = "Laurent", EmailConfirmed = true, SecurityStamp = Guid.NewGuid().ToString("D"), PasswordHash = hasher.HashPassword(null!, "Password123!") }
            };

            foreach (var user in users)
            {
                if (!context.Users.Any(u => u.Id == user.Id)) context.Users.Add(user);
            }
            context.SaveChanges();

            // ============================================================
            // 4. SEED CATEGORIES
            // ============================================================
            var categories = new List<Category>
            {
                new() { Name = "Asian", Type = "Cuisine" },
                new() { Name = "Italian", Type = "Cuisine" },
                new() { Name = "Mexican", Type = "Cuisine" },
                new() { Name = "American", Type = "Cuisine" },
                new() { Name = "French", Type = "Cuisine" },
                new() { Name = "Dessert", Type = "Course" }
            };

            foreach (var c in categories)
            {
                if (!context.Category.Any(cat => cat.Name == c.Name)) context.Category.Add(c);
            }
            context.SaveChanges();

            // Fetch IDs for linking
            var asian = context.Category.First(c => c.Name == "Asian").CategoryId;
            var italian = context.Category.First(c => c.Name == "Italian").CategoryId;
            var mexican = context.Category.First(c => c.Name == "Mexican").CategoryId;
            var american = context.Category.First(c => c.Name == "American").CategoryId;
            var french = context.Category.First(c => c.Name == "French").CategoryId;

            // ============================================================
            // 5. SEED RECIPES
            // ============================================================
            var recipes = new List<Recipe>
            {
                // --- ITALIAN ---
                new() {
                    Title = "Classic Carbonara",
                    Description = "Authentic Roman pasta. No cream, just eggs, Pecorino Romano, and guanciale.",
                    Difficulty = "Medium", CookTime = 20,
                    ImageURL = "https://images.unsplash.com/photo-1612874742237-98280d207436?auto=format&fit=crop&w=800",
                    CategoryId = italian, UserId = "user-1", Tags = "Pasta,Italian,Dinner",
                    Ingredients = new List<Ingredient> { new() { Name = "Spaghetti", Quantity = "400g" }, new() { Name = "Eggs", Quantity = "3 large" }, new() { Name = "Guanciale", Quantity = "150g" }, new() { Name = "Pecorino Romano", Quantity = "1 cup" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Boil pasta in salted water." }, new() { StepNumber = 2, StepDescription = "Crisp the guanciale in a pan." }, new() { StepNumber = 3, StepDescription = "Mix eggs and cheese in a bowl." }, new() { StepNumber = 4, StepDescription = "Toss pasta with pork fat, remove from heat, and mix in egg mixture quickly." } }
                },
                new() {
                    Title = "Creamy Mushroom Risotto",
                    Description = "Rich, creamy, and comforting Italian rice dish made with arborio rice and white wine.",
                    Difficulty = "Hard", CookTime = 45,
                    ImageURL = "https://images.unsplash.com/photo-1476124369491-e7addf5db371?auto=format&fit=crop&w=800",
                    CategoryId = italian, UserId = "user-1", Tags = "Vegetarian,Italian,Gluten-Free",
                    Ingredients = new List<Ingredient> { new() { Name = "Arborio Rice", Quantity = "1.5 cups" }, new() { Name = "Mushrooms", Quantity = "300g" }, new() { Name = "Vegetable Stock", Quantity = "1L" }, new() { Name = "White Wine", Quantity = "1/2 cup" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Sauté onions and mushrooms." }, new() { StepNumber = 2, StepDescription = "Toast rice, then deglaze with wine." }, new() { StepNumber = 3, StepDescription = "Add stock ladle by ladle, stirring constantly." } }
                },

                // --- ASIAN ---
                new() {
                    Title = "Spicy Basil Chicken",
                    Description = "A Thai street food classic (Pad Krapow Gai). Fast, furious heat with aromatic basil.",
                    Difficulty = "Easy", CookTime = 15,
                    ImageURL = "https://images.unsplash.com/photo-1589302168068-964664d93dc0?auto=format&fit=crop&w=800",
                    CategoryId = asian, UserId = "user-2", Tags = "Spicy,Thai,Quick",
                    Ingredients = new List<Ingredient> { new() { Name = "Chicken Mince", Quantity = "300g" }, new() { Name = "Thai Basil", Quantity = "1 cup" }, new() { Name = "Bird's Eye Chili", Quantity = "3" }, new() { Name = "Soy Sauce", Quantity = "2 tbsp" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Fry garlic and chili until fragrant." }, new() { StepNumber = 2, StepDescription = "Add chicken and stir-fry until cooked." }, new() { StepNumber = 3, StepDescription = "Add sauces and fold in basil leaves." } }
                },
                new() {
                    Title = "Tonkotsu Ramen",
                    Description = "Rich pork bone broth ramen served with chashu pork and a soft-boiled egg.",
                    Difficulty = "Hard", CookTime = 360,
                    ImageURL = "https://images.unsplash.com/photo-1569718212165-3a8278d5f624?auto=format&fit=crop&w=800",
                    CategoryId = asian, UserId = "user-2", Tags = "Japanese,Soup,Comfort Food",
                    Ingredients = new List<Ingredient> { new() { Name = "Pork Bones", Quantity = "1kg" }, new() { Name = "Ramen Noodles", Quantity = "2 packs" }, new() { Name = "Chashu Pork", Quantity = "4 slices" }, new() { Name = "Egg", Quantity = "2" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Boil pork bones for 6-8 hours to make broth." }, new() { StepNumber = 2, StepDescription = "Cook noodles separately." }, new() { StepNumber = 3, StepDescription = "Assemble bowl with broth, noodles, and toppings." } }
                },

                // --- MEXICAN ---
                new() {
                    Title = "Baja Fish Tacos",
                    Description = "Crispy beer-battered fish served in soft corn tortillas with a tangy cabbage slaw.",
                    Difficulty = "Medium", CookTime = 30,
                    ImageURL = "https://images.unsplash.com/photo-1512838243147-8480a29cf7fc?auto=format&fit=crop&w=800",
                    CategoryId = mexican, UserId = "user-4", Tags = "Tacos,Mexican,Seafood",
                    Ingredients = new List<Ingredient> { new() { Name = "White Fish", Quantity = "500g" }, new() { Name = "Corn Tortillas", Quantity = "8" }, new() { Name = "Cabbage", Quantity = "1/2 head" }, new() { Name = "Lime Crema", Quantity = "1/2 cup" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Make batter with flour and beer." }, new() { StepNumber = 2, StepDescription = "Fry fish pieces until golden." }, new() { StepNumber = 3, StepDescription = "Warm tortillas and assemble tacos." } }
                },
                new() {
                    Title = "Loaded Nachos Supreme",
                    Description = "The ultimate movie night snack. Layers of chips, cheese, beans, and jalapeños.",
                    Difficulty = "Easy", CookTime = 20,
                    ImageURL = "https://images.unsplash.com/photo-1513456852971-30c0b8199d4d?auto=format&fit=crop&w=800",
                    CategoryId = mexican, UserId = "user-4", Tags = "Snack,Party,Cheesy",
                    Ingredients = new List<Ingredient> { new() { Name = "Tortilla Chips", Quantity = "1 bag" }, new() { Name = "Cheddar Cheese", Quantity = "200g" }, new() { Name = "Black Beans", Quantity = "1 can" }, new() { Name = "Jalapeños", Quantity = "1/4 cup" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Layer chips and cheese on a baking sheet." }, new() { StepNumber = 2, StepDescription = "Bake at 200°C for 10 minutes." }, new() { StepNumber = 3, StepDescription = "Top with salsa and sour cream." } }
                },

                // --- AMERICAN ---
                new() {
                    Title = "Ultimate Smash Burger",
                    Description = "Crispy caramelized edges, juicy center, and melted American cheese on a brioche bun.",
                    Difficulty = "Easy", CookTime = 15,
                    ImageURL = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?auto=format&fit=crop&w=800",
                    CategoryId = american, UserId = "user-3", Tags = "Burger,Beef,Cheat Meal",
                    Ingredients = new List<Ingredient> { new() { Name = "Ground Beef (80/20)", Quantity = "2 balls" }, new() { Name = "American Cheese", Quantity = "2 slices" }, new() { Name = "Brioche Bun", Quantity = "1" }, new() { Name = "Pickles", Quantity = "3 slices" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Get cast iron skillet screaming hot." }, new() { StepNumber = 2, StepDescription = "Smash beef ball onto skillet." }, new() { StepNumber = 3, StepDescription = "Flip, add cheese, and cover to melt." } }
                },
                new() {
                    Title = "Southern Fried Chicken",
                    Description = "Buttermilk soaked chicken dredged in spiced flour and fried to perfection.",
                    Difficulty = "Medium", CookTime = 60,
                    ImageURL = "https://images.unsplash.com/photo-1626082927389-6cd097cdc6ec?auto=format&fit=crop&w=800",
                    CategoryId = american, UserId = "user-3", Tags = "Fried Chicken,Comfort Food,Crispy",
                    Ingredients = new List<Ingredient> { new() { Name = "Chicken Drumsticks", Quantity = "6" }, new() { Name = "Buttermilk", Quantity = "2 cups" }, new() { Name = "Flour", Quantity = "2 cups" }, new() { Name = "Paprika", Quantity = "1 tbsp" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Marinate chicken in buttermilk overnight." }, new() { StepNumber = 2, StepDescription = "Dredge in seasoned flour." }, new() { StepNumber = 3, StepDescription = "Deep fry at 170°C for 12-15 minutes." } }
                },
                 new() {
                    Title = "New York Cheesecake",
                    Description = "Dense, rich, and creamy cheesecake with a graham cracker crust.",
                    Difficulty = "Medium", CookTime = 90,
                    ImageURL = "https://images.unsplash.com/photo-1508737027454-e6454ef45afd?auto=format&fit=crop&w=800",
                    CategoryId = american, UserId = "user-5", Tags = "Dessert,Sweet,Baking",
                    Ingredients = new List<Ingredient> { new() { Name = "Cream Cheese", Quantity = "900g" }, new() { Name = "Sugar", Quantity = "1.5 cups" }, new() { Name = "Eggs", Quantity = "4" }, new() { Name = "Graham Crackers", Quantity = "2 cups" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Bake crust for 10 mins." }, new() { StepNumber = 2, StepDescription = "Mix filling ingredients until smooth." }, new() { StepNumber = 3, StepDescription = "Bake in water bath at 160°C for 1 hour." } }
                },

                // --- FRENCH ---
                new() {
                    Title = "Classic Croissants",
                    Description = "Buttery, flaky, and golden brown. A labor of love worth every minute.",
                    Difficulty = "Hard", CookTime = 240,
                    ImageURL = "https://images.unsplash.com/photo-1555507036-ab1f40388085?auto=format&fit=crop&w=800",
                    CategoryId = french, UserId = "user-5", Tags = "Baking,Breakfast,Pastry",
                    Ingredients = new List<Ingredient> { new() { Name = "Bread Flour", Quantity = "500g" }, new() { Name = "Butter (Cold)", Quantity = "250g" }, new() { Name = "Yeast", Quantity = "10g" }, new() { Name = "Sugar", Quantity = "50g" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Make dough and chill." }, new() { StepNumber = 2, StepDescription = "Laminate butter into dough (fold 3 times)." }, new() { StepNumber = 3, StepDescription = "Shape and proof, then bake at 200°C." } }
                },
                new() {
                    Title = "Ratatouille",
                    Description = "A rustic French vegetable stew from Provence.",
                    Difficulty = "Medium", CookTime = 60,
                    ImageURL = "https://images.unsplash.com/photo-1572453800999-e8d2d1589b7c?auto=format&fit=crop&w=800",
                    CategoryId = french, UserId = "user-5", Tags = "Vegetarian,Healthy,Vegan",
                    Ingredients = new List<Ingredient> { new() { Name = "Eggplant", Quantity = "1" }, new() { Name = "Zucchini", Quantity = "2" }, new() { Name = "Bell Pepper", Quantity = "2" }, new() { Name = "Tomato Sauce", Quantity = "400g" } },
                    Instructions = new List<Instruction> { new() { StepNumber = 1, StepDescription = "Slice all vegetables thinly." }, new() { StepNumber = 2, StepDescription = "Layer in a baking dish over tomato sauce." }, new() { StepNumber = 3, StepDescription = "Cover and bake for 45 minutes." } }
                }
            };

            context.Recipe.AddRange(recipes);
            context.SaveChanges();

            // ============================================================
            // 6. SEED REVIEWS
            // ============================================================
            // Need to fetch recipes back from DB to get their generated IDs
            var dbRecipes = context.Recipe.ToList();
            var reviews = new List<Review>();

            var comments = new[]
            {
                "Absolutely delicious! Will make again.", "A bit too salty for my taste.",
                "My kids loved it!", "Easy to follow instructions.", "Added some extra chili, perfect.",
                "Better than the restaurant version!", "Took longer than expected.", "The texture was perfect."
            };

            var random = new Random();

            foreach (var r in dbRecipes)
            {
                // Add 1-4 reviews per recipe
                int reviewCount = random.Next(1, 5);
                for (int i = 0; i < reviewCount; i++)
                {
                    // Pick a random user who isn't the creator
                    var potentialReviewers = users.Where(u => u.Id != r.UserId).ToList();
                    var reviewer = potentialReviewers[random.Next(potentialReviewers.Count)];

                    reviews.Add(new Review
                    {
                        RecipeId = r.RecipeId,
                        UserId = reviewer.Id,
                        Rating = random.Next(3, 6), // Ratings between 3 and 5
                        Comment = comments[random.Next(comments.Length)],
                        CreatedAt = DateTime.Now.AddDays(-random.Next(1, 30))
                    });
                }
            }

            context.Review.AddRange(reviews);
            context.SaveChanges();
        }
    }
}