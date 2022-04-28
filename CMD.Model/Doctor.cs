using System.Collections.Generic;

namespace CMD.Model
{
    public class Doctor
    {
        public Doctor()
        {
            AvailabilitySlots = new HashSet<DoctorAvailabilitySlot>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string DoctorPicture { get; set; }
        public string NPINumber { get; set; }
        public string PracticeLocation { get; set; }
        public string Speciality { get; set; }
        public virtual ContactDetail ContactDetail { get; set; }
        public virtual ICollection<DoctorAvailabilitySlot> AvailabilitySlots { get; set; }
    }
}