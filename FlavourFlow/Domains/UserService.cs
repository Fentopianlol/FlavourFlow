using Microsoft.Data.SqlClient;
using FlavourFlow.Domains;
using FlavourFlow.Data;

namespace FlavourFlow.Services
{
    public class UserService
    {
        private readonly string _connectionString;

        public UserService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        // 1. Get the list of recipes a specific user has saved
        public List<Recipe> GetSavedRecipes(string userId)
        {
            List<Recipe> recipes = new List<Recipe>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // Join Recipe table with Saved_Recipe table
                // NOTE: This assumes you ran the SQL script to create the 'Saved_Recipe' table earlier
                string sql = @"
                    SELECT r.* FROM Recipe r 
                    JOIN Saved_Recipe s ON r.RecipeId = s.RecipeId 
                    WHERE s.UserId = @UserId";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            recipes.Add(new Recipe
                            {
                                RecipeId = (int)reader["RecipeId"],
                                Title = reader["Title"].ToString() ?? "",
                                Description = reader["Description"].ToString() ?? "",
                                CookTime = (int)reader["CookTime"],
                                Difficulty = reader["Difficulty"].ToString() ?? "",
                                ImageURL = reader["ImageURL"].ToString() ?? ""
                            });
                        }
                    }
                }
            }
            return recipes;
        }

        // 2. Get User Stats (Count of saved items)
        public int GetSavedCount(string userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Saved_Recipe WHERE UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}