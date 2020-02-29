using _2C2PTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2C2PTest.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<SystemLog> SystemLog { get; set; }
        public DbSet<CurrencyCode> CurrencyCode { get; set; }
    }
}
