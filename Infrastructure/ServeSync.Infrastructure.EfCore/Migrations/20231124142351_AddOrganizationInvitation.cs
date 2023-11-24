using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServeSync.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizationInvitation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EventOrganizationContact",
                type: "int",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "EventOrganization",
                type: "char(36)",
                nullable: true);
            
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EventOrganization",
                type: "int",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.CreateTable(
                name: "OrganizationInvitation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationInvitation", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationInvitation");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EventOrganizationContact");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EventOrganization");
            
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "EventOrganization");
        }
    }
}
