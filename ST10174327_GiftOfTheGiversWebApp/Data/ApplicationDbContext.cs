using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ST10174327_GiftOfTheGiversWebApp.Models;

namespace ST10174327_GiftOfTheGiversWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Add ALL your DbSet properties here
        public DbSet<Disaster> Disaster { get; set; }
        public DbSet<GoodsDonation> GoodsDonation { get; set; }
        public DbSet<MoneyDonation> MoneyDonation { get; set; }
        public DbSet<Money> Money { get; set; }
        public DbSet<GoodsAllocation> GoodsAllocation { get; set; }
        public DbSet<MoneyAllocation> MoneyAllocation { get; set; }
        public DbSet<GoodsPurchase> GoodsPurchase { get; set; }
        public DbSet<GoodsInventory> GoodsInventory { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<VolunteerTask> VolunteerTasks { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Identity tables with custom names
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "Users");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            // Configure Disaster entity
            modelBuilder.Entity<Disaster>(entity =>
            {
                entity.HasKey(e => e.DISASTER_ID);
                entity.Property(e => e.DISASTER_ID).ValueGeneratedOnAdd();
                entity.Property(e => e.LOCATION).IsRequired().HasMaxLength(100);
                entity.Property(e => e.USERNAME).HasMaxLength(50);
                entity.Property(e => e.DESCRIPTION).HasMaxLength(500);
                entity.Property(e => e.AID_TYPE).IsRequired().HasMaxLength(100);
                entity.Property(e => e.STATUS).HasMaxLength(50);
            });

            // Configure GoodsDonation entity
            modelBuilder.Entity<GoodsDonation>(entity =>
            {
                entity.HasKey(e => e.GOODS_DONATION_ID);
                entity.Property(e => e.USERNAME).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ITEM_NAME).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CATEGORY).HasMaxLength(100);
                entity.Property(e => e.QUANTITY).IsRequired();
                entity.Property(e => e.DESCRIPTION).HasMaxLength(500);
                
                // Relationship with Disaster (optional)
                entity.HasOne(d => d.Disaster)
                    .WithMany()
                    .HasForeignKey(d => d.DISASTER_ID)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure MoneyDonation entity
            modelBuilder.Entity<MoneyDonation>(entity =>
            {
                entity.HasKey(e => e.MONEY_DONATION_ID);
                entity.Property(e => e.USERNAME).IsRequired().HasMaxLength(100);
                entity.Property(e => e.AMOUNT).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PAYMENT_METHOD).HasMaxLength(50);
                entity.Property(e => e.DATE).HasDefaultValueSql("GETDATE()");
                
                // Relationship with Disaster (optional)
                entity.HasOne(d => d.Disaster)
                    .WithMany()
                    .HasForeignKey(d => d.DISASTER_ID)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Money entity
            modelBuilder.Entity<Money>(entity =>
            {
                entity.HasKey(e => e.MONEY_ID);
                entity.Property(e => e.TOTAL_AMOUNT).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.REMAINING_AMOUNT).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.LAST_UPDATED).HasDefaultValueSql("GETDATE()");
            });

            // Configure GoodsInventory entity
            modelBuilder.Entity<GoodsInventory>(entity =>
            {
                entity.HasKey(e => e.GOODSINVENTORY_ID);
                entity.Property(e => e.ITEM_NAME).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CATEGORY).HasMaxLength(100);
                entity.Property(e => e.DESCRIPTION).HasMaxLength(500);
                entity.Property(e => e.QUANTITY).IsRequired();
                entity.Property(e => e.DATE_ADDED).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IS_AVAILABLE).HasDefaultValue(true);
            });

            // Configure MoneyAllocation entity
            modelBuilder.Entity<MoneyAllocation>(entity =>
            {
                entity.HasKey(e => e.ALLOCATION_ID);
                entity.Property(e => e.AMOUNT).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ALLOCATION_DATE).HasDefaultValueSql("GETDATE()");
                
                // Relationships
                entity.HasOne(ma => ma.Disaster)
                    .WithMany()
                    .HasForeignKey(ma => ma.DISASTER_ID)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(ma => ma.Money)
                    .WithMany()
                    .HasForeignKey(ma => ma.MONEY_ID)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure GoodsAllocation entity
            modelBuilder.Entity<GoodsAllocation>(entity =>
            {
                entity.HasKey(e => e.ALLOCATION_ID);
                entity.Property(e => e.QUANTITY).IsRequired();
                entity.Property(e => e.ALLOCATION_DATE).HasDefaultValueSql("GETDATE()");
                
                // Relationships
                entity.HasOne(ga => ga.Disaster)
                    .WithMany()
                    .HasForeignKey(ga => ga.DISASTER_ID)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(ga => ga.GoodsInventory)
                    .WithMany()
                    .HasForeignKey(ga => ga.GOODSINVENTORY_ID)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Volunteer entity
            modelBuilder.Entity<Volunteer>(entity =>
            {
                entity.HasKey(e => e.VOLUNTEER_ID);
                entity.Property(e => e.FIRST_NAME).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LAST_NAME).IsRequired().HasMaxLength(50);
                entity.Property(e => e.EMAIL).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PHONE).HasMaxLength(20);
                entity.Property(e => e.ADDRESS).HasMaxLength(200);
                entity.Property(e => e.CITY).HasMaxLength(100);
                entity.Property(e => e.PROVINCE).HasMaxLength(100);
                entity.Property(e => e.POSTAL_CODE).HasMaxLength(20);
                entity.Property(e => e.SKILLS).HasMaxLength(500);
                entity.Property(e => e.AVAILABILITY).HasMaxLength(200);
                entity.Property(e => e.USERNAME).IsRequired().HasMaxLength(100);
            });

            // Configure VolunteerTask entity
            modelBuilder.Entity<VolunteerTask>(entity =>
            {
                entity.HasKey(e => e.TASK_ID);
                entity.Property(e => e.TITLE).IsRequired().HasMaxLength(200);
                entity.Property(e => e.DESCRIPTION).HasMaxLength(1000);
                entity.Property(e => e.STATUS).HasMaxLength(50).HasDefaultValue("New");
                entity.Property(e => e.PRIORITY).HasMaxLength(20);
                entity.Property(e => e.LOCATION).HasMaxLength(200);
                entity.Property(e => e.SKILLS_REQUIRED).HasMaxLength(500);
                entity.Property(e => e.CREATED_DATE).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.DUE_DATE).IsRequired(false);
            });

            // Configure TaskAssignment entity
            modelBuilder.Entity<TaskAssignment>(entity =>
            {
                entity.HasKey(e => e.ASSIGNMENT_ID);
                entity.Property(e => e.ASSIGNMENT_DATE).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.STATUS).HasMaxLength(50).HasDefaultValue("Assigned");
                entity.Property(e => e.COMMENTS).HasMaxLength(1000);
                
                // Relationships
                entity.HasOne(ta => ta.Volunteer)
                    .WithMany()
                    .HasForeignKey(ta => ta.VOLUNTEER_ID)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(ta => ta.VolunteerTask)
                    .WithMany()
                    .HasForeignKey(ta => ta.TASK_ID)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}