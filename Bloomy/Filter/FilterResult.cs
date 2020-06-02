using System;
using System.Collections.Generic;
using System.Text;

namespace Bloomy.Lib.Filter
{
    /// <summary>
    /// Output of the bloom filter describing if the element might be inserted or if the element was never inserted.
    /// </summary>
    public enum BloomPresence
    {
        NotInserted,
        MightBeInserted
    }

    /// <summary>
    /// Output of <see cref="BasicFilter"./>
    /// </summary>
    public class FilterResult
    {
        public FilterResult(BloomPresence present, double p)
        {
            Presence = present;
            Probability = Math.Max(0, Math.Min(1, p));
        }

        /// <summary>
        /// Outputs if the value might be present in the filter or was never inserted.
        /// </summary>
        public BloomPresence Presence { get; }

        /// <summary>
        /// Probability of false positives.
        /// </summary>
        public double Probability { get; }
    }
}
