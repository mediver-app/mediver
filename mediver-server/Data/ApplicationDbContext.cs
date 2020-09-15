using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace mediver_server.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<RankedSite> RankedSites { get; set; }
        public DbSet<RankingEntry> Rankings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RankingEntry>()
                .HasOne(x => x.RankingUser)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RankingEntry>()
                .HasOne(x => x.RankedSite)
                .WithMany(x => x.Rankings)
                .HasPrincipalKey(x => x.Uri)
                .HasForeignKey(x => x.RankedSiteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
