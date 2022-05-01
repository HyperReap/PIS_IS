﻿// <auto-generated />
using System;
using Hotel_PIS.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Hotel_PIS.DAL.Migrations
{
    [DbContext(typeof(HotelContext))]
    partial class HotelContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.3");

            modelBuilder.Entity("Hotel_PIS.DAL.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("IdentityCardNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Clients");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "ahoj@seynam.com",
                            FirstName = "seyam",
                            IdentityCardNumber = "ID_AHOJ",
                            PhoneNumber = "+420 605 956 987",
                            SecondName = "ahoj"
                        });
                });

            modelBuilder.Entity("Hotel_PIS.DAL.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ContractDueDae")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("RoleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "manager@manager.cz",
                            FirstName = "Petr",
                            Password = "$2a$11$qahanh6DohzXdzZxzRuYAe.Bf01JgZTqXPgDI/OfZfLSIueZI3LIW",
                            RoleId = 1,
                            SecondName = "Novák"
                        },
                        new
                        {
                            Id = 2,
                            ContractDueDae = new DateTime(2022, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "recepce@recepce.cz",
                            FirstName = "Michal",
                            Password = "$2a$11$qahanh6DohzXdzZxzRuYAe.Bf01JgZTqXPgDI/OfZfLSIueZI3LIW",
                            RoleId = 2,
                            SecondName = "Bloudný"
                        },
                        new
                        {
                            Id = 4,
                            ContractDueDae = new DateTime(2022, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "uklizecka@uklizecka.cz",
                            FirstName = "Alena",
                            Password = "$2a$11$qahanh6DohzXdzZxzRuYAe.Bf01JgZTqXPgDI/OfZfLSIueZI3LIW",
                            RoleId = 4,
                            SecondName = "Novotná"
                        },
                        new
                        {
                            Id = 3,
                            Email = "technik@technik.cz",
                            FirstName = "Oto",
                            Password = "$2a$11$qahanh6DohzXdzZxzRuYAe.Bf01JgZTqXPgDI/OfZfLSIueZI3LIW",
                            RoleId = 3,
                            SecondName = "Ladský"
                        });
                });

            modelBuilder.Entity("Hotel_PIS.DAL.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Equipments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "TV"
                        },
                        new
                        {
                            Id = 2,
                            Name = "WiFi"
                        },
                        new
                        {
                            Id = 3,
                            Name = "John Cena"
                        });
                });

            modelBuilder.Entity("Hotel_PIS.DAL.Failure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsSolved")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoomId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Failures");
                });

            modelBuilder.Entity("Hotel_PIS.DAL.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClientId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Cost")
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberOfPeople")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Payed")
                        .HasColumnType("TEXT");

                    b.Property<int>("ReservationState")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Reservations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ClientId = 1,
                            Cost = 100m,
                            NumberOfPeople = 4,
                            Payed = 50m,
                            ReservationState = 0
                        });
                });

            modelBuilder.Entity("Hotel_PIS.DAL.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("NameOfRole")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            NameOfRole = "Manager"
                        },
                        new
                        {
                            Id = 2,
                            NameOfRole = "Reception"
                        },
                        new
                        {
                            Id = 3,
                            NameOfRole = "Techncian"
                        },
                        new
                        {
                            Id = 4,
                            NameOfRole = "Cleaner"
                        });
                });

            modelBuilder.Entity("Hotel_PIS.DAL.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CostPerNight")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Floor")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsCleaned")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfBeds")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfSideBeds")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Picture")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("RoomNumber")
                        .HasColumnType("INTEGER");

                    b.Property<double>("RoomSize")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Rooms");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CostPerNight = 121,
                            Floor = 11,
                            IsCleaned = true,
                            NumberOfBeds = 111,
                            NumberOfSideBeds = 1111,
                            Picture = "1111",
                            RoomNumber = 11111,
                            RoomSize = 111111.0
                        },
                        new
                        {
                            Id = 2,
                            CostPerNight = 212,
                            Floor = 22,
                            IsCleaned = true,
                            NumberOfBeds = 222,
                            NumberOfSideBeds = 2222,
                            Picture = "222",
                            RoomNumber = 22222,
                            RoomSize = 222222.0
                        });
                });

            modelBuilder.Entity("Hotel_PIS.DAL.RoomEquipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoomId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomEquipments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            EquipmentId = 1,
                            RoomId = 2
                        },
                        new
                        {
                            Id = 2,
                            EquipmentId = 3,
                            RoomId = 2
                        });
                });

            modelBuilder.Entity("Hotel_PIS.DAL.RoomReservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateFrom")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateTo")
                        .HasColumnType("TEXT");

                    b.Property<int>("ReservationId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoomId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ReservationId");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomReservations");

                    b.HasData(
                        new
                        {
                            Id = 3,
                            DateFrom = new DateTime(2022, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateTo = new DateTime(2022, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ReservationId = 1,
                            RoomId = 1
                        });
                });

            modelBuilder.Entity("Hotel_PIS.DAL.Employee", b =>
                {
                    b.HasOne("Hotel_PIS.DAL.Role", "Role")
                        .WithMany("EmployeesWithRole")
                        .HasForeignKey("RoleId");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Hotel_PIS.DAL.Failure", b =>
                {
                    b.HasOne("Hotel_PIS.DAL.Room", "Room")
                        .WithMany("Failures")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Hotel_PIS.DAL.Reservation", b =>
                {
                    b.HasOne("Hotel_PIS.DAL.Client", "Client")
                        .WithMany("Reservations")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Hotel_PIS.DAL.RoomEquipment", b =>
                {
                    b.HasOne("Hotel_PIS.DAL.Equipment", "Equipment")
                        .WithMany("RoomEquipments")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hotel_PIS.DAL.Room", "Room")
                        .WithMany("RoomEquipments")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Hotel_PIS.DAL.RoomReservation", b =>
                {
                    b.HasOne("Hotel_PIS.DAL.Reservation", "Reservation")
                        .WithMany("RoomReservations")
                        .HasForeignKey("ReservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hotel_PIS.DAL.Room", "Room")
                        .WithMany("RoomReservations")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reservation");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Hotel_PIS.DAL.Client", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("Hotel_PIS.DAL.Equipment", b =>
                {
                    b.Navigation("RoomEquipments");
                });

            modelBuilder.Entity("Hotel_PIS.DAL.Reservation", b =>
                {
                    b.Navigation("RoomReservations");
                });

            modelBuilder.Entity("Hotel_PIS.DAL.Role", b =>
                {
                    b.Navigation("EmployeesWithRole");
                });

            modelBuilder.Entity("Hotel_PIS.DAL.Room", b =>
                {
                    b.Navigation("Failures");

                    b.Navigation("RoomEquipments");

                    b.Navigation("RoomReservations");
                });
#pragma warning restore 612, 618
        }
    }
}
