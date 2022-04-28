using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.DTO.Appointments
{
    public class DoctorProfileDTO
    {
        public int? id { get; set; }
        public string doctor_name { get; set; }
        public ContactDetailDTO ContactDetails { get; set; }
        public string doctor_speciality { get; set; }
        public string doctor_npi_no { get; set; }
        public string doctor_practice_location { get; set; }
        public string doctor_profile_image { get; set; }
    }
}
