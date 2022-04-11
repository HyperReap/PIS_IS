using Microsoft.EntityFrameworkCore;

namespace Hotel_PIS.DAL
{

    public class HotelContext : DbContext
    {

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {

            var dataSource = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "HotelDatabase.db");
            options
                .UseSqlite($"Data Source={dataSource};");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomReservation> RoomReservations { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Failure> Failures { get; set; }
        public DbSet<Role> Roles { get; set; }

    }
}