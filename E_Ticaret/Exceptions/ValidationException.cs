﻿namespace E_Ticaret.Exceptions
{
    public class ValidationException : AppException
    {
        public ValidationException(string message) : base(message, 400) { }
    }
}
