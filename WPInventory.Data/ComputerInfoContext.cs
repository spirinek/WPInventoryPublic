using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WPInventory.Data.Models.Entities;

namespace WPInventory.Data
{
    public class ComputerInfoContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Computer> Computers { get; set; }
        public DbSet<CPU> CPUs { get; set; }
        public DbSet<HDD> HDDs { get; set; }
        public DbSet<ScanDate> ScanDates { get; set; }
        public DbSet<MotherBoard> MotherBoards { get; set; }
        public DbSet<RAM> RAMs { get; set; }
        public DbSet<VideoCard> VideoCards { get; set; }
        public DbSet<Monitor> Monitors { get; set; }
        public DbSet<NWAdapter> NWAdapters { get; set; }
        public DbSet<ADScope> AdScopes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)//FluentApi если потребуется.
        {
            base.OnModelCreating(modelBuilder);
        }
        public ComputerInfoContext(DbContextOptions<ComputerInfoContext> options) : base(options)
        {
            /*Database.EnsureCreated();*/ //for testing only
        }
       
    }
}
