namespace Checkout.Business.Interfaces;

using DataAccess.Models;
using ViewModels;
using Microsoft.AspNetCore.JsonPatch;

public interface IBasketService
{
    Task<BasketViewModel> GetBasketDetails(int basketId, CancellationToken cancellationToken);

    Task<Basket> CreateBasket(Basket basket, CancellationToken cancellationToken);

    Task<Item> CreateBasketItem(int basketId, Item item, CancellationToken cancellationToken);

    Task<bool> CompleteBasket(int basketId, JsonPatchDocument<Basket> patchedBasket, CancellationToken cancellationToken);
}