using System;

namespace CMD.Model.Appointments
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public virtual FeedBack FeedBack { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public string Reason { get; set; }
        public Issue Issue { get; set; }
        public Doctor Doctor { get; set; }
        public PatientDetail PatientDetail { get; set; }
    }
}
