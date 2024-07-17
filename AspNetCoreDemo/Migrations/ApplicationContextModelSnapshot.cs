﻿// <auto-generated />
using System;
using AspNetCoreDemo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AspNetCoreDemo.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AspNetCoreDemo.Models.Beer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("Abv")
                        .HasColumnType("float");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StyleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StyleId");

                    b.HasIndex("UserId");

                    b.ToTable("Beers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Abv = 4.5999999999999996,
                            Image = "/images/beer1.webp",
                            Name = "Glarus English Ale",
                            StyleId = 1,
                            UserId = 2
                        },
                        new
                        {
                            Id = 2,
                            Abv = 5.0,
                            Image = "/images/beer2.webp",
                            Name = "Rhombus Porter",
                            StyleId = 2,
                            UserId = 2
                        },
                        new
                        {
                            Id = 3,
                            Abv = 6.5999999999999996,
                            Image = "/images/beer3.webp",
                            Name = "Opasen Char",
                            StyleId = 3,
                            UserId = 3
                        });
                });

            modelBuilder.Entity("AspNetCoreDemo.Models.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("BeerId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BeerId");

                    b.HasIndex("UserId");

                    b.ToTable("Rating");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BeerId = 1,
                            UserId = 3,
                            Value = 5
                        },
                        new
                        {
                            Id = 2,
                            BeerId = 1,
                            UserId = 2,
                            Value = 2
                        },
                        new
                        {
                            Id = 3,
                            BeerId = 2,
                            UserId = 3,
                            Value = 1
                        },
                        new
                        {
                            Id = 4,
                            BeerId = 2,
                            UserId = 2,
                            Value = 3
                        },
                        new
                        {
                            Id = 5,
                            BeerId = 3,
                            UserId = 3,
                            Value = 5
                        },
                        new
                        {
                            Id = 6,
                            BeerId = 3,
                            UserId = 2,
                            Value = 5
                        });
                });

            modelBuilder.Entity("AspNetCoreDemo.Models.Style", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Styles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Special Ale"
                        },
                        new
                        {
                            Id = 2,
                            Name = "English Porter"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Indian Pale Ale"
                        });
                });

            modelBuilder.Entity("AspNetCoreDemo.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsAdmin = true,
                            Password = "MTIz",
                            Username = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            IsAdmin = false,
                            Password = "MTIz",
                            Username = "Alice"
                        },
                        new
                        {
                            Id = 3,
                            IsAdmin = false,
                            Password = "MTIz",
                            Username = "Bob"
                        });
                });

            modelBuilder.Entity("AspNetCoreDemo.Models.Beer", b =>
                {
                    b.HasOne("AspNetCoreDemo.Models.Style", "Style")
                        .WithMany("Beers")
                        .HasForeignKey("StyleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AspNetCoreDemo.Models.User", "User")
                        .WithMany("Beers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Style");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AspNetCoreDemo.Models.Rating", b =>
                {
                    b.HasOne("AspNetCoreDemo.Models.Beer", "Beer")
                        .WithMany("Ratings")
                        .HasForeignKey("BeerId");

                    b.HasOne("AspNetCoreDemo.Models.User", "User")
                        .WithMany("Ratings")
                        .HasForeignKey("UserId");

                    b.Navigation("Beer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AspNetCoreDemo.Models.Beer", b =>
                {
                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("AspNetCoreDemo.Models.Style", b =>
                {
                    b.Navigation("Beers");
                });

            modelBuilder.Entity("AspNetCoreDemo.Models.User", b =>
                {
                    b.Navigation("Beers");

                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}
