﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sidekick.Persistence;

namespace Sidekick.Persistence.Migrations
{
    [DbContext(typeof(SidekickContext))]
    [Migration("20201223003509_Version_1.2.0.4")]
    partial class Version_1204
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("sidekick")
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Sidekick.Domain.Apis.PoeNinja.Models.NinjaPrice", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Corrupted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.HasKey("Name", "Corrupted");

                    b.ToTable("NinjaPrices");
                });

            modelBuilder.Entity("Sidekick.Domain.Apis.PoeNinja.Models.NinjaTranslation", b =>
                {
                    b.Property<string>("Translation")
                        .HasColumnType("TEXT");

                    b.Property<string>("English")
                        .HasColumnType("TEXT");

                    b.HasKey("Translation");

                    b.ToTable("NinjaTranslations");
                });

            modelBuilder.Entity("Sidekick.Domain.Game.Leagues.League", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("Sidekick.Domain.Views.ViewPreference", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<double>("Height")
                        .HasColumnType("REAL");

                    b.Property<double>("Width")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("ViewPreferences");
                });

            modelBuilder.Entity("Sidekick.Persistence.Cache.Cache", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("Data")
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.ToTable("Caches");
                });

            modelBuilder.Entity("Sidekick.Persistence.ItemCategories.ItemCategory", b =>
                {
                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.Property<string>("Category")
                        .HasColumnType("TEXT");

                    b.HasKey("Type");

                    b.ToTable("ItemCategories");
                });
#pragma warning restore 612, 618
        }
    }
}