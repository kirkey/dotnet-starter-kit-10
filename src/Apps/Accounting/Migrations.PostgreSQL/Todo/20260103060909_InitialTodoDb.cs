using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace FSH.Accounting.Migrations.PostgreSQL.Todo
{
    /// <inheritdoc />
    public partial class InitialTodoDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "todo");

            migrationBuilder.CreateTable(
                name: "Todos",
                schema: "todo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    DueDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedByUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastModifiedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModifiedByUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TodoTasks",
                schema: "todo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TodoId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedByUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastModifiedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModifiedByUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoTasks_Todos_TodoId",
                        column: x => x.TodoId,
                        principalSchema: "todo",
                        principalTable: "Todos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Todos_CreatedOnUtc",
                schema: "todo",
                table: "Todos",
                column: "CreatedOnUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_DueDate",
                schema: "todo",
                table: "Todos",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_IsActive",
                schema: "todo",
                table: "Todos",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_Priority",
                schema: "todo",
                table: "Todos",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_Status",
                schema: "todo",
                table: "Todos",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_TenantId",
                schema: "todo",
                table: "Todos",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_IsActive",
                schema: "todo",
                table: "TodoTasks",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_IsCompleted",
                schema: "todo",
                table: "TodoTasks",
                column: "IsCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_SortOrder",
                schema: "todo",
                table: "TodoTasks",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_TenantId",
                schema: "todo",
                table: "TodoTasks",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_TodoId",
                schema: "todo",
                table: "TodoTasks",
                column: "TodoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoTasks",
                schema: "todo");

            migrationBuilder.DropTable(
                name: "Todos",
                schema: "todo");
        }
    }
}
