namespace Checkout.Business.Exceptions
{
    using System;

    public class CompletedBasketException : Exception
    {
        public CompletedBasketException()
        {
        }

        public CompletedBasketException(string message)
            : base(message)
        {
        }
    }
}
