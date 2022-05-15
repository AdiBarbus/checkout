namespace Checkout.UnitTests.Services;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BuilderHelpers;
using Business.Exceptions;
using Business.MappingProfiles;
using Business.Services;
using Business.ViewModels;
using DataAccess.Interfaces;
using DataAccess.Models;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using Xunit;

public class BasketServiceTests
{
    private readonly BasketService _basketService;
    private readonly IMapper _mapper;
    private readonly Mock<IBasketRepository> _basketRepositoryMock;

    public BasketServiceTests()
    {
        _basketRepositoryMock = new Mock<IBasketRepository>();

        var basketProfile = new BasketProfile();
        var itemProfile = new ItemProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile> { basketProfile, itemProfile }));
        _mapper = new Mapper(configuration);

        _basketService = new BasketService(_basketRepositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task GetBasketDetails_Should_Return_BasketViewModel()
    {
        // Arrange
        var expectedResult = _mapper.Map<BasketViewModel>(BasketBuilder.BuildBasket());

        _basketRepositoryMock.Setup(call => call.GetBasket(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(BasketBuilder.BuildBasket());

        // Act
        var result = await _basketService.GetBasketDetails(It.IsAny<int>(), CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task CreateBasket_Should_Return_Created_Basket()
    {
        // Arrange
        var expectedResult = BasketBuilder.BuildBasket();

        _basketRepositoryMock.Setup(call => call.CreateBasket(It.IsAny<Basket>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(BasketBuilder.BuildBasket());

        // Act
        var result = await _basketService.CreateBasket(BasketBuilder.BuildBasket(), CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task CreateBasketItem_Should_Return_Created_Item()
    {
        // Arrange
        var expectedResult = ItemBuilder.BuildItem();

        _basketRepositoryMock.Setup(call => call.GetBasketWithoutItems(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(BasketBuilder.BuildBasket());
        _basketRepositoryMock.Setup(call => call.AddItem(It.IsAny<Item>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ItemBuilder.BuildItem());

        // Act
        var result = await _basketService.CreateBasketItem(It.IsAny<int>(), ItemBuilder.BuildItem(), CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task CreateBasketItem_When_Basket_Is_Not_Found_Should_Throw_BasketNotFoundException()
    {
        // Arrange
        _basketRepositoryMock.Setup(call => call.GetBasketWithoutItems(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Basket)null);
        _basketRepositoryMock.Setup(call => call.AddItem(It.IsAny<Item>(), It.IsAny<CancellationToken>()))
            .Throws<BasketNotFoundException>();

        // Act
        Func<Task> act = async () => await _basketService.CreateBasketItem(It.IsAny<int>(), ItemBuilder.BuildItem(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BasketNotFoundException>();
        _basketRepositoryMock.Verify(call => call.AddItem(It.IsAny<Item>(), It.IsAny<CancellationToken>()), Times.Never);
    }
        
    [Fact]
    public async Task CreateBasketItem_When_Basket_Is_Completed_Found_Should_Throw_CompletedBasketException()
    {
        // Arrange
        _basketRepositoryMock.Setup(call => call.GetBasketWithoutItems(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(BasketBuilder.BuildBasket(isPayed:true, isClosed:true));
        _basketRepositoryMock.Setup(call => call.AddItem(It.IsAny<Item>(), It.IsAny<CancellationToken>()))
            .Throws<CompletedBasketException>();

        // Act
        Func<Task> act = async () => await _basketService.CreateBasketItem(It.IsAny<int>(), ItemBuilder.BuildItem(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CompletedBasketException>();
        _basketRepositoryMock.Verify(call => call.AddItem(It.IsAny<Item>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    public async Task CompleteBasket_Should_Return_ExpectedResult(int returnedResult, bool expectedResult)
    {
        // Arrange
        _basketRepositoryMock.Setup(call => call.GetBasketWithoutItems(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(BasketBuilder.BuildBasket());
        _basketRepositoryMock.Setup(call => call.CompleteBasket(It.IsAny<Basket>(), It.IsAny<JsonPatchDocument<Basket>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnedResult);

        // Act
        var result = await _basketService.CompleteBasket(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Basket>>(), CancellationToken.None);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task CompleteBasket_When_Basket_Is_Not_Found_Should_Throw_BasketNotFoundException()
    {
        // Arrange
        _basketRepositoryMock.Setup(call => call.GetBasketWithoutItems(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Basket)null);

        // Act
        Func<Task> act = async () => await _basketService.CompleteBasket(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Basket>>(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BasketNotFoundException>();
        _basketRepositoryMock.Verify(call => call.CompleteBasket(It.IsAny<Basket>(), It.IsAny<JsonPatchDocument<Basket>>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CompleteBasket_When_Basket_Is_Completed_Found_Should_Throw_CompletedBasketException()
    {
        // Arrange
        _basketRepositoryMock.Setup(call => call.GetBasketWithoutItems(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(BasketBuilder.BuildBasket(isPayed: true, isClosed: true));

        // Act
        Func<Task> act = async () => await _basketService.CompleteBasket(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Basket>>(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CompletedBasketException>();
        _basketRepositoryMock.Verify(call => call.CompleteBasket(It.IsAny<Basket>(), It.IsAny<JsonPatchDocument<Basket>>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}