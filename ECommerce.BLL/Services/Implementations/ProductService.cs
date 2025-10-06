using ECommerce.Api.Services.Interfaces;
using ECommerce.BLL.DTOs;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.DAL.Models;
using ECommerce.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IFileService _fileService;

        public ProductService(IGenericRepository<Product> productRepo, IFileService fileService)
        {
            _productRepo = productRepo;
            _fileService = fileService;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
            => await _productRepo.GetAllAsync();

        public async Task<Product?> GetProductById(int id)
            => await _productRepo.GetByIdAsync(id);

        public async Task<Product> CreateProduct(ProductCreateDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Category = dto.Category,
                ProductCode = dto.ProductCode,
                Price = dto.Price,
                MinimumQuantity = dto.MinimumQuantity,
                DiscountRate = dto.DiscountRate
            };

            if (dto.Image != null)
                product.ImagePath = _fileService.SaveProductImage(dto.Image);

            await _productRepo.AddAsync(product);
            await _productRepo.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateProduct(int id, ProductUpdateDto dto)
        {
            var existing = await _productRepo.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Name = dto.Name;
            existing.Category = dto.Category;
            existing.ProductCode = dto.ProductCode;
            existing.Price = dto.Price;
            existing.MinimumQuantity = dto.MinimumQuantity;
            existing.DiscountRate = dto.DiscountRate;

            if (dto.Image != null)
                existing.ImagePath = _fileService.SaveProductImage(dto.Image);

            _productRepo.Update(existing);
            await _productRepo.SaveChangesAsync();
            return existing;
        }


        public async Task<bool> DeleteProduct(int id)
        {
            var existing = await _productRepo.GetByIdAsync(id);
            if (existing == null) return false;

            _productRepo.Delete(existing);
            await _productRepo.SaveChangesAsync();
            return true;
        }
    }
}
