using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServeSync.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditableEventCollaboration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "EventCollaborationRequest",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EventCollaborationRequest",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "EventCollaborationRequest",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "EventCollaborationRequest",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "EventCollaborationRequest");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EventCollaborationRequest");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "EventCollaborationRequest");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "EventCollaborationRequest");
        }
    }
}
