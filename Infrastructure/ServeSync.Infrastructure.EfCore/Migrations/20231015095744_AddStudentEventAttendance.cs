using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServeSync.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentEventAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentEventAttendance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    StudentEventRegisterId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EventAttendanceInfoId = table.Column<Guid>(type: "char(36)", nullable: false),
                    AttendanceAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentEventAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentEventAttendance_EventAttendanceInfo_EventAttendanceIn~",
                        column: x => x.EventAttendanceInfoId,
                        principalTable: "EventAttendanceInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentEventAttendance_StudentEventRegister_StudentEventRegi~",
                        column: x => x.StudentEventRegisterId,
                        principalTable: "StudentEventRegister",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_StudentEventAttendance_EventAttendanceInfoId",
                table: "StudentEventAttendance",
                column: "EventAttendanceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentEventAttendance_StudentEventRegisterId",
                table: "StudentEventAttendance",
                column: "StudentEventRegisterId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentEventAttendance");
        }
    }
}
