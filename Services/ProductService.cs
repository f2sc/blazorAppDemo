//namespace blazorAppDemo.Models.Services
using blazorAppDemo;
using blazorAppDemo.Models;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;

namespace blazorAppDemo
{
    public class ProductService : IProductService
    {
        private readonly HttpClient client;

        private readonly JsonSerializerOptions options;

        public ProductService(HttpClient httpClient, JsonSerializerOptions optionsJson)
        {
            client = httpClient;
            options = optionsJson;
        }

        public async Task<List<Product>?> get()
        {
            var response = await client.GetAsync("v1/products");
            //return await JsonSerializer.DeserializeAsync<List<Product>>(await response.Content.ReadAsStreamAsync());
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            return JsonSerializer.Deserialize<List<Product>>(content, options);
        }

        public async Task Delete(int idProduct)
        {
            var response = await client.DeleteAsync($"v1/products/{idProduct}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
        }

        public async Task Add(Product product)
        {
            var response = await client.PostAsync("v1/products/", JsonContent.Create(product));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
        }

        //public async Task Update(Product product) 
        //{
        //    var response = await client.PutAsync($"v1/products/{product->id}",JsonContent.Create(product));
        //    var content = await response.Content.ReadAsStringAsync();
        //    if(!response.is)
        //}
    }
}

public interface IProductService
{
    Task<List<Product>?> get();

    Task Delete(int idProduct);

    Task Add(Product product);
}