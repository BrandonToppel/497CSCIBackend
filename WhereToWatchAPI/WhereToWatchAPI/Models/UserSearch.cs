using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToWatchAPI.Models
{
    public class UserSearch
    {
        public int UserId { get; set; }
        public ApplicationUsers Users { get; set; }
        public int SearchId { get; set; }
        public SearchHistory SearchHistory { get; set; }
    }
}
