using Microsoft.EntityFrameworkCore;
using SimpleDirectory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDirectory.Data
{
    public class DirectoryDbContext : DbContext
    {
        public DirectoryDbContext(DbContextOptions<DirectoryDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}
