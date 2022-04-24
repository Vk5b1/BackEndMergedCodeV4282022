using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.DTO.Appointments
{
    public class AppointmentFormDTO
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string AppointmentStatus { get; set; }
        public string Issue { get; set; }
        public string Reason { get; set; }
        public PatientDTO Patient { get; set; }
        public DoctorDTO Doctor { get; set; }
    }
}
