using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doc.Pulse.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Pulse");

            migrationBuilder.CreateTable(
                name: "UserStubs",
                schema: "Pulse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SqlCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    SqlModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    SqlModifiedUser = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true, defaultValueSql: "SYSTEM_USER")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStubs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodeCategories",
                schema: "Pulse",
                columns: table => new
                {
                    CodeCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryNumber = table.Column<int>(type: "int", nullable: false),
                    CategoryShortName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    CategoryName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Inactive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedUserId = table.Column<int>(type: "int", nullable: true),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    ModifiedUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeCategories", x => x.CodeCategoryId);
                    table.ForeignKey(
                        name: "FK_CodeCategories_UserStubs_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalSchema: "Pulse",
                        principalTable: "UserStubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CodeCategories_UserStubs_ModifiedUserId",
                        column: x => x.ModifiedUserId,
                        principalSchema: "Pulse",
                        principalTable: "UserStubs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ObjectCodes",
                schema: "Pulse",
                columns: table => new
                {
                    ObjectCodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeNumber = table.Column<int>(type: "int", nullable: false),
                    CodeName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    CodeCategoryId = table.Column<int>(type: "int", nullable: false),
                    Inactive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedUserId = table.Column<int>(type: "int", nullable: true),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    ModifiedUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectCodes", x => x.ObjectCodeId);
                    table.ForeignKey(
                        name: "FK_ObjectCodes_CodeCategories_CodeCategoryId",
                        column: x => x.CodeCategoryId,
                        principalSchema: "Pulse",
                        principalTable: "CodeCategories",
                        principalColumn: "CodeCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObjectCodes_UserStubs_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalSchema: "Pulse",
                        principalTable: "UserStubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ObjectCodes_UserStubs_ModifiedUserId",
                        column: x => x.ModifiedUserId,
                        principalSchema: "Pulse",
                        principalTable: "UserStubs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeCategories_CreatedUserId",
                schema: "Pulse",
                table: "CodeCategories",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeCategories_ModifiedUserId",
                schema: "Pulse",
                table: "CodeCategories",
                column: "ModifiedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectCodes_CodeCategoryId",
                schema: "Pulse",
                table: "ObjectCodes",
                column: "CodeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectCodes_CreatedUserId",
                schema: "Pulse",
                table: "ObjectCodes",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectCodes_ModifiedUserId",
                schema: "Pulse",
                table: "ObjectCodes",
                column: "ModifiedUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObjectCodes",
                schema: "Pulse");

            migrationBuilder.DropTable(
                name: "CodeCategories",
                schema: "Pulse");

            migrationBuilder.DropTable(
                name: "UserStubs",
                schema: "Pulse");
        }
    }
}
