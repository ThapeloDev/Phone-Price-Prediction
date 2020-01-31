using Prediction.Models.Enums;
using Prediction.Models.Hardware.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

        // CPU
        public CPU Cpu { get; set; }
        public CPUType CpuType { get; set; }
        public double CpuSpeed { get; set; }
        
        // RAM
        public int RAM { get; set; }
        
        //GPU
        public bool HasGPU { get; set; }
        
        // Headphone Jack
        public bool HeadphoneOutput { get; set; }
        
        // Networking
        public bool Capable5g { get; set; }
        
        // Camera
        public int FrontCameraMegapixel { get; set; }
        public int BackCameraMegapixel { get; set; }
        
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
        public int ReleaseYear { get; set; }

        // User interaction
        public bool isSelected { get; set; }
    }
}
