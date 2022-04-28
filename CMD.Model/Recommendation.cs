namespace CMD.Model
{
    public class Recommedation
    {
        public int RecommedationId { get; set; }
        public Doctor RecommendedDoctor { get; set; }
        public int DoctorId { get; set; }
        public int AppointmentId { get; set; }
    }
}