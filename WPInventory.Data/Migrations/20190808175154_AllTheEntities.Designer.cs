﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;


namespace WPInventory.Data.Migrations
{
    [DbContext(typeof(ComputerInfoContext))]
    [Migration("20190808175154_AllTheEntities")]
    partial class AllTheEntities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WPInventory.Models.Entities.CPU", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ComputerId");

                    b.Property<string>("MaxClockSpeed");

                    b.Property<string>("Name");

                    b.Property<int>("NumberOfCores");

                    b.Property<string>("SerialNumber");

                    b.HasKey("Id");

                    b.HasIndex("ComputerId");

                    b.ToTable("CPUs");
                });

            modelBuilder.Entity("WPInventory.Models.Entities.Computer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<Guid>("Guid");

                    b.Property<bool>("IsArchived");

                    b.Property<string>("Name");

                    b.Property<string>("OperatingSystem");

                    b.HasKey("Id");

                    b.ToTable("Computers");
                });

            modelBuilder.Entity("WPInventory.Models.Entities.HDD", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ComputerId");

                    b.Property<string>("Model");

                    b.Property<string>("SerialNumber");

                    b.Property<string>("Size");

                    b.HasKey("Id");

                    b.HasIndex("ComputerId");

                    b.ToTable("HDDs");
                });

            modelBuilder.Entity("WPInventory.Models.Entities.Monitor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ComputerId");

                    b.Property<string>("Name");

                    b.Property<DateTime>("ScanDate");

                    b.Property<string>("SerialNumber");

                    b.Property<string>("YearOfManufacture");

                    b.HasKey("Id");

                    b.HasIndex("ComputerId");

                    b.ToTable("Monitors");
                });

            modelBuilder.Entity("WPInventory.Models.Entities.MotherBoard", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Manufacturer");

                    b.Property<string>("Product");

                    b.HasKey("Id");

                    b.ToTable("MotherBoards");
                });

            modelBuilder.Entity("WPInventory.Models.Entities.NWAdapter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ComputerId");

                    b.Property<string>("MAC");

                    b.Property<string>("ProductName");

                    b.Property<string>("ServiceName");

                    b.HasKey("Id");

                    b.HasIndex("ComputerId");

                    b.ToTable("NWAdapters");
                });

            modelBuilder.Entity("WPInventory.Models.Entities.RAM", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Capacity");

                    b.Property<int>("ComputerId");

                    b.Property<string>("Manufacturer");

                    b.Property<string>("PartNumber");

                    b.Property<int>("Speed");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ComputerId");

                    b.ToTable("RAMs");
                });

            modelBuilder.Entity("WPInventory.Models.Entities.ScanDate", b =>
                {
                    b.Property<int>("Id");

                    b.Property<DateTime>("Added");

                    b.Property<DateTime?>("Changed");

                    b.HasKey("Id");

                    b.ToTable("ScanDates");
                });

            modelBuilder.Entity("WPInventory.Models.Entities.VideoCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CardModel");

                    b.Property<int>("ComputerId");

                    b.HasKey("Id");

                    b.HasIndex("ComputerId");

                    b.ToTable("VideoCards");
                });

            modelBuilder.Entity("WPInventory.Models.Entities.CPU", b =>
                {
                    b.HasOne("WPInventory.Models.Entities.Computer", "Computer")
                        .WithMany("CPUs")
                        .HasForeignKey("ComputerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WPInventory.Models.Entities.HDD", b =>
                {
                    b.HasOne("WPInventory.Models.Entities.Computer", "Computer")
                        .WithMany("PhisicalDisks")
                        .HasForeignKey("ComputerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WPInventory.Models.Entities.Monitor", b =>
                {
                    b.HasOne("WPInventory.Models.Entities.Computer", "Computer")
                        .WithMany("Monitors")
                        .HasForeignKey("ComputerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WPInventory.Models.Entities.MotherBoard", b =>
                {
                    b.HasOne("WPInventory.Models.Entities.Computer", "Computer")
                        .WithOne("MotherBoard")
                        .HasForeignKey("WPInventory.Models.Entities.MotherBoard", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WPInventory.Models.Entities.NWAdapter", b =>
                {
                    b.HasOne("WPInventory.Models.Entities.Computer", "Computer")
                        .WithMany("NWAdapters")
                        .HasForeignKey("ComputerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WPInventory.Models.Entities.RAM", b =>
                {
                    b.HasOne("WPInventory.Models.Entities.Computer", "Computer")
                        .WithMany("RAMs")
                        .HasForeignKey("ComputerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WPInventory.Models.Entities.ScanDate", b =>
                {
                    b.HasOne("WPInventory.Models.Entities.Computer", "Computer")
                        .WithOne("ScanDates")
                        .HasForeignKey("WPInventory.Models.Entities.ScanDate", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WPInventory.Models.Entities.VideoCard", b =>
                {
                    b.HasOne("WPInventory.Models.Entities.Computer", "Computer")
                        .WithMany("VideoCards")
                        .HasForeignKey("ComputerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
