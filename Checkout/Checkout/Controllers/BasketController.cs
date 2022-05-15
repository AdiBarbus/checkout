namespace Checkout.API.Controllers;

using Business.Exceptions;
using Business.Interfaces;
using Checkout.Business.ViewModels;
using DataAccess.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/baskets")]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;
    private readonly ILogger<BasketController> _logger;

    public BasketController(IBasketService basketService, ILogger<BasketController> logger)
    {
        _basketService = basketService;
        _logger = logger;
    }

    [HttpGet("{basketId}")]
    [ProducesResponseType(typeof(BasketViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<BasketViewModel> GetBasket([FromRoute] int basketId, CancellationToken cancellationToken)
    {
        try
        {
            return await _basketService.GetBasketDetails(basketId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw new Exception(ex.Message, ex);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(Basket), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<Basket> CreateBasket(Basket basket, CancellationToken cancellationToken)
    {
        try
        {
            return await _basketService.CreateBasket(basket, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw new Exception(ex.Message, ex);
        }
    }

    [HttpPut("/{basketId}/article-line")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<Item> AddItem(int basketId, Item item, CancellationToken cancellationToken)
    {
        try
        {
            return await _basketService.CreateBasketItem(basketId, item, cancellationToken);
        }
        catch (BasketNotFoundException ex)
        {
            _logger.LogError(ex, $"The basket with id {basketId} was not found.");
            throw new BasketNotFoundException($"The basket with id {basketId} was not found.");
        }
        catch (CompletedBasketException ex)
        {
            _logger.LogError(ex, $"The basket with id {basketId} is already completed.");
            throw new CompletedBasketException($"The basket with id {basketId} is already completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw new Exception(ex.Message, ex);
        }
    }

    [HttpPatch("{basketId}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<bool> CompleteBasket([FromRoute] int basketId, JsonPatchDocument<Basket> patchedBasked, 
        CancellationToken cancellationToken)
    {
        try
        {
            return await _basketService.CompleteBasket(basketId, patchedBasked, cancellationToken);
        }
        catch (BasketNotFoundException ex)
        {
            _logger.LogError(ex, $"The basket with id {basketId} was not found.");
            throw new BasketNotFoundException($"The basket with id {basketId} was not found.");
        }
        catch (CompletedBasketException ex)
        {
            _logger.LogError(ex, $"The basket with id {basketId} is already completed.");
            throw new CompletedBasketException($"The basket with id {basketId} is already completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw new Exception(ex.Message, ex);
        }
    }
}