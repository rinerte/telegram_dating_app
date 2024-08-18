using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    /// <summary>
    /// FORMS TABLE
    /// Name must be descriptive, it is shown to user
    /// </summary>
    public class Form
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "unnamed form";
        public string? Description { get; set; } = "some description";
        public bool Active { get; set; } = false;

        public override string ToString()
        {
            return Active ? Name + " [Active]" : Name + " [Disabled]";
        }
    }
    
}
