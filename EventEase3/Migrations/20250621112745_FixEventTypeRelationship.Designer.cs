﻿// <auto-generated />
using System;
using EventEase3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EventEase3.Migrations
{
    [DbContext(typeof(EventEase3Context))]
    [Migration("20250621112745_FixEventTypeRelationship")]
    partial class FixEventTypeRelationship
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EventEase3.Models.Booking", b =>
                {
                    b.Property<int>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingId"));

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<int>("VenueId")
                        .HasColumnType("int");

                    b.HasKey("BookingId");

                    b.HasIndex("EventId");

                    b.HasIndex("VenueId");

                    b.ToTable("Booking");
                });

            modelBuilder.Entity("EventEase3.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventId"));

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EventTypeID")
                        .HasColumnType("int");

                    b.HasKey("EventId");

                    b.HasIndex("EventTypeID");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("EventEase3.Models.EventType", b =>
                {
                    b.Property<int>("EventTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventTypeID"));

                    b.Property<string>("EventTypeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EventTypeID");

                    b.ToTable("EventType");
                });

            modelBuilder.Entity("EventEase3.Models.Venue", b =>
                {
                    b.Property<int>("VenueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VenueId"));

                    b.Property<bool>("Availability")
                        .HasColumnType("bit");

                    b.Property<int>("VenueCap")
                        .HasColumnType("int");

                    b.Property<string>("VenueImgURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VenueLocal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VenueName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VenueId");

                    b.ToTable("Venue");
                });

            modelBuilder.Entity("EventEase3.Models.Booking", b =>
                {
                    b.HasOne("EventEase3.Models.Event", "Event")
                        .WithMany("Bookings")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventEase3.Models.Venue", "Venue")
                        .WithMany("Bookings")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("EventEase3.Models.Event", b =>
                {
                    b.HasOne("EventEase3.Models.EventType", "EventType")
                        .WithMany("Events")
                        .HasForeignKey("EventTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventType");
                });

            modelBuilder.Entity("EventEase3.Models.Event", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("EventEase3.Models.EventType", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("EventEase3.Models.Venue", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
