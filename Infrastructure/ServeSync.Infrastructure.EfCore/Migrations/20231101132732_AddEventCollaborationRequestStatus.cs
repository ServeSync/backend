using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServeSync.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddEventCollaborationRequestStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "EventCollaborationRequest",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EventCollaborationRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventId",
                table: "EventCollaborationRequest");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EventCollaborationRequest");
        }
    }
}
