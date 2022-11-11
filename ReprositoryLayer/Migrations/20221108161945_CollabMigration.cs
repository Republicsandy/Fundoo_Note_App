using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class CollabMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollabTable_NotesTable_NoteId",
                table: "CollabTable");

            migrationBuilder.RenameColumn(
                name: "NoteId",
                table: "CollabTable",
                newName: "NoteID");

            migrationBuilder.RenameIndex(
                name: "IX_CollabTable_NoteId",
                table: "CollabTable",
                newName: "IX_CollabTable_NoteID");

            migrationBuilder.AddForeignKey(
                name: "FK_CollabTable_NotesTable_NoteID",
                table: "CollabTable",
                column: "NoteID",
                principalTable: "NotesTable",
                principalColumn: "NoteID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollabTable_NotesTable_NoteID",
                table: "CollabTable");

            migrationBuilder.RenameColumn(
                name: "NoteID",
                table: "CollabTable",
                newName: "NoteId");

            migrationBuilder.RenameIndex(
                name: "IX_CollabTable_NoteID",
                table: "CollabTable",
                newName: "IX_CollabTable_NoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_CollabTable_NotesTable_NoteId",
                table: "CollabTable",
                column: "NoteId",
                principalTable: "NotesTable",
                principalColumn: "NoteID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
