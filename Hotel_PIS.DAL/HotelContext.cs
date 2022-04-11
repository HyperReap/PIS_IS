using Microsoft.EntityFrameworkCore;

namespace Hotel_PIS.DAL
{

    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options)
            : base(options)
        {

        }
        public HotelContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public HotelContext()
        {

        }

        private readonly string _connectionString = "server=localhost;database=hotel;user=user;password=password";

        public string DbPath { get; }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
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