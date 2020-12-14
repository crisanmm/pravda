using System;
using System.Globalization;

namespace UserService.Helpers
{
    /// <summary>
    /// Custom exception class for throwing UserService specific exceptions
    /// </summary>
    public class UserException : Exception
    {
        public UserException() : base()
        {
        }

        public UserException(string message) : base(message)
        {
        }

        public UserException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}