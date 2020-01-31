using Microsoft.AspNetCore.Mvc;
using Prediction.Models.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.ChartManual
{
    public class ManualChart
    {

        public ManualChart(List<PhoneProperties> phoneInfo, List<int> selectedItems = null)
        {
            this.PhoneInfo = phoneInfo;
            this.SelectedItems = selectedItems;
        }

        public List<PhoneProperties> PhoneInfo { get; set; } = new List<PhoneProperties>();
        public List<int> SelectedItems { get; set; }
    }
}
