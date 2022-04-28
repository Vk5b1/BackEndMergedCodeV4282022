using System;
using System.Runtime.Serialization;

namespace CMD.Business.Appointments.CustomExceptions
{
    [Serializable]
    public class SameAppointmentTimingException : Exception
    {
        public SameAppointmentTimingException(string message="Cannot book two appointment on same date and time") : base(message)
        {
        }

        
    }
}