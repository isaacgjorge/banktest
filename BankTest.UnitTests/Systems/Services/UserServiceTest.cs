using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.Repository.Interfaces;
using FluentAssertions;
using Moq;
using Services;
using Xunit;

namespace UnitTests.Systems.Services;

public class UserServiceTest
{
    [Fact]
    public async Task GetById_WhenCalled_ReturnUser()
    {
        // Arrange
        User user = new User
        {
            Email = "isaac@isaac.com",
            Name = "isaac"
        };

        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository
            .Setup(_ => _.Get(It.IsAny<int>()))
            .ReturnsAsync(user);
        var sut = new UserService(mockUserRepository.Object);

        // Act
        var result = await sut.GetById(It.IsAny<int>());

        // Assert
        result.Should().Be(user);
    }
    
    [Fact]
    public async Task GetByIdEmailAndPassword_WhenCalled_ReturnUser()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository
            .Setup(_ => _.GetByEmail("test@test.com"))
            .ReturnsAsync(new User());
        var sut = new UserService(mockUserRepository.Object);

        // Act
        var result = await sut.GetByEmail("test@test.com");

        // Assert
        result.Should().BeOfType<User>();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetByIdEmailAndPassword_WhenPassEmailNullOrEmpty_ThrowException(string email)
    {
        // Arrange
      var mockUserRepository = new Mock<IUserRepository>();
      var sut = new UserService(mockUserRepository.Object);

        // Act
        Func<Task> act = async () => await sut.GetByEmail(email);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Email required");
    }

    [Fact]
    public async Task Add_WhenPassingNull_ThrowException()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var sut = new UserService(mockUserRepository.Object);

        // Act
        Func<Task> act = async () => await sut.Add(null);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("User cannot be null");
    }

    [Theory]
    [InlineData("","")]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData(null, "")]
    public async Task Add_WhenPassingEmailOrNameNullOrEmpty_ThrowException(string email, string name)
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var sut = new UserService(mockUserRepository.Object);

        // Act
        Func<Task> act = async () => await sut.Add(new User()
        {
            Email = email,
            Name = name
        });

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Email and Name are required");
    }

    [Fact]
    public async Task Add_WhenPassingUser_ReturnTrue()
    {
        // Arrange
        var user = new User
        {
            Email = "test@test.com",
            Name = "test"
        };

        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository
            .Setup(_ => _.Add(user))
            .ReturnsAsync(true);

        var sut = new UserService(mockUserRepository.Object);

        // Act
        var result = await sut.Add(user);

        // Assert
        result.Should().Be(true);
    }

}

