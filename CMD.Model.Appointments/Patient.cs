using System;
using System.Collections.Generic;

namespace CMD.Model.Appointments
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatientPicture { get; set; }
        public Gender Gender { get; set; }
        public DateTime DOB { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public int Height { get; set; }
        public virtual ContactDetail ContactDetail { get; set; }
        public virtual ICollection<Allergy> Allergies { get; set; }
    }
}