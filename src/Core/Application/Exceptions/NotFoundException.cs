using System;

namespace Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name)
            : base($"{name} is not found")
        {
        }
    }
}
