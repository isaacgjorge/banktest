using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.Repository.Interfaces;
using Moq;
using Services;
using Xunit;
using FluentAssertions;

namespace UnitTests.Systems.Services;

public class AccountServiceTest
{
    [Fact]
    public async Task GetByAccountId_WhenCalled_ReturnAccount()
    {
        // Arrange
        var mockAccountRepository = new Mock<IAccountRepository>();
        mockAccountRepository
            .Setup(_ => _.Get(It.IsAny<int>()))
            .ReturnsAsync(new Account
            {
                AccountId = 1,
                UserId = 1,
                User = It.IsAny<User>(),
                Balance = 100
            });

        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        var result = await sut.GetByAccountId(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<Account>();
    }

    [Fact]
    public async Task GetAllByUserEmail_WhenCalled_ReturnListAccount()
    {
        // Arrange
        var mockAccountRepository = new Mock<IAccountRepository>();
        mockAccountRepository
            .Setup(_ => _.GetAllByUserEmail(It.IsAny<string>()))
            .ReturnsAsync( new List<Account>
            {
               It.IsAny<Account>()
            });

        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        var result = await sut.GetAllByUserEmail("Email@email.com");

        // Assert
        result.Should().BeOfType<List<Account>>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]

    public async Task GetAllByUserEmail_WhenCalledWithNullOrEmpty_ThrowAnException(string userEmail)
    {
        // Arrange
        var mockAccountRepository = new Mock<IAccountRepository>();
        mockAccountRepository
            .Setup(_ => _.GetAllByUserEmail(It.IsAny<string>()))
            .ReturnsAsync(new List<Account>
            {
                It.IsAny<Account>()
            });

        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        Func<Task> act = async () => await sut.GetAllByUserEmail(userEmail);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("User email is required");
    }

    [Fact]
    public async Task CreateAccount_WhenCalled_AndSuccess_returnTrue()
    {
        // Arrange
        Account account = new Account
        {
            AccountId = 1,
            UserId = 1,
            User = It.IsAny<User>(),
            Balance = 100
        };

        var mockAccountRepository = new Mock<IAccountRepository>();
        mockAccountRepository
            .Setup(_ => _.Add(It.IsAny<Account>()))
            .ReturnsAsync(true);

        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        var result = await sut.CreateAccount(account);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CreateAccount_WhenCalled_InitialBalanceMustBeGreaterThan100()
    {
        // Arrange
        Account account = new Account
        {
            AccountId = 1,
            UserId = 1,
            User = It.IsAny<User>(),
            Balance = 100
        };

        var mockAccountRepository = new Mock<IAccountRepository>();
        mockAccountRepository
            .Setup(_ => _.Add(It.IsAny<Account>()))
            .ReturnsAsync(true);

        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        var result = await sut.CreateAccount(account);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CreateAccount_WhenCalledWithInitialBalanceLessThan100_ThrowAnException()
    {
        // Arrange
        Account account = new Account
        {
            AccountId = 1,
            UserId = 1,
            User = It.IsAny<User>(),
            Balance = 99
        };

        var mockAccountRepository = new Mock<IAccountRepository>();
        mockAccountRepository
            .Setup(_ => _.Add(It.IsAny<Account>()))
            .ReturnsAsync(true);

        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        Func<Task> act = async () => await sut.CreateAccount(account);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Initial deposit must be greater than $100");
    }

    [Fact]
    public async Task CreateAccount_WhenCalled_AndFailure_returnFalse()
    {
        // Arrange
        Account account = new Account
        {
            AccountId = 1,
            UserId = 1,
            User = It.IsAny<User>(),
            Balance = 100
        };

        var mockAccountRepository = new Mock<IAccountRepository>();
        mockAccountRepository
            .Setup(_ => _.Add(It.IsAny<Account>()))
            .ReturnsAsync(false);

        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        var result = await sut.CreateAccount(account);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAccount_WhenCalled_AndSuccess_returnTrue()
    {
        // Arrange
        var mockAccountRepository = new Mock<IAccountRepository>();
        mockAccountRepository
            .Setup(_ => _.DeleteById(It.IsAny<int>()))
            .ReturnsAsync(true);

        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        var result = await sut.DeleteAccountByAccountId(It.IsAny<int>());

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAccount_WhenCalled_AndFailure_returnFalse()
    {
        // Arrange
        var mockAccountRepository = new Mock<IAccountRepository>();
        mockAccountRepository
            .Setup(_ => _.DeleteById(It.IsAny<int>()))
            .ReturnsAsync(false);

        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        var result = await sut.DeleteAccountByAccountId(It.IsAny<int>());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAccountByAccountId_WhenCalled_AndSuccess_returnTrue()
    {
        // Arrange
        var mockAccountRepository = new Mock<IAccountRepository>();
        mockAccountRepository
            .Setup(_ => _.DeleteById(It.IsAny<int>()))
            .ReturnsAsync(true);

        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        var result = await sut.DeleteAccountByAccountId(It.IsAny<int>());

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAccountByAccountId_WhenCalled_AndFailure_returnFalse()
    {
        // Arrange
        var mockAccountRepository = new Mock<IAccountRepository>();
        mockAccountRepository
            .Setup(_ => _.DeleteById(It.IsAny<int>()))
            .ReturnsAsync(false);

        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        var result = await sut.DeleteAccountByAccountId(It.IsAny<int>());

        // Assert
        result.Should().BeFalse();
    }


    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Withdraw_WhenCalled_ReturnsTrueOrFalse(bool expected)
    {
        // Arrange
        Account account = new Account
        {
            AccountId = 1,
            UserId = 1,
            User = It.IsAny<User>(),
            Balance = 1000
        };


        var mockAccountRepository = new Mock<IAccountRepository>();
        mockAccountRepository
            .Setup(_ => _.Get(It.IsAny<int>()))
            .ReturnsAsync(account);

        mockAccountRepository
            .Setup(_ => _.Update(It.IsAny<Account>()))
            .ReturnsAsync(expected);

        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        var result = await sut.Withdraw(It.IsAny<int>(), 800);
        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public async Task Withdraw_WhenCalled_ValueMustReduce()
    {
        // Arrange
        Account account = new Account
        {
            AccountId = 1,
            UserId = 1,
            User = It.IsAny<User>(),
            Balance = 1000
        };


        var mockAccountRepository = new Mock<IAccountRepository>();
        mockAccountRepository
            .Setup(_ => _.Get(It.IsAny<int>()))
            .ReturnsAsync(account);

        mockAccountRepository
            .Setup(_ => _.Update(It.IsAny<Account>()))
            .ReturnsAsync(true);

        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        var result = await sut.Withdraw(It.IsAny<int>(), 100);
        // Assert
        account.Balance.Should().Be(900);
    }


    [Fact]
    public async Task Withdraw_WhenCalled_ThrowExceptionWhenAmountIsGreaterThan90PercentOfBalance()
    {
        // Arrange
        Account account = new Account
        {
            AccountId = 1,
            UserId = 1,
            User = It.IsAny<User>(),
            Balance = 1000
        };


        var mockAccountRepository = new Mock<IAccountRepository>();
        mockAccountRepository
            .Setup(_ => _.Get(It.IsAny<int>()))
            .ReturnsAsync(account);

        mockAccountRepository
            .Setup(_ => _.Update(It.IsAny<Account>()))
            .ReturnsAsync(true);

        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        Func<Task> act = async () => await sut.Withdraw(It.IsAny<int>(), 999);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Withdraw amount cannot be more than 90% of total balance");
    }

    [Fact]
    public async Task Withdraw_WhenCalled_ThrowExceptionWhenResidualBalanceIsLessThan100()
    {
        // Arrange
        Account account = new Account
        {
            AccountId = 1,
            UserId = 1,
            User = It.IsAny<User>(),
            Balance = 100
        };

        var mockAccountRepository = new Mock<IAccountRepository>();
        mockAccountRepository
            .Setup(_ => _.Get(It.IsAny<int>()))
            .ReturnsAsync(account);

        mockAccountRepository
            .Setup(_ => _.Update(It.IsAny<Account>()))
            .ReturnsAsync(true);

        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        Func<Task> act = async () => await sut.Withdraw(It.IsAny<int>(), 1);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Account cannot have less than $100 at any time");
    }

    [Fact]
    public async Task Deposit_WhenCalled_ReturnTrue()
    {
        // Arrange
        Account account = new Account
        {
            AccountId = 1,
            UserId = 1,
            User = It.IsAny<User>(),
            Balance = 100
        };

        var mockAccountRepository = new Mock<IAccountRepository>();

        mockAccountRepository
            .Setup(_ => _.Get(It.IsAny<int>()))
            .ReturnsAsync(account);

        mockAccountRepository
            .Setup(_ => _.Update(It.IsAny<Account>()))
            .ReturnsAsync(true);


        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        var result = await sut.Deposit(It.IsAny<int>(), 100);

        // Assert
        account.Balance.Should().Be(200m);
    }

    [Fact]
    public async Task Deposit_WhenCalled_ThrowExceptionWhenAmountIsGreaterThan10000()
    {
        // Arrange
        var mockAccountRepository = new Mock<IAccountRepository>();
        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        Func<Task> act = async () => await sut.Deposit(It.IsAny<int>(), 10000.01m);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cannot deposit more than $10,000 in a single transaction");
    }

    [Fact]
    public async Task Deposit_WhenCalled_ValueMustIncrease()
    {
        // Arrange
        var mockAccountRepository = new Mock<IAccountRepository>();
        var sut = new AccountService(mockAccountRepository.Object);

        // Act
        Func<Task> act = async () => await sut.Deposit(It.IsAny<int>(), 10000.01m);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cannot deposit more than $10,000 in a single transaction");
    }

}

