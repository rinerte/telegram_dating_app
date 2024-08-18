using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class UsersStamp
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Form Form { get; set; }
        public int FormId { get; set; }
        public string Stamp { get; set; }
        public string Pattern { get; set; }
    }
}
