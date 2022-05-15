namespace Checkout.Business.ViewModels;

public class BasketViewModel
{
    public int Id { get; set; }

    public double TotalNet { get; set; }

    public double TotalGross { get; set; }

    public string Customer { get; set; }

    public bool PaysVat { get; set; }

    public IEnumerable<ItemViewModel> Items { get; set; }
}