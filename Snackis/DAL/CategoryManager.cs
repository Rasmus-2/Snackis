using System.Text.Json;

namespace Snackis.DAL
{
    public class CategoryManager
    {
        private static Uri BaseAddress = new Uri("https://matapi.azurewebsites.net/");

        public async Task<List<Models.Category>> GetCategories()
        {
            List<Models.Category> categories = new List<Models.Category>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.GetAsync("api/Category");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    categories = JsonSerializer.Deserialize<List<Models.Category>>(responseString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                return categories;
            }
        }

        public async Task<Models.Category> GetCategory(int id)
        {
            Models.Category category = new Models.Category();

            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.GetAsync("api/Category/" + id);
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    category = JsonSerializer.Deserialize<Models.Category>(responseString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                return category;
            }
        }

        public async Task AddCategory(Models.Category category)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                var json = JsonSerializer.Serialize(category);

                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("api/Category/", httpContent);
            }
        }

        public async Task UpdateCategory(int id, Models.Category category)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                var json = JsonSerializer.Serialize(category, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("api/Category/" + id, httpContent);
            }
        }

        public async Task DeleteCategory(int id)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.DeleteAsync("api/Category/" + id);
            }
        }
    }
}
