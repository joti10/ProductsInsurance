using Insurance.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Insurance.Api.Controllers;

namespace Insurance.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        //TODO : Read Base Address from Configuration
        private readonly ILogger<InsuranceController> logger;
        private const string baseAddress = "http://localhost:5002";
        private readonly HttpClient client;
        public ProductRepository()
        {
            this.client = new HttpClient { BaseAddress = new Uri(baseAddress) }; ;

        }
        public async Task<List<ProductType>> GetProductTypes()
        {
            var json = "";
            try
            {
                json = await client.GetAsync("/product_types").Result.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            var productTypes = JsonConvert.DeserializeObject<List<ProductType>>(json);

            return productTypes;

        }

        public async Task<Product> GetProduct(int productId)
        {
            var json = "";
            try
            {
                json = await client.GetAsync(string.Format("/products/{0:G}", productId)).Result.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            var product = JsonConvert.DeserializeObject<Product>(json);

            return product;
        }
    }
}
