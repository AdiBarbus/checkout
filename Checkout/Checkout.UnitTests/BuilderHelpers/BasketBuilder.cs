namespace Checkout.UnitTests.BuilderHelpers;

using DataAccess.Models;

public static class BasketBuilder
{
    public static Basket BuildBasket(int id = 1, int itemsCount = 3, bool isPayed = false, bool isClosed = false)
    {
        return new Basket
        {
            Id = id,
            Customer = $"Customer {id}",
            PaysVat = true,
            IsClosed = isClosed,
            IsPayed = isPayed,
            CreatedBy = "User",
            ModifiedBy = "User",
            Items = ItemBuilder.BuildItems(itemsCount)
        };
    }
}