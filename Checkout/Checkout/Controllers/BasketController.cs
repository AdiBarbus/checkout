namespace Checkout.API.Controllers;

using Business.Interfaces;
using Checkout.Business.ViewModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/baskets")]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BasketViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<BasketViewModel> GetBasket([FromRoute] int id, CancellationToken cancellationToken)
    {
        return await _basketService.GetBasketDetails(id, cancellationToken);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Basket), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<Basket> CreateBasket(Basket basket, CancellationToken cancellationToken)
    {
        return await _basketService.CreateBasket(basket, cancellationToken);
    }

    [HttpPut("/{id}/article-line")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<Item> AddItem(int id, Item item, CancellationToken cancellationToken)
    {
        var createdItem = await _basketService.CreateBasketItem(id, item, cancellationToken);

        return createdItem;
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<bool> CompleteBasket([FromRoute] int id, CancellationToken cancellationToken)
    {
        return await _basketService.CompleteBasket(id, cancellationToken);
    }
}