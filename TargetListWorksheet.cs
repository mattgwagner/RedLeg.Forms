using System.Collections.Generic;

namespace RedLeg.Forms
{
    public class TargetListWorksheet
    {
        public int Sheet { get; set; }

        public int SheetOf { get; set; }

        /// <summary>
        /// A maximum of 21 targets can fit per page of the TLWS
        /// </summary>
        public List<Target> Targets { get; set; } = new List<Target>();

        public class Target
        {
            public string TargetNumber { get; set; }

            public string Description { get; set; }

            public string Location { get; set; }

            public string Altitude { get; set; }

            public string Attitude { get; set; }

            public string SizeLength { get; set; }

            public string SizeWidth { get; set; }

            public string SourceOrAccuracy { get; set; }

            public string Remarks { get; set; }

            public string Group_1 { get; set; }

            public string Group_2 { get; set; }

            public string Group_3 { get; set; }

            public string Group_4 { get; set; }

            public string Group_5 { get; set; }
        }
    }
}