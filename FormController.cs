using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;

namespace RedLeg.Forms
{
    [Route("/")]
    public partial class FormController : Controller
    {
        [HttpGet, ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult Index() => Redirect("/swagger");

        [HttpPost("[action]")]
        public FileContentResult DA4856([FromBody]Counseling model)
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

        [HttpPost("[action]")]
        public FileContentResult DA5500()
        {
            throw new NotImplementedException();
        }

        [HttpPost("[action]")]
        public FileContentResult DA5501()
        {
            throw new NotImplementedException();
        }

        [HttpPost("[action]")]
        public FileContentResult DA705()
        {
            throw new NotImplementedException();
        }

        [HttpPost("[action]")]
        public FileContentResult DA3749()
        {
            throw new NotImplementedException();
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
