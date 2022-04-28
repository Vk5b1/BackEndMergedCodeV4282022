using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMD.Model
{
    public class Patient
    {
        public Patient()
        {
            Allergies = new HashSet<Allergy>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string PatientPicture { get; set; }
        public Gender Gender { get; set; }
        [Column(TypeName = "Date")]
        public DateTime DOB { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public int Height { get; set; }
        public virtual ContactDetail ContactDetail { get; set; }
        public virtual ICollection<Allergy> Allergies { get; set; }
    }
}