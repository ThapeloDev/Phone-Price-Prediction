using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Prediction.Migrations.PhoneProperties
{
    public partial class HardwareWithGpu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReleaseYear",
                table: "PhoneProperties",
                newName: "RearCameraCount");

            migrationBuilder.RenameColumn(
                name: "CpuType",
                table: "PhoneProperties",
                newName: "MaxFramerateMinResolution");

            migrationBuilder.RenameColumn(
                name: "Capable5g",
                table: "PhoneProperties",
                newName: "IsWifiCapable");

            migrationBuilder.AddColumn<bool>(
                name: "BuiltInCamera",
                table: "PhoneProperties",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanRecordVideo",
                table: "PhoneProperties",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CpuCoreCount",
                table: "PhoneProperties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "FrontCamera",
                table: "PhoneProperties",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "GPU",
                table: "PhoneProperties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasBluetooth",
                table: "PhoneProperties",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasGPS",
                table: "PhoneProperties",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasMemoryCardReader",
                table: "PhoneProperties",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is2gCapable",
                table: "PhoneProperties",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is3gCapable",
                table: "PhoneProperties",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is4gCapable",
                table: "PhoneProperties",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is5gCapable",
                table: "PhoneProperties",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxFramerateMaxResolution",
                table: "PhoneProperties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "MaximumLensApeture",
                table: "PhoneProperties",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OriginalPrice",
                table: "PhoneProperties",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ProductPage",
                table: "PhoneProperties",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "PhoneProperties",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuiltInCamera",
                table: "PhoneProperties");

            migrationBuilder.DropColumn(
                name: "CanRecordVideo",
                table: "PhoneProperties");

            migrationBuilder.DropColumn(
                name: "CpuCoreCount",
                table: "PhoneProperties");

            migrationBuilder.DropColumn(
                name: "FrontCamera",
                table: "PhoneProperties");

            migrationBuilder.DropColumn(
                name: "GPU",
                table: "PhoneProperties");

            migrationBuilder.DropColumn(
                name: "HasBluetooth",
                table: "PhoneProperties");

            migrationBuilder.DropColumn(
                name: "HasGPS",
                table: "PhoneProperties");

            migrationBuilder.DropColumn(
                name: "HasMemoryCardReader",
                table: "PhoneProperties");

            migrationBuilder.DropColumn(
                name: "Is2gCapable",
                table: "PhoneProperties");

            migrationBuilder.DropColumn(
                name: "Is3gCapable",
                table: "PhoneProperties");

            migrationBuilder.DropColumn(
                name: "Is4gCapable",
                table: "PhoneProperties");

            migrationBuilder.DropColumn(
                name: "Is5gCapable",
                table: "PhoneProperties");

            migrationBuilder.DropColumn(
                name: "MaxFramerateMaxResolution",
                table: "PhoneProperties");

            migrationBuilder.DropColumn(
                name: "MaximumLensApeture",
                table: "PhoneProperties");

            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "PhoneProperties");

            migrationBuilder.DropColumn(
                name: "ProductPage",
                table: "PhoneProperties");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "PhoneProperties");

            migrationBuilder.RenameColumn(
                name: "RearCameraCount",
                table: "PhoneProperties",
                newName: "ReleaseYear");

            migrationBuilder.RenameColumn(
                name: "MaxFramerateMinResolution",
                table: "PhoneProperties",
                newName: "CpuType");

            migrationBuilder.RenameColumn(
                name: "IsWifiCapable",
                table: "PhoneProperties",
                newName: "Capable5g");
        }
    }
}
