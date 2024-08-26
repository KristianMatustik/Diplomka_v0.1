using System;


namespace Diplomka
{
    internal class Cyclist
    {
        public double PowerLIMIT { get; set; }
        public double PowerMAX { get; set; }
        public double mass { get; set; }
        public double crr { get; set; }
        public double efficiency { get; set; }
        public double bankAngle { get; set; }
        public double brakingForce { get; set; }

        public double[,] CdA_matrix { get; set; }
        public double[] CdA_power { get; set; }
        public double[] CdA_slope { get; set; }

        public Func<double, double> f { get; set; }
        public Func<double, double> f_inv { get; set; }

        public Cyclist(double _PowerLIMIT, double _PowerMAX, double _mass, double _crr, double _efficiency, double _bankAngle, double _brakingForce, double _CdA, Func<double, double> _f, Func<double, double> _f_inv)
        {
            this.PowerLIMIT = _PowerLIMIT;
            this.PowerMAX = _PowerMAX;
            this.mass = _mass;
            this.crr = _crr;
            this.efficiency = _efficiency;
            this.bankAngle = _bankAngle;
            this.brakingForce = _brakingForce;
            this.CdA_matrix = new double[1, 1] { { _CdA } }; CdA_power = new double[0]; CdA_slope = new double[0];
            this.f = _f;
            this.f_inv = _f_inv;
        }

        public Cyclist(double _PowerLIMIT, double _PowerMAX, double _mass, double _crr, double _efficiency, double _bankAngle, double _brakingForce, double[,] _CdA, double[] _powerCdA, double[] _slopeCdA, Func<double, double> _f, Func<double, double> _f_inv)
        {
            this.PowerMAX = _PowerMAX;
            this.PowerLIMIT = _PowerLIMIT;
            this.mass = _mass;
            this.crr = _crr;
            this.efficiency = _efficiency;
            this.bankAngle = _bankAngle;
            this.brakingForce = _brakingForce;
            this.CdA_matrix = _CdA;
            this.CdA_power = _powerCdA;
            this.CdA_slope = _slopeCdA;
            this.f = _f;
            this.f_inv = _f_inv;
        }

        internal double CdA(double Power = 0, double slope = 0)
        {
            int p = CdA_power.Length;
            int s = CdA_slope.Length;
            for (int i = 0; i < CdA_power.Length; i++)
            {
                if (CdA_power[i] > Power)
                {
                    p = i;
                    break;
                }
            }
            for (int i = 0; i < CdA_slope.Length; i++)
            {
                if (CdA_slope[i] > slope)
                {
                    s = i;
                    break;
                }
            }
            return CdA_matrix[p, s];
        }
    }
}
