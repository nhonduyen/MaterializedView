using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MaterializedViews.API.Migrations
{
    /// <inheritdoc />
    public partial class initdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Source");

            migrationBuilder.EnsureSchema(
                name: "Dest");

            migrationBuilder.CreateTable(
                name: "Event",
                schema: "Source",
                columns: table => new
                {
                    WidgetID = table.Column<int>(type: "int", nullable: false),
                    EventTypeID = table.Column<int>(type: "int", nullable: false),
                    TripID = table.Column<int>(type: "int", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => new { x.TripID, x.WidgetID, x.EventTypeID, x.EventDate });
                });

            migrationBuilder.CreateTable(
                name: "EventType",
                schema: "Source",
                columns: table => new
                {
                    EventTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventTypeCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EventTypeDesc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventType", x => x.EventTypeID);
                });

            migrationBuilder.CreateTable(
                name: "WidgetLatestState",
                schema: "Dest",
                columns: table => new
                {
                    WidgetID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastTripID = table.Column<int>(type: "int", nullable: false),
                    LastEventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartureDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WidgetLatestState", x => x.WidgetID);
                });

            migrationBuilder.InsertData(
                schema: "Source",
                table: "EventType",
                columns: new[] { "EventTypeID", "EventTypeCode", "EventTypeDesc" },
                values: new object[,]
                {
                    { 1, "ARRIVE", "Widget Arrival" },
                    { 2, "CAN_ARRIVE", "Cancel Widget Arrival" },
                    { 3, "LEAVE", "Widget Depart" },
                    { 4, "CAN_LEAVE", "Cancel Widget Depart" }
                });

            migrationBuilder.CreateIndex(
                name: "IDX_Event_Date",
                schema: "Source",
                table: "Event",
                columns: new[] { "EventDate", "EventTypeID" })
                .Annotation("SqlServer:Include", new[] { "WidgetID" });

            migrationBuilder.CreateIndex(
                name: "IDX_Event_Trip",
                schema: "Source",
                table: "Event",
                columns: new[] { "TripID", "WidgetID" });

            migrationBuilder.CreateIndex(
                name: "IDX_Event_Widget",
                schema: "Source",
                table: "Event",
                columns: new[] { "WidgetID", "TripID", "EventDate" })
                .Annotation("SqlServer:Include", new[] { "EventTypeID" });

            migrationBuilder.CreateIndex(
                name: "IDX_WidgetLatestState_Arrival",
                schema: "Dest",
                table: "WidgetLatestState",
                column: "ArrivalDate");

            migrationBuilder.CreateIndex(
                name: "IDX_WidgetLatestState_Departure",
                schema: "Dest",
                table: "WidgetLatestState",
                column: "DepartureDate");

            migrationBuilder.CreateIndex(
                name: "IDX_WidgetLatestState_LastTrip",
                schema: "Dest",
                table: "WidgetLatestState",
                column: "LastTripID");

            migrationBuilder.Sql(@"
CREATE VIEW [Dest].[uv_WidgetLatestState]
AS
SELECT
	lw.WidgetID
	, la.LastTripID
	, lw.LastEventDate
	, la.ArrivalDate
	, (SELECT MAX(de.EventDate)
		FROM [Source].[Event] de
		WHERE de.EventTypeID = 3
		AND de.WidgetID = lw.WidgetID
		AND de.TripID = la.LastTripID
		AND NOT EXISTS
			(SELECT 0
			FROM [Source].[Event] dc
			WHERE lw.WidgetID = dc.WidgetID
			AND la.LastTripID = dc.TripID
			AND dc.EventTypeID = 4
			AND dc.EventDate > de.EventDate)) AS DepartureDate
FROM
	(SELECT
		e.WidgetID
		, MAX(e.EventDate) AS LastEventDate
	FROM
		[Source].[Event] e
	GROUP BY
		e.WidgetID) lw
	LEFT OUTER JOIN
	(SELECT
		ae.WidgetID
		, ae.TripID AS LastTripID
		, ae.EventDate AS ArrivalDate
	FROM
		[Source].[Event] ae
	WHERE
		ae.EventTypeID = 1
	AND	ae.EventDate =
		(SELECT MAX(la.EventDate)
		FROM [Source].[Event] la
		WHERE la.EventTypeID = 1
		AND la.WidgetID = ae.WidgetID
		AND NOT EXISTS
			(SELECT 0
			FROM [Source].[Event] ac
			WHERE la.WidgetID = ac.WidgetID
			AND la.TripID = ac.TripID
			AND ac.EventTypeID = 2
			AND ac.EventDate > la.EventDate))) AS la ON lw.WidgetID = la.WidgetID
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event",
                schema: "Source");

            migrationBuilder.DropTable(
                name: "EventType",
                schema: "Source");

            migrationBuilder.DropTable(
                name: "WidgetLatestState",
                schema: "Dest");

            migrationBuilder.Sql(@"
drop view [Dest].[uv_WidgetLatestState];
");
        }
    }
}
