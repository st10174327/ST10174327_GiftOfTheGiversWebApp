using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ST10174327_GiftOfTheGiversWebApp.Models;

namespace ST10174327_GiftOfTheGiversWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MoneyDonation> MoneyDonation { get; set; }
        public DbSet<GoodsDonation> GoodsDonation { get; set; }
        public DbSet<Disaster> Disaster { get; set; }
        public DbSet<Money> Money { get; set; }
        public DbSet<MoneyAllocation> MoneyAllocation { get; set; }
        public DbSet<GoodsAllocation> GoodsAllocation { get; set; }
        public DbSet<GoodsPurchase> GoodsPurchase { get; set; }
        public DbSet<GoodsInventory> GoodsInventory { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<VolunteerTask> VolunteerTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fix precision for decimal properties
            modelBuilder.Entity<GoodsPurchase>()
                .Property(g => g.GoodsPurchasePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Money>()
                .Property(m => m.TotalMoney)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Money>()
                .Property(m => m.RemainingMoney)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MoneyAllocation>()
                .Property(m => m.AllocationAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MoneyDonation>()
                .Property(m => m.AMOUNT)
                .HasPrecision(18, 2);

            // Configure date-only columns
            modelBuilder.Entity<Disaster>()
                .Property(d => d.STARTDATE)
                .HasColumnType("date");

            modelBuilder.Entity<Disaster>()
                .Property(d => d.ENDDATE)
                .HasColumnType("date");

            modelBuilder.Entity<MoneyDonation>()
                .Property(d => d.DATE)
                .HasColumnType("date");

            modelBuilder.Entity<GoodsDonation>()
                .Property(d => d.DATE)
                .HasColumnType("date");

            // Explicitly fix typo: map to Disaster table
            modelBuilder.Entity<Disaster>().ToTable("Disaster");

            // Configure relationships
            modelBuilder.Entity<MoneyAllocation>()
                .HasOne(ma => ma.Disaster)
                .WithMany(d => d.MoneyAllocations)
                .HasForeignKey(ma => ma.DISASTER_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GoodsAllocation>()
                .HasOne(ga => ga.Disaster)
                .WithMany(d => d.GoodsAllocations)
                .HasForeignKey(ga => ga.DISASTER_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GoodsAllocation>()
                .HasOne(ga => ga.GoodsInventory)
                .WithMany(gi => gi.GoodsAllocations)
                .HasForeignKey(ga => ga.GOODSINVENTORY_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MoneyDonation>()
                .HasOne(md => md.Disaster)
                .WithMany(d => d.MoneyDonations)
                .HasForeignKey(md => md.DISASTER_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GoodsDonation>()
                .HasOne(gd => gd.Disaster)
                .WithMany(d => d.GoodsDonations)
                .HasForeignKey(gd => gd.DISASTER_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GoodsPurchase>()
                .HasOne(gp => gp.GoodsInventory)
                .WithMany(gi => gi.GoodsPurchases)
                .HasForeignKey(gp => gp.GOODSINVENTORY_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Volunteer)
                .WithMany(v => v.Tasks)
                .HasForeignKey(ta => ta.VolunteerID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.VolunteerTask)
                .WithMany()
                .HasForeignKey(ta => ta.TaskID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}