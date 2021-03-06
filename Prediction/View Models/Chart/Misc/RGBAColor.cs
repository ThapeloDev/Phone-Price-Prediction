﻿namespace Prediction.Models.Chart.Misc
{
    public class RGBAColor
    {
        public RGBAColor(int R, int G, int B, double? A = null)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = A;
        }

        public int R { get; private set; }
        public int G { get; private set; }
        public int B { get; private set; }
        public double? A { get; private set; }
    }
}
