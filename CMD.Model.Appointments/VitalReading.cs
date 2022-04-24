namespace CMD.Model.Appointments
{
    public class VitalReading
    {
        public int Id { get; set; }
        public Vital Vital { get; set; }
        public float Reading { get; set; }
    }
}