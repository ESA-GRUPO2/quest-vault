using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;


namespace questvaultTest
{
    public class ApplicationDbContextFixture : IDisposable
    {
        public ApplicationDbContext DbContext { get; private set; }

        public ApplicationDbContextFixture() {

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(connection)
                    .Options;
            DbContext = new ApplicationDbContext(options);

            DbContext.Database.EnsureCreated();

            //Add data to test
            DbContext.Games.Add(new Game { GameID = 1, 
                Name = "Dark Souls 3", 
                IgdbRating = 85, 
                Summary = "Dark Souls continues to push the boundaries with the latest, ambitious chapter in the critically-acclaimed and genre-defining series. Prepare yourself and embrace the darkness!"
            });

            DbContext.SaveChanges();
        }

        public void Dispose() => DbContext.Dispose();
    }
}
