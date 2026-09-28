using System.ComponentModel.DataAnnotations;

namespace SpaceGeeks.Models
{
    public class Star
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public double ApparentMagnitude { get; set; }
        
        public double DistanceInLightYears { get; set; }
        
        public Hemisphere Hemisphere { get; set; }
        
        public string Constellation { get; set; } = string.Empty;
    }
}