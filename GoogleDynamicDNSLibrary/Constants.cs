using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDynamicDNSLibrary
{
    public static class Constants
    {
        static Constants()
        {
            Assembly currentAssem = Assembly.GetEntryAssembly();
            object[] attribs = currentAssem.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true);
            if (attribs.Length > 0)
            {
                Company = ((AssemblyCompanyAttribute)attribs[0]).Company;
            }
            attribs = currentAssem.GetCustomAttributes(typeof(System.Reflection.AssemblyTitleAttribute), true);
            if (attribs.Length > 0)
            {
                Application = ((AssemblyTitleAttribute)attribs[0]).Title;
            }

        }
        public static string Company
        {
            get;
            private set;
        }
        public static string Application
        {
            get;
            private set;
        }
        public const string URL = "https://{0}:{1}@domains.google.com/nic/update";
        public const string UserAgent = "Google Dynamic DNS Client by Russ Judge";
        public const string hostname = "hostname";
        public const string myip = "myip";
        public const string offline = "offline";
        public const string IPCheckURL = "https://domains.google.com/checkip";
        public const string Software = "Software";
    }
}
