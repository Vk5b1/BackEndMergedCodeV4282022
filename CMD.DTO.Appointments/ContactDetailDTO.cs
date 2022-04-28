using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.DTO.Appointments
{
    public class ContactDetailDTO
    {
        public int Id { get; set; }
        public string doctor_email_id { get; set; }
        public string doctor_phone_number { get; set; }
    }
}
