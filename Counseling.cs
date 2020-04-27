using System;

namespace RedLeg.Forms
{
    public partial class FormController
    {
        public class Counseling
        {
            public String Name { get; set; }

            public Rank Rank { get; set; }

            public DateTime Date { get; set; } = DateTime.Today;

            public String Organization { get; set; }

            public String Counselor { get; set; }

            public String Purpose { get; set; }

            public String KeyPointsOfDiscussion { get; set; }

            public String PlanOfAction { get; set; }

            public String LeadersResponsibilities { get; set; }

            public String Assessment { get; set; }
        }
    }
}
