using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Net.Http.Json;
using System.Text.Json;
using static System.Reflection.Metadata.BlobBuilder;
using ComparerEntities;
using System.Text;
using System.Runtime;
using HtmlDiff;

namespace HtmlComparerService.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult Demo()
        {
            ViewBag.html1 = "<p>example test 1<p>";
            ViewBag.html2 = "<p>example test 1 with some test updated<p>";
            ViewBag.action = "demo/processForm";
            return View("Demo");
        }

        [HttpPost]
        public IActionResult processForm()
        {
            string html1 = Request.Form["hdnhtml1"];
            string html2 = Request.Form["hdnhtml2"];
            string result = Request.Form["result"];
            string mode = "2";
            Response res = new Response();
            
            res = test(html1, html2, mode).Result;
                       

            ViewBag.html1 = html1;
            ViewBag.html2 = html2;

            //test base 64 decoding response
            if (res.responseType == "base64")
            {
                byte[] data = Convert.FromBase64String(res.responseContent);
                res.responseContent = Encoding.UTF8.GetString(data);                
            }
            
            ViewBag.result = res.responseContent;
            ViewBag.disabled = "disabled";
            ViewBag.action = "demo/processForm";
            return View("Demo");

        }
        private async Task<Response> test(string html1, string html2, string mode) {

            //get service end point
            string uri = AppSettingsVariables.urlService;
            
            //request creation
            var client = new HttpClient();
            var values = new Dictionary<string, string>()
                    {
                        {"html1", html1},
                        {"html2", html2},
                        {"mode",mode}

                    };

            var content = new FormUrlEncodedContent(values);

            //call service
            var response = await client.PostAsync(uri, content);

            //get resposne
            var data = await response.Content.ReadAsStringAsync();
            
            //return result
            var result = JsonSerializer.Deserialize<Response>(data);                        
            response.EnsureSuccessStatusCode();
            return result;
        } 

    }
}
