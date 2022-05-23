#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.JWT.Model;
using ToDoAPI.Model;

namespace ToDoAPI.Data
{
    public class ToDoAPIContext : DbContext
    {
        public ToDoAPIContext (DbContextOptions<ToDoAPIContext> options)
            : base(options)
        {
           
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(t => t.ToDoList)
                .WithOne(u => u.User);

            modelBuilder.Entity<UserRole>()
                        .HasOne(x => x.Role)
                        .WithMany(x => x.UsersRole)
                        .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<UserRole>()
                        .HasOne(x => x.User)
                        .WithMany(x => x.UserRoles)
                        .HasForeignKey(x => x.UserId);
        }
        public DbSet<ToDoAPI.JWT.Model.User> Users { get; set; }
        public DbSet<ToDoAPI.Model.ToDo> ToDoes { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
