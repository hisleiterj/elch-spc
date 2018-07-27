using System;

namespace Elchwinkel.Spc
{
    public class InvalidSpcFileException : Exception
    {
        public InvalidSpcFileException(string message) : base(message)
        {
        }
    }
}