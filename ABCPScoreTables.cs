using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RedLeg.Forms
{
    internal class ABCPScoreTables
    {
        public enum ABCPAgeGroup : byte
        {
            [Display(Name = "17-20")]
            Group_17_to_20,

            [Display(Name = "21-27")]
            Group_21_to_27,

            [Display(Name = "28-39")]
            Group_28_to_39,

            [Display(Name = "40+")]
            Group_40_Plus,
        }

        private static IEnumerable<ABCPAgeGroup> AgeGroups { get { return Enum.GetValues(typeof(ABCPAgeGroup)).Cast<ABCPAgeGroup>(); } }

        public static IEnumerable<Entry> ScreeningWeights
        {
            get
            {
                using (var stream = typeof(ABCPScoreTables).GetTypeInfo().Assembly.GetManifestResourceStream($"RedLeg.Forms.{nameof(ABCP)}.csv"))
                using (var reader = new StreamReader(stream))
                {
                    string line = string.Empty;

                    while (!String.IsNullOrWhiteSpace(line = reader.ReadLine()))
                    {
                        if (line.Contains("Gender") || line.Contains("Height")) continue;

                        var items = new Queue(line.Split(','));

                        var height = Convert.ToInt32(items.Dequeue());

                        foreach (var group in AgeGroups)
                        {
                            foreach (var gender in new[] { Gender.Male, Gender.Female })
                            {
                                yield return new Entry
                                {
                                    Gender = gender,
                                    AgeGroup = group,
                                    Height = height,
                                    Weight = Convert.ToInt32(items.Dequeue())
                                };
                            }
                        }
                    }
                }
            }
        }

        public static IEnumerable<MaximumAllowableBodyFat> MaxAllowablePercentages
        {
            get
            {
                yield return new MaximumAllowableBodyFat { Gender = Gender.Male, AgeGroup = ABCPAgeGroup.Group_17_to_20, Maximum = 20 };
                yield return new MaximumAllowableBodyFat { Gender = Gender.Female, AgeGroup = ABCPAgeGroup.Group_17_to_20, Maximum = 30 };

                yield return new MaximumAllowableBodyFat { Gender = Gender.Male, AgeGroup = ABCPAgeGroup.Group_21_to_27, Maximum = 22 };
                yield return new MaximumAllowableBodyFat { Gender = Gender.Female, AgeGroup = ABCPAgeGroup.Group_21_to_27, Maximum = 32 };

                yield return new MaximumAllowableBodyFat { Gender = Gender.Male, AgeGroup = ABCPAgeGroup.Group_28_to_39, Maximum = 24 };
                yield return new MaximumAllowableBodyFat { Gender = Gender.Female, AgeGroup = ABCPAgeGroup.Group_28_to_39, Maximum = 34 };

                yield return new MaximumAllowableBodyFat { Gender = Gender.Male, AgeGroup = ABCPAgeGroup.Group_40_Plus, Maximum = 26 };
                yield return new MaximumAllowableBodyFat { Gender = Gender.Female, AgeGroup = ABCPAgeGroup.Group_40_Plus, Maximum = 36 };
            }
        }

        public class MaximumAllowableBodyFat
        {
            public Gender Gender { get; set; }

            public ABCPAgeGroup AgeGroup { get; set; }

            public Double Maximum { get; set; }
        }

        public class Entry
        {
            public Gender Gender { get; set; }

            public ABCPAgeGroup AgeGroup { get; set; }

            public int Height { get; set; }

            public int Weight { get; set; }
        }
    }
}