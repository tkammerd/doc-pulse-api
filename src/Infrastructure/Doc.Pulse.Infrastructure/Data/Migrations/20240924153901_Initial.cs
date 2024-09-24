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
                name: "AccountOrganizations",
                schema: "Pulse",
                columns: table => new
                {
                    AccountOrganizationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountOrganizationNumber = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    CostCenterDescription = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Inactive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedUserId = table.Column<int>(type: "int", nullable: true),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    ModifiedUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountOrganizations", x => x.AccountOrganizationId);
                    table.ForeignKey(
                        name: "FK_AccountOrganizations_UserStubs_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalSchema: "Pulse",
                        principalTable: "UserStubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccountOrganizations_UserStubs_ModifiedUserId",
                        column: x => x.ModifiedUserId,
                        principalSchema: "Pulse",
                        principalTable: "UserStubs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Agencies",
                schema: "Pulse",
                columns: table => new
                {
                    AgencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgencyName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Inactive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedUserId = table.Column<int>(type: "int", nullable: true),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    ModifiedUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencies", x => x.AgencyId);
                    table.ForeignKey(
                        name: "FK_Agencies_UserStubs_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalSchema: "Pulse",
                        principalTable: "UserStubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Agencies_UserStubs_ModifiedUserId",
                        column: x => x.ModifiedUserId,
                        principalSchema: "Pulse",
                        principalTable: "UserStubs",
                        principalColumn: "Id");
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
                name: "Programs",
                schema: "Pulse",
                columns: table => new
                {
                    ProgramId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramCode = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    ProgramName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    ProgramDescription = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Inactive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedUserId = table.Column<int>(type: "int", nullable: true),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    ModifiedUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.ProgramId);
                    table.ForeignKey(
                        name: "FK_Programs_UserStubs_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalSchema: "Pulse",
                        principalTable: "UserStubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Programs_UserStubs_ModifiedUserId",
                        column: x => x.ModifiedUserId,
                        principalSchema: "Pulse",
                        principalTable: "UserStubs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                schema: "Pulse",
                columns: table => new
                {
                    VendorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Inactive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedUserId = table.Column<int>(type: "int", nullable: true),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    ModifiedUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.VendorId);
                    table.ForeignKey(
                        name: "FK_Vendors_UserStubs_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalSchema: "Pulse",
                        principalTable: "UserStubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vendors_UserStubs_ModifiedUserId",
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
                    CodeCategoryId = table.Column<int>(type: "int", nullable: true),
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
                        principalColumn: "CodeCategoryId");
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

            migrationBuilder.CreateTable(
                name: "Appropriations",
                schema: "Pulse",
                columns: table => new
                {
                    AppropriationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Facility = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    FiscalYear = table.Column<short>(type: "smallint", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    ObjectCodeId = table.Column<int>(type: "int", nullable: false),
                    CurrentModifiedAmount = table.Column<decimal>(type: "money", nullable: true),
                    PreEncumberedAmount = table.Column<decimal>(type: "money", nullable: true),
                    EncumberedAmount = table.Column<decimal>(type: "money", nullable: true),
                    ExpendedAmount = table.Column<decimal>(type: "money", nullable: true),
                    ProjectedAmount = table.Column<decimal>(type: "money", nullable: true),
                    PriorYearActualAmount = table.Column<decimal>(type: "money", nullable: true),
                    TotalObligated = table.Column<decimal>(type: "money", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedUserId = table.Column<int>(type: "int", nullable: true),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    ModifiedUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appropriations", x => x.AppropriationId);
                    table.ForeignKey(
                        name: "FK_Appropriations_ObjectCodes_ObjectCodeId",
                        column: x => x.ObjectCodeId,
                        principalSchema: "Pulse",
                        principalTable: "ObjectCodes",
                        principalColumn: "ObjectCodeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appropriations_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalSchema: "Pulse",
                        principalTable: "Programs",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appropriations_UserStubs_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalSchema: "Pulse",
                        principalTable: "UserStubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appropriations_UserStubs_ModifiedUserId",
                        column: x => x.ModifiedUserId,
                        principalSchema: "Pulse",
                        principalTable: "UserStubs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rfps",
                schema: "Pulse",
                columns: table => new
                {
                    RfpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FiscalYear = table.Column<short>(type: "smallint", nullable: false),
                    RfpNumber = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Facility = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    RfpDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    ObjectCodeId = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    AgencyId = table.Column<int>(type: "int", nullable: false),
                    AccountOrganizationId = table.Column<int>(type: "int", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrderNumber = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    AmountObligated = table.Column<decimal>(type: "money", nullable: true),
                    Completed = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    CheckOrDocumentNumber = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Comments = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    ReportingCategory = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    VerifiedOnIsis = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    RequestedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedUserId = table.Column<int>(type: "int", nullable: true),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    ModifiedUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rfps", x => x.RfpId);
                    table.ForeignKey(
                        name: "FK_Rfps_AccountOrganizations_AccountOrganizationId",
                        column: x => x.AccountOrganizationId,
                        principalSchema: "Pulse",
                        principalTable: "AccountOrganizations",
                        principalColumn: "AccountOrganizationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rfps_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalSchema: "Pulse",
                        principalTable: "Agencies",
                        principalColumn: "AgencyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rfps_ObjectCodes_ObjectCodeId",
                        column: x => x.ObjectCodeId,
                        principalSchema: "Pulse",
                        principalTable: "ObjectCodes",
                        principalColumn: "ObjectCodeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rfps_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalSchema: "Pulse",
                        principalTable: "Programs",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rfps_UserStubs_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalSchema: "Pulse",
                        principalTable: "UserStubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rfps_UserStubs_ModifiedUserId",
                        column: x => x.ModifiedUserId,
                        principalSchema: "Pulse",
                        principalTable: "UserStubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rfps_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalSchema: "Pulse",
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Receipts",
                schema: "Pulse",
                columns: table => new
                {
                    ReceiptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Facility = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    FiscalYear = table.Column<short>(type: "smallint", nullable: false),
                    ReceiptNumber = table.Column<int>(type: "int", nullable: false),
                    RfpId = table.Column<int>(type: "int", nullable: true),
                    ReceiptDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReceivingReportAmount = table.Column<decimal>(type: "money", nullable: true),
                    AmountInIsis = table.Column<decimal>(type: "money", nullable: true),
                    ReceiverNumber = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    CheckNumber = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    CheckDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedUserId = table.Column<int>(type: "int", nullable: true),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    ModifiedUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipts", x => x.ReceiptId);
                    table.ForeignKey(
                        name: "FK_Receipts_Rfps_RfpId",
                        column: x => x.RfpId,
                        principalSchema: "Pulse",
                        principalTable: "Rfps",
                        principalColumn: "RfpId");
                    table.ForeignKey(
                        name: "FK_Receipts_UserStubs_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalSchema: "Pulse",
                        principalTable: "UserStubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Receipts_UserStubs_ModifiedUserId",
                        column: x => x.ModifiedUserId,
                        principalSchema: "Pulse",
                        principalTable: "UserStubs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountOrganizations_CreatedUserId",
                schema: "Pulse",
                table: "AccountOrganizations",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountOrganizations_ModifiedUserId",
                schema: "Pulse",
                table: "AccountOrganizations",
                column: "ModifiedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_CreatedUserId",
                schema: "Pulse",
                table: "Agencies",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_ModifiedUserId",
                schema: "Pulse",
                table: "Agencies",
                column: "ModifiedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Appropriations_CreatedUserId",
                schema: "Pulse",
                table: "Appropriations",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Appropriations_ModifiedUserId",
                schema: "Pulse",
                table: "Appropriations",
                column: "ModifiedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Appropriations_ObjectCodeId",
                schema: "Pulse",
                table: "Appropriations",
                column: "ObjectCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Appropriations_ProgramId",
                schema: "Pulse",
                table: "Appropriations",
                column: "ProgramId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Programs_CreatedUserId",
                schema: "Pulse",
                table: "Programs",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_ModifiedUserId",
                schema: "Pulse",
                table: "Programs",
                column: "ModifiedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_CreatedUserId",
                schema: "Pulse",
                table: "Receipts",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_ModifiedUserId",
                schema: "Pulse",
                table: "Receipts",
                column: "ModifiedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_RfpId",
                schema: "Pulse",
                table: "Receipts",
                column: "RfpId");

            migrationBuilder.CreateIndex(
                name: "IX_Rfps_AccountOrganizationId",
                schema: "Pulse",
                table: "Rfps",
                column: "AccountOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rfps_AgencyId",
                schema: "Pulse",
                table: "Rfps",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Rfps_CreatedUserId",
                schema: "Pulse",
                table: "Rfps",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rfps_ModifiedUserId",
                schema: "Pulse",
                table: "Rfps",
                column: "ModifiedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rfps_ObjectCodeId",
                schema: "Pulse",
                table: "Rfps",
                column: "ObjectCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rfps_ProgramId",
                schema: "Pulse",
                table: "Rfps",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Rfps_VendorId",
                schema: "Pulse",
                table: "Rfps",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_CreatedUserId",
                schema: "Pulse",
                table: "Vendors",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_ModifiedUserId",
                schema: "Pulse",
                table: "Vendors",
                column: "ModifiedUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appropriations",
                schema: "Pulse");

            migrationBuilder.DropTable(
                name: "Receipts",
                schema: "Pulse");

            migrationBuilder.DropTable(
                name: "Rfps",
                schema: "Pulse");

            migrationBuilder.DropTable(
                name: "AccountOrganizations",
                schema: "Pulse");

            migrationBuilder.DropTable(
                name: "Agencies",
                schema: "Pulse");

            migrationBuilder.DropTable(
                name: "ObjectCodes",
                schema: "Pulse");

            migrationBuilder.DropTable(
                name: "Programs",
                schema: "Pulse");

            migrationBuilder.DropTable(
                name: "Vendors",
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
