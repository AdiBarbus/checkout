namespace Checkout.UnitTests.BuilderHelpers;

using System.Collections.Generic;
using DataAccess.Models;

public static class ItemBuilder
{
    public static Item BuildItem(int id = 1, int basketId = 2)
    {
        return new Item
        {
            Id = id,
            BasketId = basketId,
            Name = $"Item {id}",
            Price = 20
        };
    }

    public static IList<Item> BuildItems(int count = 3)
    {
        var items = new List<Item>();

        for (var i = 0; i < count; i++)
        {
            items.Add(BuildItem(i + 1, i + 2));
        }

        return items;
    }
}