using AutoMapper;
using FitnessApp_Domain.Common;
using FitnessApp_Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;
using FitnessApp_Domain.Entities.Fitness;
using FitnessApp_Domain.Entities.Helper;

namespace FitnessApp_Infrastructure
{
    public class DBContext : IdentityDbContext<Users, Roles, Guid, UserClaims, UserRoles, UserLogins, RoleClaims, UserTokens>
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region Identity
            // Map entities to their new tables names
            modelBuilder.Entity<Users>(table =>
            {
                table.ToTable("Users");
                // Each User can have many UserRoles
                table.HasMany(e => e.UserRoles)
                    .WithOne()
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                table.Property(u => u.Email).IsRequired(false);
                table.Property(u => u.PhoneNumber).IsRequired(false);

                table.HasQueryFilter(u => !u.IsDeleted);
              
                // Each User can have many UserClaims
                table.HasMany(e => e.UserClaims)
                    .WithOne()
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();
                // Each User can have many UserLogins
                table.HasMany(e => e.UserLogins)
                    .WithOne()
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                table.HasMany(e => e.UserTokens)
                    .WithOne()
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<UserClaims>(table =>
            {
                table.ToTable("UserClaims");
            });
            modelBuilder.Entity<UserLogins>(table =>
            {
                table.ToTable("UserLogins");
            });
            modelBuilder.Entity<UserTokens>(table =>
            {
                table.ToTable("UserTokens");
            });
            modelBuilder.Entity<Roles>(table =>
            {
                table.ToTable("Roles");
                // Each Role can have many UserRoles
                table.HasMany(e => e.UserRoles)
                    .WithOne()
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            modelBuilder.Entity<RoleClaims>(table =>
            {
                table.ToTable("RoleClaims");
            });
            modelBuilder.Entity<UserRoles>(table =>
            {
                table.ToTable("UserRoles");
            });
            modelBuilder.Entity<UserRoles>()
                .HasOne(ur => ur.Role)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
            modelBuilder.Entity<UserRoles>()
                .HasOne(ur => ur.User)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<IdentityServer4.EntityFramework.Entities.PersistedGrant>(table =>
            {
                table.HasKey(t => t.Key);
            });
            #endregion
            #region Fitness
            modelBuilder.Entity<Activity>().HasQueryFilter(d => !d.IsDeleted);
            modelBuilder.Entity<Activity>().Property(c => c.Name).IsRequired(true);
            modelBuilder.Entity<Activity>().Property(c => c.Description).IsRequired(true);
            modelBuilder.Entity<Activity>().Property(c => c.Duration).IsRequired(true);
            modelBuilder.Entity<Activity>().HasIndex(c => c.UserId).IsUnique(false);
            modelBuilder.Entity<Activity>().HasIndex(c => c.Date).IsUnique(false);
            modelBuilder.Entity<Activity>()
                .Property(d => d.ActivityType)
                .HasConversion(new EnumToStringConverter<ActivityType>());
            modelBuilder.Entity<Activity>()
                .HasOne(c => c.User)
                .WithMany(r => r.Activities)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }

        // Identity sets
        public DbSet<IdentityServer4.EntityFramework.Entities.PersistedGrant> PersistedGrants { get; set; }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added when entry!.Entity is BaseEntity:
                        (entry.Entity as BaseEntity)!.CreatedOn = DateTime.Now;
                        break;
                    case EntityState.Modified when entry!.Entity is BaseEntity:
                        (entry.Entity as BaseEntity)!.UpdatedOn = DateTime.Now;
                        break;
                    case EntityState.Deleted when entry!.Entity is ISoftDelete:
                        (entry.Entity as ISoftDelete)!.IsDeleted = true;
                        (entry.Entity as BaseEntity)!.UpdatedOn = DateTime.Now;
                        break;
                }
            }
            return base.SaveChanges();
        }
    }
}
