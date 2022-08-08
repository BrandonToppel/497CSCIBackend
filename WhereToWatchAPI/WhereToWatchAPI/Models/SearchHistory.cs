using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToWatchAPI.Models
{
    public class SearchHistory
    {
        public int Id { get; set; }
        public string Search { get; set; }
        public DateTime SearchDate { get; set; }
        public int SearchNumber { get; set; }
        public virtual ICollection<UserSearch> UserSearch { get; set; }
    }
}
