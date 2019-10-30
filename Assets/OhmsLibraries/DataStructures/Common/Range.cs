using System;
using System.Collections.Generic;
using System.Text;

namespace OhmsLibraries.DataStructures {
    public class Range {
        //Los rangos definene el límite superior (no incluyente) y todos empiezan en cero.
        private int[] ranges;
        public Range ( int[] _ranges ) {
            ranges = new int[_ranges.Length];
            Array.Copy( _ranges, this.ranges, _ranges.Length );
        }

        public int GetRange ( int value ) {
            int range = 0;
            int accumm = ranges[range] - 1;
            while ( range < ranges.Length - 1 ) {
                if ( accumm >= value ) {
                    return range;
                }
                accumm += ranges[range + 1];
                range++;
            }
            return range;
        }

        public void ModifyRange ( int range, int value ) {
            ranges[range] += value;
        }
    }
}
