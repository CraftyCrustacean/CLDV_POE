using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEase3.Migrations
{
    /// <inheritdoc />
    public partial class AddEventTypeAndAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Availability",
                table: "Venue",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EventTypeId",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EventType",
                columns: table => new
                {
                    EventTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventTypeID1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventType", x => x.EventTypeID);
                    table.ForeignKey(
                        name: "FK_EventType_EventType_EventTypeID1",
                        column: x => x.EventTypeID1,
                        principalTable: "EventType",
                        principalColumn: "EventTypeID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Event_EventTypeId",
                table: "Event",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EventType_EventTypeID1",
                table: "EventType",
                column: "EventTypeID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_EventType_EventTypeId",
                table: "Event",
                column: "EventTypeId",
                principalTable: "EventType",
                principalColumn: "EventTypeID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_EventType_EventTypeId",
                table: "Event");

            migrationBuilder.DropTable(
                name: "EventType");

            migrationBuilder.DropIndex(
                name: "IX_Event_EventTypeId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Venue");

            migrationBuilder.DropColumn(
                name: "EventTypeId",
                table: "Event");
        }
    }
}
