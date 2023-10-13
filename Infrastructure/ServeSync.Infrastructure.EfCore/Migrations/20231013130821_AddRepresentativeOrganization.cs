using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServeSync.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddRepresentativeOrganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RepresentativeOrganizationId",
                table: "Event",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Event_RepresentativeOrganizationId",
                table: "Event",
                column: "RepresentativeOrganizationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_OrganizationInEvent_RepresentativeOrganizationId",
                table: "Event",
                column: "RepresentativeOrganizationId",
                principalTable: "OrganizationInEvent",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_OrganizationInEvent_RepresentativeOrganizationId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_RepresentativeOrganizationId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "RepresentativeOrganizationId",
                table: "Event");
        }
    }
}
