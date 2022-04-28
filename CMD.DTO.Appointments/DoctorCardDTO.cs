using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.DTO.Appointments
{
    public class DoctorCardDTO
    {
        public int Id { get; set; }
        public string DoctorPicture { get; set; }
        public string Name { get; set; }
        public string SpecialityName { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public string NPINumber { get; set; }
        public string PracticeLocation { get; set; }
    }
}
