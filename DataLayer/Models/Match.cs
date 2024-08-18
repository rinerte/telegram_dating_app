using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }
        public UsersStamp UsersStamp { get; set; }
        public int UsersStampId { get; set; }
        public User FoundUser { get; set; }
        public int FoundUserId { get; set; }
        public bool MessageSent { get; set; } = false;
    }
}
