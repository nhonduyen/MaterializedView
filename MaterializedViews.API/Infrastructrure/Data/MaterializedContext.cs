using MaterializedViews.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;

namespace MaterializedViews.API.Infrastructrure.Data
{
    public class MaterializedContext : DbContext
    {
        public MaterializedContext(DbContextOptions<MaterializedContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Source");
            modelBuilder.HasDefaultSchema("Dest");

            modelBuilder.Entity<Event>()
            .HasIndex(p => new { p.TripID, p.WidgetID })
            .HasDatabaseName("IDX_Event_Trip");

            modelBuilder.Entity<Event>()
            .HasIndex(p => new { p.EventDate, p.EventTypeID })
            .IncludeProperties(b => new
            {
                b.WidgetID
            })
            .HasDatabaseName("IDX_Event_Date");

            modelBuilder.Entity<Event>()
            .HasIndex(p => new { p.WidgetID, p.TripID, p.EventDate })
            .IncludeProperties(b => new
            {
                b.EventTypeID
            })
            .HasDatabaseName("IDX_Event_Widget");

            modelBuilder.Entity<WidgetLatestState>()
            .HasIndex(p => p.LastTripID)
            .HasDatabaseName("IDX_WidgetLatestState_LastTrip");

            modelBuilder.Entity<WidgetLatestState>()
            .HasIndex(p => p.ArrivalDate)
            .HasDatabaseName("IDX_WidgetLatestState_Arrival");

            modelBuilder.Entity<WidgetLatestState>()
            .HasIndex(p => p.DepartureDate)
            .HasDatabaseName("IDX_WidgetLatestState_Departure");

            modelBuilder.Entity<EventType>().HasData(
               new EventType
               {
                   EventTypeID = 1,
                   EventTypeCode = "ARRIVE",
                   EventTypeDesc = "Widget Arrival"
               },
               new EventType
               {
                   EventTypeID = 2,
                   EventTypeCode = "CAN_ARRIVE",
                   EventTypeDesc = "Cancel Widget Arrival"
               },
               new EventType
               {
                   EventTypeID = 3,
                   EventTypeCode = "LEAVE",
                   EventTypeDesc = "Widget Depart"
               },
               new EventType
               {
                   EventTypeID = 4,
                   EventTypeCode = "CAN_LEAVE",
                   EventTypeDesc = "Cancel Widget Depart"
               }
               );
        }

        public DbSet<EventType> EventType { get; set; }
        public DbSet<Event> Event { get; set; }
    }
}
