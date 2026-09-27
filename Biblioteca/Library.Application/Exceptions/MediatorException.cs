using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Application.Exceptions
{
    public class MediatorException : Exception
    {
        public MediatorException(string message) : base(message)
        {

        }
    }
}
