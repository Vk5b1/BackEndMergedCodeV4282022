namespace CMD.Model.Appointments.Entities
{
    public class QuestionRating
    {
        public int Id { get; set; }
        public Question Question { get; set; }
        public int Rating { get; set; }
    }
}