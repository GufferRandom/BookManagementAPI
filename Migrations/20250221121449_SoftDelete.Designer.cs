﻿// <auto-generated />
using BookManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookManagementAPI.Migrations
{
    [DbContext(typeof(AppDataContext))]
    [Migration("20250221121449_SoftDelete")]
    partial class SoftDelete
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BookManagementAPI.Models.Books", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PublicationYear")
                        .HasColumnType("int");

                    b.Property<bool>("SoftDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ViewsCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorName = "Harry Poter",
                            PublicationYear = 2000,
                            SoftDeleted = false,
                            Title = "Harry Poter And The Grinch",
                            ViewsCount = 0
                        },
                        new
                        {
                            Id = 2,
                            AuthorName = "Henry Sirius",
                            PublicationYear = 1999,
                            SoftDeleted = false,
                            Title = "The Ring",
                            ViewsCount = 0
                        },
                        new
                        {
                            Id = 3,
                            AuthorName = "The Bottle",
                            PublicationYear = 2025,
                            SoftDeleted = false,
                            Title = "The Laptop And Its Adventures",
                            ViewsCount = 0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
