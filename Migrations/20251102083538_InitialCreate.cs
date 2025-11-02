using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FutureV.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Tagline = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    DriveType = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    EnergySystem = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    AutonomyLevel = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    PowerOutput = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    BasePrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    RangePerCharge = table.Column<int>(type: "integer", nullable: false),
                    TopSpeed = table.Column<int>(type: "integer", nullable: false),
                    ZeroToSixty = table.Column<double>(type: "double precision", nullable: false),
                    FastChargeMinutes = table.Column<double>(type: "double precision", nullable: false),
                    SeatingCapacity = table.Column<int>(type: "integer", nullable: false),
                    ReleaseYear = table.Column<int>(type: "integer", nullable: false),
                    Narrative = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CarId = table.Column<int>(type: "integer", nullable: false),
                    ViewAngle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarImages_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarImages_CarId",
                table: "CarImages",
                column: "CarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarImages");

            migrationBuilder.DropTable(
                name: "Cars");
        }
    }
}
