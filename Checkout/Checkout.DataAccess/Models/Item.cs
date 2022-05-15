namespace Checkout.DataAccess.Models;

using Base;

public class Item : BaseAuditEntity
{
    public string? Name { get; set; }

    public double Price { get; set; }

    public int BasketId { get; set; }
}