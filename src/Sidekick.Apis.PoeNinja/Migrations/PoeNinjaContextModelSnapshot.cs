﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sidekick.Apis.PoeNinja.Repository;

namespace Sidekick.Apis.PoeNinja.Migrations
{
    [DbContext(typeof(PoeNinjaContext))]
    partial class PoeNinjaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("Sidekick.Apis.PoeNinja.Repository.Models.NinjaPrice", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Corrupted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MapTier")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GemLevel")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.HasKey("Name", "Corrupted", "MapTier", "GemLevel");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("Sidekick.Apis.PoeNinja.Repository.Models.NinjaTranslation", b =>
                {
                    b.Property<string>("Translation")
                        .HasColumnType("TEXT");

                    b.Property<string>("English")
                        .HasColumnType("TEXT");

                    b.HasKey("Translation");

                    b.ToTable("Translations");
                });
#pragma warning restore 612, 618
        }
    }
}
