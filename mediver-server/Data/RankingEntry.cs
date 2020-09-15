using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mediver_server.Data
{
    public sealed class RankingEntry
    {
        public int Id { get; set; }

        public string RankingUserId { get; set; }
        public MediverUser RankingUser { get; set; }
        public float Rank { get; set; }
        
        public Uri RankedSiteId { get; set; }
        public RankedSite RankedSite { get; set; }
    }
}
