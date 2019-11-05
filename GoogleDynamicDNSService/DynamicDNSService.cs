using GoogleDynamicDNSLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDynamicDNSService
{
    public partial class DynamicDNSService : ServiceBase
    {
        public DynamicDNSService()
        {
            InitializeComponent();
        }

        System.Timers.Timer timer = null;
        protected override void OnStart(string[] args)
        {
            timer = new System.Timers.Timer(60000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            var data = new SaveData();
            bool oneSuccess = false;
            foreach (var host in data.Hosts)
            {
                if (!string.IsNullOrEmpty(host.Hostname) && GoogleDynamicDNSLibrary.Process.CheckIfUpdateRequired(host.Hostname))
                {
                    var processor = DynProcessor.GetProcessor(host);
                    if (processor.SubmitUpdate())
                    {
                        using (EventLog evtLog = new EventLog("Application"))
                        {
                            evtLog.Source = "Application";
                            evtLog.WriteEntry(string.Format("Updated {3}, host={0}:\r\n{1}\r\n({2})",
                                host.Hostname, processor.StatusMessage, processor.ResultCode, processor.GetProcessorName()), 
                                EventLogEntryType.Information, 1, 1);
                        }
                        oneSuccess = true;
                        timer.Interval = 60000;
                    }
                    else
                    {
                        using (EventLog evtLog = new EventLog("Application"))
                        {
                            evtLog.Source = "Application";
                            evtLog.WriteEntry(string.Format("Failed to update {3}, host={0}:\r\n{1}\r\n({2})",
                                host.Hostname,  processor.StatusMessage, processor.ResultCode, processor.GetProcessorName()),
                                EventLogEntryType.Error, 1, 1);
                        }

                    }
                }
            }
            if (!oneSuccess)
            {

                if (timer.Interval < 360000)
                {
                    timer.Interval += 60000;
                }
            }
            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Stop();
            timer.Dispose();
        }
    }
}
