using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PublishData.Migrations
{
    /// <inheritdoc />
    public partial class bookcoverrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artists_BookCovers_BookCoverId",
                table: "Artists");

            migrationBuilder.DropIndex(
                name: "IX_Artists_BookCoverId",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "BookCoverId",
                table: "Artists");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "BookCovers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ArtistBookCover",
                columns: table => new
                {
                    ArtistsArtistId = table.Column<int>(type: "int", nullable: false),
                    CoversBookCoverId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistBookCover", x => new { x.ArtistsArtistId, x.CoversBookCoverId });
                    table.ForeignKey(
                        name: "FK_ArtistBookCover_Artists_ArtistsArtistId",
                        column: x => x.ArtistsArtistId,
                        principalTable: "Artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistBookCover_BookCovers_CoversBookCoverId",
                        column: x => x.CoversBookCoverId,
                        principalTable: "BookCovers",
                        principalColumn: "BookCoverId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "ArtistId", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "Pablo", "Foot" },
                    { 2, "Lewis", "Foot" }
                });

            migrationBuilder.InsertData(
                table: "BookCovers",
                columns: new[] { "BookCoverId", "BookId", "DesignIdeas", "DigitalOnly" },
                values: new object[] { 1, 1, "Pablo", true });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "AuthorId", "BasePrice", "PublishDate", "Title" },
                values: new object[] { 2, 1, 0m, new DateOnly(1996, 4, 13), "Lewis New Book" });

            migrationBuilder.InsertData(
                table: "BookCovers",
                columns: new[] { "BookCoverId", "BookId", "DesignIdeas", "DigitalOnly" },
                values: new object[] { 2, 2, "Lewis", false });

            migrationBuilder.CreateIndex(
                name: "IX_BookCovers_BookId",
                table: "BookCovers",
                column: "BookId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistBookCover_CoversBookCoverId",
                table: "ArtistBookCover",
                column: "CoversBookCoverId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCovers_Books_BookId",
                table: "BookCovers",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCovers_Books_BookId",
                table: "BookCovers");

            migrationBuilder.DropTable(
                name: "ArtistBookCover");

            migrationBuilder.DropIndex(
                name: "IX_BookCovers_BookId",
                table: "BookCovers");

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "ArtistId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "ArtistId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BookCovers",
                keyColumn: "BookCoverId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BookCovers",
                keyColumn: "BookCoverId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "BookCovers");

            migrationBuilder.AddColumn<int>(
                name: "BookCoverId",
                table: "Artists",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Artists_BookCoverId",
                table: "Artists",
                column: "BookCoverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_BookCovers_BookCoverId",
                table: "Artists",
                column: "BookCoverId",
                principalTable: "BookCovers",
                principalColumn: "BookCoverId");
        }
    }
}
