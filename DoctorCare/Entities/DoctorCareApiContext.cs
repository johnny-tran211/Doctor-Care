using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCare.Entities
{
    public class DoctorCareApiContext : DbContext
    {
        public DoctorCareApiContext(DbContextOptions<DoctorCareApiContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role);

            modelBuilder.Entity<Appointment>()
                .HasOne(u => u.Status);

            modelBuilder.Entity<User>()
                .HasMany(m => m.DoctorAppointments)
                .WithOne(l => l.Doctor);

            modelBuilder.Entity<User>()
                .HasMany(m => m.PatientAppointments)
                .WithOne(l => l.Patient);
        }
    }
}
