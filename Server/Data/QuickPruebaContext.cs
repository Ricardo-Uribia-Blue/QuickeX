using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using QuickEx.Server.Models.QuickPrueba;

namespace QuickEx.Server.Data
{
    public partial class QuickPruebaContext : DbContext
    {
        public QuickPruebaContext()
        {
        }

        public QuickPruebaContext(DbContextOptions<QuickPruebaContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.OnModelBuilding(builder);
        }

        public DbSet<QuickEx.Server.Models.QuickPrueba.Alumno> Alumnos { get; set; }
    }
}