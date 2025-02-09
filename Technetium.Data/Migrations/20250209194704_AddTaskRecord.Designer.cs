﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Technetium.Data;

#nullable disable

namespace Technetium.Data.Migrations
{
    [DbContext(typeof(TechnetiumDataContext))]
    [Migration("20250209194704_AddTaskRecord")]
    partial class AddTaskRecord
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("Technetium.Data.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("TEXT");

                    b.Property<string>("StartDateTime")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StartTimeZone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Technetium.Data.TaskRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ScheduledTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TaskRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
