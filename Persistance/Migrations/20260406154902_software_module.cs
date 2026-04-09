using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class software_module : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SoftwareProjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FrontendType = table.Column<int>(type: "int", nullable: false),
                    FrontendLibrariesJson = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BackendType = table.Column<int>(type: "int", nullable: false),
                    BackendFramework = table.Column<int>(type: "int", nullable: true),
                    Database = table.Column<int>(type: "int", nullable: true),
                    GithubUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LiveDemoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedByAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedByAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoftwareProjects_Admins_CreatedByAdminId",
                        column: x => x.CreatedByAdminId,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SoftwareProjects_Admins_UpdatedByAdminId",
                        column: x => x.UpdatedByAdminId,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareProjects_BackendType",
                table: "SoftwareProjects",
                column: "BackendType");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareProjects_CreatedAt",
                table: "SoftwareProjects",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareProjects_CreatedByAdminId",
                table: "SoftwareProjects",
                column: "CreatedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareProjects_FrontendType",
                table: "SoftwareProjects",
                column: "FrontendType");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareProjects_NameAr",
                table: "SoftwareProjects",
                column: "NameAr");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareProjects_NameEn",
                table: "SoftwareProjects",
                column: "NameEn");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareProjects_UpdatedByAdminId",
                table: "SoftwareProjects",
                column: "UpdatedByAdminId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoftwareProjects");
        }
    }
}
