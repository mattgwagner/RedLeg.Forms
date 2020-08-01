using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RedLeg.Forms
{
    [Route("/")]
    public partial class FormController : Controller
    {
        /// <summary>
        /// Generates a DA 4856 Developmental Counseling Form as PDF
        /// </summary>
        [HttpPost("[action]", Name = "GenerateDA4856"), Produces("application/pdf", Type = typeof(FileContentResult))]
        public IActionResult DA4856([FromBody] Counseling model)
        {
            const String prefix = "form1[0]";

            using (var stream = typeof(FormController).GetTypeInfo().Assembly.GetManifestResourceStream("RedLeg.Forms.DA4856.pdf"))
            using (var output = new MemoryStream())
            {
                var reader = new PdfReader(stream);
                var stamper = new PdfStamper(reader, output);

                var form = stamper.AcroFields;

                form.SetField($"{prefix}.Page1[0].Name[0]", model.Name);
                form.SetField($"{prefix}.Page1[0].Name_Title_Counselor[0]", model.Counselor);
                form.SetField($"{prefix}.Page1[0].Key_Points_Disscussion[0]", model.KeyPointsOfDiscussion);
                form.SetField($"{prefix}.Page1[0].Date_Counseling[0]", model.Date.ToString("yyyy-MM-dd"));
                form.SetField($"{prefix}.Page1[0].Rank_Grade[0]", ShortName(model.Rank));
                form.SetField($"{prefix}.Page2[0].Leader_Responsibilities[0]", model.LeadersResponsibilities);
                form.SetField($"{prefix}.Page1[0].Purpose_Counseling[0]", model.Purpose);
                form.SetField($"{prefix}.Page1[0].Organization[0]", model.Organization);
                form.SetField($"{prefix}.Page2[0].Plan_Action[0]", model.PlanOfAction);
                form.SetField($"{prefix}.Page2[0].Assessment[0]", model.Assessment);

                stamper.Close();

                return File(output.ToArray(), "application/pdf", "DA4856-Developmental-Counseling.pdf");
            }
        }

        /// <summary>
        /// Generates a DA 5500 Body Composition Worksheet (Male) as PDF
        /// </summary>
        [HttpPost("[action]", Name = "GenerateDA5500"), Produces("application/pdf", Type = typeof(FileContentResult))]
        public IActionResult DA5500([FromBody] ABCP model)
        {
            const String prefix = "form1[0].Page1[0]";

            using (var stream = typeof(Program).GetTypeInfo().Assembly.GetManifestResourceStream("RedLeg.Forms.DA5500.pdf"))
            using (var output = new MemoryStream())
            {
                var reader = new PdfReader(stream);
                var stamper = new PdfStamper(reader, output);

                var form = stamper.AcroFields;

                // Update the form fields as appropriate

                var name = $"{model.Soldier.LastName}, {model.Soldier.FirstName} {model.Soldier.MiddleName?.ToCharArray().FirstOrDefault()}";

                var name_encoded = Encoding.Default.GetBytes(name);

                name = Encoding.ASCII.GetString(name_encoded).Replace('\0', ' ');

                form.SetField($"{prefix}.NAME[0]", name);
                form.SetField($"{prefix}.RANK[0]", $"{ShortName(model.Soldier.Rank)}");

                form.SetField($"{prefix}.HEIGHT[0]", $"{model.Height}");
                form.SetField($"{prefix}.WEIGHT[0]", $"{model.Weight}");
                form.SetField($"{prefix}.AGE[0]", $"{model.Soldier.Age}");

                form.SetField($"{prefix}.DATE_A[0]", $"{model.Date:yyyyMMdd}");
                form.SetField($"{prefix}.DATE_B[0]", $"{model.Date:yyyyMMdd}");

                var q = new Queue<String>(new[] { "FIRST", "SCND", "THIRD" });

                foreach (var measurement in model.Measurements)
                {
                    // Abdomen

                    var m = q.Dequeue();

                    form.SetField($"{prefix}.{m}_A[0]", $"{measurement.Neck}");
                    form.SetField($"{prefix}.{m}_B[0]", $"{measurement.Waist}");
                }

                // Check

                if (model.RequiresTape)
                {
                    form.SetField($"{prefix}.IS[0]", model.IsPassing ? "1" : "0");
                    form.SetField($"{prefix}.ISNOT[0]", model.IsPassing ? "0" : "2");
                }

                form.SetField($"{prefix}.AVE_B[0]", $"{model.WaistAverage}");
                form.SetField($"{prefix}.AVE_A[0]", $"{model.NeckAverage}");
                form.SetField($"{prefix}.AVE_C[0]", $"{model.NeckAverage}");
                form.SetField($"{prefix}.AVE_D[0]", $"{model.WaistAverage}");
                form.SetField($"{prefix}.AVE_E[0]", $"{model.CircumferenceValue}");
                form.SetField($"{prefix}.AVE_F[0]", $"{model.Height}");
                form.SetField($"{prefix}.AVE_G[0]", $"{model.BodyFatPercentage}%");

                form.SetField($"{prefix}.REMRKS[0]", $@"
                    Soldier's Actual Weight: {model.Weight} lbs
                    Screening Table Weight: {model.Screening_Weight} lbs
                    {(model.RequiresTape ? "OVER " : "UNDER")} {(Math.Abs(model.Screening_Weight - model.Weight))} lbs

                    Soldier's Actual Body Fat %: {model.BodyFatPercentage}%
                    Authorized Body Fat %: {model.MaximumAllowableBodyFat}%

                    Individual is {(model.IsPassing ? "" : "not")} in compliance with Army standards.
                ");

                stamper.Close();

                return File(output.ToArray(), "application/pdf", "DA5501-Body-Composition-Worksheet.pdf");
            }
        }

        /// <summary>
        /// Generates a DA 5501 Body Composition Worksheet (Female) as PDF
        /// </summary>
        [HttpPost("[action]", Name = "GenerateDA5501"), Produces("application/pdf", Type = typeof(FileContentResult))]
        public IActionResult DA5501([FromBody] ABCP model)
        {
            const String prefix = "form1[0]";

            using (var stream = typeof(Program).GetTypeInfo().Assembly.GetManifestResourceStream("RedLeg.Forms.DA5501.pdf"))
            using (var output = new MemoryStream())
            {
                var reader = new PdfReader(stream);
                var stamper = new PdfStamper(reader, output);

                var form = stamper.AcroFields;

                // Update the form fields as appropriate

                form.SetField($"{prefix}.NAME[0]", $"{model.Soldier.LastName} {model.Soldier.FirstName}");
                form.SetField($"{prefix}.RANK[0]", $"{ShortName(model.Soldier.Rank)}");

                form.SetField($"{prefix}.HEIGHT[0]", $"{model.Height}");
                form.SetField($"{prefix}.WEIGHT[0]", $"{model.Weight}");
                form.SetField($"{prefix}.AGE[0]", $"{model.Soldier.Age}");

                form.SetField($"{prefix}.DATE[0]", $"{model.Date:yyyyMMdd}");
                form.SetField($"{prefix}.DATE_B[0]", $"{model.Date:yyyyMMdd}");

                // form.SetField($"{prefix}.Page1[0].Name[0]", model.Name);

                //form1[0].Page1[0].NECK_A[0] / B / C

                var q = new Queue<String>(new[] { "A", "B", "C" });

                foreach (var measurement in model.Measurements)
                {
                    // Abdomen

                    var m = q.Dequeue();

                    form.SetField($"{prefix}.NECK_{m}[0]", $"{measurement.Neck}");
                    form.SetField($"{prefix}.ARM_{m}[0]", $"{measurement.Waist}");
                    form.SetField($"{prefix}.HIP_{m}[0]", $"{measurement.Hips}");
                }

                form.SetField($"{prefix}.AVE_NECK[0]", $"{model.NeckAverage}");
                form.SetField($"{prefix}.AVE_ARM[0]", $"{model.WaistAverage}");
                form.SetField($"{prefix}.AVE_HIP[0]", $"{model.HipAverage}");

                form.SetField($"{prefix}.WE_FACTR[0]", $"{model.WaistAverage}");
                form.SetField($"{prefix}.HE_FACTR[0]", $"{model.Height}");
                form.SetField($"{prefix}.TOT_A[0]", $"{model.HipAverage + model.WaistAverage}");
                form.SetField($"{prefix}.H_FACTR[0]", $"{model.HipAverage}");
                form.SetField($"{prefix}.N_FACTR[0]", $"{model.NeckAverage}");
                form.SetField($"{prefix}.F_FACTR[0]", $"{model.HipAverage + model.WaistAverage - model.NeckAverage}");
                form.SetField($"{prefix}.HE_FACTR[0]", $"{model.Height}");

                form.SetField($"{prefix}.BODY_FAT[0]", $"{model.BodyFatPercentage}");

                //form1[0].Page1[0].APPRVD[0]

                form.SetField($"{prefix}.REMRKS[0]", $@"
                    AUTHORIZED BODY FAT IS: {model.MaximumAllowableBodyFat}%
                         TOTAL BODY FAT IS: {model.BodyFatPercentage}%

                    SOLDIER {(model.IsPassing ? "MEETS" : "DOES NOT MEET")} ARMY STANDARDS
                ");

                // Check

                form.SetField($"{prefix}.IS[0]", model.IsPassing ? "1" : "0");
                form.SetField($"{prefix}.ISNOT[0]", model.IsPassing ? "0" : "2");

                stamper.Close();

                return File(output.ToArray(), "application/pdf", "DA5501-Body-Composition-Worksheet.pdf");
            }
        }

        //[HttpPost("[action]", Name = "GenerateDA705"), Produces("application/pdf", Type = typeof(FileContentResult))]
        //public IActionResult DA705([FromBody]APFT model)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Generate a DA 3749 Equipment Receipt as PDF
        /// </summary>
        [HttpPost("[action]", Name = "GenerateDA3749"), Produces("application/pdf", Type = typeof(FileContentResult))]
        public IActionResult DA3749([FromBody] EquipmentReceipt model)
        {
            const String prefix = "form1[0].Page1[0]";

            using (var stream = typeof(Program).GetTypeInfo().Assembly.GetManifestResourceStream("RedLeg.Forms.DA3749.pdf"))
            using (var output = new MemoryStream())
            {
                var reader = new PdfReader(stream);
                var stamper = new PdfStamper(reader, output);

                var form = stamper.AcroFields;

                // Update the form fields as appropriate -- 4 per page, _B, _C, _D

                form.SetField($"{prefix}.UNIT[0]", $"{model.Unit}");
                form.SetField($"{prefix}.RECEIPT[0]", $"{model.ReceiptNumber}");
                form.SetField($"{prefix}.STOCK[0]", $"{model.StockNumber}");
                form.SetField($"{prefix}.SERIAL[0]", $"{model.SerialNumber}");
                form.SetField($"{prefix}.DESCRIPT[0]", $"{model.Description}");
                form.SetField($"{prefix}.FROM[0]", $"{model.From}");
                form.SetField($"{prefix}.NAME[0]", $"{model.Name}");
                form.SetField($"{prefix}.GRADE[0]", $"{ShortName(model.Grade)}");

                stamper.Close();

                return File(output.ToArray(), "application/pdf", "DA3749-Equipment-Receipt.pdf");
            }
        }

        /// <summary>
        /// Generate a DA 4655-R Target List Worksheet
        /// </summary>
        [HttpPost("[action]", Name = "GenerateDA4655R"), Produces("application/pdf", Type = typeof(FileContentResult))]
        public IActionResult DA4655R([FromBody] TargetListWorksheet model)
        {
            const String prefix = "form1[0].Page1[0].#subform[0]";

            using (var stream = typeof(Program).GetTypeInfo().Assembly.GetManifestResourceStream("RedLeg.Forms.DA4655_R.pdf"))
            using (var output = new MemoryStream())
            {
                var reader = new PdfReader(stream);
                var stamper = new PdfStamper(reader, output);

                var form = stamper.AcroFields;

                int row = 0;

                foreach (var target in model.Targets)
                {
                    // FIRST ROW NO _

                    var row_suffix = row > 0 ? $"_{row}" : "";

                    form.SetField($"{prefix}.TARGET[0]{row_suffix}", target.TargetNumber);
                    form.SetField($"{prefix}.DESC[0]{row_suffix}", target.Description);
                    form.SetField($"{prefix}.LOC[0]{row_suffix}", target.Location);
                    form.SetField($"{prefix}.ALT[0]{row_suffix}", target.Altitude);
                    form.SetField($"{prefix}.ATT[0]{row_suffix}", target.Attitude);
                    form.SetField($"{prefix}.LENGTH[0]{row_suffix}", target.SizeLength);
                    form.SetField($"{prefix}.WIDTH[0]{row_suffix}", target.SizeWidth);
                    form.SetField($"{prefix}.SOURCE[0]{row_suffix}", target.SourceOrAccuracy);
                    form.SetField($"{prefix}.REMARKS[0]{row_suffix}", target.Remarks);
                    form.SetField($"{prefix}.UNK_A[0]{row_suffix}", target.Group_1);
                    form.SetField($"{prefix}.UNK_B[0]{row_suffix}", target.Group_2);
                    form.SetField($"{prefix}.UNK_C[0]{row_suffix}", target.Group_3);
                    form.SetField($"{prefix}.UNK_D[0]{row_suffix}", target.Group_4);
                    form.SetField($"{prefix}.UNK_E[0]{row_suffix}", target.Group_5);

                    // ATT_{#}
                    // UNK_(A,B,C,D,E)_{#}
                    // LOC_{#}
                    // REMARKS_{#}
                    // WIDTH_{#}
                    // LENGTH_{#}
                    // DESC_{#}
                    // ALT_{#}
                    // TARGET_{#}
                    // SOURCE_{#}

                    row++;
                }

                stamper.Close();

                return File(output.ToArray(), "application/pdf", "DA4655-R-Target-List-Worksheet.pdf");
            }
        }

        private static string DisplayName(Enum val)
        {
            return GetDisplayValue(val, a => a.Name);
        }

        private static string ShortName(Enum val)
        {
            return GetDisplayValue(val, a => a.ShortName);
        }

        private static string GetDisplayValue(Enum val, Func<DisplayAttribute, String> selector)
        {
            FieldInfo fi = val.GetType().GetField(val.ToString());

            DisplayAttribute[] attributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return selector.Invoke(attributes[0]);
            }

            return val.ToString();
        }
    }
}