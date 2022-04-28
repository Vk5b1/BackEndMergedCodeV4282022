using System.Collections.Generic;

namespace CMD.Model
{
    public class FeedBack
    {
        public FeedBack()
        {
            Rating = new HashSet<QuestionRating>();
        }
        public int Id { get; set; }
        public ICollection<QuestionRating> Rating { get; set; }
        public string AdditionalComment { get; set; }
    }
}