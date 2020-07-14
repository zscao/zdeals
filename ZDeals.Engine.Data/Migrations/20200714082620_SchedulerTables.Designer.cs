﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZDeals.Engine.Data;

namespace ZDeals.Engine.Data.Migrations
{
    [DbContext(typeof(EngineDbContext))]
    [Migration("20200714082620_SchedulerTables")]
    partial class SchedulerTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ZDeals.Engine.Data.Entities.ProductEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Brand")
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20);

                    b.Property<DateTime>("CreatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(2000) CHARACTER SET utf8mb4")
                        .HasMaxLength(2000);

                    b.Property<decimal>("FullPrice")
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("HighLight")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.Property<string>("Manufacturer")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("Mpn")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<DateTime>("ParsedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PriceCurrency")
                        .IsRequired()
                        .HasColumnType("varchar(3) CHARACTER SET utf8mb4")
                        .HasMaxLength(3);

                    b.Property<decimal>("SalePrice")
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("Sku")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("Store")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200);

                    b.Property<string>("Url")
                        .HasColumnType("varchar(400) CHARACTER SET utf8mb4")
                        .HasMaxLength(400);

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ZDeals.Engine.Data.Entities.VisitedPageEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("LastVisitedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("varchar(400) CHARACTER SET utf8mb4")
                        .HasMaxLength(400);

                    b.Property<int>("UrlHash")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UrlHash");

                    b.ToTable("VisitedPages");
                });
#pragma warning restore 612, 618
        }
    }
}
