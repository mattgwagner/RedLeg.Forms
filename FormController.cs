using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;

namespace RedLeg.Forms
{
    public class FormController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            return Content("Hello world!", "text/plain");
        }

        public IActionResult DA4856(Counseling model)
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

        public ActionResult DA5500()
        {
            throw new NotImplementedException();
        }

        public ActionResult DA5501()
        {
            throw new NotImplementedException();
        }

        public ActionResult DA705()
        {
            throw new NotImplementedException();
        }

        public ActionResult DA3749()
        {
            throw new NotImplementedException();
        }

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

        public string DisplayName(Enum val)
        {
            return GetDisplayValue(val, a => a.Name);
        }

        public string ShortName(Enum val)
        {
            return GetDisplayValue(val, a => a.ShortName);
        }

        private string GetDisplayValue(Enum val, Func<DisplayAttribute, String> selector)
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
