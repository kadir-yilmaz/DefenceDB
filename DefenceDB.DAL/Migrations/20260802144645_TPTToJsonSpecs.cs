using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DefenceDB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class TPTToJsonSpecs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // --- INJECTED BY FIX SCRIPT ---
            migrationBuilder.AddColumn<string>(
                name: "Specs",
                table: "DefenseProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
DECLARE @tableName NVARCHAR(128);
DECLARE @sql NVARCHAR(MAX);
DECLARE @columns NVARCHAR(MAX);

DECLARE table_cursor CURSOR FOR
SELECT t.name 
FROM sys.tables t
INNER JOIN sys.foreign_keys fk ON t.object_id = fk.parent_object_id
WHERE fk.referenced_object_id = OBJECT_ID('DefenseProducts')
AND t.name NOT IN ('ProductRelationships', 'ProductImages');

OPEN table_cursor;
FETCH NEXT FROM table_cursor INTO @tableName;

WHILE @@FETCH_STATUS = 0
BEGIN
    SELECT @columns = STRING_AGG(QUOTENAME(c.name) + ' AS ' + QUOTENAME(c.name), ', ')
    FROM sys.columns c
    WHERE c.object_id = OBJECT_ID(@tableName) AND c.name <> 'Id';

    IF @columns IS NOT NULL
    BEGIN
        SET @sql = '
        UPDATE dp
        SET Specs = (
            SELECT ' + @columns + '
            FROM ' + QUOTENAME(@tableName) + ' t
            WHERE t.Id = dp.Id
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        )
        FROM DefenseProducts dp
        INNER JOIN ' + QUOTENAME(@tableName) + ' t ON dp.Id = t.Id;';
        
        EXEC sp_executesql @sql;
    END

    FETCH NEXT FROM table_cursor INTO @tableName;
END

CLOSE table_cursor;
DEALLOCATE table_cursor;
");
            // --- END INJECTED BY FIX SCRIPT ---


            

            migrationBuilder.DropTable(
                name: "AirborneRadars");

            migrationBuilder.DropTable(
                name: "AirDefenseRadars");

            migrationBuilder.DropTable(
                name: "AirDefenseSystems");

            migrationBuilder.DropTable(
                name: "AirSojAircrafts");

            migrationBuilder.DropTable(
                name: "AirToAirMissiles");

            migrationBuilder.DropTable(
                name: "AntiRadiationMissiles");

            migrationBuilder.DropTable(
                name: "AntiShipMissiles");

            migrationBuilder.DropTable(
                name: "AwacsAircrafts");

            migrationBuilder.DropTable(
                name: "BallisticMissiles");

            migrationBuilder.DropTable(
                name: "BomberAircrafts");

            migrationBuilder.DropTable(
                name: "CargoAircrafts");

            migrationBuilder.DropTable(
                name: "Corvettes");

            migrationBuilder.DropTable(
                name: "CruiseMissiles");

            migrationBuilder.DropTable(
                name: "Destroyers");

            migrationBuilder.DropTable(
                name: "ElectricNuclearPowers");

            migrationBuilder.DropTable(
                name: "FastAttackCrafts");

            migrationBuilder.DropTable(
                name: "FighterAircrafts");

            migrationBuilder.DropTable(
                name: "Frigates");

            migrationBuilder.DropTable(
                name: "HypersonicGlideVehicles");

            migrationBuilder.DropTable(
                name: "InfantryWeapons");

            migrationBuilder.DropTable(
                name: "KamikazeUAVs");

            migrationBuilder.DropTable(
                name: "KamikazeUSVs");

            migrationBuilder.DropTable(
                name: "LandVehicles");

            migrationBuilder.DropTable(
                name: "MarineGasTurbines");

            migrationBuilder.DropTable(
                name: "MaritimePatrolAircrafts");

            migrationBuilder.DropTable(
                name: "Minehunters");

            migrationBuilder.DropTable(
                name: "NavalRadars");

            migrationBuilder.DropTable(
                name: "PistonEngines");

            migrationBuilder.DropTable(
                name: "ProductReadModels");

            migrationBuilder.DropTable(
                name: "RocketMotors");

            migrationBuilder.DropTable(
                name: "Submarines");

            migrationBuilder.DropTable(
                name: "TrainerAircrafts");

            migrationBuilder.DropTable(
                name: "TurbofanEngines");

            migrationBuilder.DropTable(
                name: "TurbojetEngines");

            migrationBuilder.DropTable(
                name: "TurbopropEngines");

            migrationBuilder.DropTable(
                name: "TurboshaftEngines");

            migrationBuilder.DropTable(
                name: "UAVs");

            migrationBuilder.DropTable(
                name: "UGVs");

            migrationBuilder.DropTable(
                name: "USVs");migrationBuilder.DropColumn(
                name: "ModelTypeName",
                table: "Categories");migrationBuilder.CreateTable(
                name: "CategoryAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Options = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryAttributes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DefenseProducts_Country",
                table: "DefenseProducts",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_DefenseProducts_CreatedAt",
                table: "DefenseProducts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_DefenseProducts_IsActive_IsShowcase",
                table: "DefenseProducts",
                columns: new[] { "IsActive", "IsShowcase" });

            migrationBuilder.CreateIndex(
                name: "IX_DefenseProducts_Slug",
                table: "DefenseProducts",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryAttributes_CategoryId_Name",
                table: "CategoryAttributes",
                columns: new[] { "CategoryId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryAttributes");

            migrationBuilder.DropIndex(
                name: "IX_DefenseProducts_Country",
                table: "DefenseProducts");

            migrationBuilder.DropIndex(
                name: "IX_DefenseProducts_CreatedAt",
                table: "DefenseProducts");

            migrationBuilder.DropIndex(
                name: "IX_DefenseProducts_IsActive_IsShowcase",
                table: "DefenseProducts");

            migrationBuilder.DropIndex(
                name: "IX_DefenseProducts_Slug",
                table: "DefenseProducts");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Slug",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Specs",
                table: "DefenseProducts");

            migrationBuilder.AddColumn<string>(
                name: "ModelTypeName",
                table: "Categories",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AirborneRadars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CoolingSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrequencyBand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxRangeKm = table.Column<double>(type: "float", nullable: true),
                    RadarType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScanCoverage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetTrackingCapacity = table.Column<int>(type: "int", nullable: true),
                    TrModuleCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirborneRadars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirborneRadars_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AirDefenseRadars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CoolingSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrequencyBand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxRangeKm = table.Column<double>(type: "float", nullable: true),
                    RadarType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScanCoverage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetTrackingCapacity = table.Column<int>(type: "int", nullable: true),
                    TrModuleCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirDefenseRadars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirDefenseRadars_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AirDefenseSystems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    HasAntiBallisticCapability = table.Column<bool>(type: "bit", nullable: false),
                    MaxEngagementAltitudeFt = table.Column<double>(type: "float", nullable: true),
                    MaxSearchRangeKm = table.Column<double>(type: "float", nullable: true),
                    MaxTrackedTargets = table.Column<int>(type: "int", nullable: true),
                    MaxTrackingRangeKm = table.Column<double>(type: "float", nullable: true),
                    MissilesPerLauncher = table.Column<int>(type: "int", nullable: true),
                    SystemType = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirDefenseSystems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirDefenseSystems_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AirSojAircrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FrequencyRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JammerType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxRangeKm = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirSojAircrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirSojAircrafts_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AirToAirMissiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FoxCode = table.Column<byte>(type: "tinyint", nullable: true),
                    GuidanceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxSpeedMach = table.Column<double>(type: "float", nullable: true),
                    RangeKm = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirToAirMissiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirToAirMissiles_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AntiRadiationMissiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MaxSpeedMach = table.Column<double>(type: "float", nullable: true),
                    RangeKm = table.Column<double>(type: "float", nullable: true),
                    SeekerType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntiRadiationMissiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntiRadiationMissiles_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AntiShipMissiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MaxSpeedMach = table.Column<double>(type: "float", nullable: true),
                    RangeKm = table.Column<double>(type: "float", nullable: true),
                    SeaSkimming = table.Column<bool>(type: "bit", nullable: false),
                    SpeedClass = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntiShipMissiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntiShipMissiles_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AwacsAircrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DetectionRangeKm = table.Column<double>(type: "float", nullable: true),
                    MaxTrackedTargets = table.Column<int>(type: "int", nullable: true),
                    RadarType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AwacsAircrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AwacsAircrafts_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BallisticMissiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BallisticType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasMirv = table.Column<bool>(type: "bit", nullable: false),
                    IsNuclearCapable = table.Column<bool>(type: "bit", nullable: false),
                    MaxSpeedMach = table.Column<double>(type: "float", nullable: true),
                    PayloadKg = table.Column<double>(type: "float", nullable: true),
                    RangeKm = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BallisticMissiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BallisticMissiles_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BomberAircrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CombatRadiusKm = table.Column<double>(type: "float", nullable: true),
                    IsNuclearCapable = table.Column<bool>(type: "bit", nullable: false),
                    PayloadCapacityKg = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BomberAircrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BomberAircrafts_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CargoAircrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CargoVolumeCubicMeters = table.Column<double>(type: "float", nullable: true),
                    PayloadCapacityTons = table.Column<double>(type: "float", nullable: true),
                    RangeWithMaxPayloadKm = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargoAircrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CargoAircrafts_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Corvettes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DisplacementTons = table.Column<double>(type: "float", nullable: true),
                    HasHelipad = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corvettes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Corvettes_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CruiseMissiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CepMeters = table.Column<double>(type: "float", nullable: true),
                    MaxSpeedMach = table.Column<double>(type: "float", nullable: true),
                    RangeKm = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CruiseMissiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CruiseMissiles_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Destroyers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DisplacementTons = table.Column<double>(type: "float", nullable: true),
                    RadarSystemType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VlsCellsCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destroyers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Destroyers_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElectricNuclearPowers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PowerOutputMw = table.Column<double>(type: "float", nullable: true),
                    SystemType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectricNuclearPowers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElectricNuclearPowers_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FastAttackCrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DisplacementTons = table.Column<double>(type: "float", nullable: true),
                    MaxSpeedKnots = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastAttackCrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FastAttackCrafts_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FighterAircrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CombatRadiusKm = table.Column<double>(type: "float", nullable: true),
                    Generation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasAesaRadar = table.Column<bool>(type: "bit", nullable: false),
                    HasStealth = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FighterAircrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FighterAircrafts_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Frigates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DisplacementTons = table.Column<double>(type: "float", nullable: true),
                    VlsCellsCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frigates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Frigates_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HypersonicGlideVehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CarrierPlatform = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxSpeedMach = table.Column<double>(type: "float", nullable: true),
                    RangeKm = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HypersonicGlideVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HypersonicGlideVehicles_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InfantryWeapons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Caliber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EffectiveRangeMeters = table.Column<int>(type: "int", nullable: true),
                    MagazineCapacity = table.Column<int>(type: "int", nullable: true),
                    RateOfFireRpm = table.Column<int>(type: "int", nullable: true),
                    WeightKg = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfantryWeapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfantryWeapons_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KamikazeUAVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EnduranceHours = table.Column<double>(type: "float", nullable: true),
                    MaxSpeedKmh = table.Column<double>(type: "float", nullable: true),
                    RangeKm = table.Column<double>(type: "float", nullable: true),
                    WarheadWeightKg = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KamikazeUAVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KamikazeUAVs_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KamikazeUSVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MaxSpeedKnots = table.Column<double>(type: "float", nullable: true),
                    RangeNauticalMiles = table.Column<double>(type: "float", nullable: true),
                    WarheadWeightKg = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KamikazeUSVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KamikazeUSVs_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LandVehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CrewCount = table.Column<int>(type: "int", nullable: true),
                    EngineHorsePower = table.Column<int>(type: "int", nullable: true),
                    HasAutoloader = table.Column<bool>(type: "bit", nullable: false),
                    MainGunCaliberMm = table.Column<double>(type: "float", nullable: true),
                    WeightTons = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LandVehicles_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarineGasTurbines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ShaftHorsePowerHp = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarineGasTurbines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarineGasTurbines_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaritimePatrolAircrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EnduranceHours = table.Column<double>(type: "float", nullable: true),
                    HasTorpedoTubes = table.Column<bool>(type: "bit", nullable: false),
                    SonarType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaritimePatrolAircrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaritimePatrolAircrafts_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Minehunters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DisplacementTons = table.Column<double>(type: "float", nullable: true),
                    SonarType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Minehunters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Minehunters_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NavalRadars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CoolingSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrequencyBand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxRangeKm = table.Column<double>(type: "float", nullable: true),
                    RadarType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScanCoverage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScanType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetTrackingCapacity = table.Column<int>(type: "int", nullable: true),
                    TrModuleCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavalRadars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NavalRadars_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PistonEngines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Cylinders = table.Column<int>(type: "int", nullable: true),
                    FuelType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HorsePower = table.Column<double>(type: "float", nullable: true),
                    TorqueNm = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PistonEngines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PistonEngines_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductReadModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategorySlug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsShowcase = table.Column<bool>(type: "bit", nullable: false),
                    MainImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NatoReportingName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SpecificPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    YearIntroduced = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReadModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RocketMotors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BurnTimeSeconds = table.Column<double>(type: "float", nullable: true),
                    PropellantType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ThrustKn = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RocketMotors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RocketMotors_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Submarines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DisplacementTons = table.Column<double>(type: "float", nullable: true),
                    MaxDepthMeters = table.Column<double>(type: "float", nullable: true),
                    PropulsionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TorpedoTubesCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submarines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submarines_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainerAircrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EngineType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxSeats = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerAircrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainerAircrafts_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TurbofanEngines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BypassRatio = table.Column<double>(type: "float", nullable: true),
                    DryThrustLbf = table.Column<double>(type: "float", nullable: true),
                    HasAfterburner = table.Column<bool>(type: "bit", nullable: false),
                    MaxThrustLbf = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurbofanEngines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TurbofanEngines_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TurbojetEngines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DryThrustLbf = table.Column<double>(type: "float", nullable: true),
                    HasAfterburner = table.Column<bool>(type: "bit", nullable: false),
                    MaxThrustLbf = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurbojetEngines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TurbojetEngines_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TurbopropEngines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ShaftHorsePowerHp = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurbopropEngines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TurbopropEngines_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TurboshaftEngines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ShaftHorsePowerHp = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurboshaftEngines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TurboshaftEngines_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UAVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CruisingSpeedKmh = table.Column<double>(type: "float", nullable: true),
                    EnduranceHours = table.Column<double>(type: "float", nullable: true),
                    MaxAltitudeFeet = table.Column<int>(type: "int", nullable: true),
                    PayloadCapacityKg = table.Column<double>(type: "float", nullable: true),
                    WingSpanMeters = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UAVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UAVs_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UGVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DriveType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaxSpeedKmh = table.Column<double>(type: "float", nullable: true),
                    OperationalRangeKm = table.Column<double>(type: "float", nullable: true),
                    WeightKg = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UGVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UGVs_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DisplacementTons = table.Column<double>(type: "float", nullable: true),
                    EnduranceHours = table.Column<double>(type: "float", nullable: true),
                    MaxSpeedKnots = table.Column<double>(type: "float", nullable: true),
                    OperationalRangeNauticalMiles = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_USVs_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "ModelTypeName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "ModelTypeName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "ModelTypeName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "ModelTypeName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.AirToAirMissile");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.BallisticMissile");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.AntiShipMissile");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.CruiseMissile");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.AntiRadiationMissile");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.HypersonicGlideVehicle");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.FighterAircraft");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.BomberAircraft");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 13,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.TrainerAircraft");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 14,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.FastAttackCraft");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 15,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.Corvette");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 16,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.Frigate");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 17,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.Destroyer");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 18,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.Submarine");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 19,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.AirDefenseRadar");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 20,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.AirborneRadar");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 21,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.NavalRadar");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 22,
                column: "ModelTypeName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 23,
                column: "ModelTypeName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 24,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.UAV");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 25,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.USV");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 26,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.UGV");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 27,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.KamikazeUAV");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 28,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.KamikazeUSV");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 30,
                column: "ModelTypeName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 31,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.TurbofanEngine");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 32,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.PistonEngine");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 33,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.RocketMotor");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 34,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.ElectricNuclearPower");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 35,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.TurbojetEngine");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 36,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.TurbopropEngine");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 37,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.MarineGasTurbine");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 38,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.TurboshaftEngine");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 39,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.AirDefenseSystem");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 40,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.AirSojAircraft");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 41,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.CargoAircraft");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 42,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.MaritimePatrolAircraft");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 43,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.AwacsAircraft");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 44,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.AirDefenseSystem");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 45,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.AirDefenseSystem");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 46,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.AirDefenseSystem");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 47,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.AirDefenseSystem");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 48,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.LandVehicle");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 49,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.LandVehicle");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 50,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.LandVehicle");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 51,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.LandVehicle");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 52,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.LandVehicle");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 53,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.LandVehicle");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 54,
                column: "ModelTypeName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 55,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.InfantryWeapon");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 56,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.InfantryWeapon");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 57,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.InfantryWeapon");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 58,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.InfantryWeapon");

            migrationBuilder.InsertData(
                table: "DefenseProducts",
                columns: new[] { "Id", "CategoryId", "Country", "CreatedAt", "Description", "IsActive", "IsShowcase", "Manufacturer", "Name", "NatoReportingName", "Slug", "Status", "ThumbnailUrl", "UpdatedAt", "VideoUrl", "YearIntroduced" },
                values: new object[,]
                {
                    { 1, 11, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Çok amaçlı 4. nesil savaş uçağı.", true, true, "Lockheed Martin", "F-16 Fighting Falcon", null, "f-16-fighting-falcon", null, null, null, null, null },
                    { 2, 11, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "5. nesil çok amaçlı hayalet savaş uçağı.", true, true, "Lockheed Martin", "F-35 Lightning II", null, "f-35-lightning-ii", null, null, null, null, null },
                    { 3, 11, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hava üstünlüğü sağlayan 5. nesil savaş uçağı.", true, false, "Lockheed Martin", "F-22 Raptor", null, "f-22-raptor", null, null, null, null, null },
                    { 4, 11, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Milli Muharip Uçak (MMU). 5. nesil çok rollü savaş uçağı.", true, true, "TUSAŞ", "KAAN", null, "kaan", null, null, null, null, null },
                    { 5, 11, "Avrupa Birliği", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Çift motorlu, delta kanatlı çok rollü savaş uçağı.", true, false, "Eurofighter Jagdflugzeug", "Eurofighter Typhoon", null, "eurofighter-typhoon", null, null, null, null, null },
                    { 6, 11, "Fransa", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Omnirole savaş uçağı.", true, false, "Dassault Aviation", "Dassault Rafale", null, "dassault-rafale", null, null, null, null, null },
                    { 7, 11, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Rusya'nın 5. nesil savaş uçağı.", true, false, "Sukhoi", "Su-57", null, "su-57", null, null, null, null, null },
                    { 8, 11, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gelişmiş 4++ nesil çok amaçlı savaş uçağı.", true, false, "Sukhoi", "Su-35", null, "su-35", null, null, null, null, null },
                    { 9, 11, "Çin", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Çin'in 5. nesil ağır savaş uçağı.", true, false, "Chengdu", "J-20 Mighty Dragon", null, "j-20-mighty-dragon", null, null, null, null, null },
                    { 10, 11, "İsveç", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hafif, tek motorlu çok amaçlı uçak.", true, false, "Saab", "JAS 39 Gripen", null, "jas-39-gripen", null, null, null, null, null },
                    { 11, 11, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "F-15'in modernize edilmiş en gelişmiş versiyonu.", true, false, "Boeing", "F-15EX Eagle II", null, "f-15ex-eagle-ii", null, null, null, null, null },
                    { 12, 5, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Raytheon", "AIM-9 Sidewinder", null, "aim-9-sidewinder", null, null, null, null, null },
                    { 13, 5, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Raytheon", "AIM-120 AMRAAM", null, "aim-120-amraam", null, null, null, null, null },
                    { 14, 5, "Avrupa Birliği", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "MBDA", "Meteor", null, "meteor", null, null, null, null, null },
                    { 15, 5, "Almanya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Diehl Defence", "IRIS-T", null, "iris-t", null, null, null, null, null },
                    { 16, 5, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "TÜBİTAK SAGE", "GÖKDOĞAN", null, "gokdogan", null, null, null, null, null },
                    { 17, 5, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "TÜBİTAK SAGE", "BOZDOĞAN", null, "bozdogan", null, null, null, null, null },
                    { 18, 5, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Vympel", "R-77 (AA-12 Adder)", null, "r-77-aa-12-adder", null, null, null, null, null },
                    { 19, 5, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Vympel", "R-73 (AA-11 Archer)", null, "r-73-aa-11-archer", null, null, null, null, null },
                    { 20, 5, "Çin", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "CASC", "PL-15", null, "pl-15", null, null, null, null, null },
                    { 21, 5, "Fransa", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "MBDA", "MICA", null, "mica", null, null, null, null, null },
                    { 22, 20, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Northrop Grumman", "AN/APG-81", null, "anapg-81", null, null, null, null, null },
                    { 23, 20, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Northrop Grumman", "AN/APG-77", null, "anapg-77", null, null, null, null, null },
                    { 24, 20, "Avrupa Birliği", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Euroradar", "Captor-E", null, "captor-e", null, null, null, null, null },
                    { 25, 20, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "NIIP", "Irbis-E", null, "irbis-e", null, null, null, null, null },
                    { 26, 20, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Aselsan", "MURAD", null, "murad", null, null, null, null, null },
                    { 27, 19, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Lockheed Martin", "AN/SPY-1", null, "anspy-1", null, null, null, null, null },
                    { 28, 19, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Aselsan", "Ers-Int (Erken İhbar)", null, "ers-int-erken-ihbar", null, null, null, null, null },
                    { 29, 19, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Raytheon", "Patriot AN/MPQ-65", null, "patriot-anmpq-65", null, null, null, null, null },
                    { 30, 19, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Almaz-Antey", "S-400 91N6E", null, "s-400-91n6e", null, null, null, null, null },
                    { 31, 7, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Boeing", "Harpoon", null, "harpoon", null, null, null, null, null },
                    { 32, 7, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Roketsan", "ATMACA", null, "atmaca", null, null, null, null, null },
                    { 33, 6, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Lockheed Martin", "Trident II D5", null, "trident-ii-d5", null, null, null, null, null },
                    { 34, 6, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Roketsan", "TAYFUN", null, "tayfun", null, null, null, null, null },
                    { 35, 16, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "STM", "İstif Sınıfı (TCG İstanbul)", null, "istif-sinifi-tcg-istanbul", null, null, null, null, null },
                    { 36, 16, "Fransa", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Naval Group", "FREMM Sınıfı", null, "fremm-sinifi", null, null, null, null, null },
                    { 37, 18, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "General Dynamics", "Virginia Sınıfı", null, "virginia-sinifi", null, null, null, null, null },
                    { 38, 18, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Gölcük Tersanesi", "Reis Sınıfı (Tip 214TN)", null, "reis-sinifi-tip-214tn", null, null, null, null, null },
                    { 39, 8, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Raytheon", "Tomahawk", null, "tomahawk", null, null, null, null, null },
                    { 40, 8, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "TÜBİTAK SAGE / Roketsan", "SOM", null, "som", null, null, null, null, null },
                    { 101, 48, "Almanya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "KMW / Rheinmetall", "Leopard 2A7", null, "leopard-2a7", null, null, null, null, 2014 },
                    { 102, 48, "Güney Kore", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Hyundai Rotem", "K2 Black Panther", null, "k2-black-panther", null, null, null, null, 2014 },
                    { 103, 48, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "BMC / Otokar", "Altay", null, "altay", null, null, null, null, 2025 },
                    { 104, 48, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "General Dynamics", "M1A2 SEPv3 Abrams", null, "m1a2-abrams", null, null, null, null, 2020 },
                    { 105, 48, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Uralvagonzavod", "T-72B3", null, "t-72b3", null, null, null, null, 2013 },
                    { 106, 48, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Omsktransmash", "T-80BVM", null, "t-80bvm", null, null, null, null, 2017 },
                    { 107, 48, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Uralvagonzavod", "T-90M Proryv", null, "t-90m-proryv", null, null, null, null, 2020 },
                    { 108, 48, "Birleşik Krallık", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "BAE Systems", "Challenger 2", null, "challenger-2", null, null, null, null, 1998 },
                    { 109, 48, "İsrail", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "MANTAK", "Merkava Mk.4", null, "merkava-mk4", null, null, null, null, 2004 },
                    { 110, 48, "Fransa", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Nexter", "Leclerc", null, "leclerc", null, null, null, null, 1992 },
                    { 111, 48, "Japonya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Mitsubishi Heavy Industries", "Type 10", null, "type-10", null, null, null, null, 2012 },
                    { 112, 48, "İtalya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Iveco-Oto Melara", "C1 Ariete", null, "c1-ariete", null, null, null, null, 1995 },
                    { 201, 24, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Baykar", "Bayraktar TB2", null, "bayraktar-tb2", null, null, null, null, null },
                    { 202, 24, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Baykar", "Bayraktar TB3", null, "bayraktar-tb3", null, null, null, null, null },
                    { 203, 24, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Baykar", "Bayraktar Akıncı", null, "bayraktar-akinci", null, null, null, null, null },
                    { 204, 24, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Baykar", "Bayraktar Kızılelma", null, "bayraktar-kizilelma", null, null, null, null, null },
                    { 205, 24, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "General Atomics", "MQ-9 Reaper", null, "mq-9-reaper", null, null, null, null, null },
                    { 206, 24, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Northrop Grumman", "RQ-4 Global Hawk", null, "rq-4-global-hawk", null, null, null, null, null },
                    { 207, 24, "Çin", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Chengdu", "Wing Loong II", null, "wing-loong-ii", null, null, null, null, null },
                    { 208, 24, "Çin", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "CASC", "CH-4 Rainbow", null, "ch-4-rainbow", null, null, null, null, null },
                    { 209, 24, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Kronshtadt Group", "Orion", null, "orion", null, null, null, null, null },
                    { 210, 24, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Sukhoi", "S-70 Okhotnik", null, "s-70-okhotnik", null, null, null, null, null },
                    { 211, 24, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "TUSAŞ", "Anka-S", null, "anka-s", null, null, null, null, null },
                    { 212, 24, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "TUSAŞ", "Aksungur", null, "aksungur", null, null, null, null, null },
                    { 213, 24, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "TUSAŞ", "Anka-3", null, "anka-3", null, null, null, null, null },
                    { 301, 31, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "TEI", "TEI-TF6000", null, "tei-tf6000", null, null, null, null, null },
                    { 302, 31, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "TEI", "TEI-TF10000", null, "tei-tf10000", null, null, null, null, null },
                    { 303, 31, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Pratt & Whitney", "F135-PW-100", null, "f135-pw-100", null, null, null, null, null },
                    { 304, 31, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "General Electric", "F110-GE-129", null, "f110-ge-129", null, null, null, null, null },
                    { 305, 31, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "NPO Saturn", "AL-31F", null, "al-31f", null, null, null, null, null },
                    { 306, 35, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Kale Arge", "Kale KTJ-3200", null, "kale-ktj-3200", null, null, null, null, null },
                    { 307, 38, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "TEI", "TEI-TS1400", null, "tei-ts1400", null, null, null, null, null },
                    { 308, 36, "Kanada", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Pratt & Whitney Canada", "PT6A-67A", null, "pt6a-67a", null, null, null, null, null },
                    { 309, 37, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "General Electric", "GE LM2500", null, "ge-lm2500", null, null, null, null, null },
                    { 311, 32, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "TEI", "TEI-PD170", null, "tei-pd170", null, null, null, null, null },
                    { 312, 32, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "BMC Power", "BATU", null, "batu", null, null, null, null, null },
                    { 313, 32, "Almanya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "MTU", "MTU MB 873 Ka-501", null, "mtu-mb-873", null, null, null, null, null },
                    { 321, 33, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Roketsan", "Roketsan Katı Yakıtlı Roket Motoru", null, "roketsan-kati-yakitli-motor", null, null, null, null, null },
                    { 322, 33, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "SpaceX", "Raptor", null, "raptor-engine", null, null, null, null, null },
                    { 331, 34, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "General Electric", "S9G Nükleer Reaktör", null, "s9g-nuclear-reactor", null, null, null, null, null },
                    { 332, 34, "Almanya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Siemens", "Siemens PEM Yakıt Hücresi (AIP)", null, "siemens-pem-aip", null, null, null, null, null },
                    { 401, 47, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Rusya yapımı orta ve uzun menzilli uçaksavar ve füze savunma sistemi.", true, false, "Almaz-Antey", "S-400 Triumf", null, "s-400-triumf", null, null, null, null, null },
                    { 402, 47, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Uzay ve kıtalararası balistik füze savunması odaklı yeni nesil uzun menzilli hava savunma sistemi.", true, false, "Almaz-Antey", "S-500 Prometey", null, "s-500-prometey", null, null, null, null, null },
                    { 403, 47, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Amerika Birleşik Devletleri ordusunun ana taktik hava ve balistik füze savunma sistemi.", true, false, "Raytheon / Lockheed Martin", "MIM-104 Patriot (PAC-3)", null, "mim-104-patriot-pac-3", null, null, null, null, null },
                    { 404, 47, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Atmosfer içi ve atmosfer dışı (Terminal aşamada) kısa, orta ve ara menzilli balistik füzeleri önleme sistemi.", true, false, "Lockheed Martin", "THAAD", null, "thaad", null, null, null, null, null },
                    { 405, 47, "Fransa", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Aster 30 füzelerini kullanan, uçak ve seyir füzelerine ek olarak balistik füzelere karşı da etkili Avrupa menşeili hava savunma sistemi.", true, false, "Eurosam", "SAMP/T Mamba", null, "samp-t-mamba", null, null, null, null, null },
                    { 406, 47, "Çin", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Çin Halk Kurtuluş Ordusu'nun ana uzun menzilli hava ve füze savunma sistemi.", true, false, "CASIC", "HQ-9", null, "hq-9", null, null, null, null, null },
                    { 408, 47, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Türkiye tarafından milli imkanlarla geliştirilen, savaş uçakları, helikopterler, seyir füzeleri ve İHA'lara karşı etkili alçak irtifa hava savunma sistemi.", true, false, "Aselsan / Roketsan", "HİSAR-A+", null, "hisar-a-plus", null, null, null, null, null },
                    { 409, 47, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Türkiye tarafından yerli imkanlarla tasarlanan ve geliştirilen, savaş uçakları, İHA'lar, seyir füzeleri ve helikopterler gibi hedefleri imha etmek üzere tasarlanmış orta irtifa hava savunma sistemi.", true, false, "Aselsan / Roketsan", "HİSAR-O+", null, "hisar-o-plus", null, null, null, null, null },
                    { 411, 47, "Güney Kore", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Güney Kore ordusunun kritik tesislerini ve birliklerini korumak için tasarlanmış mobil alçak irtifa hava savunma sistemi.", true, false, "Hanwha Defense / LIG Nex1", "K-SAM Chunma (Pegasus)", null, "k-sam-chunma-pegasus", null, null, null, null, null },
                    { 412, 47, "Güney Kore", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Güney Kore yapımı, orta irtifadaki hava tehditlerine (uçaklar ve seyir füzeleri) karşı geliştirilmiş ilk nesil Cheongung hava savunma sistemi.", true, false, "LIG Nex1 / Hanwha Systems", "Cheongung I (M-SAM)", null, "cheongung-i-m-sam", null, null, null, null, null },
                    { 413, 47, "Güney Kore", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Güney Kore'nin katmanlı füze savunma kalkanı (KAMD) kapsamında geliştirdiği, üst irtifadaki balistik füzeleri ve hava tehditlerini önleme amaçlı uzun menzilli hava savunma sistemi.", true, false, "LIG Nex1 / Hanwha Systems", "L-SAM", null, "l-sam", null, null, null, null, null },
                    { 414, 47, "Güney Kore", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cheongung I'in geliştirilmiş versiyonu olan orta irtifa hava savunma sistemi. Daha gelişmiş radar, artırılmış menzil ve çoklu hedef takip yeteneği sunar.", true, false, "LIG Nex1 / Hanwha Systems", "Cheongung II (M-SAM Block II)", null, "cheongung-ii-m-sam-block-ii", null, null, null, null, null },
                    { 415, 44, "Güney Kore", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Güney Kore'nin taşınabilir hava savunma sistemi (MANPADS). Helikopterler, İHA'lar ve alçak uçan uçaklara karşı etkili.", true, false, "LIG Nex1", "KP-SAM (Shin-Gung)", null, "kp-sam-shin-gung", null, null, null, null, null },
                    { 416, 46, "Güney Kore", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Güney Kore yapımı kendinden tahrikli uçaksavar topu (SPAAG). 30mm çift namlulu top ve kısa menzilli füzelerle donatılmıştır.", true, false, "Doosan DST / S&T Dynamics", "Biho (K30)", null, "biho-k30", null, null, null, null, null },
                    { 417, 47, "Güney Kore", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "K-SAM Chunma'nın geliştirilmiş versiyonu olan alçak irtifa hava savunma sistemi. Daha modern radar ve geliştirilmiş füze teknolojisi kullanır.", true, false, "Hanwha Defense / LIG Nex1", "Cheonma (K-SAM Block II)", null, "cheonma-k-sam-block-ii", null, null, null, null, null },
                    { 418, 47, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Patriot sisteminin PAC-2 konfigürasyonu. Öncelikle geleneksel hava tehditlerine (uçaklar, helikopterler, seyir füzeleri) karşı etkili, sınırlı balistik füze savunma yeteneği vardır.", true, false, "Raytheon / Lockheed Martin", "MIM-104 Patriot (PAC-2)", null, "mim-104-patriot-pac-2", null, null, null, null, null },
                    { 501, 40, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gelişmiş hava stand-off jammer karıştırma uçağı.", true, true, "TUSAŞ / Bombardier", "HAVA SOJ (Global 6000)", null, "hava-soj-global-6000", null, null, null, null, null },
                    { 502, 41, "Avrupa Birliği", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dört motorlu askeri kargo ve nakliye uçağı.", true, true, "Airbus Defence and Space", "A400M Atlas", null, "a400m-atlas", null, null, null, null, null },
                    { 503, 42, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Denizaltı savunma harbi, su üstü harbi ve istihbarat uçağı.", true, true, "Boeing", "P-8A Poseidon", null, "p-8a-poseidon", null, null, null, null, null },
                    { 504, 42, "Türkiye / İtalya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Denizaltı savunma harbi ve deniz karakol uçağı.", true, true, "Alenia Aermacchi / TUSAŞ", "ATR 72 Meltem III", null, "atr-72-meltem-iii", null, null, null, null, null },
                    { 505, 43, "Türkiye / ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Havadan erken ihbar ve kontrol uçağı (HİK).", true, true, "Boeing / TUSAŞ", "E-7T Barış Kartalı", null, "e-7t-baris-kartali", null, null, null, null, null },
                    { 601, 55, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Emniyet Genel Müdürlüğü ve Türk Silahlı Kuvvetleri'nin ana hizmet tabancası.", true, true, "Sarsılmaz", "SAR 9", null, "sar-9", null, null, null, null, null },
                    { 602, 56, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tamamen milli imkanlarla geliştirilen TSK'nın ana piyade tüfeği.", true, true, "MKE / Kale Kalıp / Sarsılmaz", "MPT-76", null, "mpt-76", null, null, null, null, null },
                    { 603, 56, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Özel kuvvetler ve emniyet birimleri için kısa namlulu ve hafif milli piyade tüfeği.", true, false, "MKE", "MPT-55", null, "mpt-55", null, null, null, null, null },
                    { 604, 57, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Araç üstü ve piyade kullanımı için geliştirilmiş yerli makineli tüfek.", true, true, "Sarsılmaz", "SAR 762 MT", null, "sar-762-mt", null, null, null, null, null },
                    { 605, 58, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MPT-76 platformu üzerinden geliştirilen yarı otomatik manga tipi keskin nişancı tüfeği.", true, true, "MKE", "KNT-76", null, "knt-76", null, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "AirDefenseRadars",
                columns: new[] { "Id", "CoolingSystem", "FrequencyBand", "MaxRangeKm", "RadarType", "ScanCoverage", "TargetTrackingCapacity", "TrModuleCount" },
                values: new object[,]
                {
                    { 27, null, null, 310.0, "PESA", null, 100, null },
                    { 28, null, null, 600.0, "AESA", null, 200, null },
                    { 29, null, null, 150.0, "AESA", null, 100, null },
                    { 30, null, null, 600.0, "AESA", null, 300, null }
                });

            migrationBuilder.InsertData(
                table: "AirDefenseSystems",
                columns: new[] { "Id", "HasAntiBallisticCapability", "MaxEngagementAltitudeFt", "MaxSearchRangeKm", "MaxTrackedTargets", "MaxTrackingRangeKm", "MissilesPerLauncher", "SystemType" },
                values: new object[,]
                {
                    { 401, true, 100000.0, 600.0, 300, 400.0, 4, 4 },
                    { 402, true, 650000.0, 800.0, 100, 600.0, 4, 4 },
                    { 403, true, 80000.0, 150.0, 100, 100.0, 16, 4 },
                    { 404, true, 490000.0, 1000.0, 100, 800.0, 8, 4 },
                    { 405, true, 65000.0, 150.0, 100, 100.0, 8, 4 },
                    { 406, true, 98000.0, 250.0, 100, 180.0, 4, 4 },
                    { 408, false, 26000.0, 35.0, 60, 25.0, 4, 2 },
                    { 409, false, 49000.0, 80.0, 60, 60.0, 6, 3 },
                    { 411, false, 16000.0, 20.0, 20, 16.0, 8, 2 },
                    { 412, false, 49000.0, 100.0, 40, 80.0, 8, 3 },
                    { 413, true, 190000.0, 300.0, 100, 250.0, 6, 4 },
                    { 414, false, 60000.0, 150.0, 50, 120.0, 8, 3 },
                    { 415, false, 12000.0, 8.0, 2, 6.0, 1, 1 },
                    { 416, false, 10000.0, 15.0, 10, 12.0, 4, 1 },
                    { 417, false, 20000.0, 25.0, 25, 20.0, 8, 2 },
                    { 418, false, 60000.0, 100.0, 100, 90.0, 4, 4 }
                });

            migrationBuilder.InsertData(
                table: "AirSojAircrafts",
                columns: new[] { "Id", "FrequencyRange", "JammerType", "MaxRangeKm" },
                values: new object[] { 501, "Multi-band HF/VHF/UHF/SHF", "Stand-off Jammer (SOJ)", 3000.0 });

            migrationBuilder.InsertData(
                table: "AirToAirMissiles",
                columns: new[] { "Id", "FoxCode", "GuidanceType", "MaxSpeedMach", "RangeKm" },
                values: new object[,]
                {
                    { 12, (byte)2, "Infrared (IR)", 2.5, null },
                    { 13, (byte)3, "Aktif Radar", 4.0, null },
                    { 14, (byte)3, "Aktif Radar", 4.0, null },
                    { 15, (byte)2, "Infrared (IR)", 3.0, null },
                    { 16, (byte)3, "Aktif Radar", 4.0, null },
                    { 17, (byte)2, "Infrared (IR)", 4.0, null },
                    { 18, (byte)3, "Aktif Radar", 4.0, null },
                    { 19, (byte)2, "Infrared (IR)", 2.5, null },
                    { 20, (byte)3, "Aktif Radar", 4.5, null },
                    { 21, null, "IR/RF", 3.0, null }
                });

            migrationBuilder.InsertData(
                table: "AirborneRadars",
                columns: new[] { "Id", "CoolingSystem", "FrequencyBand", "MaxRangeKm", "RadarType", "ScanCoverage", "TargetTrackingCapacity", "TrModuleCount" },
                values: new object[,]
                {
                    { 22, null, null, 160.0, "AESA", null, 23, null },
                    { 23, null, null, 240.0, "AESA", null, 100, null },
                    { 24, null, null, 200.0, "AESA", null, 60, null },
                    { 25, null, null, 400.0, "PESA", null, 30, null },
                    { 26, null, null, 150.0, "AESA", null, 40, null }
                });

            migrationBuilder.InsertData(
                table: "AntiShipMissiles",
                columns: new[] { "Id", "MaxSpeedMach", "RangeKm", "SeaSkimming", "SpeedClass" },
                values: new object[,]
                {
                    { 31, null, 140.0, true, "Subsonic" },
                    { 32, null, 220.0, true, "Subsonic" }
                });

            migrationBuilder.InsertData(
                table: "AwacsAircrafts",
                columns: new[] { "Id", "DetectionRangeKm", "MaxTrackedTargets", "RadarType" },
                values: new object[] { 505, 400.0, 180, "MESA (Çok Rollü Elektronik Taramalı Dizi)" });

            migrationBuilder.InsertData(
                table: "BallisticMissiles",
                columns: new[] { "Id", "BallisticType", "HasMirv", "IsNuclearCapable", "MaxSpeedMach", "PayloadKg", "RangeKm" },
                values: new object[,]
                {
                    { 33, null, true, true, null, 2800.0, 12000.0 },
                    { 34, null, false, false, null, 500.0, 560.0 }
                });

            migrationBuilder.InsertData(
                table: "CargoAircrafts",
                columns: new[] { "Id", "CargoVolumeCubicMeters", "PayloadCapacityTons", "RangeWithMaxPayloadKm" },
                values: new object[] { 502, 340.0, 37.0, 3300.0 });

            migrationBuilder.InsertData(
                table: "CruiseMissiles",
                columns: new[] { "Id", "CepMeters", "MaxSpeedMach", "RangeKm" },
                values: new object[,]
                {
                    { 39, 10.0, null, 1600.0 },
                    { 40, 5.0, null, 250.0 }
                });

            migrationBuilder.InsertData(
                table: "ElectricNuclearPowers",
                columns: new[] { "Id", "PowerOutputMw", "SystemType" },
                values: new object[,]
                {
                    { 331, 30.0, "Nükleer Reaktör" },
                    { 332, 0.23999999999999999, "AIP (Hava Bağımsız Tahrik)" }
                });

            migrationBuilder.InsertData(
                table: "FighterAircrafts",
                columns: new[] { "Id", "CombatRadiusKm", "Generation", "HasAesaRadar", "HasStealth" },
                values: new object[,]
                {
                    { 1, 550.0, "4", true, false },
                    { 2, 1090.0, "5", true, true },
                    { 3, 850.0, "5", true, true },
                    { 4, 1100.0, "5", true, true },
                    { 5, 1390.0, "4.5", true, false },
                    { 6, 1850.0, "4.5", true, false },
                    { 7, 1500.0, "5", true, true },
                    { 8, 1600.0, "4.5", true, false },
                    { 9, 2000.0, "5", true, true },
                    { 10, 800.0, "4.5", true, false },
                    { 11, 1270.0, "4.5", true, false }
                });

            migrationBuilder.InsertData(
                table: "Frigates",
                columns: new[] { "Id", "DisplacementTons", "VlsCellsCount" },
                values: new object[,]
                {
                    { 35, 3000.0, 16 },
                    { 36, 6000.0, 32 }
                });

            migrationBuilder.InsertData(
                table: "InfantryWeapons",
                columns: new[] { "Id", "Caliber", "EffectiveRangeMeters", "MagazineCapacity", "RateOfFireRpm", "WeightKg" },
                values: new object[,]
                {
                    { 601, "9x19mm Parabellum", 50, 15, null, 0.79000000000000004 },
                    { 602, "7.62x51mm NATO", 600, 20, 700, 4.0999999999999996 },
                    { 603, "5.56x45mm NATO", 400, 30, 800, 3.2999999999999998 },
                    { 604, "7.62x51mm NATO", 1200, 100, 850, 12.0 },
                    { 605, "7.62x51mm NATO", 800, 20, null, 4.7000000000000002 }
                });

            migrationBuilder.InsertData(
                table: "LandVehicles",
                columns: new[] { "Id", "CrewCount", "EngineHorsePower", "HasAutoloader", "MainGunCaliberMm", "WeightTons" },
                values: new object[,]
                {
                    { 101, 4, 1500, false, 120.0, 66.5 },
                    { 102, 3, 1500, true, 120.0, 55.0 },
                    { 103, 4, 1500, false, 120.0, 65.0 },
                    { 104, 4, 1500, false, 120.0, 66.799999999999997 },
                    { 105, 3, 1130, true, 125.0, 46.0 },
                    { 106, 3, 1250, true, 125.0, 46.0 },
                    { 107, 3, 1130, true, 125.0, 48.0 },
                    { 108, 4, 1200, false, 120.0, 64.0 },
                    { 109, 4, 1500, false, 120.0, 65.0 },
                    { 110, 3, 1500, true, 120.0, 57.399999999999999 },
                    { 111, 3, 1200, true, 120.0, 44.0 },
                    { 112, 4, 1247, false, 120.0, 54.0 }
                });

            migrationBuilder.InsertData(
                table: "MarineGasTurbines",
                columns: new[] { "Id", "ShaftHorsePowerHp" },
                values: new object[] { 309, 33600.0 });

            migrationBuilder.InsertData(
                table: "MaritimePatrolAircrafts",
                columns: new[] { "Id", "EnduranceHours", "HasTorpedoTubes", "SonarType" },
                values: new object[,]
                {
                    { 503, 10.5, true, "AN/APY-10" },
                    { 504, 9.0, true, "AMASCOS" }
                });

            migrationBuilder.InsertData(
                table: "PistonEngines",
                columns: new[] { "Id", "Cylinders", "FuelType", "HorsePower", "TorqueNm" },
                values: new object[,]
                {
                    { 311, 4, "Dizel / JP-8", 170.0, null },
                    { 312, 12, "Dizel", 1500.0, null },
                    { 313, 12, "Dizel", 1500.0, null }
                });

            migrationBuilder.InsertData(
                table: "RocketMotors",
                columns: new[] { "Id", "BurnTimeSeconds", "PropellantType", "ThrustKn" },
                values: new object[,]
                {
                    { 321, null, "Katı", null },
                    { 322, null, "Sıvı (Metan/LOX)", 2200.0 }
                });

            migrationBuilder.InsertData(
                table: "Submarines",
                columns: new[] { "Id", "DisplacementTons", "MaxDepthMeters", "PropulsionType", "TorpedoTubesCount" },
                values: new object[,]
                {
                    { 37, 7900.0, 240.0, "Nükleer", 4 },
                    { 38, 2010.0, 400.0, "AIP (Hava Bağımsız)", 8 }
                });

            migrationBuilder.InsertData(
                table: "TurbofanEngines",
                columns: new[] { "Id", "BypassRatio", "DryThrustLbf", "HasAfterburner", "MaxThrustLbf" },
                values: new object[,]
                {
                    { 301, 1.0800000000000001, 6000.0, false, 6000.0 },
                    { 302, 1.0800000000000001, 6000.0, true, 10000.0 },
                    { 303, 0.56999999999999995, 28000.0, true, 43000.0 },
                    { 304, 0.76000000000000001, 17155.0, true, 29500.0 },
                    { 305, 0.58999999999999997, 17130.0, true, 27560.0 }
                });

            migrationBuilder.InsertData(
                table: "TurbojetEngines",
                columns: new[] { "Id", "DryThrustLbf", "HasAfterburner", "MaxThrustLbf" },
                values: new object[] { 306, 720.0, false, 720.0 });

            migrationBuilder.InsertData(
                table: "TurbopropEngines",
                columns: new[] { "Id", "ShaftHorsePowerHp" },
                values: new object[] { 308, 1200.0 });

            migrationBuilder.InsertData(
                table: "TurboshaftEngines",
                columns: new[] { "Id", "ShaftHorsePowerHp" },
                values: new object[] { 307, 1400.0 });

            migrationBuilder.InsertData(
                table: "UAVs",
                columns: new[] { "Id", "CruisingSpeedKmh", "EnduranceHours", "MaxAltitudeFeet", "PayloadCapacityKg", "WingSpanMeters" },
                values: new object[,]
                {
                    { 201, 130.0, 27.0, 25000, 150.0, 12.0 },
                    { 202, 160.0, 24.0, 25000, 280.0, 14.0 },
                    { 203, 277.0, 24.0, 40000, 1500.0, 20.0 },
                    { 204, 735.0, 5.0, 45000, 1500.0, 10.0 },
                    { 205, 313.0, 27.0, 50000, 1700.0, 20.0 },
                    { 206, 575.0, 34.0, 60000, 1360.0, 39.899999999999999 },
                    { 207, 370.0, 32.0, 32500, 480.0, 20.5 },
                    { 208, 235.0, 40.0, 23600, 345.0, 18.0 },
                    { 209, 120.0, 24.0, 24600, 250.0, 16.300000000000001 },
                    { 210, 1000.0, 12.0, 34400, 2800.0, 20.0 },
                    { 211, 200.0, 30.0, 30000, 350.0, 17.5 },
                    { 212, 250.0, 50.0, 40000, 750.0, 24.0 },
                    { 213, 800.0, 10.0, 40000, 1200.0, 12.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductReadModels_CategoryId",
                table: "ProductReadModels",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReadModels_Country",
                table: "ProductReadModels",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReadModels_CreatedAt",
                table: "ProductReadModels",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReadModels_IsActive_IsShowcase",
                table: "ProductReadModels",
                columns: new[] { "IsActive", "IsShowcase" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductReadModels_ProductType",
                table: "ProductReadModels",
                column: "ProductType");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReadModels_Slug",
                table: "ProductReadModels",
                column: "Slug");
        }
    }
}
