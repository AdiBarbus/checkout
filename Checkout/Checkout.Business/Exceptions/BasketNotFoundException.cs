namespace Checkout.Business.Exceptions
{
    public class BasketNotFoundException : Exception
    {
        public BasketNotFoundException()
        {
        }

        public BasketNotFoundException(string message)
            : base(message)
        {
        }
    }
}
