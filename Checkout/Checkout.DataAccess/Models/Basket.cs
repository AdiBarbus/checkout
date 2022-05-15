namespace Checkout.DataAccess.Models;

using Base;

public class Basket : BaseAuditEntity
{
    public string Customer { get; set; }

    public bool PaysVat { get; set; }

    public bool IsClosed { get; set; }

    public bool IsPayed { get; set; }

    public IList<Item> Items { get; set; }
}