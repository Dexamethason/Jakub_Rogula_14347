using Api.Application.Interfaces;
using Api.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testy.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IRepository<Product>> _productRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productService = new ProductService(_productRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetProductsAsync_ShouldReturnProducts()
        {
            var products = new List<Product>
            {
                new Product { ProductId = 1, Name = "Product1", Price = 10 },
                new Product { ProductId = 2, Name = "Product2", Price = 20 }
            };
            _productRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);
            var result = await _productService.GetProductsAsync();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct()
        {
            var product = new Product { ProductId = 1, Name = "Product1", Price = 10 };
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);
            var result = await _productService.GetProductByIdAsync(1);
            Assert.Equal(product, result);
        }

        [Fact]
        public async Task CreateProductAsync_ShouldAddProduct()
        {
            var product = new Product { ProductId = 1, Name = "Product1", Price = 10 };
            await _productService.CreateProductAsync(product);
            _productRepositoryMock.Verify(repo => repo.AddAsync(product), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProduct()
        {
            var product = new Product { ProductId = 1, Name = "Product1", Price = 10 };
            await _productService.UpdateProductAsync(product);
            _productRepositoryMock.Verify(repo => repo.UpdateAsync(product), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldRemoveProduct()
        {
            var product = new Product { ProductId = 1, Name = "Product1", Price = 10 };
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);
            await _productService.DeleteProductAsync(1);
            _productRepositoryMock.Verify(repo => repo.DeleteAsync(product), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }
    }
}