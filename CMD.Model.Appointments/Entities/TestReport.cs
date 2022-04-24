namespace CMD.Model.Appointments.Entities
{
    public class TestReport
    {
        public int Id { get; set; }
        public Test Test { get; set; }
        public string Report { get; set; }
        public float Value { get; set; }
    }
}