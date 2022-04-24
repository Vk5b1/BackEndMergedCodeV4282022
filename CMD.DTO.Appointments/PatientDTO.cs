using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.DTO.Appointments
{
    public class PatientDTO
    {
        // patient name, patient dob, patient picture,
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string PatientPicture { get; set; }
    }
}
