using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public long UserId { get; set; }
        public bool IsBot { get; set; } = false;
        public string FirstName { get; set; } = string.Empty;
        public string? Login { get; set; } = null;
    }
}
