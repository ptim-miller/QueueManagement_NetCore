using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using QMS.Models;

namespace QMS.Data
{
    public class PatientDbContext : DbContext
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options){
        }

        public DbSet<Patient> patients { get; set; }
        public DbSet<Encounter> encounters { get; set; }

    }
}

