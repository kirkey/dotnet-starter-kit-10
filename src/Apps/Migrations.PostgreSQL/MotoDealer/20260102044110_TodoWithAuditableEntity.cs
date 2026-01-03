using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Apps.Migrations.PostgreSQL.MotoDealer
{
    /// <inheritdoc />
    public partial class TodoWithAuditableEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                schema: "todo",
                table: "TodoTasks",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                schema: "todo",
                table: "Todos",
                newName: "Name");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "todo",
                table: "TodoTasks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "todo",
                table: "TodoTasks",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "todo",
                table: "TodoTasks",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "todo",
                table: "Todos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "todo",
                table: "Todos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "todo",
                table: "Todos",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_IsActive",
                schema: "todo",
                table: "TodoTasks",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_IsActive",
                schema: "todo",
                table: "Todos",
                column: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TodoTasks_IsActive",
                schema: "todo",
                table: "TodoTasks");

            migrationBuilder.DropIndex(
                name: "IX_Todos_IsActive",
                schema: "todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "todo",
                table: "TodoTasks");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "todo",
                table: "TodoTasks");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "todo",
                table: "TodoTasks");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "todo",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "todo",
                table: "TodoTasks",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "todo",
                table: "Todos",
                newName: "Title");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "todo",
                table: "Todos",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }
    }
}
