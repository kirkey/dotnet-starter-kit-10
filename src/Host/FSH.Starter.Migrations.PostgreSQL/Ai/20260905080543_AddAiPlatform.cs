using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.Migrations.PostgreSQL.Ai
{
    /// <inheritdoc />
    public partial class AddAiPlatform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgentRuns",
                schema: "ai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScheduleId = table.Column<Guid>(type: "uuid", nullable: true),
                    Trigger = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Status = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Input = table.Column<string>(type: "text", nullable: false),
                    Output = table.Column<string>(type: "text", nullable: true),
                    Error = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    StartedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentRuns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgentSchedules",
                schema: "ai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cron = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TaskType = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    TaskConfigJson = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LastRunOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastContentHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatSessions",
                schema: "ai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Model = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Variant = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastActivityUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    MessageCount = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "ai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                schema: "ai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ProviderType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    BaseUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    ChatModel = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmbeddingModel = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmbeddingDimensions = table.Column<int>(type: "integer", nullable: false),
                    IsDefaultChat = table.Column<bool>(type: "boolean", nullable: false),
                    IsDefaultEmbedding = table.Column<bool>(type: "boolean", nullable: false),
                    Revision = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProviderSecrets",
                schema: "ai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    KeyName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProtectedValue = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderSecrets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Runtimes",
                schema: "ai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Family = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DetectedVersion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Source = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    IsOnline = table.Column<bool>(type: "boolean", nullable: false),
                    LastSeenOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Runtimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                schema: "ai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ordinal = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CitedSourcesJson = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatSessions_SessionId",
                        column: x => x.SessionId,
                        principalSchema: "ai",
                        principalTable: "ChatSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Agents",
                schema: "ai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Instructions = table.Column<string>(type: "text", nullable: false),
                    SkillsJson = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: false),
                    RuntimeBinding = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Variant = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    AccessMode = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    AccessUserIdsJson = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    ArchivedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agents_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "ai",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProviderModels",
                schema: "ai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModelId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SupportsChat = table.Column<bool>(type: "boolean", nullable: false),
                    SupportsEmbeddings = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderModels_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalSchema: "ai",
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgentRuns_AgentId",
                schema: "ai",
                table: "AgentRuns",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentRuns_Status",
                schema: "ai",
                table: "AgentRuns",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_DepartmentId",
                schema: "ai",
                table: "Agents",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_DepartmentId_Name",
                schema: "ai",
                table: "Agents",
                columns: new[] { "DepartmentId", "Name", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agents_IsArchived",
                schema: "ai",
                table: "Agents",
                column: "IsArchived");

            migrationBuilder.CreateIndex(
                name: "IX_AgentSchedules_AgentId",
                schema: "ai",
                table: "AgentSchedules",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentSchedules_IsEnabled",
                schema: "ai",
                table: "AgentSchedules",
                column: "IsEnabled");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SessionId",
                schema: "ai",
                table: "ChatMessages",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_LastActivityUtc",
                schema: "ai",
                table: "ChatSessions",
                column: "LastActivityUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_OwnerId",
                schema: "ai",
                table: "ChatSessions",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderModels_ProviderId",
                schema: "ai",
                table: "ProviderModels",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_IsDefaultChat",
                schema: "ai",
                table: "Providers",
                column: "IsDefaultChat");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_IsDefaultEmbedding",
                schema: "ai",
                table: "Providers",
                column: "IsDefaultEmbedding");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderSecrets_ProviderId",
                schema: "ai",
                table: "ProviderSecrets",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Runtimes_Family",
                schema: "ai",
                table: "Runtimes",
                columns: new[] { "Family", "TenantId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgentRuns",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "Agents",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "AgentSchedules",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "ChatMessages",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "ProviderModels",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "ProviderSecrets",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "Runtimes",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "ChatSessions",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "Providers",
                schema: "ai");
        }
    }
}
