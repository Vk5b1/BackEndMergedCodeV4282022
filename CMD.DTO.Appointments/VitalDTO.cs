using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.DTO.Appointments
{
    public class VitalDTO
    {
        public int id { get; set; }
        public float ecg { get; set; }
        public float temperature { get; set; }
        public float diabetes { get; set; }
        public float respiration_rate { get; set; }
    }
}
