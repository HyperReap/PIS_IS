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
            //TODO:: create Factory 
        }

        private readonly string _connectionString;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test");
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomReservation> RoomReservations { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Failure> Failures { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }

     
}