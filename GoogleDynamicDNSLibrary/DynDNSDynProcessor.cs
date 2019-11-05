using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDynamicDNSLibrary
{
    public class DynDNSDynProcessor : DynProcessor
    {
        public DynDNSDynProcessor(HostConfig config) : base(config) { }
        const string url = "https://dyndnss.net/?user={0}&pass={1}&domain={2}&updater=other";
        public override string GetProcessorName()
        {
            return "DynDNS";
        }

        public override bool SubmitUpdate()
        {
            string publicIP = Process.GetPublicIP();
            string requeststring = string.Format(url, Config.Username, Config.Password, Config.Hostname);
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

            bool result = retVal.Contains("Manuelles Update war erfolgreich!");
            StatusMessage = retVal;
            ResultCode = retVal;
            return result;
        }
    }
}
