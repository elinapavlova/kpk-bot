﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using kpk_telegram_bot.Common.Database;

#nullable disable

namespace kpk_telegram_bot.Migrations
{
    [DbContext(typeof(KpkTelegramBotContext))]
    [Migration("20230130113051_AddItemParent")]
    partial class AddItemParent
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.11");

            modelBuilder.Entity("kpk_telegram_bot.Common.Database.Entities.ItemEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateDeleted")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("TypeId");

                    b.ToTable("ItemEntity");
                });

            modelBuilder.Entity("kpk_telegram_bot.Common.Database.Entities.ItemPropertyEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateDeleted")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("TypeId");

                    b.ToTable("ItemPropertyEntity");
                });

            modelBuilder.Entity("kpk_telegram_bot.Common.Database.Entities.ItemPropertyTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateDeleted")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ItemPropertyTypeEntity");
                });

            modelBuilder.Entity("kpk_telegram_bot.Common.Database.Entities.ItemTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateDeleted")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ItemTypeEntity");
                });

            modelBuilder.Entity("kpk_telegram_bot.Common.Database.Entities.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateDeleted")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("GroupId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("RoleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("kpk_telegram_bot.Common.Database.Entities.ItemEntity", b =>
                {
                    b.HasOne("kpk_telegram_bot.Common.Database.Entities.ItemEntity", "Parent")
                        .WithMany("Childs")
                        .HasForeignKey("ParentId");

                    b.HasOne("kpk_telegram_bot.Common.Database.Entities.ItemTypeEntity", "Type")
                        .WithMany("Items")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parent");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("kpk_telegram_bot.Common.Database.Entities.ItemPropertyEntity", b =>
                {
                    b.HasOne("kpk_telegram_bot.Common.Database.Entities.ItemEntity", "Item")
                        .WithMany("Properties")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("kpk_telegram_bot.Common.Database.Entities.ItemPropertyTypeEntity", "Type")
                        .WithMany("Properties")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("kpk_telegram_bot.Common.Database.Entities.UserEntity", b =>
                {
                    b.HasOne("kpk_telegram_bot.Common.Database.Entities.ItemEntity", "Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupId");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("kpk_telegram_bot.Common.Database.Entities.ItemEntity", b =>
                {
                    b.Navigation("Childs");

                    b.Navigation("Properties");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("kpk_telegram_bot.Common.Database.Entities.ItemPropertyTypeEntity", b =>
                {
                    b.Navigation("Properties");
                });

            modelBuilder.Entity("kpk_telegram_bot.Common.Database.Entities.ItemTypeEntity", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
