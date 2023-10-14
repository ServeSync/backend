using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServeSync.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditableEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Event",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Event",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Event",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Event",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Event");
        }
    }
}
