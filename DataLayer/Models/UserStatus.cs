using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class UserStatus
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Form Form { get; set; }
        public int FormId { get; set; }
        public Enums.UserStatuses Status { get; set; }
        public DateTime StatusAssignedAt { get; set; } = DateTime.Now.ToUniversalTime();
    }
}
