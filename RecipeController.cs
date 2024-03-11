using MealProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MealProject.Controllers
{
    public class RecipeController : Controller
    {
        private readonly HttpClient _httpClient;
        public RecipeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await GetCategories();
            return View(categories);
        }
        public async Task<IActionResult> Recipes(string category)
        {
            var recipes = await GetRecipesByCategory(category);
            return View(recipes);
        }       
        private async Task<List<Category>> GetCategories()
        {
            var response = await _httpClient.GetAsync("https://www.themealdb.com/api/json/v1/1/categories.php");
            var content = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<Dictionary<string, List<Category>>>(content)["categories"];
            return categories;
        }
        private async Task<List<Recipe>> GetRecipesByCategory(string category)
        {
            var response = await _httpClient.GetAsync($"https://www.themealdb.com/api/json/v1/1/filter.php?c={category}");
            var content = await response.Content.ReadAsStringAsync();
            var recipes = JsonConvert.DeserializeObject<Dictionary<string, List<Recipe>>>(content)["meals"];
            return recipes;
        }       
    }
}
