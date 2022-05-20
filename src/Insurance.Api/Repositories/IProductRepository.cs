using Insurance.Api.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Insurance.Api.Repositories
{
    public interface IProductRepository
    {
        Task<List<ProductType>> GetProductTypes();
        Task<Product> GetProduct(int productId);

    }
}
