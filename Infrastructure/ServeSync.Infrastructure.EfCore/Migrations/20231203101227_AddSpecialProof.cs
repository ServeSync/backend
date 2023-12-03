using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServeSync.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecialProof : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AttendanceAt",
                table: "Proof",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.CreateTable(
                name: "SpecialProof",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: false),
                    Role = table.Column<string>(type: "longtext", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Score = table.Column<double>(type: "double", nullable: false),
                    ActivityId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialProof", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialProof_EventActivity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "EventActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialProof_Proof_Id",
                        column: x => x.Id,
                        principalTable: "Proof",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialProof_ActivityId",
                table: "SpecialProof",
                column: "ActivityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialProof");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AttendanceAt",
                table: "Proof",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);
        }
    }
}
