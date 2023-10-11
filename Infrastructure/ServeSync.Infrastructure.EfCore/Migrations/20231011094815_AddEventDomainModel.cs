using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServeSync.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddEventDomainModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategory", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EventOrganization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: false),
                    Address = table.Column<string>(type: "longtext", nullable: true),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventOrganization", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EventActivity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    MinScore = table.Column<double>(type: "double", nullable: false),
                    MaxScore = table.Column<double>(type: "double", nullable: false),
                    EventCategoryId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventActivity_EventCategory_EventCategoryId",
                        column: x => x.EventCategoryId,
                        principalTable: "EventCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EventOrganizationContact",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Gender = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Birth = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: false),
                    Address = table.Column<string>(type: "longtext", nullable: true),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false),
                    Position = table.Column<string>(type: "longtext", nullable: true),
                    EventOrganizationId = table.Column<Guid>(type: "char(36)", nullable: false),
                    IdentityId = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventOrganizationContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventOrganizationContact_EventOrganization_EventOrganization~",
                        column: x => x.EventOrganizationId,
                        principalTable: "EventOrganization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Introduction = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Address_FullAddress = table.Column<string>(type: "longtext", nullable: false),
                    Address_Longitude = table.Column<double>(type: "double", nullable: false),
                    Address_Latitude = table.Column<double>(type: "double", nullable: false),
                    ActivityId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Event_EventActivity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "EventActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EventCollaborationRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Introduction = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Address_FullAddress = table.Column<string>(type: "longtext", nullable: false),
                    Address_Longitude = table.Column<double>(type: "double", nullable: false),
                    Address_Latitude = table.Column<double>(type: "double", nullable: false),
                    Organization_Name = table.Column<string>(type: "longtext", nullable: false),
                    Organization_Description = table.Column<string>(type: "longtext", nullable: true),
                    Organization_Email = table.Column<string>(type: "longtext", nullable: false),
                    Organization_PhoneNumber = table.Column<string>(type: "longtext", nullable: false),
                    Organization_Address = table.Column<string>(type: "longtext", nullable: true),
                    Organization_ImageUrl = table.Column<string>(type: "longtext", nullable: false),
                    OrganizationContact_Name = table.Column<string>(type: "longtext", nullable: false),
                    OrganizationContact_Email = table.Column<string>(type: "longtext", nullable: false),
                    OrganizationContact_PhoneNumber = table.Column<string>(type: "longtext", nullable: false),
                    OrganizationContact_Gender = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    OrganizationContact_Address = table.Column<string>(type: "longtext", nullable: true),
                    OrganizationContact_Birth = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizationContact_Position = table.Column<string>(type: "longtext", nullable: true),
                    OrganizationContact_ImageUrl = table.Column<string>(type: "longtext", nullable: false),
                    ActivityId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCollaborationRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventCollaborationRequest_EventActivity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "EventActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EventAttendanceInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Code = table.Column<string>(type: "longtext", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EventId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAttendanceInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventAttendanceInfo_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EventRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    IsNeedApprove = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Score = table.Column<double>(type: "double", nullable: false),
                    EventId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventRole_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrganizationInEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EventId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Role = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationInEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationInEvent_EventOrganization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "EventOrganization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationInEvent_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrganizationRepInEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    OrganizationInEventId = table.Column<Guid>(type: "char(36)", nullable: false),
                    OrganizationRepId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Role = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationRepInEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationRepInEvent_EventOrganizationContact_Organization~",
                        column: x => x.OrganizationRepId,
                        principalTable: "EventOrganizationContact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationRepInEvent_OrganizationInEvent_OrganizationInEve~",
                        column: x => x.OrganizationInEventId,
                        principalTable: "OrganizationInEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Event_ActivityId",
                table: "Event",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_EventActivity_EventCategoryId",
                table: "EventActivity",
                column: "EventCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EventAttendanceInfo_EventId",
                table: "EventAttendanceInfo",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventCollaborationRequest_ActivityId",
                table: "EventCollaborationRequest",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_EventOrganizationContact_EventOrganizationId",
                table: "EventOrganizationContact",
                column: "EventOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRole_EventId",
                table: "EventRole",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationInEvent_EventId",
                table: "OrganizationInEvent",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationInEvent_OrganizationId",
                table: "OrganizationInEvent",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationRepInEvent_OrganizationInEventId",
                table: "OrganizationRepInEvent",
                column: "OrganizationInEventId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationRepInEvent_OrganizationRepId",
                table: "OrganizationRepInEvent",
                column: "OrganizationRepId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventAttendanceInfo");

            migrationBuilder.DropTable(
                name: "EventCollaborationRequest");

            migrationBuilder.DropTable(
                name: "EventRole");

            migrationBuilder.DropTable(
                name: "OrganizationRepInEvent");

            migrationBuilder.DropTable(
                name: "EventOrganizationContact");

            migrationBuilder.DropTable(
                name: "OrganizationInEvent");

            migrationBuilder.DropTable(
                name: "EventOrganization");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "EventActivity");

            migrationBuilder.DropTable(
                name: "EventCategory");
        }
    }
}
