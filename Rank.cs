using System.ComponentModel.DataAnnotations;

namespace RedLeg.Forms
{
    public partial class FormController
    {
        public enum Rank : byte
        {
            [Display(Name = "Cadet", ShortName = "CDT")]
            CDT = 0,

            [Display(Name = "Private", ShortName = "PV1")]
            E1 = 1,

            [Display(Name = "Private (PV2)", ShortName = "PV2")]
            E2 = 2,

            [Display(Name = "Private First Class", ShortName = "PFC")]
            E3 = 3,

            [Display(Name = "Specialist", ShortName = "SPC")]
            E4 = 4,

            [Display(Name = "Sergeant", ShortName = "SGT")]
            E5 = 5,

            [Display(Name = "Staff Sergeant", ShortName = "SSG")]
            E6 = 6,

            [Display(Name = "Sergeant First Class", ShortName = "SFC")]
            E7 = 7,

            [Display(Name = "First Sergeant", ShortName = "1SG")]
            E8 = 8,

            [Display(Name = "Sergeant Major", ShortName = "SGM")]
            E9 = 9,

            [Display(Name = "Second Lieutenant", ShortName = "2LT")]
            O1 = 10,

            [Display(Name = "First Lieutenant", ShortName = "1LT")]
            O2 = 11,

            [Display(Name = "Captain", ShortName = "CPT")]
            O3 = 12,

            [Display(Name = "Major", ShortName = "MAJ")]
            O4 = 13,

            [Display(Name = "Lieutenant Colonel", ShortName = "LTC")]
            O5 = 14,

            [Display(Name = "Colonel", ShortName = "COL")]
            O6 = 15,

            [Display(Name = "Master Sergeant", ShortName = "MSG")]
            E8_MSG = 16,

            [Display(Name = "Warrant Officer", ShortName = "WO1")]
            WO1 = 17,

            [Display(Name = "Chief Warrant Officer", ShortName = "WO2")]
            WO2 = 18,

            [Display(Name = "Chief Warrant Officer", ShortName = "WO3")]
            WO3 = 19,

            [Display(Name = "Chief Warrant Officer", ShortName = "WO4")]
            WO4 = 20,

            [Display(Name = "Chief Warrant Officer", ShortName = "WO5")]
            WO5 = 21,

            [Display(Name = "Corporal", ShortName = "CPL")]
            E4_CPL = 22
        }
    }
}
