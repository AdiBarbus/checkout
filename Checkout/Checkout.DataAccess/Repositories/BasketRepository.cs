namespace Checkout.DataAccess.Repositories;

using DbContexts;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

public class BasketRepository : IBasketRepository
{
    private readonly CheckoutDbContext _dbContext;

    public BasketRepository(CheckoutDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Basket> GetBasket(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Baskets.Where(b => b.Id.Equals(id))
            .Include(b => b.Items)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Basket> CreateBasket(Basket basket, CancellationToken cancellationToken)
    {
        await _dbContext.Baskets.AddAsync(basket, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return basket;
    }

    public async Task<Item> AddItem(int basketId, Item item, CancellationToken cancellationToken)
    {
        await _dbContext.Items.AddAsync(item, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return item;
    }
    
    public Task<int> SaveChangesAsync()
    {
        return _dbContext.SaveChangesAsync();
    }
}