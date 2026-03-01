using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticleHouse.Migrations
{
    /// <inheritdoc />
    public partial class IdFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tbl_creator",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tbl_article",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_creator",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_article",
                newName: "Id");
        }
    }
}
