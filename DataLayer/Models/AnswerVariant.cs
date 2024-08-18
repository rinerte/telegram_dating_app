using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class AnswerVariant
    {
        [Key]
        public int Id { get; set; }
        public AnswerBlock AnswerBlock { get; set; }
        public int AnswerBlockId { get; set; }
        /// <summary>
        /// data to show to an user
        /// </summary>
        public string Caption { get; set; } = String.Empty;
        /// <summary>
        /// data for internal purposes
        /// </summary>
        public string Data { get; set; }
        public override string ToString()
        {
            return Data + ") "+ Caption;
        }
    }
}
