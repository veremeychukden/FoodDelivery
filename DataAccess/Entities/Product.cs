using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("Products")]
    public class Product
    {
        [Key] public int Id { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<Pizza> Pizzas { get; set; }
    }
}