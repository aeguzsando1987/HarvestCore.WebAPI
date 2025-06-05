using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HarvestCore.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCropEntityAndRelatioships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    IdCountry = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.IdCountry);
                });

            migrationBuilder.CreateTable(
                name: "Crops",
                columns: table => new
                {
                    IdCrop = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CropKey = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Variety = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Season = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crops", x => x.IdCrop);
                });

            migrationBuilder.CreateTable(
                name: "HarvestTables",
                columns: table => new
                {
                    IdHarvestTable = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HarvestTableKey = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HarvestTables", x => x.IdHarvestTable);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    IdState = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCountry = table.Column<int>(type: "int", nullable: false),
                    StateCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.IdState);
                    table.ForeignKey(
                        name: "FK_States_Countries_IdCountry",
                        column: x => x.IdCountry,
                        principalTable: "Countries",
                        principalColumn: "IdCountry",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MacroTunnels",
                columns: table => new
                {
                    IdMacroTunnel = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MacroTunnelKey = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IdHarvestTable = table.Column<int>(type: "int", nullable: false),
                    WalkwayNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacroTunnels", x => x.IdMacroTunnel);
                    table.ForeignKey(
                        name: "FK_MacroTunnels_HarvestTables_IdHarvestTable",
                        column: x => x.IdHarvestTable,
                        principalTable: "HarvestTables",
                        principalColumn: "IdHarvestTable",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Communities",
                columns: table => new
                {
                    IdCommunity = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdState = table.Column<int>(type: "int", nullable: false),
                    CommunityKey = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communities", x => x.IdCommunity);
                    table.ForeignKey(
                        name: "FK_Communities_States_IdState",
                        column: x => x.IdState,
                        principalTable: "States",
                        principalColumn: "IdState",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Crews",
                columns: table => new
                {
                    IdCrew = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCommunity = table.Column<int>(type: "int", nullable: false),
                    CrewKey = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ResponsibleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Community = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crews", x => x.IdCrew);
                    table.ForeignKey(
                        name: "FK_Crews_Communities_IdCommunity",
                        column: x => x.IdCommunity,
                        principalTable: "Communities",
                        principalColumn: "IdCommunity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Harvesters",
                columns: table => new
                {
                    IdHarvester = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HarvesterKey = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdCrew = table.Column<int>(type: "int", nullable: false),
                    Photo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Encoder = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Harvesters", x => x.IdHarvester);
                    table.ForeignKey(
                        name: "FK_Harvesters_Crews_IdCrew",
                        column: x => x.IdCrew,
                        principalTable: "Crews",
                        principalColumn: "IdCrew",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Harvests",
                columns: table => new
                {
                    IdHarvest = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HarvestKey = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IdHarvester = table.Column<int>(type: "int", nullable: false),
                    IdMacroTunnel = table.Column<int>(type: "int", nullable: false),
                    IdCrop = table.Column<int>(type: "int", nullable: true),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    QualityLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TransDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Photo = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Harvests", x => x.IdHarvest);
                    table.ForeignKey(
                        name: "FK_Harvests_Crops_IdCrop",
                        column: x => x.IdCrop,
                        principalTable: "Crops",
                        principalColumn: "IdCrop",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Harvests_Harvesters_IdHarvester",
                        column: x => x.IdHarvester,
                        principalTable: "Harvesters",
                        principalColumn: "IdHarvester",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Harvests_MacroTunnels_IdMacroTunnel",
                        column: x => x.IdMacroTunnel,
                        principalTable: "MacroTunnels",
                        principalColumn: "IdMacroTunnel",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Communities_CommunityKey",
                table: "Communities",
                column: "CommunityKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Communities_IdState",
                table: "Communities",
                column: "IdState");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CountryCode",
                table: "Countries",
                column: "CountryCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Crews_CrewKey",
                table: "Crews",
                column: "CrewKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Crews_IdCommunity",
                table: "Crews",
                column: "IdCommunity");

            migrationBuilder.CreateIndex(
                name: "IX_Crops_CropKey",
                table: "Crops",
                column: "CropKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Harvesters_HarvesterKey",
                table: "Harvesters",
                column: "HarvesterKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Harvesters_IdCrew",
                table: "Harvesters",
                column: "IdCrew");

            migrationBuilder.CreateIndex(
                name: "IX_Harvests_HarvestKey",
                table: "Harvests",
                column: "HarvestKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Harvests_IdCrop",
                table: "Harvests",
                column: "IdCrop");

            migrationBuilder.CreateIndex(
                name: "IX_Harvests_IdHarvester",
                table: "Harvests",
                column: "IdHarvester");

            migrationBuilder.CreateIndex(
                name: "IX_Harvests_IdMacroTunnel",
                table: "Harvests",
                column: "IdMacroTunnel");

            migrationBuilder.CreateIndex(
                name: "IX_MacroTunnels_IdHarvestTable",
                table: "MacroTunnels",
                column: "IdHarvestTable");

            migrationBuilder.CreateIndex(
                name: "IX_MacroTunnels_MacroTunnelKey",
                table: "MacroTunnels",
                column: "MacroTunnelKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_States_IdCountry",
                table: "States",
                column: "IdCountry");

            migrationBuilder.CreateIndex(
                name: "IX_States_StateCode",
                table: "States",
                column: "StateCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Harvests");

            migrationBuilder.DropTable(
                name: "Crops");

            migrationBuilder.DropTable(
                name: "Harvesters");

            migrationBuilder.DropTable(
                name: "MacroTunnels");

            migrationBuilder.DropTable(
                name: "Crews");

            migrationBuilder.DropTable(
                name: "HarvestTables");

            migrationBuilder.DropTable(
                name: "Communities");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
