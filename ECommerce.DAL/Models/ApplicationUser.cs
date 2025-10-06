using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace ECommerce.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        
        public DateTime? LastLoginTime { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

       
    }
}
