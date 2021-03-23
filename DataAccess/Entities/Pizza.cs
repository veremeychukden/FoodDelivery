using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Pizza
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        public int Price { get; set; }
        public string Photo { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        public float ProductRating { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}