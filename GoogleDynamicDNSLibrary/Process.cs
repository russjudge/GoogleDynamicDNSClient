using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDynamicDNSLibrary
{
    public static class Process
    {
        public static string GetIP(string hostname)
        {
            string retVal = null;
            var host = Dns.GetHostEntry(hostname);
            foreach (var ip in host.AddressList)
            {
                //if (ip.AddressFamily == AddressFamily.InterNetwork)
                //{
                    retVal = ip.ToString();
                //}
            }
            return retVal;
        }
        public static bool CheckIfUpdateRequired(string hostname)
        {
            string hostIP = GetIP(hostname);
            string localIP = GetPublicIP();
            return !hostIP.Equals(localIP);
        }
        
        public static string GetPublicIP()
        {
            var request = (HttpWebRequest)WebRequest.Create(Constants.IPCheckURL);

            request.UserAgent = Constants.UserAgent;

            string publicIPAddress;

            request.Method = "GET";
            using (WebResponse response = request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    publicIPAddress = reader.ReadToEnd();
                }
            }

            return publicIPAddress.Replace("\n", "");
        }
    }
}
