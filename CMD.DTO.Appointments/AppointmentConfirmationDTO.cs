using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.DTO.Appointments
{
    public class AppointmentConfirmationDTO
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string PatientName { get; set; }
        public DateTime PatientDOB { get; set; }
        public string IssueName { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSpeciality { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }
}
