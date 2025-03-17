using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZooKeepers.Migrations
{
    /// <inheritdoc />
    public partial class AddedEnclosureId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Enclosures_EnclosureId",
                table: "Animals");

            migrationBuilder.AlterColumn<int>(
                name: "EnclosureId",
                table: "Animals",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Enclosures_EnclosureId",
                table: "Animals",
                column: "EnclosureId",
                principalTable: "Enclosures",
                principalColumn: "EnclosureId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Enclosures_EnclosureId",
                table: "Animals");

            migrationBuilder.AlterColumn<int>(
                name: "EnclosureId",
                table: "Animals",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Enclosures_EnclosureId",
                table: "Animals",
                column: "EnclosureId",
                principalTable: "Enclosures",
                principalColumn: "EnclosureId");
        }
    }
}
