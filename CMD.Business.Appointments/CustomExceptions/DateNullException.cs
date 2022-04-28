using System;
using System.Runtime.Serialization;

namespace CMD.Business.Appointments.CustomExceptions
{
    [Serializable]
    public class DateNullException : ApplicationException
    {
        public DateNullException(string message="Date cannot be null") : base(message)
        {
        }

    }
}