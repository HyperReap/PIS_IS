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

            var roomSeed = new List<Room>
            {
                new Room
                {
                    Id = 1,
                    Floor = 11,
                    IsCleaned = true,
                    NumberOfBeds = 111,
                    NumberOfSideBeds = 1_111,
                    RoomNumber = 11_111,
                    RoomSize = 111_111,
                },
                new Room
                {
                    Id = 2,
                    Floor = 22,
                    IsCleaned = true,
                    NumberOfBeds = 222,
                    NumberOfSideBeds = 2_222,
                    RoomNumber = 22_222,
                    RoomSize = 222_222,
                },
            };



            modelBuilder.Entity<Room>().HasData(roomSeed);

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