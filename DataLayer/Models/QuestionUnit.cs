using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    /// <summary>
    /// table for all texts of questions
    /// </summary>
    public class QuestionUnit
    {
        [Key]
        public int Id { get; set; }
        public string TextQuestion { get; set; }  = string.Empty;

        public override string ToString()
        {
            return TextQuestion;
        }
    }
}
