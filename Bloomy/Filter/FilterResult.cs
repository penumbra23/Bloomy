using System;
using System.Collections.Generic;
using System.Text;

namespace Bloomy.Lib.Filter
{
    public enum BloomPresence
    {
        NotInserted,
        MightBeInserted
    }

    public class FilterResult
    {
        public FilterResult(BloomPresence present, double p)
        {
            Presence = present;
            Probability = Math.Max(0, Math.Min(1, p));
        }

        public BloomPresence Presence { get; }
        public double Probability { get; }
    }
}
