﻿using Microsoft.EntityFrameworkCore;
using ProjetoPizzaria.Models;

namespace ProjetoPizzaria.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<PizzaModel> Pizzas { get; set; }
    }
}
