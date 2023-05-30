using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using ComparerEntities;
using System.Text;

namespace HtmlComparer.Controllers
{
    [ApiController]
  
    public class ComparerController : ControllerBase
    {
        [Route("[controller]")]
        [HttpGet]
        public IActionResult comparerServiceFile( ) 
        {
            //string htmlResultComparision = "juan";
            //Response oResopnse = new Response();
            //oResopnse.responseContent = readFile();


            //return oResopnse;
            //string htmlResultComparision = "juan";
            //Response oResopnse = new Response();
            //oResopnse.responseContent = readFile();
            //HttpResponseMessage res = new HttpResponseMessage();
            //string XML = "<note><body>" + oResopnse.responseContent + "</body></note>";

            //res.Content = new StringContent(XML, Encoding.UTF8, "application/xml");


            //return res;
            Response oResopnse = new Response();
            oResopnse.result = true;
            oResopnse.date = DateTime.Now;            
            oResopnse.responseContent = readFile();
            

            OkObjectResult result = Ok(oResopnse);

            // currently result.Formatters is empty but we'd like to ensure it will be so in the future
            result.Formatters.Clear();

            // force response as xml
            result.Formatters.Add(new Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerOutputFormatter());

            return result;
        }
        private string getResponseXML() 
        {
            Response oResopnse = new Response();
            oResopnse.responseContent = readFile();
            HttpResponseMessage res = new HttpResponseMessage();
            string XML = "<note><body>" + oResopnse.responseContent + "</body></note>";

            return XML;
        }


        //[HttpGet]        
        //public HttpResponseMessage compare()
        //{
        //    string htmlResultComparision = "juan";
        //    Response oResopnse = new Response();
        //    oResopnse.responseContent = readFile();
        //    HttpResponseMessage res = new HttpResponseMessage();
        //    string XML = "<note><body>" + oResopnse.responseContent + "</body></note>";

        //    res.Content = new StringContent(XML, Encoding.UTF8, "application/xml");


        //    return res;
        //}
        private string readFile() 
        {
            string path = @"C:\temp\html.html";
            string file = @"<p><i>This is</i> some sample text to <strong>demonstrate</strong> the capability of the <strong>HTML diff tool</strong>.</p>
                                <p>It is based on the <b>Ruby</b> implementation found <a href='http://github.com/myobie/htmldiff'>here</a>. Note how the link has no tooltip</p>
                                <p>What about a number change: 123456?</p>
                                <table cellpadding='0' cellspacing='0'>
                                <tr><td>Some sample text</td><td>Some sample value</td></tr>
                                <tr><td>Data 1 (this row will be removed)</td><td>Data 2</td></tr>
                                </table>
                                Here is a number 2 32
                                <br><br>
                                This date: 1 Jan 2016 is about to change (note how it is treated as a block change!)";
            //file = System.IO.File.ReadAllText(path);
            return file;
        }
    }
}
