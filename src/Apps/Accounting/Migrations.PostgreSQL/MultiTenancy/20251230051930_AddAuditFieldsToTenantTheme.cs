using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Accounting.Migrations.PostgreSQL.MultiTenancy
{
    /// <inheritdoc />
    public partial class AddAuditFieldsToTenantTheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                schema: "tenant",
                table: "TenantThemes",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedByUserName",
                schema: "tenant",
                table: "TenantThemes",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserName",
                schema: "tenant",
                table: "TenantThemes");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserName",
                schema: "tenant",
                table: "TenantThemes");
        }
    }
}
