using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublishData.Migrations
{
    /// <inheritdoc />
    public partial class adddeletestoreprocedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE DeleteCover
                    @coverId int
                AS
                BEGIN
                    DELETE FROM Covers WHERE CoverId = @coverId
                END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE DeleteCover");
        }
    }
}
