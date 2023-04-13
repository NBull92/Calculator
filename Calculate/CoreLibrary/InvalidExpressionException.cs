using System;

namespace CoreLibrary
{
    public sealed class InvalidExpressionException : Exception
    {
        public InvalidExpressionException() : base("Invalid Expression") { }
    }
}
