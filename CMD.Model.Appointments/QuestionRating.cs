namespace CMD.Model
{
    public class QuestionRating
    {
        public int Id { get; set; }
        public Question Question { get; set; }
        public int Rating { get; set; }
    }
}