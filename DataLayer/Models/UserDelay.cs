using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class UserDelay
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public TimeSpan Delay { get; set; }
        public DateTime? LastMessageSent { get; set; }
        public int Counter { get; set; }
    }
}
