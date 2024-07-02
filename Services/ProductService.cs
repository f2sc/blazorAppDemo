//namespace blazorAppDemo.Models.Services
using blazorAppDemo;
using blazorAppDemo.Models;
using blazorAppDemo.Pages;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;

namespace blazorAppDemo
{
    public class ProductService : IProductService
    {
        private readonly HttpClient client;

        private readonly JsonSerializerOptions options;

        public ProductService(HttpClient httpClient)
        {
            client = httpClient;
            options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
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
            var products = JsonSerializer.Deserialize<List<Product>>(content, options);

            // Verificar y decodificar las URLs de las imágenes si es necesario
            if (products != null)
            {
                foreach (var product in products)
                {
                    if (product.Images != null && product.Images.Length > 0)
                    {
                        product.Images = DecodeImages(product.Images);
                    }
                }
            }

            return products;
        }

        private string[] DecodeImages(string[] images)
        {
            // Las imágenes vienen como un JSON string, por lo tanto, decodificamos el primer elemento y deserializamos
            if (images.Length > 0)
            {
                var imagesJsonString = string.Join(",", images);
                var decodedImages = JsonSerializer.Deserialize<string[]>(imagesJsonString);
                return decodedImages.Select(url => System.Net.WebUtility.HtmlDecode(url)).ToArray();
            }

            return images;
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