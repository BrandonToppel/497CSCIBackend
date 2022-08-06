using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToWatchAPI.Models
{
    public class Watchlist
    {
        public int UserId { get; set; }
        public ApplicationUsers Users { get; set; }
        public int MoviesId { get; set; }
        public Movies Movies { get; set; }
    }
}
