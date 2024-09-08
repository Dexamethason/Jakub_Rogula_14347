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
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly OrdersController _ordersController;
        private readonly IMapper _mapper;

        public OrdersControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<Order, OrderDto>().ReverseMap();
                mc.CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            });

            _mapper = mappingConfig.CreateMapper();
            _ordersController = new OrdersController(_orderServiceMock.Object, _mapper);
        }

        [Fact]
        public async Task GetOrders_ShouldReturnOkResultWithOrders()
        {
            var orders = new List<Order>
            {
                new Order { OrderId = 1, OrderDate = new System.DateTime(2023, 1, 1), UserId = 1 },
                new Order { OrderId = 2, OrderDate = new System.DateTime(2023, 1, 2), UserId = 2 }
            };

            _orderServiceMock.Setup(service => service.GetOrdersAsync()).ReturnsAsync(orders);

            var result = await _ordersController.GetOrders();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<OrderDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetOrder_ShouldReturnOkResultWithOrder()
        {
            var order = new Order { OrderId = 1, OrderDate = new System.DateTime(2023, 1, 1), UserId = 1 };

            _orderServiceMock.Setup(service => service.GetOrderByIdAsync(1)).ReturnsAsync(order);

            var result = await _ordersController.GetOrder(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OrderDto>(okResult.Value);
            Assert.Equal(order.OrderId, returnValue.OrderId);
        }

        [Fact]
        public async Task CreateOrder_ShouldReturnCreatedAtActionResult()
        {
            var orderDto = new OrderDto { OrderId = 1, OrderDate = new System.DateTime(2023, 1, 1), UserId = 1 };
            var order = _mapper.Map<Order>(orderDto);

            _orderServiceMock.Setup(service => service.CreateOrderAsync(order)).Returns(Task.CompletedTask);

            var result = await _ordersController.CreateOrder(orderDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<OrderDto>(createdAtActionResult.Value);
            Assert.Equal(orderDto.OrderId, returnValue.OrderId);
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnNoContentResult()
        {
            var orderDto = new OrderDto { OrderId = 1, OrderDate = new System.DateTime(2023, 1, 1), UserId = 1 };
            var order = _mapper.Map<Order>(orderDto);

            _orderServiceMock.Setup(service => service.GetOrderByIdAsync(1)).ReturnsAsync(order);
            _orderServiceMock.Setup(service => service.UpdateOrderAsync(order)).Returns(Task.CompletedTask);

            var result = await _ordersController.UpdateOrder(1, orderDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteOrder_ShouldReturnNoContentResult()
        {
            var order = new Order { OrderId = 1, OrderDate = new System.DateTime(2023, 1, 1), UserId = 1 };

            _orderServiceMock.Setup(service => service.GetOrderByIdAsync(1)).ReturnsAsync(order);
            _orderServiceMock.Setup(service => service.DeleteOrderAsync(1)).Returns(Task.CompletedTask);

            var result = await _ordersController.DeleteOrder(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}