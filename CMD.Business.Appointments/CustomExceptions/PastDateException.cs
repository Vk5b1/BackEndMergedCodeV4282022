using System;
using System.Runtime.Serialization;

namespace CMD.Business.Appointments.CustomExceptions
{
    [Serializable]
    public class PastDateException : Exception
    {
        public PastDateException(string message="Cannot book appointment for past dates") : base(message)
        {
        }

    }
}