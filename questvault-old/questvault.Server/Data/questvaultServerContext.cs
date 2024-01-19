using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using questvault.Server.Model;

namespace questvault.Server.Data
{
    public class questvaultServerContext : DbContext
    {
        public questvaultServerContext (DbContextOptions<questvaultServerContext> options)
            : base(options)
        {
        }

        public DbSet<questvault.Server.Model.User> User { get; set; } = default!;
    }
}
