using Prediction.Models.Enums;
using Prediction.Models.Hardware.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace Prediction.Models.Hardware
{
    public class PhoneProperties
    {
        [Key]
        public int ConfigId { get; set; }

        // Basic naming information
        public Brand Brand { get; set; }
        public string Model { get; set; }

        // Storage
        public Storage Storage { get; set; }
        public bool HasMemoryCardReader { get; set; }

        // CPU
        public CPU Cpu { get; set; }
        public int CpuCoreCount { get; set; }
        public double CpuSpeed { get; set; }

        // RAM
        public int RAM { get; set; }

        //GPU
        public bool HasGPU { get; set; }
        public GPUType GPU { get; set; }

        // Connectors and Communication
        public bool HeadphoneOutput { get; set; }
        public bool Is2gCapable { get; set; }
        public bool Is3gCapable { get; set; }
        public bool Is4gCapable { get; set; }
        public bool Is5gCapable { get; set; }
        public bool HasBluetooth { get; set; }
        public bool HasGPS { get; set; }
        public bool IsWifiCapable { get; set; }

        // Camera
        public bool BuiltInCamera { get; set; }
        public bool FrontCamera { get; set; }
        public int FrontCameraMegapixel { get; set; }
        public int BackCameraMegapixel { get; set; }
        public double MaximumLensApeture { get; set; }
        public int RearCameraCount { get; set; }
        public bool CanRecordVideo { get; set; }
        public int MaxFramerateMaxResolution { get; set; }
        public int MaxFramerateMinResolution { get; set; }

        // Battery
        public int BatteryCapacity { get; set; }
        public bool ExchangableBattery { get; set; }

        // Dimensions
        public double Depth { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Weight { get; set; }

        // Features
        public bool WirelessCharging { get; set; }
        public WirelessStandard WirelessStandard { get; set; }
        public bool DualSim { get; set; }
        public SIMCard SimCard { get; set; }
        public bool FastCharging { get; set; }
        public bool WaterResistance { get; set; }

        // Release
        public double OriginalPrice { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        //Link
        public string ProductPage { get; set; }

        // User interaction
        public bool isSelected { get; set; }
    }
}
