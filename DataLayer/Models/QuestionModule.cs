using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    /// <summary>
    /// question itself.
    /// contains all information about question
    /// </summary>
    public class QuestionModule
    {
        [Key]
        public int Id { get; set; }
        public string Descripion { get; set; } = "untagged questions module";

        [Required]
        public QuestionUnit FirstQuestion { get; set; }
        public int FirstQuestionId { get; set; }
        /// <summary>
        /// If SecondQuestion is NULL - its single question, If it`s not - it`s paired question
        /// </summary>
        public QuestionUnit? SecondQuestion { get; set; }
        public int? SecondQuestionId { get; set; }

        /// <summary>
        /// By answerBlock we can get AnswerVariants for this question
        /// </summary>
        [Required]
        public AnswerBlock FirstQuestionAnswerBlock { get; set; }
        public int FirstQuestionAnswerBlockId { get; set; }
        /// <summary>
        /// By answerBlock we can get AnswerVariants for this question
        /// </summary>
        public AnswerBlock? SecondQuestionAnswerBlock { get; set; }
        public int? SecondQuestionAnswerBlockId { get; set; }
        /// <summary>
        /// Json of Dictionary<string,string[]> stored.
        /// Where KEY - DATA of AnswerVariant
        /// And VALUE - array of relevant AnswerVariant`s DATAS
        /// </summary>
        public string JSONAnswerMatrix { get; set; }

        public bool FirstMultiselectable { get; set; } = false;
        public bool SecondMultiselectable { get; set; } = false;

        public override string ToString()
        {
            return Descripion;
        }
    }
}
