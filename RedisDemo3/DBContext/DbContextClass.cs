﻿using Microsoft.EntityFrameworkCore;
using RedisDemo3.Entity;

namespace RedisDemo3.DBContext
{
    public class DbContextClass : DbContext
    {
        public DbContextClass(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
