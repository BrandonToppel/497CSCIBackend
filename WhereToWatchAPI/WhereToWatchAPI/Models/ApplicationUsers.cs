using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToWatchAPI.Models
{
    public class ApplicationUsers : IdentityUser<int>
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
}
