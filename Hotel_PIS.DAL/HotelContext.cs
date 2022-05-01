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
            seed(modelBuilder);
            configureRelations(modelBuilder);
        }

        private void seed(ModelBuilder modelBuilder)
        {
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
                    Picture = "1111",
                    CostPerNight = 121,
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
                    Picture = "222",
                    CostPerNight = 212,
                },
            };

            var roomReservationSeed = new List<RoomReservation>
            {
                new RoomReservation
                {
                    Id = 3,
                    DateFrom = new DateTime(2022,04,25),
                    DateTo = new DateTime(2022,04,28),
                    RoomId = 1,
                    ReservationId = 1,
                }
            };

            var reservationSeed = new List<Reservation>
            {
                new Reservation
                {
                    Id = 1,
                    Cost = 100,
                    NumberOfPeople = 4,
                    Payed = 50,
                    ReservationState = ReservationStateEnum.Reserved,
                    ClientId = 1,
                }
            };


            var equipmentSeed = new List<Equipment>
            {
                new Equipment
                {
                    Id = 1,
                    Name = "TV"
                },
                new Equipment
                {
                    Id = 2,
                    Name = "WiFi"
                },
                new Equipment
                {
                    Id = 3,
                    Name = "John Cena"
                },
            };

            var equipmentRoomSeed = new List<RoomEquipment>
            {
                new RoomEquipment
                {
                    Id = 1,
                    EquipmentId = 1,
                    RoomId = 2,
                },
                new RoomEquipment
                {
                    Id = 2,
                    EquipmentId = 3,
                    RoomId = 2
                },
            };

            var clientSeed = new List<Client>
            {
                new Client
                {
                    Id =1,
                    SecondName = "ahoj",
                    IdentityCardNumber = "ID_AHOJ",
                    Email = "ahoj@seynam.com",
                    FirstName = "seyam",
                    PhoneNumber = "+420 605 956 987"
                }
            };


            var roleSeed = new List<Role>
            {
                new Role
                {
                    Id =1,
                    NameOfRole = "Manager"
                },
                new Role
                {
                    Id =2,
                    NameOfRole = "Reception"
                },
                new Role
                {
                    Id =3,
                    NameOfRole = "Techncian"
                },
                new Role
                {
                    Id =4,
                    NameOfRole = "Cleaner"
                },
            };

            var employeeSeed = new List<Employee>
            {
                new Employee
                {
                    ContractDueDae = null,
                    Email = "manager@manager.cz",
                    FirstName = "Petr",
                    SecondName = "Novák",
                    RoleId = 1,
                    Id = 1,
                    Password = "$2a$11$qahanh6DohzXdzZxzRuYAe.Bf01JgZTqXPgDI/OfZfLSIueZI3LIW", //123-aaa-BBB
                },
                   new Employee
                {
                    ContractDueDae = new DateTime(2022,05,07),
                    Email = "recepce@recepce.cz",
                    FirstName = "Michal",
                    SecondName = "Bloudný",
                    RoleId = 2,
                    Id = 2,
                    Password = "$2a$11$qahanh6DohzXdzZxzRuYAe.Bf01JgZTqXPgDI/OfZfLSIueZI3LIW", //123-aaa-BBB
                },
                      new Employee
                {
                    ContractDueDae = new DateTime(2022,12,07),
                    Email = "uklizecka@uklizecka.cz",
                    FirstName = "Alena",
                    SecondName = "Novotná",
                    RoleId = 4,
                    Id = 4,
                    Password = "$2a$11$qahanh6DohzXdzZxzRuYAe.Bf01JgZTqXPgDI/OfZfLSIueZI3LIW", //123-aaa-BBB
                },
                                  new Employee
                {
                    ContractDueDae = null,
                    Email = "technik@technik.cz",
                    FirstName = "Oto",
                    SecondName = "Ladský",
                    RoleId = 3,
                    Id = 3,
                    Password = "$2a$11$qahanh6DohzXdzZxzRuYAe.Bf01JgZTqXPgDI/OfZfLSIueZI3LIW", //123-aaa-BBB
                },

            };



            modelBuilder.Entity<Employee>().HasData(employeeSeed);
            modelBuilder.Entity<Role>().HasData(roleSeed);
            modelBuilder.Entity<Client>().HasData(clientSeed);
            modelBuilder.Entity<Room>().HasData(roomSeed);
            modelBuilder.Entity<Equipment>().HasData(equipmentSeed);
            modelBuilder.Entity<Reservation>().HasData(reservationSeed);
            modelBuilder.Entity<RoomReservation>().HasData(roomReservationSeed);
            modelBuilder.Entity<RoomEquipment>().HasData(equipmentRoomSeed);
        }

        private void configureRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomEquipment>(entity =>
            entity.HasOne(eq => eq.Room)
            .WithMany(re => re.RoomEquipments)
            .HasForeignKey(e => e.RoomId));

            modelBuilder.Entity<RoomEquipment>(entity =>
            entity.HasOne(eq => eq.Equipment)
            .WithMany(re => re.RoomEquipments)
            .HasForeignKey(e => e.EquipmentId));



            modelBuilder.Entity<RoomReservation>(entity =>
            entity.HasOne(r => r.Reservation)
            .WithMany(rr => rr.RoomReservations)
            .HasForeignKey(e => e.ReservationId));

            modelBuilder.Entity<RoomReservation>(entity =>
            entity.HasOne(r => r.Room)
            .WithMany(rr => rr.RoomReservations)
            .HasForeignKey(e => e.RoomId));

            modelBuilder.Entity<Reservation>()
           .HasOne(p => p.Client)
           .WithMany(b => b.Reservations)
           .HasForeignKey(p => p.ClientId);


        }



        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomReservation> RoomReservations { get; set; }
        public DbSet<RoomEquipment> RoomEquipments { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Failure> Failures { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Equipment> Equipments { get; set; }

    }
}