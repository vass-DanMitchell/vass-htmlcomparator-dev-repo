using ComparerEntities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Localization;
using HtmlDiff;
using System.Text.RegularExpressions;
using HtmlComparerService.Models;
namespace HtmlComparerService.Controllers
{
    public class ComparerController : Controller
    {
        const string ERROR_EMPTY_HTML1 = "The content on the first html is null or empty";
        const string ERROR_INPUTS_VALIDATION = "Error validating input parameters";
       
        const string ERROR_OUTPUT_RESULT = "Error on the comparision result";
        const string ERROR_OUTPUT_EMPTY = "The output HTML is null or empty";

        const string UNHANDLED_EX = "An error was ocurred processing the request";

        #region serviceEndPoints
      

        [HttpPost]
        [Route("compare")]
        public async Task<IActionResult> ApiComparePost()
        {
            Response oResopnse = new Response();
            Tuple<string, bool> TupleValidationResult = null;
            string html1 = Request.Form["html1"].ToString();
            string html2 = Request.Form["html2"].ToString();
            int mode = Int32.Parse(Request.Form["mode"].ToString());

            /*log request*/
            await logRequest(html1, html2);

            try
            {

                /*Validate inputs */
                TupleValidationResult = validateInputs(html1, html2);
                bool validationResult = TupleValidationResult.Item2;
                if (validationResult == false)
                {
                    oResopnse = buildResponseResult(ERROR_INPUTS_VALIDATION, TupleValidationResult.Item1, 100, false, "");
                    return Ok(oResopnse);
                }

                /*Do comparission*/
                oResopnse.responseContent = CallComparissionEngine(html1, html2);
                
                if (oResopnse.responseContent.Length <= 0)
                {
                    oResopnse = buildResponseResult(ERROR_OUTPUT_RESULT, ERROR_OUTPUT_EMPTY, 200, false, "");
                    return Ok(oResopnse);
                }

                /*Build response */
                oResopnse = buildResponseResult("", "", 0, true, oResopnse.responseContent);
                oResopnse.responseType = "html";
                ComparerModel oComparerModel = new ComparerModel();
                oComparerModel.htmlResponse = oResopnse.responseContent;

                if (mode == 1) //return on html format
                {
                    return View("WebComparer", oComparerModel);
                }
                else if (mode == 2) // return on json format
                {
                    oResopnse.responseType = "json";
                    return Ok(oResopnse);
                }
                else if (mode == 3) //return on base64
                {
                    oResopnse.responseType = "base64";
                    oResopnse.responseContent = returnStringOnBase64Encoded(oResopnse.responseContent).Result;
                }
            }
            catch (Exception ex)
            {
                oResopnse = buildResponseResult(UNHANDLED_EX, ex.Message, 300, false, "");
            }

            return Ok(oResopnse);
        }
        #endregion


        #region serviceUtils
        private async Task<bool> logError(Exception exError, string step)
        {
            try
            {
                var rootFolder = Directory.GetCurrentDirectory();
                string file = "logfile.txt";
                string fullpath = rootFolder + @"\\" + file;

                string error = DateTime.Now.ToString();
                error = error + " - " + exError.Message;
                error = error + " - Step: " + step;                
                using (FileStream fs = new FileStream(fullpath, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(error);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        private async Task<bool> logRequest(string html1, string html2)
        {
            try
            {
                var rootFolder = Directory.GetCurrentDirectory();
                string file = "logfile.txt";
                string fullpath = rootFolder + @"\\" + file;
                string content = DateTime.Now.ToString() + " - " + html1.Length.ToString() + " - " + html2.Length.ToString();
                using (FileStream fs = new FileStream(fullpath, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(content);
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;

        }
        private async Task<string> returnStringOnBase64Encoded(string htmlToEncode)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(htmlToEncode);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        #endregion

        #region serviceProcess
        private string CallComparissionEngine(string html1, string html2)
        {
            string outputHTML = "";
            var diffHelper = new HtmlDiff.HtmlDiff(html1, html2);
            diffHelper.AddBlockExpression(new Regex(@"[\d]{1,2}[\s]*(Jan|Feb)[\s]*[\d]{4}", RegexOptions.IgnoreCase));

            /*perform the comparision*/
            outputHTML = diffHelper.Build();
            return outputHTML;
        }
        private Response buildResponseResult(string errorMessage, string errorText, int errorCode, bool result, string responseContent)
        {
            return new Response
            {
                result = result,
                errorMessage = errorMessage,
                errorText = errorText,
                errorCode = errorCode.ToString(),
                date = DateTime.Now.ToString(),
                responseContent = responseContent                
            };
        }
        private Tuple<string, bool> validateInputs(string html1, string html2)
        {

            Tuple<string, bool> ValidationResult = null;
            if (html1 == null)
            {

                ValidationResult = new Tuple<string, bool>(ERROR_EMPTY_HTML1, false);
                return ValidationResult;
            }

            if (html1.Trim().Length <= 0)
            {

                ValidationResult = new Tuple<string, bool>(ERROR_EMPTY_HTML1, false);
                return ValidationResult;
            } 

            ValidationResult = new Tuple<string, bool>("", true);
            return ValidationResult;
        }
        #endregion
    }
}
