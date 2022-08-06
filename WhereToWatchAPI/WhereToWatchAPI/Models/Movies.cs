using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToWatchAPI.Models
{
    public class Movies
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Watchlist> Watchlist { get; set; }
    }
}
