using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMD.Model
{
    public class Appointment
    {
        public Appointment()
        {
            Recommedations = new HashSet<Recommedation>();
            Status = AppointmentStatus.Open;
            FeedBack = new FeedBack();
        }

        public int Id { get; set; }
        public string Comment { get; set; }
        public virtual FeedBack FeedBack { get; set; }
        [Column(TypeName = "Date")]
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public string Reason { get; set; }
        public Issue Issue { get; set; }
        public Doctor Doctor { get; set; }
        public PatientDetail PatientDetail { get; set; }
        public ICollection<Recommedation> Recommedations { get; set; }
    }
}
