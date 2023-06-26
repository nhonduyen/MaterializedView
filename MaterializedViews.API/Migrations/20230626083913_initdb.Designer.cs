﻿// <auto-generated />
using System;
using MaterializedViews.API.Infrastructrure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MaterializedViews.API.Migrations
{
    [DbContext(typeof(MaterializedContext))]
    [Migration("20230626083913_initdb")]
    partial class initdb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Dest")
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MaterializedViews.API.Models.Event", b =>
                {
                    b.Property<int>("TripID")
                        .HasColumnType("int");

                    b.Property<int>("WidgetID")
                        .HasColumnType("int");

                    b.Property<int>("EventTypeID")
                        .HasColumnType("int");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("datetime2");

                    b.HasKey("TripID", "WidgetID", "EventTypeID", "EventDate");

                    b.HasIndex("EventDate", "EventTypeID")
                        .HasDatabaseName("IDX_Event_Date");

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex("EventDate", "EventTypeID"), new[] { "WidgetID" });

                    b.HasIndex("TripID", "WidgetID")
                        .HasDatabaseName("IDX_Event_Trip");

                    b.HasIndex("WidgetID", "TripID", "EventDate")
                        .HasDatabaseName("IDX_Event_Widget");

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex("WidgetID", "TripID", "EventDate"), new[] { "EventTypeID" });

                    b.ToTable("Event", "Source");
                });

            modelBuilder.Entity("MaterializedViews.API.Models.EventType", b =>
                {
                    b.Property<int>("EventTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventTypeID"));

                    b.Property<string>("EventTypeCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("EventTypeDesc")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("EventTypeID");

                    b.ToTable("EventType", "Source");

                    b.HasData(
                        new
                        {
                            EventTypeID = 1,
                            EventTypeCode = "ARRIVE",
                            EventTypeDesc = "Widget Arrival"
                        },
                        new
                        {
                            EventTypeID = 2,
                            EventTypeCode = "CAN_ARRIVE",
                            EventTypeDesc = "Cancel Widget Arrival"
                        },
                        new
                        {
                            EventTypeID = 3,
                            EventTypeCode = "LEAVE",
                            EventTypeDesc = "Widget Depart"
                        },
                        new
                        {
                            EventTypeID = 4,
                            EventTypeCode = "CAN_LEAVE",
                            EventTypeDesc = "Cancel Widget Depart"
                        });
                });

            modelBuilder.Entity("MaterializedViews.API.Models.WidgetLatestState", b =>
                {
                    b.Property<int>("WidgetID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WidgetID"));

                    b.Property<DateTime>("ArrivalDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DepartureDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastEventDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LastTripID")
                        .HasColumnType("int");

                    b.HasKey("WidgetID");

                    b.HasIndex("ArrivalDate")
                        .HasDatabaseName("IDX_WidgetLatestState_Arrival");

                    b.HasIndex("DepartureDate")
                        .HasDatabaseName("IDX_WidgetLatestState_Departure");

                    b.HasIndex("LastTripID")
                        .HasDatabaseName("IDX_WidgetLatestState_LastTrip");

                    b.ToTable("WidgetLatestState", "Dest");

                    b.ToView("uv_WidgetLatestState", "Dest");
                });
#pragma warning restore 612, 618
        }
    }
}