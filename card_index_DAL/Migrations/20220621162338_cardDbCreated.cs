using Microsoft.EntityFrameworkCore.Migrations;

namespace card_index_DAL.Migrations
{
#pragma warning disable CS1591
    public partial class cardDbCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorTextCard_Author_AuthorsId",
                table: "AuthorTextCard");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorTextCard_TextCard_TextCardsId",
                table: "AuthorTextCard");

            migrationBuilder.DropForeignKey(
                name: "FK_RateDetail_TextCard_TextCardId",
                table: "RateDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_RateDetail_Users_UserId",
                table: "RateDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_TextCard_Genre_GenreId",
                table: "TextCard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TextCard",
                table: "TextCard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RateDetail",
                table: "RateDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genre",
                table: "Genre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Author",
                table: "Author");

            migrationBuilder.RenameTable(
                name: "TextCard",
                newName: "TextCards");

            migrationBuilder.RenameTable(
                name: "RateDetail",
                newName: "RateDetails");

            migrationBuilder.RenameTable(
                name: "Genre",
                newName: "Genres");

            migrationBuilder.RenameTable(
                name: "Author",
                newName: "Authors");

            migrationBuilder.RenameIndex(
                name: "IX_TextCard_GenreId",
                table: "TextCards",
                newName: "IX_TextCards_GenreId");

            migrationBuilder.RenameIndex(
                name: "IX_RateDetail_UserId",
                table: "RateDetails",
                newName: "IX_RateDetails_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RateDetail_TextCardId",
                table: "RateDetails",
                newName: "IX_RateDetails_TextCardId");

            migrationBuilder.AddColumn<double>(
                name: "CardRating",
                table: "TextCards",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "RateValue",
                table: "RateDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TextCards",
                table: "TextCards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RateDetails",
                table: "RateDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genres",
                table: "Genres",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Authors",
                table: "Authors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorTextCard_Authors_AuthorsId",
                table: "AuthorTextCard",
                column: "AuthorsId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorTextCard_TextCards_TextCardsId",
                table: "AuthorTextCard",
                column: "TextCardsId",
                principalTable: "TextCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RateDetails_TextCards_TextCardId",
                table: "RateDetails",
                column: "TextCardId",
                principalTable: "TextCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RateDetails_Users_UserId",
                table: "RateDetails",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TextCards_Genres_GenreId",
                table: "TextCards",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorTextCard_Authors_AuthorsId",
                table: "AuthorTextCard");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorTextCard_TextCards_TextCardsId",
                table: "AuthorTextCard");

            migrationBuilder.DropForeignKey(
                name: "FK_RateDetails_TextCards_TextCardId",
                table: "RateDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RateDetails_Users_UserId",
                table: "RateDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TextCards_Genres_GenreId",
                table: "TextCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TextCards",
                table: "TextCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RateDetails",
                table: "RateDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genres",
                table: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Authors",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "CardRating",
                table: "TextCards");

            migrationBuilder.RenameTable(
                name: "TextCards",
                newName: "TextCard");

            migrationBuilder.RenameTable(
                name: "RateDetails",
                newName: "RateDetail");

            migrationBuilder.RenameTable(
                name: "Genres",
                newName: "Genre");

            migrationBuilder.RenameTable(
                name: "Authors",
                newName: "Author");

            migrationBuilder.RenameIndex(
                name: "IX_TextCards_GenreId",
                table: "TextCard",
                newName: "IX_TextCard_GenreId");

            migrationBuilder.RenameIndex(
                name: "IX_RateDetails_UserId",
                table: "RateDetail",
                newName: "IX_RateDetail_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RateDetails_TextCardId",
                table: "RateDetail",
                newName: "IX_RateDetail_TextCardId");

            migrationBuilder.AlterColumn<double>(
                name: "RateValue",
                table: "RateDetail",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TextCard",
                table: "TextCard",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RateDetail",
                table: "RateDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genre",
                table: "Genre",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Author",
                table: "Author",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorTextCard_Author_AuthorsId",
                table: "AuthorTextCard",
                column: "AuthorsId",
                principalTable: "Author",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorTextCard_TextCard_TextCardsId",
                table: "AuthorTextCard",
                column: "TextCardsId",
                principalTable: "TextCard",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RateDetail_TextCard_TextCardId",
                table: "RateDetail",
                column: "TextCardId",
                principalTable: "TextCard",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RateDetail_Users_UserId",
                table: "RateDetail",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TextCard_Genre_GenreId",
                table: "TextCard",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
