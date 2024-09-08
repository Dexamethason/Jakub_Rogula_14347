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
    public class OrderServiceTests
    {
        private readonly Mock<IRepository<Order>> _orderRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IOrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepositoryMock = new Mock<IRepository<Order>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _orderService = new OrderService(_orderRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetOrdersAsync_ShouldReturnOrders()
        {
            var orders = new List<Order>
            {
                new Order { OrderId = 1, OrderDate = new System.DateTime(2023, 1, 1), UserId = 1 },
                new Order { OrderId = 2, OrderDate = new System.DateTime(2023, 1, 2), UserId = 2 }
            };
            _orderRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(orders);
            var result = await _orderService.GetOrdersAsync();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnOrder()
        {
            var order = new Order { OrderId = 1, OrderDate = new System.DateTime(2023, 1, 1), UserId = 1 };
            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(order);
            var result = await _orderService.GetOrderByIdAsync(1);
            Assert.Equal(order, result);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldAddOrder()
        {
            var order = new Order { OrderId = 1, OrderDate = new System.DateTime(2023, 1, 1), UserId = 1 };
            await _orderService.CreateOrderAsync(order);
            _orderRepositoryMock.Verify(repo => repo.AddAsync(order), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderAsync_ShouldUpdateOrder()
        {
            var order = new Order { OrderId = 1, OrderDate = new System.DateTime(2023, 1, 1), UserId = 1 };
            await _orderService.UpdateOrderAsync(order);
            _orderRepositoryMock.Verify(repo => repo.UpdateAsync(order), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldRemoveOrder()
        {
            var order = new Order { OrderId = 1, OrderDate = new System.DateTime(2023, 1, 1), UserId = 1 };
            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(order);
            await _orderService.DeleteOrderAsync(1);
            _orderRepositoryMock.Verify(repo => repo.DeleteAsync(order), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }
    }
}