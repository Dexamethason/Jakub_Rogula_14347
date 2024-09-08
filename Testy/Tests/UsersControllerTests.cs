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
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UsersController _usersController;
        private readonly IMapper _mapper;

        public UsersControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<User, UserDto>().ReverseMap();
            });

            _mapper = mappingConfig.CreateMapper();
            _usersController = new UsersController(_userServiceMock.Object, _mapper);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnOkResultWithUsers()
        {
            var users = new List<User>
            {
                new User { UserId = 1, Username = "user1", Email = "user1@example.com" },
                new User { UserId = 2, Username = "user2", Email = "user2@example.com" }
            };

            _userServiceMock.Setup(service => service.GetUsersAsync()).ReturnsAsync(users);

            var result = await _usersController.GetUsers();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<UserDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetUser_ShouldReturnOkResultWithUser()
        {
            var user = new User { UserId = 1, Username = "user1", Email = "user1@example.com" };

            _userServiceMock.Setup(service => service.GetUserByIdAsync(1)).ReturnsAsync(user);

            var result = await _usersController.GetUser(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal(user.UserId, returnValue.UserId);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnCreatedAtActionResult()
        {
            var userDto = new UserDto { UserId = 1, Username = "user1", Email = "user1@example.com" };
            var user = _mapper.Map<User>(userDto);

            _userServiceMock.Setup(service => service.CreateUserAsync(user)).Returns(Task.CompletedTask);

            var result = await _usersController.CreateUser(userDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<UserDto>(createdAtActionResult.Value);
            Assert.Equal(userDto.UserId, returnValue.UserId);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNoContentResult()
        {
            var userDto = new UserDto { UserId = 1, Username = "user1", Email = "user1@example.com" };
            var user = _mapper.Map<User>(userDto);

            _userServiceMock.Setup(service => service.GetUserByIdAsync(1)).ReturnsAsync(user);
            _userServiceMock.Setup(service => service.UpdateUserAsync(user)).Returns(Task.CompletedTask);

            var result = await _usersController.UpdateUser(1, userDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNoContentResult()
        {
            var user = new User { UserId = 1, Username = "user1", Email = "user1@example.com" };

            _userServiceMock.Setup(service => service.GetUserByIdAsync(1)).ReturnsAsync(user);
            _userServiceMock.Setup(service => service.DeleteUserAsync(1)).Returns(Task.CompletedTask);

            var result = await _usersController.DeleteUser(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}