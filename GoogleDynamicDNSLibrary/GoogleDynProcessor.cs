using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDynamicDNSLibrary
{
    public class GoogleDynProcessor: DynProcessor
    {
        const string GoogleDNSURL = "https://{0}:{1}@domains.google.com/nic/update";
        const string GoogleDNShostname = "hostname";
        const string myip = "myip";

        public GoogleDynProcessor(HostConfig configuration) : base(configuration) { }
        public override string GetProcessorName()
        {
            return "Google Dynamic DNS";
        }
        public override bool SubmitUpdate()
        {
            string publicIP = Process.GetPublicIP();
            string requeststring = string.Format(GoogleDNSURL, Config.Username, Config.Password) + "?" 
                + GoogleDNShostname + "=" + Config.Hostname + "&" + myip + "=" + publicIP;
            var request = (HttpWebRequest)WebRequest.Create(requeststring);

            request.UserAgent = Constants.UserAgent;
            //request.Method = "POST";
            request.Method = "GET";
            request.Credentials = new NetworkCredential(Config.Username, Config.Password);
            

            var response = (HttpWebResponse)request.GetResponse();
            string retVal = null;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                retVal = sr.ReadToEnd();
            }

            bool result = (GoogleResponses.GetStatus(retVal));
            StatusMessage = GoogleResponses.GetDescription(retVal);
            ResultCode = retVal;
            return result;
        }
    }
}
