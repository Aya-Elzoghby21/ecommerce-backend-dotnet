using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.Models
{ 
    public class Product
    {
            public int Id { get; set; }

            [Required, StringLength(100)]
            public string Category { get; set; }

            [Required, StringLength(50)]
            public string ProductCode { get; set; }

            [Required, StringLength(200)]
            public string Name { get; set; }

            public string ImagePath { get; set; } 

            [Range(0.01, double.MaxValue)]
            public decimal Price { get; set; }

            [Range(1, int.MaxValue)]
            public int MinimumQuantity { get; set; }

            [Range(0, 100)]
            public double DiscountRate { get; set; }
        

    }
}
