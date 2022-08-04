using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhereToWatchAPI.Models;

namespace WhereToWatchAPI
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUsers, ApplicationRoles, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
            
        }
    }
}
