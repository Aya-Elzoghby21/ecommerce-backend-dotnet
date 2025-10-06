using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.Models
{ 
     public class User
    {
            public int Id { get; set; }

            [Required, StringLength(50)]
            public string Username { get; set; }

            [Required]
            public string Password { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

            public DateTime? LastLoginTime { get; set; }

            public string RefreshToken { get; set; }

            public DateTime? RefreshTokenExpiry { get; set; }
        

    }
}
