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
    public class UserServiceTests
    {
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IRepository<User>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userService = new UserService(_userRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnUsers()
        {
            var users = new List<User>
            {
                new User { UserId = 1, Username = "user1", Email = "user1@example.com" },
                new User { UserId = 2, Username = "user2", Email = "user2@example.com" }
            };
            _userRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);
            var result = await _userService.GetUsersAsync();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser()
        {
            var user = new User { UserId = 1, Username = "user1", Email = "user1@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);
            var result = await _userService.GetUserByIdAsync(1);
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldAddUser()
        {
            var user = new User { UserId = 1, Username = "user1", Email = "user1@example.com" };
            await _userService.CreateUserAsync(user);
            _userRepositoryMock.Verify(repo => repo.AddAsync(user), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUser()
        {
            var user = new User { UserId = 1, Username = "user1", Email = "user1@example.com" };
            await _userService.UpdateUserAsync(user);
            _userRepositoryMock.Verify(repo => repo.UpdateAsync(user), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldRemoveUser()
        {
            var user = new User { UserId = 1, Username = "user1", Email = "user1@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);
            await _userService.DeleteUserAsync(1);
            _userRepositoryMock.Verify(repo => repo.DeleteAsync(user), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }
    }
}