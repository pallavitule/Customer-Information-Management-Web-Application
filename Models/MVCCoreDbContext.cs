﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
namespace MVCDHProject5.Models
{
    public class MVCCoreDbContext : IdentityDbContext
    {
        public MVCCoreDbContext(DbContextOptions options) : base(options) 
        { 
        }
        public DbSet<Customer> Customers { get; set; }

         //base.OnModelCreating(modelBuilder);
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customer>().HasData(        
            new Customer{ Custid = 101, Name = "Sai", Balance = 50000.00m, City = "Delhi", Status = true, Country = "USA", Continent = "North America", State = "New York" },
            new Customer{ Custid = 102, Name = "Sonia", Balance = 40000.00m, City = "Mumbai", Status = true, Country = "USA", Continent = "North America", State = "New York" },
            new Customer{ Custid = 103, Name = "Pankaj", Balance = 30000.00m, City = "Chennai", Status = true, Country = "USA", Continent = "North America", State = "New York" },
            new Customer{ Custid = 104, Name = "Samuels", Balance = 25000.00m, City = "Bengaluru", Status = true, Country = "USA", Continent = "North America", State = "New York" }
         );
        }
    }
}
