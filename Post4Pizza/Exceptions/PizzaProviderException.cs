using System;

namespace Post4Pizza.Exceptions
{
    public class PizzaProviderException : Exception
    {
        public PizzaProviderException(string message) : base(message)
        {
        }
    }
}