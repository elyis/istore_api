﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using istore_api.src.Infrastructure.Data;

#nullable disable

namespace istore_api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240111133027_init4")]
    partial class init4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("ProductProductCharacteristic", b =>
                {
                    b.Property<string>("CharacteristicsName")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProductsId")
                        .HasColumnType("TEXT");

                    b.HasKey("CharacteristicsName", "ProductsId");

                    b.HasIndex("ProductsId");

                    b.ToTable("ProductProductCharacteristic");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.DeviceModel", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductCategoryName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Name");

                    b.HasIndex("ProductCategoryName");

                    b.ToTable("DeviceModels");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.OptionsCombiningCharacteristic", b =>
                {
                    b.Property<string>("ImageFilename")
                        .HasColumnType("TEXT");

                    b.Property<string>("CharacteristicName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductCharacteristicName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductImageFilename")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ImageFilename", "CharacteristicName");

                    b.HasIndex("ProductCharacteristicName");

                    b.HasIndex("ProductImageFilename");

                    b.ToTable("OptionsCombiningCharacteristics");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<string>("CommunicationMethod")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("TotalSum")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.OrderProducts", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.HasKey("OrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderProducts");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceModelName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("Price")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("DeviceModelName");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.ProductCategory", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Name");

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.ProductCharacteristic", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Values")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Name");

                    b.ToTable("ProductCharacteristics");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.ProductImage", b =>
                {
                    b.Property<string>("Filename")
                        .HasColumnType("TEXT");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPreviewImage")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("TEXT");

                    b.HasKey("Filename");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RestoreCode")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("RestoreCodeValidBefore")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("TokenValidBefore")
                        .HasColumnType("TEXT");

                    b.Property<bool>("WasPasswordResetRequest")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Token")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ProductProductCharacteristic", b =>
                {
                    b.HasOne("istore_api.src.Domain.Models.ProductCharacteristic", null)
                        .WithMany()
                        .HasForeignKey("CharacteristicsName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("istore_api.src.Domain.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.DeviceModel", b =>
                {
                    b.HasOne("istore_api.src.Domain.Models.ProductCategory", "ProductCategory")
                        .WithMany("DeviceModels")
                        .HasForeignKey("ProductCategoryName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductCategory");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.OptionsCombiningCharacteristic", b =>
                {
                    b.HasOne("istore_api.src.Domain.Models.ProductCharacteristic", "ProductCharacteristic")
                        .WithMany("OptionsCombiningCharacteristic")
                        .HasForeignKey("ProductCharacteristicName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("istore_api.src.Domain.Models.ProductImage", "ProductImage")
                        .WithMany("OptionsCombiningCharacteristic")
                        .HasForeignKey("ProductImageFilename")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductCharacteristic");

                    b.Navigation("ProductImage");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.OrderProducts", b =>
                {
                    b.HasOne("istore_api.src.Domain.Models.Order", "Order")
                        .WithMany("Products")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("istore_api.src.Domain.Models.Product", "Product")
                        .WithMany("Orders")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.Product", b =>
                {
                    b.HasOne("istore_api.src.Domain.Models.DeviceModel", "DeviceModel")
                        .WithMany("Products")
                        .HasForeignKey("DeviceModelName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeviceModel");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.ProductImage", b =>
                {
                    b.HasOne("istore_api.src.Domain.Models.Product", "Product")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.DeviceModel", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.Order", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.Product", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("ProductImages");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.ProductCategory", b =>
                {
                    b.Navigation("DeviceModels");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.ProductCharacteristic", b =>
                {
                    b.Navigation("OptionsCombiningCharacteristic");
                });

            modelBuilder.Entity("istore_api.src.Domain.Models.ProductImage", b =>
                {
                    b.Navigation("OptionsCombiningCharacteristic");
                });
#pragma warning restore 612, 618
        }
    }
}
