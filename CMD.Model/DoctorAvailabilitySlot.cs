using System;

namespace CMD.Model
{
    public class DoctorAvailabilitySlot
    {
        public int Id { get; set; }
        public DateTime SlotDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public float SlotDuration { get; set; }
        public string Description { get; set; }
    }
}