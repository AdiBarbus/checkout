namespace Checkout.DataAccess.Interfaces;

using Microsoft.AspNetCore.JsonPatch;
using Models;

public interface IBasketRepository
{
    Task<Basket> GetBasket(int id, CancellationToken cancellationToken);

    Task<Basket> GetBasketWithoutItems(int id, CancellationToken cancellationToken);

    Task<Basket> CreateBasket(Basket basket, CancellationToken cancellationToken);

    Task<Item> AddItem(Item item, CancellationToken cancellationToken);

    Task<int> CompleteBasket(Basket basket, JsonPatchDocument<Basket> patchedBasket, CancellationToken cancellationToken);
}