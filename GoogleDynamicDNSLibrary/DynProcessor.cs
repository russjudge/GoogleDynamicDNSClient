using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDynamicDNSLibrary
{
    public abstract class DynProcessor
    {
		
        static DynProcessor()
        {
            ProcessorList = new Dictionary<string, string>();
            ProcessorList.Add("GoogleDynProcessor", "Google Domains");
            ProcessorList.Add("AfraidDynProcessor", "Free DNS (afraid.org)");
            ProcessorList.Add("DynDNSDynProcessor", "DynDNS (dyndnss.net)");
        }
		public static Dictionary<string,string> ProcessorList { get; private set; }
        public HostConfig Config { get; set; }

        public DynProcessor(HostConfig configuration)
        {
            Config = configuration;
        }

        public static DynProcessor GetProcessor(HostConfig configuration)
        {
            Type type = Type.GetType(string.Format("GoogleDynamicDNSLibrary.{0}", configuration.Processor));
            var constructorParmTypes = new Type[] { typeof(HostConfig) };
            var constructor = type.GetConstructor(constructorParmTypes);
            var parms = new object[] { configuration };
            return constructor.Invoke(parms) as DynProcessor;
        }
        public abstract bool SubmitUpdate();
        public abstract string GetProcessorName();

        public string StatusMessage { get; set; }
        public string ResultCode { get; set; }
    }
}
