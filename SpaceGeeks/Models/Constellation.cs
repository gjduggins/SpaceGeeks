using System.ComponentModel.DataAnnotations;

namespace SpaceGeeks.Models
{
    public class Constellation
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public Hemisphere Hemisphere { get; set; }
        
        public List<string> Stars { get; set; } = new List<string>();
    }
}