using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Direction
    {
        
        [Key]
        public int Id { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public int FlatNumber { get; set; }
        public int IntercomCode { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}