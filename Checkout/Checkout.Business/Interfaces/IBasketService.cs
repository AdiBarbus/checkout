namespace Checkout.Business.Interfaces;

using DataAccess.Models;
using ViewModels;

public interface IBasketService
{
    Task<BasketViewModel> GetBasketDetails(int id, CancellationToken cancellationToken);
    Task<Basket> CreateBasket(Basket basket, CancellationToken cancellationToken);
    Task<Item> CreateBasketItem(int basketId, Item item, CancellationToken cancellationToken);
    Task<bool> CompleteBasket(int id, CancellationToken cancellationToken);
}