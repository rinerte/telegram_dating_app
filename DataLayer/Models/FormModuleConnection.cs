using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    /// <summary>
    /// this table links questions to forms
    /// </summary>
    public class FormModuleConnection
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Form Form { get; set; }
        public int FormId { get; set; }
        [Required]
        public QuestionModule QuestionModule { get; set; }
        public int QuestionModuleId { get; set; }
        /// <summary>
        /// Number of question(s) in form.
        /// Start with 1, not 0
        /// </summary>
        [Required]
        public int NumberInSequence { get; set; } = 1;

        public override string ToString()
        {
            try
            {
                return QuestionModule.ToString();
            }
             catch (Exception ex)
            {
                return "module ID -  " + QuestionModuleId;
            }
            
        }

    }
}
