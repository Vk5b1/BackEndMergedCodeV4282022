using System.Collections.Generic;

namespace CMD.Model
{
    public class FeedBack
    {
        public int Id { get; set; }
        public ICollection<QuestionRating> Rating { get; set; }
        public string AdditionalComment { get; set; }
    }
}