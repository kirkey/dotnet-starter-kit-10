using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.Migrations.PostgreSQL.Ai
{
    /// <inheritdoc />
    public partial class AddAiAutomation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WebhookToken",
                schema: "ai",
                table: "AgentSchedules",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AgentSchedules_WebhookToken",
                schema: "ai",
                table: "AgentSchedules",
                columns: new[] { "WebhookToken", "TenantId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AgentSchedules_WebhookToken",
                schema: "ai",
                table: "AgentSchedules");

            migrationBuilder.DropColumn(
                name: "WebhookToken",
                schema: "ai",
                table: "AgentSchedules");
        }
    }
}
