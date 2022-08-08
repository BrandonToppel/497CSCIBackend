using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToWatchAPI.Models
{
    public class ApplicationUsers : IdentityUser<int>
    {
        public string firstName { get; set; }
        public string lastName { get; set; }

        [NotMapped]
        public string password { get; set; }
        [NotMapped]
        public bool rememberMe { get; set; }
        public virtual ICollection<Watchlist> Watchlist { get; set; }
        public virtual ICollection<UserSearch> UserSearch { get; set; }
    }
}
