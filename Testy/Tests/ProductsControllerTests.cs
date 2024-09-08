using Api.Application.Interfaces;
using Api.Domain.Entities;
using Api.Presentation.Controllers;
using Api.Presentation.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testy.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductsController _productsController;
        private readonly IMapper _mapper;

        public ProductsControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<Product, ProductDto>().ReverseMap();
            });

            _mapper = mappingConfig.CreateMapper();
            _productsController = new ProductsController(_productServiceMock.Object, _mapper);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOkResultWithProducts()
        {
            var products = new List<Product>
            {
                new Product { ProductId = 1, Name = "Product1", Price = 10 },
                new Product { ProductId = 2, Name = "Product2", Price = 20 }
            };

            _productServiceMock.Setup(service => service.GetProductsAsync()).ReturnsAsync(products);

            var result = await _productsController.GetProducts();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnOkResultWithProduct()
        {
            var product = new Product { ProductId = 1, Name = "Product1", Price = 10 };

            _productServiceMock.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(product);

            var result = await _productsController.GetProduct(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(product.ProductId, returnValue.ProductId);
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnCreatedAtActionResult()
        {
            var productDto = new ProductDto { ProductId = 1, Name = "Product1", Price = 10 };
            var product = _mapper.Map<Product>(productDto);

            _productServiceMock.Setup(service => service.CreateProductAsync(product)).Returns(Task.CompletedTask);

            var result = await _productsController.CreateProduct(productDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<ProductDto>(createdAtActionResult.Value);
            Assert.Equal(productDto.ProductId, returnValue.ProductId);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnNoContentResult()
        {
            var productDto = new ProductDto { ProductId = 1, Name = "Product1", Price = 10 };
            var product = _mapper.Map<Product>(productDto);

            _productServiceMock.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(product);
            _productServiceMock.Setup(service => service.UpdateProductAsync(product)).Returns(Task.CompletedTask);

            var result = await _productsController.UpdateProduct(1, productDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNoContentResult()
        {
            var product = new Product { ProductId = 1, Name = "Product1", Price = 10 };

            _productServiceMock.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(product);
            _productServiceMock.Setup(service => service.DeleteProductAsync(1)).Returns(Task.CompletedTask);

            var result = await _productsController.DeleteProduct(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}