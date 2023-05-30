using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComparerEntities
{
    public class Response
    {
        
        public bool result { get; set; }
        public string errorMessage { get; set; }
        public string errorText { get; set; }
        public string errorCode { get; set; }
        public string responseContent { get; set; }
        public string responseType { get; set; }
        public string  date { get; set; }

        public Response() {
            this.result = false;
            this.errorMessage = "";
            this.errorText = "";
            this.errorCode = "";
            this.responseContent = "";
            this.responseType = "";


        }   
    }
}
