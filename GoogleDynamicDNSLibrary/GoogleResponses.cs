using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDynamicDNSLibrary
{
    public static class GoogleResponses
    {
        public const string good= "good";
        public const string nochg = "nochg";
        public const string nohost = "nohost";
        public const string badauth = "badauth";
        public const string notfqdn = "notfqdn";
        public const string badagent = "badagent";
        public const string abuse = "abuse";
        public const string x911 = "911";
        public const string conflict = "conflict";

        static GoogleResponses()
        {
            ResponseStatus.Add(good, true);
            ResponseStatus.Add(nochg, true);
            ResponseStatus.Add(nohost, false);
            ResponseStatus.Add(badauth, false);
            ResponseStatus.Add(notfqdn, false);
            ResponseStatus.Add(badagent, false);
            ResponseStatus.Add(abuse, false);
            ResponseStatus.Add(x911, false);
            ResponseStatus.Add(conflict, false);

            ResponseDescription.Add(good, "The update was successful. You should not attempt another update until your IP address changes.");
            ResponseDescription.Add(nochg, "The supplied IP address is already set for this host. You should not attempt another update until your IP address changes.");
            ResponseDescription.Add(nohost, "The hostname does not exist, or does not have Dynamic DNS enabled.");
            ResponseDescription.Add(badauth, "The username / password combination is not valid for the specified host.");
            ResponseDescription.Add(notfqdn, "The supplied hostname is not a valid fully-qualified domain name.");
            ResponseDescription.Add(badagent, "Your Dynamic DNS client is making bad requests. Ensure the user agent is set in the request.");
            ResponseDescription.Add(abuse, "Dynamic DNS access for the hostname has been blocked due to failure to interpret previous responses correctly.");
            ResponseDescription.Add(x911, "An error happened on our end. Wait 5 minutes and retry.");
            ResponseDescription.Add(conflict, "A custom A or AAAA resource record conflicts with the update. Delete the indicated resource record within DNS settings page and try the update again.");

        }
        public static Dictionary<string, bool> ResponseStatus = 
            new Dictionary<string, bool>();
        public static Dictionary<string, string> ResponseDescription =
            new Dictionary<string, string>();

        static string GetResponseKey(string response)
        {
            string resp = null;
            int i = response.IndexOf(' ');
            if (i > -1)
            {
                resp = response.Substring(0, i).Trim();
            }
            else
            {
                resp = response.Trim();
            }
            return resp;
        }
        public static bool GetStatus(string response)
        {
            string resp = GetResponseKey(response);
            if (ResponseStatus.ContainsKey(resp))
            {
                return ResponseStatus[resp];
            }
            else
            {
                return false;
            }
        }
        public static string GetDescription(string response)
        {
            string resp = GetResponseKey(response);
            if (ResponseDescription.ContainsKey(resp))
            {
                return ResponseDescription[resp];
            }
            else
            {
                return "Unknown Response: " + response;
            }
        }
    }
}
