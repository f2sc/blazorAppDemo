//namespace blazorAppDemo.Models.Services
using blazorAppDemo;
using System.Text.Json;

namespace blazorAppDemo
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient client;

        private readonly JsonSerializerOptions options;

        public CategoryService(HttpClient httpClient)
        {
            client = httpClient;
            options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<Category>?> get()
        {
            var response = await client.GetAsync($"v1/categories");
            //return await JsonSerializer.DeserializeAsync<List<Category>>(await response.Content.ReadAsStreamAsync());
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            return JsonSerializer.Deserialize<List<Category>>(content, options);
        }
    }
}

public interface ICategoryService
{
    Task<List<Category>?> get();
}
