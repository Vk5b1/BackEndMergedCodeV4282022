
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.DTO.Appointments
{
    public class PatientCardDTO
    {
        public int Id { get; set; }
        public string PatientPicture { get; set; }
        public string Name { get; set; }
        public string DOB { get; set; }
        public int Age { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
    }
}
