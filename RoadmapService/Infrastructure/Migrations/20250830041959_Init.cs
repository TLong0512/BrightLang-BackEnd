using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roadmap",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LevelStartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LevelEndId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roadmap", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoadmapElement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoadmapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionPerDay = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    RangeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadmapElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoadmapElement_Roadmap_RoadmapId",
                        column: x => x.RoadmapId,
                        principalTable: "Roadmap",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRoadmap",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoadmapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoadmap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoadmap_Roadmap_RoadmapId",
                        column: x => x.RoadmapId,
                        principalTable: "Roadmap",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Process",
                columns: table => new
                {
                    UserRoadmapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoadmapElementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    IsOpened = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Process", x => new { x.UserRoadmapId, x.RoadmapElementId });
                    table.ForeignKey(
                        name: "FK_Process_RoadmapElement_RoadmapElementId",
                        column: x => x.RoadmapElementId,
                        principalTable: "RoadmapElement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Process_UserRoadmap_UserRoadmapId",
                        column: x => x.UserRoadmapId,
                        principalTable: "UserRoadmap",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Process_RoadmapElementId",
                table: "Process",
                column: "RoadmapElementId");

            migrationBuilder.CreateIndex(
                name: "IX_Roadmap_LevelEndId",
                table: "Roadmap",
                column: "LevelEndId");

            migrationBuilder.CreateIndex(
                name: "IX_Roadmap_LevelStartId",
                table: "Roadmap",
                column: "LevelStartId");

            migrationBuilder.CreateIndex(
                name: "IX_RoadmapElement_RangeId",
                table: "RoadmapElement",
                column: "RangeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoadmapElement_RoadmapId",
                table: "RoadmapElement",
                column: "RoadmapId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoadmap_RoadmapId",
                table: "UserRoadmap",
                column: "RoadmapId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoadmap_UserId",
                table: "UserRoadmap",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Process");

            migrationBuilder.DropTable(
                name: "RoadmapElement");

            migrationBuilder.DropTable(
                name: "UserRoadmap");

            migrationBuilder.DropTable(
                name: "Roadmap");
        }
    }
}
