using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace FSH.Starter.Migrations.PostgreSQL.Ai
{
    /// <inheritdoc />
    public partial class AddAiEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ai");

            migrationBuilder.CreateTable(
                name: "Sources",
                schema: "ai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Kind = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Status = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    SourceRef = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    MdStorageKey = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    ErrorReason = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chunks",
                schema: "ai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ordinal = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CharCount = table.Column<int>(type: "integer", nullable: false),
                    Embedding = table.Column<HalfVector>(type: "halfvec(3072)", nullable: true),
                    EmbeddingModel = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chunks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chunks_Sources_SourceId",
                        column: x => x.SourceId,
                        principalSchema: "ai",
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chunks_Embedding",
                schema: "ai",
                table: "Chunks",
                column: "Embedding")
                .Annotation("Npgsql:IndexMethod", "hnsw")
                .Annotation("Npgsql:IndexOperators", new[] { "halfvec_cosine_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_Chunks_Ordinal",
                schema: "ai",
                table: "Chunks",
                column: "Ordinal");

            migrationBuilder.CreateIndex(
                name: "IX_Chunks_SourceId",
                schema: "ai",
                table: "Chunks",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_IsDeleted",
                schema: "ai",
                table: "Sources",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_SourceRef",
                schema: "ai",
                table: "Sources",
                columns: new[] { "SourceRef", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sources_Status",
                schema: "ai",
                table: "Sources",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chunks",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "Sources",
                schema: "ai");
        }
    }
}
