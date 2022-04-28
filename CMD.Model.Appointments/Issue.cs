using System.ComponentModel.DataAnnotations;

namespace CMD.Model
{
    public class Issue
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}