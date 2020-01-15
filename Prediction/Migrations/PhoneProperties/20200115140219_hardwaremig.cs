using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Prediction.Migrations.PhoneProperties
{
    public partial class hardwaremig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhoneProperties",
                columns: table => new
                {
                    ConfigId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Brand = table.Column<int>(nullable: false),
                    Model = table.Column<string>(nullable: true),
                    Storage = table.Column<int>(nullable: false),
                    Cpu = table.Column<int>(nullable: false),
                    CpuType = table.Column<int>(nullable: false),
                    CpuSpeed = table.Column<double>(nullable: false),
                    RAM = table.Column<int>(nullable: false),
                    HasGPU = table.Column<bool>(nullable: false),
                    HeadphoneOutput = table.Column<bool>(nullable: false),
                    Capable5g = table.Column<bool>(nullable: false),
                    FrontCameraMegapixel = table.Column<int>(nullable: false),
                    BackCameraMegapixel = table.Column<int>(nullable: false),
                    BatteryCapacity = table.Column<int>(nullable: false),
                    ExchangableBattery = table.Column<bool>(nullable: false),
                    Depth = table.Column<double>(nullable: false),
                    Height = table.Column<double>(nullable: false),
                    Width = table.Column<double>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    WirelessCharging = table.Column<bool>(nullable: false),
                    WirelessStandard = table.Column<int>(nullable: false),
                    DualSim = table.Column<bool>(nullable: false),
                    SimCard = table.Column<int>(nullable: false),
                    FastCharging = table.Column<bool>(nullable: false),
                    WaterResistance = table.Column<bool>(nullable: false),
                    ReleaseYear = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneProperties", x => x.ConfigId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhoneProperties");
        }
    }
}
