namespace Checkout.UnitTests.Controllers;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Controllers;
using AutoMapper;
using BuilderHelpers;
using Business.Exceptions;
using Business.Interfaces;
using Business.MappingProfiles;
using Business.ViewModels;
using DataAccess.Models;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;

public class BasketControllerTests
{
    private readonly BasketController _basketController;
    private readonly IMapper _mapper;
    private readonly Mock<IBasketService> _basketServiceMock;

    public BasketControllerTests()
    {
        _basketServiceMock = new Mock<IBasketService>();
        var loggerMock = new Mock<ILogger<BasketController>>();

        _basketController = new BasketController(_basketServiceMock.Object, loggerMock.Object);

        var basketProfile = new BasketProfile();
        var itemProfile = new ItemProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile> { basketProfile, itemProfile }));
        _mapper = new Mapper(configuration);
    }

    [Fact]
    public async Task GetBasket_Should_Return_Basket()
    {
        // Arrange
        const int basketId = 10;
        var basket = BasketBuilder.BuildBasket(basketId);
        var expectedResult = _mapper.Map<BasketViewModel>(BasketBuilder.BuildBasket(basketId));

        _basketServiceMock.Setup(call => call.GetBasketDetails(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mapper.Map<BasketViewModel>(basket));

        // Act
        var result = await _basketController.GetBasket(basketId, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetBasket_Should_Return_Null()
    {
        // Arrange
        const int basketId = 10;

        _basketServiceMock.Setup(call => call.GetBasketDetails(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((BasketViewModel) null);

        // Act
        var result = await _basketController.GetBasket(basketId, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo((BasketViewModel) null);
    }

    [Fact]
    public async Task GetBasket_Should_Throw_Exception()
    {
        // Arrange
        const int basketId = 10;

        _basketServiceMock.Setup(call => call.GetBasketDetails(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Throws<Exception>();

        // Act
        Func<Task> act = async () => await _basketController.GetBasket(basketId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task CreateBasket_Should_Return_Created_Basket()
    {
        // Arrange
        var expectedResult = BasketBuilder.BuildBasket();

        _basketServiceMock.Setup(call => call.CreateBasket(It.IsAny<Basket>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(BasketBuilder.BuildBasket());

        // Act
        var result = await _basketController.CreateBasket(It.IsAny<Basket>(), CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task CreateBasket_Should_ThrowException()
    {
        // Arrange
        _basketServiceMock.Setup(call => call.CreateBasket(It.IsAny<Basket>(), It.IsAny<CancellationToken>()))
            .Throws<Exception>();

        // Act
        Func<Task> act = async () => await _basketController.CreateBasket(It.IsAny<Basket>(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    
    [Fact]
    public async Task AddItem_Should_Return_Created_Item()
    {
        // Arrange
        var expectedResult = ItemBuilder.BuildItem();

        _basketServiceMock.Setup(call => call.CreateBasketItem(It.IsAny<int>(), It.IsAny<Item>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ItemBuilder.BuildItem());

        // Act
        var result = await _basketController.AddItem(It.IsAny<int>(), It.IsAny<Item>(), CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task AddItem_Should_Throw_BasketNotFoundException()
    {
        // Arrange
        _basketServiceMock.Setup(call => call.CreateBasketItem(It.IsAny<int>(), It.IsAny<Item>(), It.IsAny<CancellationToken>()))
            .Throws<BasketNotFoundException>();

        // Act
        Func<Task> act = async () => await _basketController.AddItem(It.IsAny<int>(), It.IsAny<Item>(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BasketNotFoundException>();
    }

    [Fact]
    public async Task AddItem_Should_Throw_CompletedBasketException()
    {
        // Arrange
        _basketServiceMock.Setup(call => call.CreateBasketItem(It.IsAny<int>(), It.IsAny<Item>(), It.IsAny<CancellationToken>()))
            .Throws<CompletedBasketException>();

        // Act
        Func<Task> act = async () => await _basketController.AddItem(It.IsAny<int>(), It.IsAny<Item>(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CompletedBasketException>();
    }

    [Fact]
    public async Task AddItem_Should_Throw_Exception()
    {
        // Arrange
        _basketServiceMock.Setup(call => call.CreateBasketItem(It.IsAny<int>(), It.IsAny<Item>(), It.IsAny<CancellationToken>()))
            .Throws<Exception>();

        // Act
        Func<Task> act = async () => await _basketController.AddItem(It.IsAny<int>(), It.IsAny<Item>(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    
    [Fact]
    public async Task CompleteBasketShould_Return_True()
    {
        // Arrange
        _basketServiceMock.Setup(call => call.CompleteBasket(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Basket>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _basketController.CompleteBasket(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Basket>>(), CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CompleteBasket_Should_Throw_BasketNotFoundException()
    {
        // Arrange
        _basketServiceMock.Setup(call => call.CompleteBasket(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Basket>>(), It.IsAny<CancellationToken>()))
            .Throws<BasketNotFoundException>();

        // Act
        Func<Task> act = async () => await _basketController.CompleteBasket(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Basket>>(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BasketNotFoundException>();
    }

    [Fact]
    public async Task CompleteBasket_Should_Throw_CompletedBasketException()
    {
        // Arrange
        _basketServiceMock.Setup(call => call.CompleteBasket(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Basket>>(), It.IsAny<CancellationToken>()))
            .Throws<CompletedBasketException>();

        // Act
        Func<Task> act = async () => await _basketController.CompleteBasket(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Basket>>(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CompletedBasketException>();
    }

    [Fact]
    public async Task CompleteBasket_Should_Throw_Exception()
    {
        // Arrange
        _basketServiceMock.Setup(call => call.CompleteBasket(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Basket>>(), It.IsAny<CancellationToken>()))
            .Throws<Exception>();

        // Act
        Func<Task> act = async () => await _basketController.CompleteBasket(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Basket>>(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}