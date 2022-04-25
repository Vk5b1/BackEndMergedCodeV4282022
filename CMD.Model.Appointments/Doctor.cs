using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMD.Model.Appointments
{
    public class Doctor
    {
        public Doctor()
        {
            Specialities = new HashSet<Speciality>();
            AvailabilitySlots = new HashSet<DoctorAvailabilitySlot>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Column(TypeName ="Date")]
        public DateTime DOB { get; set; }
        public string DoctorPicture { get; set; }
        public int NPINumber { get; set; }
        public string PracticeLocation { get; set; }
        public ICollection<Speciality> Specialities { get; set; }
        public virtual ContactDetail ContactDetail { get; set; }
        public virtual ICollection<DoctorAvailabilitySlot> AvailabilitySlots { get; set; }
    }
}
