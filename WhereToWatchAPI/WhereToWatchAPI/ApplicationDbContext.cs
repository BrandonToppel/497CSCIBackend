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
        public DbSet<Movies> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Watchlist>()
                .HasKey(um => new { um.UserId, um.MoviesId });
            builder.Entity<Watchlist>()
                .HasOne(um => um.Users)
                .WithMany(u => u.Watchlist)
                .HasForeignKey(um => um.UserId);
            builder.Entity<Watchlist>()
                .HasOne(um => um.Movies)
                .WithMany(m => m.Watchlist)
                .HasForeignKey(um => um.MoviesId);
        }

        public DbSet<WhereToWatchAPI.Models.Watchlist> Watchlist { get; set; }
    }
}
