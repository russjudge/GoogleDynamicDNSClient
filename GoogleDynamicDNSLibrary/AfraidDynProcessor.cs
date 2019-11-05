using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDynamicDNSLibrary
{
    class AfraidDynProcessor : DynProcessor
    {
        const string hostname = "hostname";
        const string url = "http://{0}:{1}@freedns.afraid.org/nic/update?{2}={3}&{4}={5}";
        const string myip = "myip";
        public AfraidDynProcessor(HostConfig configuration) : base(configuration) { }
        public override string GetProcessorName()
        {
            return "Free DNS";
        }
        public override bool SubmitUpdate()
        {
            string publicIP = Process.GetPublicIP();
            string requeststring = string.Format(url, Config.Username, Config.Password, hostname, Config.Hostname, myip, publicIP);
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

            bool result = !retVal.ToUpperInvariant().Contains("ERROR");
            StatusMessage = retVal;
            ResultCode = retVal;
            return result;
        }
    }
}
