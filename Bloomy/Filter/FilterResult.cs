using System;
using System.Collections.Generic;
using System.Text;

namespace Bloomy.Lib.Filter
{
    public class FilterResult
    {
        public FilterResult(bool present, double p)
        {
            Present = present;
            Probability = Math.Max(0, Math.Min(1, p));
        }

        public bool Present { get; }
        public double Probability { get; }
    }
}
