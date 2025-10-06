using Xunit;
using Moq;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Services.Implementations;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.DAL.Models;
using ECommerce.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using ECommerce.Api.Services.Interfaces;
using ECommerce.BLL.DTOs;

namespace ECommerce.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IGenericRepository<Product>> _mockRepo;
        private readonly Mock<IFileService> _mockFileService;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _mockRepo = new Mock<IGenericRepository<Product>>();
            _mockFileService = new Mock<IFileService>();
            _service = new ProductService(_mockRepo.Object, _mockFileService.Object);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnListOfProducts()
        {
            
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Test 1", Category = "Cat1", Price = 10, MinimumQuantity = 1, DiscountRate = 0 },
                new Product { Id = 2, Name = "Test 2", Category = "Cat2", Price = 20, MinimumQuantity = 1, DiscountRate = 0 }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

         
            var result = await _service.GetAllProducts();

            result.Should().HaveCount(2);
            result.Should().Contain(p => p.Name == "Test 1");
        }

        [Fact]
        public async Task CreateProduct_ShouldSaveProduct()
        {
            var dto = new ProductCreateDto
            {
                Name = "Test",
                Category = "Cat1",
                ProductCode = "P001",
                Price = 10,
                MinimumQuantity = 1,
                DiscountRate = 0
               
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var result = await _service.CreateProduct(dto);

            result.Should().NotBeNull();
            result.Name.Should().Be("Test");
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task UpdateProduct_ShouldUpdateExistingProduct()
        {
            var existing = new Product
            {
                Id = 1,
                Name = "Old",
                Category = "Cat1",
                Price = 10,
                MinimumQuantity = 1,
                DiscountRate = 0
            };
            var updatedDto = new ProductUpdateDto
            {
                Name = "New",
                Category = "Cat2",
                ProductCode = "P002",
                Price = 20,
                MinimumQuantity = 2,
                DiscountRate = 5
                
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            var result = await _service.UpdateProduct(1, updatedDto);

            result.Should().NotBeNull();
            result.Name.Should().Be("New");
            _mockRepo.Verify(r => r.Update(existing), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task DeleteProduct_ShouldRemoveProduct()
        {
           
            var product = new Product { Id = 1, Name = "Test" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

           
            var result = await _service.DeleteProduct(1);

            
            result.Should().BeTrue();
            _mockRepo.Verify(r => r.Delete(product), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
