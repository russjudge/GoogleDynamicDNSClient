using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDynamicDNSLibrary
{
    public static class Constants
    {
        public const string URL = "https://{0}:{1}@domains.google.com/nic/update";
        public const string UserAgent = "Google Dynamic DNS Client by Russ Judge";
        public const string hostname = "hostname";
        public const string myip = "myip";
        public const string offline = "offline";
        public const string IPCheckURL = "https://domains.google.com/checkip";
    }
}
