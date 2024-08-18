using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class AnswerBlock
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}
