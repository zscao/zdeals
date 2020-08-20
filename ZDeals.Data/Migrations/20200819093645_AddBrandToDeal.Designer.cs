﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZDeals.Data;

namespace ZDeals.Data.Migrations
{
    [DbContext(typeof(ZDealsDbContext))]
    [Migration("20200819093645_AddBrandToDeal")]
    partial class AddBrandToDeal
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ZDeals.Data.Entities.CategoryEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("ParentId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ZDeals.Data.Entities.DealCategoryJoin", b =>
                {
                    b.Property<int>("DealId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("DealId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("DealCategory");
                });

            modelBuilder.Entity("ZDeals.Data.Entities.DealEntity", b =>
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

                    b.Property<decimal>("DealPrice")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("DefaultPicture")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("DeletedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(400) CHARACTER SET utf8mb4")
                        .HasMaxLength(400);

                    b.Property<string>("Discount")
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20);

                    b.Property<DateTime?>("ExpiryDate")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("FullPrice")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("Highlight")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("LastVisitedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Source")
                        .HasColumnType("varchar(400) CHARACTER SET utf8mb4")
                        .HasMaxLength(400);

                    b.Property<int?>("StoreId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200);

                    b.Property<int>("TotalVisited")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("VerifiedBy")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("VerifiedTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("StoreId");

                    b.HasIndex("Title");

                    b.ToTable("Deals");
                });

            modelBuilder.Entity("ZDeals.Data.Entities.DealPictureEntity", b =>
                {
                    b.Property<string>("FileName")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.Property<int>("DealId")
                        .HasColumnType("int");

                    b.Property<string>("Alt")
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200);

                    b.Property<string>("Title")
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200);

                    b.HasKey("FileName", "DealId");

                    b.HasIndex("DealId");

                    b.ToTable("DealPictures");
                });

            modelBuilder.Entity("ZDeals.Data.Entities.StoreEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Domain")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("Website")
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("ZDeals.Data.Entities.VisitHistoryEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClientIp")
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200);

                    b.Property<int>("DealId")
                        .HasColumnType("int");

                    b.Property<DateTime>("VisitedTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("DealId");

                    b.ToTable("VisitHistory");
                });

            modelBuilder.Entity("ZDeals.Data.Entities.CategoryEntity", b =>
                {
                    b.HasOne("ZDeals.Data.Entities.CategoryEntity", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("ZDeals.Data.Entities.DealCategoryJoin", b =>
                {
                    b.HasOne("ZDeals.Data.Entities.CategoryEntity", "Category")
                        .WithMany("DealCategory")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ZDeals.Data.Entities.DealEntity", "Deal")
                        .WithMany("DealCategory")
                        .HasForeignKey("DealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ZDeals.Data.Entities.DealEntity", b =>
                {
                    b.HasOne("ZDeals.Data.Entities.StoreEntity", "Store")
                        .WithMany("Deals")
                        .HasForeignKey("StoreId");
                });

            modelBuilder.Entity("ZDeals.Data.Entities.DealPictureEntity", b =>
                {
                    b.HasOne("ZDeals.Data.Entities.DealEntity", "Deal")
                        .WithMany("Pictures")
                        .HasForeignKey("DealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ZDeals.Data.Entities.VisitHistoryEntity", b =>
                {
                    b.HasOne("ZDeals.Data.Entities.DealEntity", "Deal")
                        .WithMany("VisitHistory")
                        .HasForeignKey("DealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
