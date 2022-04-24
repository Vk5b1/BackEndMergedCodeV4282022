using System.Collections.Generic;

namespace CMD.Model.Appointments.Entities
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
        public string DoctorPicture { get; set; }
        public int NPINumber { get; set; }
        public string PracticeLocation { get; set; }
        public ICollection<Speciality> Specialities { get; set; }
        public virtual ContactDetail ContactDetail { get; set; }
        public virtual ICollection<DoctorAvailabilitySlot> AvailabilitySlots { get; set; }
    }
}
