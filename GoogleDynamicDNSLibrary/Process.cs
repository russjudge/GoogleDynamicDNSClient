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
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    retVal = ip.ToString();
                }
            }
            return retVal;
        }
        public static bool CheckIfUpdateRequired(string hostname)
        {
            string hostIP = GetIP(hostname);
            string localIP = GetPublicIP();
            return !hostIP.Equals(localIP);
        }
        public static string SubmitUpdate(string username, string password,
            string hostname)
        {
            string publicIP = GetPublicIP();
            string requeststring = string.Format(Constants.URL, username, password) + "?" + Constants.hostname + "=" + hostname + "&" + Constants.myip + "=" + publicIP;
            var request = (HttpWebRequest)WebRequest.Create(requeststring);

            request.UserAgent = Constants.UserAgent;
            //request.Method = "POST";
            request.Method = "GET";
            request.Credentials = new NetworkCredential(username, password);
            //NameValueCollection outgoingQueryString =
            //    System.Web.HttpUtility.ParseQueryString(String.Empty);

            //outgoingQueryString.Add(Constants.hostname, hostname);
            //outgoingQueryString.Add(Constants.myip, publicIP);
            //string postdata = outgoingQueryString.ToString();
            //byte[] buffer = System.Text.ASCIIEncoding.ASCII.GetBytes(postdata);
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = buffer.Length;
            //using (var stream = request.GetRequestStream())
            //{
            //    stream.Write(buffer, 0, buffer.Length);
            //}
            var response = (HttpWebResponse)request.GetResponse();
            string retVal = null;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                retVal = sr.ReadToEnd();
            }
            return retVal;
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
