using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class UserAnswer
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public FormModuleConnection FormModuleConnection { get; set; }
        public int FormModuleConnectionId { get; set; }
        /// <summary>
        /// json of string[] answerVariant`s data
        /// </summary>
        public string FirstQuestionAnswerVariantsJSON { get; set; }
        /// <summary>
        /// json of string[] answerVariant`s data
        /// </summary>
        public string? SecondQuestionAnswerVariantsJSON { get; set; }
    }
}
