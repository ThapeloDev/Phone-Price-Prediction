using Prediction.Models.Hardware;
using Prediction.Models.Time_Series_Forecasting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.View_Models.Hardware_Suggestion
{
    public class PurchaseSuggestion
    {
        public PurchaseSuggestion(List<Item> transactions, List<PhoneProperties> hardware, int? storage = null, 
            bool? hasMemoryCardReader = null, int? cpuCoreCount = null, double? cpuSpeed = null, int? ram = null, bool? headphoneOutput =null, 
            bool? is5gCapable = null, int? frontCameraMgpx = null, int? backCameraMgpx = null, int? rearCameraMgpx = null, 
            bool? exchangableBattery = null, bool? wirelessCharging = null, bool? fastCharging = null, bool? waterResistant = null)
        {

            if (hasMemoryCardReader == null)
                hasMemoryCardReader = false;
            if (headphoneOutput == null)
                headphoneOutput = false;
            if (is5gCapable == null)
                is5gCapable = false;
            if (exchangableBattery == null)
                exchangableBattery = false;
            if (wirelessCharging == null)
                wirelessCharging = false;
            if (fastCharging == null)
                fastCharging = false;
            if (waterResistant == null)
                waterResistant = false;

            Storage = storage;
            //HasMemoryCardReader = hasMemoryCardReader;
            CpuCoreCount = cpuCoreCount;
            CpuSpeed = cpuSpeed;
            RAM = ram;
            //HeadphoneOutput = headphoneOutput;
            //Is5gCapable = is5gCapable;
            FrontCameraMegapixel = frontCameraMgpx;
            BackCameraMegapixel = backCameraMgpx;
            RearCameraCount = rearCameraMgpx;
            //ExchangableBattery = exchangableBattery;
            //WirelessCharging = wirelessCharging;
            //FastCharging = fastCharging;
            //WaterResistant = waterResistant;


            this.Transactions = transactions;
            this.Hardware = GetPhonesWithCorrespondingSpecs(hardware);
        }

        private List<Item> Transactions { get; set; }
        private List<PhoneProperties> Hardware { get; set; }

        // Storage
        public bool HasMemoryCardReader { get; set; }
        public int? Storage { get; set; }

        // CPU
        public int? CpuCoreCount { get; set; }
        public double? CpuSpeed { get; set; }

        // RAM
        public int? RAM { get; set; }

        // Connectors and Communication
        public bool HeadphoneOutput { get; set; }
        public bool Is5gCapable { get; set; }

        // Camera
        public int? FrontCameraMegapixel { get; set; }
        public int? BackCameraMegapixel { get; set; }
        public int? RearCameraCount { get; set; }

        // Battery
        public bool ExchangableBattery { get; set; }

        // Misc
        public bool WirelessCharging { get; set; }
        public bool FastCharging { get; set; }
        public bool WaterResistant { get; set; }


        public List<PhoneProperties> GetPhonesWithCorrespondingSpecs(List<PhoneProperties> allSpecs)
        {
            if (Storage.HasValue)
                allSpecs = allSpecs.Where(x => x.Storage >= Storage).ToList();

            allSpecs = allSpecs.Where(x => x.HasMemoryCardReader == HasMemoryCardReader || x.HasMemoryCardReader == true).ToList();

            if (CpuCoreCount.HasValue)
                allSpecs = allSpecs.Where(x => x.CpuCoreCount >= CpuCoreCount).ToList();
            if (CpuSpeed.HasValue)
                allSpecs = allSpecs.Where(x => x.CpuSpeed >= CpuSpeed).ToList();
            if (RAM.HasValue)
                allSpecs = allSpecs.Where(x => x.RAM >= RAM).ToList();

            allSpecs = allSpecs.Where(x => x.HeadphoneOutput == HeadphoneOutput || x.HeadphoneOutput == true).ToList();
            allSpecs = allSpecs.Where(x => x.Is5gCapable == Is5gCapable || x.Is5gCapable == true).ToList();

            if (FrontCameraMegapixel.HasValue)
                allSpecs = allSpecs.Where(x => x.FrontCameraMegapixel >= FrontCameraMegapixel).ToList();
            if (BackCameraMegapixel.HasValue)
                allSpecs = allSpecs.Where(x => x.BackCameraMegapixel >= BackCameraMegapixel).ToList();
            if (RearCameraCount.HasValue)
                allSpecs = allSpecs.Where(x => x.RearCameraCount >= RearCameraCount).ToList();

            allSpecs = allSpecs.Where(x => x.ExchangableBattery == ExchangableBattery || x.ExchangableBattery == true).ToList();
            allSpecs = allSpecs.Where(x => x.WirelessCharging == WirelessCharging || x.WirelessCharging == true).ToList();
            allSpecs = allSpecs.Where(x => x.FastCharging == FastCharging || x.FastCharging == true).ToList();
            allSpecs = allSpecs.Where(x => x.WaterResistance == WaterResistant || x.WaterResistance == true).ToList();

            foreach(PhoneProperties p in allSpecs)
            {
            }


            System.Diagnostics.Debug.WriteLine($"Phones that comply: {allSpecs.Count}");

            return allSpecs;
        }

    }

}
