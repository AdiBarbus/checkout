namespace Checkout.Business.Services;

using AutoMapper;
using DataAccess.Interfaces;
using DataAccess.Models;
using Exceptions;
using Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using ViewModels;

public class BasketService : IBasketService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IMapper _mapper;

    public BasketService(IBasketRepository basketRepository, IMapper mapper)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
    }
    
    public async Task<BasketViewModel> GetBasketDetails(int basketId, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasket(basketId, cancellationToken);
        
        return _mapper.Map<BasketViewModel>(basket);
    }

    public async Task<Basket> CreateBasket(Basket basket, CancellationToken cancellationToken)
    {
        basket.SetAsCreated();

        return await _basketRepository.CreateBasket(basket, cancellationToken);
    }

    public async Task<Item> CreateBasketItem(int basketId, Item item, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasketWithoutItems(basketId, cancellationToken);

        ValidateBasket(basketId, basket);

        item.SetAsCreated();
        item.BasketId = basketId;

        return await _basketRepository.AddItem(item, cancellationToken);
    }
    
    public async Task<bool> CompleteBasket(int basketId, JsonPatchDocument<Basket> patchedBasket, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasketWithoutItems(basketId, cancellationToken);

        ValidateBasket(basketId, basket);

        basket.SetAsModified();

        var result = await _basketRepository.CompleteBasket(basket, patchedBasket, cancellationToken);

        return result > 0;
    }

    private static void ValidateBasket(int basketId, Basket basket)
    {
        if (basket is null)
        {
            throw new BasketNotFoundException($"The basket with id {basketId} was not found.");
        }

        if (basket.IsClosed || basket.IsPayed)
        {
            throw new CompletedBasketException($"The basket with id {basketId} is already completed.");
        }
    }
}