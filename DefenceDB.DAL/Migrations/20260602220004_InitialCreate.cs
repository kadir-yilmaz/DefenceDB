using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DefenceDB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IconClass = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModelTypeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DefenseProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NatoReportingName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    YearIntroduced = table.Column<int>(type: "int", nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsShowcase = table.Column<bool>(type: "bit", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefenseProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefenseProducts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AirborneRadars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    RadarType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxRangeKm = table.Column<double>(type: "float", nullable: true),
                    TargetTrackingCapacity = table.Column<int>(type: "int", nullable: true),
                    TrModuleCount = table.Column<int>(type: "int", nullable: true),
                    FrequencyBand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScanCoverage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoolingSystem = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    RadarType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxRangeKm = table.Column<double>(type: "float", nullable: true),
                    TargetTrackingCapacity = table.Column<int>(type: "int", nullable: true),
                    TrModuleCount = table.Column<int>(type: "int", nullable: true),
                    FrequencyBand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScanCoverage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoolingSystem = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    MaxSearchRangeKm = table.Column<double>(type: "float", nullable: true),
                    MaxTrackingRangeKm = table.Column<double>(type: "float", nullable: true),
                    MaxEngagementAltitudeFt = table.Column<double>(type: "float", nullable: true),
                    MaxTrackedTargets = table.Column<int>(type: "int", nullable: true),
                    MissilesPerLauncher = table.Column<int>(type: "int", nullable: true),
                    HasAntiBallisticCapability = table.Column<bool>(type: "bit", nullable: false)
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
                name: "AirToAirMissiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    GuidanceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxSpeedMach = table.Column<double>(type: "float", nullable: true),
                    FoxCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    SeekerType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RangeKm = table.Column<double>(type: "float", nullable: true),
                    MaxSpeedMach = table.Column<double>(type: "float", nullable: true)
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
                    SeaSkimming = table.Column<bool>(type: "bit", nullable: false),
                    SpeedClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RangeKm = table.Column<double>(type: "float", nullable: true),
                    MaxSpeedMach = table.Column<double>(type: "float", nullable: true)
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
                name: "BallisticMissiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    RangeKm = table.Column<double>(type: "float", nullable: true),
                    PayloadKg = table.Column<double>(type: "float", nullable: true),
                    IsNuclearCapable = table.Column<bool>(type: "bit", nullable: false),
                    HasMirv = table.Column<bool>(type: "bit", nullable: false),
                    MaxSpeedMach = table.Column<double>(type: "float", nullable: true),
                    BallisticType = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    PayloadCapacityKg = table.Column<double>(type: "float", nullable: true),
                    IsNuclearCapable = table.Column<bool>(type: "bit", nullable: false),
                    CombatRadiusKm = table.Column<double>(type: "float", nullable: true)
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
                    RangeKm = table.Column<double>(type: "float", nullable: true),
                    CepMeters = table.Column<double>(type: "float", nullable: true),
                    MaxSpeedMach = table.Column<double>(type: "float", nullable: true)
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
                    VlsCellsCount = table.Column<int>(type: "int", nullable: true),
                    RadarSystemType = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Generation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasStealth = table.Column<bool>(type: "bit", nullable: false),
                    HasAesaRadar = table.Column<bool>(type: "bit", nullable: false),
                    CombatRadiusKm = table.Column<double>(type: "float", nullable: true)
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
                    MaxSpeedMach = table.Column<double>(type: "float", nullable: true),
                    CarrierPlatform = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "KamikazeUAVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    WarheadWeightKg = table.Column<double>(type: "float", nullable: true),
                    EnduranceHours = table.Column<double>(type: "float", nullable: true),
                    RangeKm = table.Column<double>(type: "float", nullable: true),
                    MaxSpeedKmh = table.Column<double>(type: "float", nullable: true)
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
                    WarheadWeightKg = table.Column<double>(type: "float", nullable: true),
                    RangeNauticalMiles = table.Column<double>(type: "float", nullable: true),
                    MaxSpeedKnots = table.Column<double>(type: "float", nullable: true)
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
                    RadarType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScanType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxRangeKm = table.Column<double>(type: "float", nullable: true),
                    TargetTrackingCapacity = table.Column<int>(type: "int", nullable: true),
                    TrModuleCount = table.Column<int>(type: "int", nullable: true),
                    FrequencyBand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScanCoverage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoolingSystem = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    HorsePower = table.Column<double>(type: "float", nullable: true),
                    TorqueNm = table.Column<double>(type: "float", nullable: true),
                    Cylinders = table.Column<int>(type: "int", nullable: true),
                    FuelType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
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
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMainImage = table.Column<bool>(type: "bit", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_DefenseProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductRelationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceProductId = table.Column<int>(type: "int", nullable: false),
                    TargetProductId = table.Column<int>(type: "int", nullable: false),
                    RelationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRelationships_DefenseProducts_SourceProductId",
                        column: x => x.SourceProductId,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductRelationships_DefenseProducts_TargetProductId",
                        column: x => x.TargetProductId,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RocketMotors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ThrustKn = table.Column<double>(type: "float", nullable: true),
                    BurnTimeSeconds = table.Column<double>(type: "float", nullable: true),
                    PropellantType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
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
                    TorpedoTubesCount = table.Column<int>(type: "int", nullable: true),
                    PropulsionType = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "Tanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EngineHorsePower = table.Column<int>(type: "int", nullable: true),
                    MainGunCaliberMm = table.Column<double>(type: "float", nullable: true),
                    WeightTons = table.Column<double>(type: "float", nullable: true),
                    CrewCount = table.Column<int>(type: "int", nullable: true),
                    HasAutoloader = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tanks_DefenseProducts_Id",
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
                    MaxThrustLbf = table.Column<double>(type: "float", nullable: true),
                    DryThrustLbf = table.Column<double>(type: "float", nullable: true),
                    HasAfterburner = table.Column<bool>(type: "bit", nullable: false),
                    BypassRatio = table.Column<double>(type: "float", nullable: true)
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
                    MaxThrustLbf = table.Column<double>(type: "float", nullable: true),
                    DryThrustLbf = table.Column<double>(type: "float", nullable: true),
                    HasAfterburner = table.Column<bool>(type: "bit", nullable: false)
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
                    EnduranceHours = table.Column<double>(type: "float", nullable: true),
                    MaxAltitudeFeet = table.Column<int>(type: "int", nullable: true),
                    PayloadCapacityKg = table.Column<double>(type: "float", nullable: true),
                    WingSpanMeters = table.Column<double>(type: "float", nullable: true),
                    CruisingSpeedKmh = table.Column<double>(type: "float", nullable: true)
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
                    WeightKg = table.Column<double>(type: "float", nullable: true),
                    MaxSpeedKmh = table.Column<double>(type: "float", nullable: true),
                    OperationalRangeKm = table.Column<double>(type: "float", nullable: true),
                    DriveType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
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
                    EnduranceHours = table.Column<double>(type: "float", nullable: true),
                    MaxSpeedKnots = table.Column<double>(type: "float", nullable: true),
                    DisplacementTons = table.Column<double>(type: "float", nullable: true),
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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "IconClass", "ModelTypeName", "Name", "ParentCategoryId", "Slug", "SortOrder", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bi bi-rocket-takeoff", null, "Füzeler", null, "fuzeler", 1, null },
                    { 2, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bi bi-airplane-engines", null, "Savaş Uçakları", null, "savas-ucaklari", 2, null },
                    { 3, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bi bi-tsunami", null, "Savaş Gemileri", null, "savas-gemileri", 3, null },
                    { 4, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bi bi-radar", null, "Radarlar", null, "radarlar", 4, null },
                    { 22, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bi bi-shield-shaded", "DefenceDB.EL.Models.Products.Tank", "Tanklar", null, "tanklar", 5, null },
                    { 23, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bi bi-robot", null, "İnsansız Platformlar", null, "insansiz-platformlar", 6, null },
                    { 30, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bi bi-gear-wide-connected", null, "Motor ve Güç Sistemleri", null, "motor-ve-guc-sistemleri", 7, null },
                    { 39, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bi bi-shield-fill-check", "DefenceDB.EL.Models.Products.AirDefenseSystem", "Hava Savunma Sistemleri", null, "hava-savunma-sistemleri", 8, null },
                    { 5, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.AirToAirMissile", "Hava-Hava Füzeleri", 1, "hava-hava-fuzeleri", 0, null },
                    { 6, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.BallisticMissile", "Balistik Füzeler", 1, "balistik-fuzeler", 0, null },
                    { 7, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.AntiShipMissile", "Anti-Gemi Füzeleri", 1, "anti-gemi-fuzeleri", 0, null },
                    { 8, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.CruiseMissile", "Seyir Füzeleri", 1, "seyir-fuzeleri", 0, null },
                    { 9, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.AntiRadiationMissile", "Anti-Radyasyon Füzeleri", 1, "anti-radyasyon", 0, null },
                    { 10, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.HypersonicGlideVehicle", "Hipersonik Süzülme Araçları", 1, "hgv", 0, null },
                    { 11, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.FighterAircraft", "Avcı (Fighter)", 2, "avci-ucaklari", 0, null },
                    { 12, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.BomberAircraft", "Bombardıman", 2, "bombardiman", 0, null },
                    { 13, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.TrainerAircraft", "Eğitim", 2, "egitim", 0, null },
                    { 14, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.FastAttackCraft", "Hücumbot", 3, "hucumbot", 0, null },
                    { 15, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.Corvette", "Korvet", 3, "korvet", 0, null },
                    { 16, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.Frigate", "Fırkateyn", 3, "firkateyn", 0, null },
                    { 17, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.Destroyer", "Muhrip (Destroyer)", 3, "muhrip", 0, null },
                    { 18, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.Submarine", "Denizaltı", 3, "denizalti", 0, null },
                    { 19, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.AirDefenseRadar", "Hava Savunma Radarları", 4, "hava-savunma-radarlari", 0, null },
                    { 20, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.AirborneRadar", "Hava Radarları (Airborne)", 4, "airborne-radarlar", 0, null },
                    { 21, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.NavalRadar", "Deniz Radarları (Naval)", 4, "deniz-radarlari", 0, null },
                    { 24, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.UAV", "İHA (UAV)", 23, "iha-uav", 0, null },
                    { 25, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.USV", "İDA (USV)", 23, "ida-usv", 0, null },
                    { 26, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.UGV", "İKA (UGV)", 23, "ika-ugv", 0, null },
                    { 27, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.KamikazeUAV", "Kamikaze İHA", 23, "kamikaze-iha", 0, null },
                    { 28, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.KamikazeUSV", "Kamikaze İDA", 23, "kamikaze-ida", 0, null },
                    { 31, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.TurbofanEngine", "Turbofan Motorlar", 30, "turbofan-motorlar", 0, null },
                    { 32, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.PistonEngine", "Pistonlu/İçten Yanmalı Motorlar", 30, "pistonlu-motorlar", 0, null },
                    { 33, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.RocketMotor", "Roket Motorları", 30, "roket-motorlari", 0, null },
                    { 34, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.ElectricNuclearPower", "Elektrik ve Nükleer Güç", 30, "elektrik-ve-nukleer-guc", 0, null },
                    { 35, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.TurbojetEngine", "Turbojet Motorlar", 30, "turbojet-motorlar", 0, null },
                    { 36, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.TurbopropEngine", "Turboprop Motorlar", 30, "turboprop-motorlar", 0, null },
                    { 37, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.MarineGasTurbine", "Deniz Gaz Türbinleri", 30, "deniz-gaz-turbinleri", 0, null },
                    { 38, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.TurboshaftEngine", "Turboshaft Motorlar", 30, "turboshaft-motorlar", 0, null }
                });

            migrationBuilder.InsertData(
                table: "DefenseProducts",
                columns: new[] { "Id", "CategoryId", "Country", "CreatedAt", "Description", "IsActive", "IsShowcase", "Manufacturer", "Name", "NatoReportingName", "Slug", "Status", "ThumbnailUrl", "UpdatedAt", "VideoUrl", "YearIntroduced" },
                values: new object[,]
                {
                    { 101, 22, "Almanya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "KMW / Rheinmetall", "Leopard 2A7", null, "leopard-2a7", null, null, null, null, 2014 },
                    { 102, 22, "Güney Kore", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Hyundai Rotem", "K2 Black Panther", null, "k2-black-panther", null, null, null, null, 2014 },
                    { 103, 22, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "BMC / Otokar", "Altay", null, "altay", null, null, null, null, 2025 },
                    { 104, 22, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "General Dynamics", "M1A2 SEPv3 Abrams", null, "m1a2-abrams", null, null, null, null, 2020 },
                    { 105, 22, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Uralvagonzavod", "T-72B3", null, "t-72b3", null, null, null, null, 2013 },
                    { 106, 22, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Omsktransmash", "T-80BVM", null, "t-80bvm", null, null, null, null, 2017 },
                    { 107, 22, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Uralvagonzavod", "T-90M Proryv", null, "t-90m-proryv", null, null, null, null, 2020 },
                    { 108, 22, "Birleşik Krallık", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "BAE Systems", "Challenger 2", null, "challenger-2", null, null, null, null, 1998 },
                    { 109, 22, "İsrail", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "MANTAK", "Merkava Mk.4", null, "merkava-mk4", null, null, null, null, 2004 },
                    { 110, 22, "Fransa", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Nexter", "Leclerc", null, "leclerc", null, null, null, null, 1992 },
                    { 111, 22, "Japonya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Mitsubishi Heavy Industries", "Type 10", null, "type-10", null, null, null, null, 2012 },
                    { 112, 22, "İtalya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Iveco-Oto Melara", "C1 Ariete", null, "c1-ariete", null, null, null, null, 1995 },
                    { 401, 39, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Rusya yapımı orta ve uzun menzilli uçaksavar ve füze savunma sistemi.", true, true, "Almaz-Antey", "S-400 Triumf", null, "s-400-triumf", null, null, null, null, null },
                    { 402, 39, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Uzay ve kıtalararası balistik füze savunması odaklı yeni nesil uzun menzilli hava savunma sistemi.", true, true, "Almaz-Antey", "S-500 Prometey", null, "s-500-prometey", null, null, null, null, null },
                    { 403, 39, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Amerika Birleşik Devletleri ordusunun ana taktik hava ve balistik füze savunma sistemi.", true, true, "Raytheon / Lockheed Martin", "MIM-104 Patriot (PAC-3)", null, "mim-104-patriot-pac-3", null, null, null, null, null },
                    { 404, 39, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Atmosfer içi ve atmosfer dışı (Terminal aşamada) kısa, orta ve ara menzilli balistik füzeleri önleme sistemi.", true, true, "Lockheed Martin", "THAAD", null, "thaad", null, null, null, null, null },
                    { 405, 39, "Fransa", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Aster 30 füzelerini kullanan, uçak ve seyir füzelerine ek olarak balistik füzelere karşı da etkili Avrupa menşeili hava savunma sistemi.", true, false, "Eurosam", "SAMP/T Mamba", null, "samp-t-mamba", null, null, null, null, null },
                    { 406, 39, "Çin", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Çin Halk Kurtuluş Ordusu'nun ana uzun menzilli hava ve füze savunma sistemi.", true, false, "CASIC", "HQ-9", null, "hq-9", null, null, null, null, null },
                    { 408, 39, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Türkiye tarafından milli imkanlarla geliştirilen, savaş uçakları, helikopterler, seyir füzeleri ve İHA'lara karşı etkili alçak irtifa hava savunma sistemi.", true, true, "Aselsan / Roketsan", "HİSAR-A+", null, "hisar-a-plus", null, null, null, null, null },
                    { 409, 39, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Türkiye tarafından yerli imkanlarla tasarlanan ve geliştirilen, savaş uçakları, İHA'lar, seyir füzeleri ve helikopterler gibi hedefleri imha etmek üzere tasarlanmış orta irtifa hava savunma sistemi.", true, true, "Aselsan / Roketsan", "HİSAR-O+", null, "hisar-o-plus", null, null, null, null, null },
                    { 411, 39, "Güney Kore", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Güney Kore ordusunun kritik tesislerini ve birliklerini korumak için tasarlanmış mobil alçak irtifa hava savunma sistemi.", true, false, "Hanwha Defense / LIG Nex1", "K-SAM Chunma (Pegasus)", null, "k-sam-chunma-pegasus", null, null, null, null, null },
                    { 412, 39, "Güney Kore", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Güney Kore yapımı, orta irtifadaki hava tehditlerine (uçaklar ve seyir füzeleri) karşı geliştirilmiş ilk nesil Cheongung hava savunma sistemi.", true, false, "LIG Nex1 / Hanwha Systems", "Cheongung I (M-SAM)", null, "cheongung-i-m-sam", null, null, null, null, null },
                    { 413, 39, "Güney Kore", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Güney Kore'nin katmanlı füze savunma kalkanı (KAMD) kapsamında geliştirdiği, üst irtifadaki balistik füzeleri ve hava tehditlerini önleme amaçlı uzun menzilli hava savunma sistemi.", true, true, "LIG Nex1 / Hanwha Systems", "L-SAM", null, "l-sam", null, null, null, null, null },
                    { 414, 39, "Güney Kore", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cheongung I'in geliştirilmiş versiyonu olan orta irtifa hava savunma sistemi. Daha gelişmiş radar, artırılmış menzil ve çoklu hedef takip yeteneği sunar.", true, false, "LIG Nex1 / Hanwha Systems", "Cheongung II (M-SAM Block II)", null, "cheongung-ii-m-sam-block-ii", null, null, null, null, null },
                    { 415, 39, "Güney Kore", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Güney Kore'nin taşınabilir hava savunma sistemi (MANPADS). Helikopterler, İHA'lar ve alçak uçan uçaklara karşı etkili.", true, false, "LIG Nex1", "KP-SAM (Shin-Gung)", null, "kp-sam-shin-gung", null, null, null, null, null },
                    { 416, 39, "Güney Kore", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Güney Kore yapımı kendinden tahrikli uçaksavar topu (SPAAG). 30mm çift namlulu top ve kısa menzilli füzelerle donatılmıştır.", true, false, "Doosan DST / S&T Dynamics", "Biho (K30)", null, "biho-k30", null, null, null, null, null },
                    { 417, 39, "Güney Kore", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "K-SAM Chunma'nın geliştirilmiş versiyonu olan alçak irtifa hava savunma sistemi. Daha modern radar ve geliştirilmiş füze teknolojisi kullanır.", true, false, "Hanwha Defense / LIG Nex1", "Cheonma (K-SAM Block II)", null, "cheonma-k-sam-block-ii", null, null, null, null, null },
                    { 418, 39, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Patriot sisteminin PAC-2 konfigürasyonu. Öncelikle geleneksel hava tehditlerine (uçaklar, helikopterler, seyir füzeleri) karşı etkili, sınırlı balistik füze savunma yeteneği vardır.", true, false, "Raytheon / Lockheed Martin", "MIM-104 Patriot (PAC-2)", null, "mim-104-patriot-pac-2", null, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "AirDefenseSystems",
                columns: new[] { "Id", "HasAntiBallisticCapability", "MaxEngagementAltitudeFt", "MaxSearchRangeKm", "MaxTrackedTargets", "MaxTrackingRangeKm", "MissilesPerLauncher" },
                values: new object[,]
                {
                    { 401, true, 100000.0, 600.0, 300, 400.0, 4 },
                    { 402, true, 650000.0, 800.0, 100, 600.0, 4 },
                    { 403, true, 80000.0, 150.0, 100, 100.0, 16 },
                    { 404, true, 490000.0, 1000.0, 100, 800.0, 8 },
                    { 405, true, 65000.0, 150.0, 100, 100.0, 8 },
                    { 406, true, 98000.0, 250.0, 100, 180.0, 4 },
                    { 408, false, 26000.0, 35.0, 60, 25.0, 4 },
                    { 409, false, 49000.0, 80.0, 60, 60.0, 6 },
                    { 411, false, 16000.0, 20.0, 20, 16.0, 8 },
                    { 412, false, 49000.0, 100.0, 40, 80.0, 8 },
                    { 413, true, 190000.0, 300.0, 100, 250.0, 6 },
                    { 414, false, 60000.0, 150.0, 50, 120.0, 8 },
                    { 415, false, 12000.0, 8.0, 2, 6.0, 1 },
                    { 416, false, 10000.0, 15.0, 10, 12.0, 4 },
                    { 417, false, 20000.0, 25.0, 25, 20.0, 8 },
                    { 418, false, 60000.0, 100.0, 100, 90.0, 4 }
                });

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
                    { 332, 34, "Almanya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Siemens", "Siemens PEM Yakıt Hücresi (AIP)", null, "siemens-pem-aip", null, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Tanks",
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
                table: "AirToAirMissiles",
                columns: new[] { "Id", "FoxCode", "GuidanceType", "MaxSpeedMach", "RangeKm" },
                values: new object[,]
                {
                    { 12, "Fox 2", "Infrared (IR)", 2.5, null },
                    { 13, "Fox 3", "Aktif Radar", 4.0, null },
                    { 14, "Fox 3", "Aktif Radar", 4.0, null },
                    { 15, "Fox 2", "Infrared (IR)", 3.0, null },
                    { 16, "Fox 3", "Aktif Radar", 4.0, null },
                    { 17, "Fox 2", "Infrared (IR)", 4.0, null },
                    { 18, "Fox 3", "Aktif Radar", 4.0, null },
                    { 19, "Fox 2", "Infrared (IR)", 2.5, null },
                    { 20, "Fox 3", "Aktif Radar", 4.5, null },
                    { 21, "Fox 2 / Fox 3", "IR/RF", 3.0, null }
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
                table: "BallisticMissiles",
                columns: new[] { "Id", "BallisticType", "HasMirv", "IsNuclearCapable", "MaxSpeedMach", "PayloadKg", "RangeKm" },
                values: new object[,]
                {
                    { 33, null, true, true, null, 2800.0, 12000.0 },
                    { 34, null, false, false, null, 500.0, 560.0 }
                });

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
                table: "MarineGasTurbines",
                columns: new[] { "Id", "ShaftHorsePowerHp" },
                values: new object[] { 309, 33600.0 });

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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DefenseProducts_CategoryId",
                table: "DefenseProducts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRelationships_SourceProductId",
                table: "ProductRelationships",
                column: "SourceProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRelationships_TargetProductId",
                table: "ProductRelationships",
                column: "TargetProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirborneRadars");

            migrationBuilder.DropTable(
                name: "AirDefenseRadars");

            migrationBuilder.DropTable(
                name: "AirDefenseSystems");

            migrationBuilder.DropTable(
                name: "AirToAirMissiles");

            migrationBuilder.DropTable(
                name: "AntiRadiationMissiles");

            migrationBuilder.DropTable(
                name: "AntiShipMissiles");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BallisticMissiles");

            migrationBuilder.DropTable(
                name: "BomberAircrafts");

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
                name: "KamikazeUAVs");

            migrationBuilder.DropTable(
                name: "KamikazeUSVs");

            migrationBuilder.DropTable(
                name: "MarineGasTurbines");

            migrationBuilder.DropTable(
                name: "Minehunters");

            migrationBuilder.DropTable(
                name: "NavalRadars");

            migrationBuilder.DropTable(
                name: "PistonEngines");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductRelationships");

            migrationBuilder.DropTable(
                name: "RocketMotors");

            migrationBuilder.DropTable(
                name: "Submarines");

            migrationBuilder.DropTable(
                name: "Tanks");

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
                name: "USVs");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "DefenseProducts");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
