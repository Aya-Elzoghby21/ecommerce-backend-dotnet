using ECommerce.BLL.DTOs;
using ECommerce.DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.Services.Interfaces
{  
       public interface IProductService
        {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetProductById(int id);
        Task<Product> CreateProduct(ProductCreateDto dto);
        Task<Product?> UpdateProduct(int id, ProductUpdateDto dto);
        Task<bool> DeleteProduct(int id);
    }
    

}
