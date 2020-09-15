using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace mediver_server.Data
{
    public sealed class RankedSite
    {
        [Key]
        public Uri Uri { get; set; }
        public float Score => Ignore ? 0 : Rankings.Sum(x => x.Rank * x.RankingUser.Weight);
        public bool Ignore { get; set; }

        public List<RankingEntry> Rankings { get; set; } = new List<RankingEntry>();
    }
}
