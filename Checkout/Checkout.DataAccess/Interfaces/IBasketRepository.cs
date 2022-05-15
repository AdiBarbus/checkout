namespace Checkout.DataAccess.Interfaces;

using Models;

public interface IBasketRepository
{
    Task<Basket> GetBasket(int id, CancellationToken cancellationToken);

    Task<Basket> CreateBasket(Basket basket, CancellationToken cancellationToken);

    Task<Item> AddItem(int basketId, Item item, CancellationToken cancellationToken);

    Task<int> SaveChangesAsync();
}