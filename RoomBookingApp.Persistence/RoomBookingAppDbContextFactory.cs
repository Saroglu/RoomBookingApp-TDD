using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RoomBookingApp.Persistence
{
    public class RoomBookingAppDbContextFactory : IDesignTimeDbContextFactory<RoomBookingAppDbContext>
    {
        public RoomBookingAppDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Data Source=RoomBookingApp.db";

            var optionsBuilder = new DbContextOptionsBuilder<RoomBookingAppDbContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new RoomBookingAppDbContext(optionsBuilder.Options);
        }
    }
}
