using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FutureV.Migrations
{
    /// <inheritdoc />
    public partial class AddCarImageBinaryStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "CarImages",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "CarImages",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "CarImages",
                type: "character varying(260)",
                maxLength: 260,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "CarImages",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "CarImages");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "CarImages");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "CarImages");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "CarImages",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
