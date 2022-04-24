using System.Collections.Generic;

namespace CMD.Model.Appointments
{
    public class FeedBack
    {
        public int Id { get; set; }
        public ICollection<QuestionRating> Rating { get; set; }
        public string AdditionalComment { get; set; }
    }
}