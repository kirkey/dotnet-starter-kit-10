using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Playground.Migrations.PostgreSQL.Todo
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "todo");

            migrationBuilder.CreateTable(
                name: "TodoLists",
                schema: "todo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    DueDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedByUserName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LastModifiedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModifiedByUserName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TodoItems",
                schema: "todo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TodoListId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    DueDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CompletedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    EstimatedHours = table.Column<int>(type: "integer", nullable: true),
                    ActualHours = table.Column<int>(type: "integer", nullable: true),
                    AssignedToUserId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AssignedToUserName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedByUserName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LastModifiedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModifiedByUserName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoItems_TodoLists_TodoListId",
                        column: x => x.TodoListId,
                        principalSchema: "todo",
                        principalTable: "TodoLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_AssignedToUserId",
                schema: "todo",
                table: "TodoItems",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_CreatedOnUtc",
                schema: "todo",
                table: "TodoItems",
                column: "CreatedOnUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_DueDate",
                schema: "todo",
                table: "TodoItems",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_IsActive",
                schema: "todo",
                table: "TodoItems",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_Name",
                schema: "todo",
                table: "TodoItems",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_Priority",
                schema: "todo",
                table: "TodoItems",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_Status",
                schema: "todo",
                table: "TodoItems",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_TodoListId",
                schema: "todo",
                table: "TodoItems",
                column: "TodoListId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoLists_CreatedOnUtc",
                schema: "todo",
                table: "TodoLists",
                column: "CreatedOnUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TodoLists_IsActive",
                schema: "todo",
                table: "TodoLists",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TodoLists_Name",
                schema: "todo",
                table: "TodoLists",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TodoLists_Status",
                schema: "todo",
                table: "TodoLists",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TodoLists_TenantId",
                schema: "todo",
                table: "TodoLists",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoItems",
                schema: "todo");

            migrationBuilder.DropTable(
                name: "TodoLists",
                schema: "todo");
        }
    }
}
