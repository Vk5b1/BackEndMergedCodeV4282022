using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.DTO.Appointments
{
    public class DoctorAppointmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string DoctorPicture { get; set; }
        public ICollection<string> Specialities { get; set; }
    }
}
