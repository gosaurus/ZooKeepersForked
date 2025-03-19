using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZooKeepers.Migrations
{
    /// <inheritdoc />
    public partial class LinkAnimalToEnclosure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnclosureId",
                table: "Animals",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animals_EnclosureId",
                table: "Animals",
                column: "EnclosureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Enclosures_EnclosureId",
                table: "Animals",
                column: "EnclosureId",
                principalTable: "Enclosures",
                principalColumn: "EnclosureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Enclosures_EnclosureId",
                table: "Animals");

            migrationBuilder.DropIndex(
                name: "IX_Animals_EnclosureId",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "EnclosureId",
                table: "Animals");
        }
    }
}
