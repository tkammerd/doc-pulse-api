using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doc.Pulse.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "Pulse",
                table: "Vendors",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "Pulse",
                table: "Rfps",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "Pulse",
                table: "Receipts",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "Pulse",
                table: "Programs",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "Pulse",
                table: "ObjectCodes",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "Pulse",
                table: "CodeCategories",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "Pulse",
                table: "Appropriations",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "Pulse",
                table: "Agencies",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "Pulse",
                table: "AccountOrganizations",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "Pulse",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "Pulse",
                table: "Rfps");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "Pulse",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "Pulse",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "Pulse",
                table: "ObjectCodes");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "Pulse",
                table: "CodeCategories");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "Pulse",
                table: "Appropriations");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "Pulse",
                table: "Agencies");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "Pulse",
                table: "AccountOrganizations");
        }
    }
}
