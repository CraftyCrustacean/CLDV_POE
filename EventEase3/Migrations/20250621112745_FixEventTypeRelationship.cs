using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEase3.Migrations
{
    /// <inheritdoc />
    public partial class FixEventTypeRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_EventType_EventTypeId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_EventType_EventType_EventTypeID1",
                table: "EventType");

            migrationBuilder.DropIndex(
                name: "IX_EventType_EventTypeID1",
                table: "EventType");

            migrationBuilder.DropColumn(
                name: "EventTypeID1",
                table: "EventType");

            migrationBuilder.RenameColumn(
                name: "EventTypeId",
                table: "Event",
                newName: "EventTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Event_EventTypeId",
                table: "Event",
                newName: "IX_Event_EventTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_EventType_EventTypeID",
                table: "Event",
                column: "EventTypeID",
                principalTable: "EventType",
                principalColumn: "EventTypeID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_EventType_EventTypeID",
                table: "Event");

            migrationBuilder.RenameColumn(
                name: "EventTypeID",
                table: "Event",
                newName: "EventTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Event_EventTypeID",
                table: "Event",
                newName: "IX_Event_EventTypeId");

            migrationBuilder.AddColumn<int>(
                name: "EventTypeID1",
                table: "EventType",
                type: "int",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_EventType_EventType_EventTypeID1",
                table: "EventType",
                column: "EventTypeID1",
                principalTable: "EventType",
                principalColumn: "EventTypeID");
        }
    }
}
