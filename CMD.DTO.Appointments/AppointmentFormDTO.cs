using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.DTO.Appointments
{
    public class AppointmentFormDTO
    {
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string Issue { get; set; }
        public string Reason { get; set; }
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }
    }
}
