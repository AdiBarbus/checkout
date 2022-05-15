namespace Checkout.Business.Services;

using AutoMapper;
using DataAccess.Interfaces;
using DataAccess.Models;
using Interfaces;
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
    
    public async Task<BasketViewModel> GetBasketDetails(int id, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasket(id, cancellationToken);
        return _mapper.Map<BasketViewModel>(basket);
    }

    public async Task<Basket> CreateBasket(Basket basket, CancellationToken cancellationToken)
    {
        basket.SetAsCreated();
        return await _basketRepository.CreateBasket(basket, cancellationToken);
    }

    public async Task<Item> CreateBasketItem(int basketId, Item item, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasket(basketId, cancellationToken);

        if (IsNullOrCompletedBasket(basket))
        {
            return null;
        }

        item.SetAsCreated();
        item.BasketId = basketId;

        return await _basketRepository.AddItem(basketId, item, cancellationToken);
    }
    
    public async Task<bool> CompleteBasket(int id, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasket(id, cancellationToken);

        if (IsNullOrCompletedBasket(basket))
        {
            return false;
        }

        basket.SetAsModified();
        basket.IsClosed = true;
        basket.IsPayed = true;

        await _basketRepository.SaveChangesAsync();

        return true;
    }

    private static bool IsNullOrCompletedBasket(Basket basket)
    {
        return basket is null || basket.IsClosed || basket.IsPayed;
    }
}