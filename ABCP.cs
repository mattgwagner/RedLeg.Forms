using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static RedLeg.Forms.ABCPScoreTables;

namespace RedLeg.Forms
{
    public class ABCP : IValidatableObject
    {
        /// <summary>
        /// Measurements are considered invalid if they're greater than 1 inch apart
        /// </summary>
        public const int MAX_DIFFERNCE = 1;

        public virtual ABCP_Soldier Soldier { get; set; }

        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; } = DateTime.Today;

        /// <summary>
        /// Recorded to nearest half inch when used for body fat percentage calculations
        /// </summary>
        [Required, Range(0, 90)]
        public Decimal Height { get; set; }

        /// <summary>
        /// Recorded to the nearest pound for all usage
        /// </summary>
        [Required, Range(0, 400)]
        public int Weight { get; set; }

        internal ABCPAgeGroup AgeGroup
        {
            get
            {
                if (Soldier.Age <= 20) return ABCPAgeGroup.Group_17_to_20;
                if (Soldier.Age <= 27) return ABCPAgeGroup.Group_21_to_27;
                if (Soldier.Age <= 39) return ABCPAgeGroup.Group_28_to_39;

                return ABCPAgeGroup.Group_40_Plus;
            }
        }

        [Display(Name = "Screening Weight")]
        internal Decimal Screening_Weight
        {
            get
            {
                return ABCPScoreTables
                    .ScreeningWeights
                    .Where(_ => _.AgeGroup == AgeGroup)
                    .Where(_ => _.Gender == Soldier?.Gender)
                    .Where(_ => _.Height == Math.Round(Height, decimals: 0, mode: MidpointRounding.AwayFromZero))
                    .Select(_ => _.Weight)
                    .SingleOrDefault();
            }
        }

        [Display(Name = "Requires Taping?")]
        internal Boolean RequiresTape => Screening_Weight < Weight;

        public ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();

        internal Boolean AreMeasurementsValid
        {
            // Returns false if any measurement is greater than 1 inch away from another measurement

            get
            {
                foreach (var measurement in Measurements)
                {
                    if (Measurements.Any(check => Math.Abs(check.Waist - measurement.Waist) > MAX_DIFFERNCE)) return false;
                    if (Measurements.Any(check => Math.Abs(check.Neck - measurement.Neck) > MAX_DIFFERNCE)) return false;
                    if (Measurements.Any(check => Math.Abs(check.Hips - measurement.Hips) > MAX_DIFFERNCE)) return false;
                }

                return true;
            }
        }

        internal Double WaistAverage => Average_To_Half(Measurements.Select(_ => _.Waist));

        internal Double NeckAverage => Average_To_Half(Measurements.Select(_ => _.Neck));

        internal Double HipAverage => Average_To_Half(Measurements.Select(_ => _.Hips));

        [Display(Name = "Circumference Value")]
        internal Double CircumferenceValue
        {
            get
            {
                switch (Soldier?.Gender)
                {
                    case Gender.Female:
                        return (WaistAverage + HipAverage) - NeckAverage;

                    case Gender.Male:
                    default:
                        return WaistAverage - NeckAverage;
                }
            }
        }

        [Display(Name = "Calculated Body Fat %")]
        internal Double BodyFatPercentage
        {
            get
            {
                switch (Soldier?.Gender)
                {
                    case Gender.Female:
                        return Math.Round(((163.205 * Math.Log10(WaistAverage + HipAverage - NeckAverage)) - (97.684 * Math.Log10((Double)Height)) - 78.387));

                    case Gender.Male:
                    default:
                        return Math.Round((86.010 * Math.Log10(WaistAverage - NeckAverage)) - (70.041 * Math.Log10((Double)Height)) + 36.76);
                }
            }
        }

        [Display(Name = "Max Body Fat %")]
        internal Double MaximumAllowableBodyFat
        {
            get
            {
                return
                    ABCPScoreTables
                    .MaxAllowablePercentages
                    .Where(_ => _.Gender == Soldier?.Gender)
                    .Where(_ => _.AgeGroup == AgeGroup)
                    .Select(_ => _.Maximum)
                    .SingleOrDefault();
            }
        }

        [Display(Name = "Is Passing Tape?")]
        internal Boolean IsPassingTape => RequiresTape && Measurements.Any() && BodyFatPercentage <= MaximumAllowableBodyFat;

        [Display(Name = "Is Passing?")]
        internal Boolean IsPassing => !RequiresTape || IsPassingTape;

        public class Measurement
        {
            [Range(0, 50)]
            public Double Waist { get; set; }

            [Range(0, 50)]
            public Double Neck { get; set; }

            [Range(0, 50)]
            public Double Hips { get; set; }

            public override string ToString()
            {
                var print = $"{Waist} / {Neck}";

                if (Hips > 0)
                {
                    print += $" / {Hips}";
                }

                return print;
            }
        }

        private static Double Average_To_Half(IEnumerable<Double> values)
        {
            if (values.Any())
            {
                return Math.Round(values.Average() * 2, MidpointRounding.AwayFromZero) / 2;
            }

            return 0;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date > DateTime.Today) yield return new ValidationResult("Cannot select a date after today", new[] { nameof(Date) });

            if (Height < 58 || Height > 80) yield return new ValidationResult("Height seems out of tolerance", new[] { nameof(Height) });

            if (Weight < 100) yield return new ValidationResult("Weight under minimum, recheck", new[] { nameof(Weight) });

            if (!AreMeasurementsValid) yield return new ValidationResult("Measurements are out of tolerance, differ by more than 1 inch", new[] { nameof(Measurements) });
        }

        public class ABCP_Soldier
        {
            public Rank Rank { get; set; }

            [Required, StringLength(50)]
            [Display(Name = "Last Name")]
            public String LastName { get; set; }

            [Required, StringLength(50)]
            [Display(Name = "First Name")]
            public String FirstName { get; set; }

            [StringLength(50), Display(Name = "Middle Name")]
            public String MiddleName { get; set; }

            public int Age { get; set; }

            [Required]
            public Gender Gender { get; set; } = Gender.Male;
        }
    }
}